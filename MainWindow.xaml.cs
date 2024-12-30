using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using System.Windows.Media;
using TextBox = System.Windows.Controls.TextBox;
using System.Threading.Tasks;
using System.Windows.Documents;
using RichTextBox = System.Windows.Controls.RichTextBox;
using System.IO;
using DataFormats = System.Windows.DataFormats;
using System.Collections.Generic;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace MiniWriter
{
    public partial class MainWindow : Window
    {
        private readonly ConfigManager _configManager;
        private readonly FileManager _fileManager;
        private readonly TrayIconManager _trayManager;
        private bool _isDragging = false;
        private Point _lastPosition;
        private readonly System.Windows.Forms.Timer _autoSaveTimer;
        private readonly System.Windows.Forms.Timer _opacityTimer;
        private bool _isMouseOver = false;
        private bool _isDarkTheme;
        private readonly SolidColorBrush _lightBackground = new SolidColorBrush(Color.FromArgb(77, 245, 245, 245));  // #F5F5F5 with 0.3 opacity
        private readonly SolidColorBrush _darkBackground = new SolidColorBrush(Color.FromArgb(64, 26, 26, 26));      // #1A1A1A with 0.25 opacity
        private readonly SolidColorBrush _lightText = new SolidColorBrush(Color.FromRgb(204, 204, 204));            // #CCCCCC
        private readonly SolidColorBrush _darkText = new SolidColorBrush(Color.FromRgb(184, 184, 184));             // #B8B8B8
        private readonly SolidColorBrush _lightBorder = new SolidColorBrush(Color.FromRgb(204, 204, 204));          // #CCCCCC
        private readonly SolidColorBrush _darkBorder = new SolidColorBrush(Color.FromRgb(56, 56, 56));              // #383838

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                
                _configManager = new ConfigManager();
                _fileManager = new FileManager();
                _trayManager = new TrayIconManager(this);

                _autoSaveTimer = new System.Windows.Forms.Timer
                {
                    Interval = 500
                };
                _autoSaveTimer.Tick += AutoSave;

                _opacityTimer = new System.Windows.Forms.Timer
                {
                    Interval = 3000
                };
                _opacityTimer.Tick += (s, e) => 
                {
                    if (!_isMouseOver)
                    {
                        mainBorder.Opacity = 0.3;
                    }
                };
                
                InitializeWindow();
                RegisterEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void InitializeWindow()
        {
            // 加载上次窗口位置
            var position = _configManager.LoadWindowPosition();
            Left = position.X;
            Top = position.Y;

            // 加载主题设置
            _isDarkTheme = _configManager.LoadThemeConfig();
            ApplyTheme(_isDarkTheme);

            // 加载保存的文本
            var textRange = new TextRange(inputTextBox.Document.ContentStart, inputTextBox.Document.ContentEnd);
            _fileManager.LoadText(textRange);

            // 确保窗口不在任务栏显示
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            
            // 初始化完成后隐藏窗口
            Hide();
        }

        private void RegisterEvents()
        {
            // 修改拖动事件的处理
            MouseLeftButtonDown += (s, e) => 
            {
                if (e.Source is Border || e.Source is Grid)  // 只有点边框或网格时才允许拖动
                {
                    _isDragging = true;
                    _lastPosition = e.GetPosition(this);
                    CaptureMouse();
                }
            };

            MouseLeftButtonUp += (s, e) => 
            {
                if (_isDragging)  // 只有在拖动状态下才处理
                {
                    _isDragging = false;
                    ReleaseMouseCapture();
                    _configManager.SaveWindowPosition(Left, Top);
                }
            };

            MouseMove += (s, e) =>
            {
                if (_isDragging)
                {
                    var position = e.GetPosition(this);
                    var newLeft = Left + position.X - _lastPosition.X;
                    var newTop = Top + position.Y - _lastPosition.Y;

                    // 确保窗口不会移出屏幕
                    var screenWidth = SystemParameters.PrimaryScreenWidth;
                    var screenHeight = SystemParameters.PrimaryScreenHeight;
                    
                    newLeft = Math.Max(0, Math.Min(newLeft, screenWidth - Width));
                    newTop = Math.Max(0, Math.Min(newTop, screenHeight - Height));

                    Left = newLeft;
                    Top = newTop;
                }
            };

            // 添加鼠标进入离开事件
            MouseEnter += (s, e) =>
            {
                _isMouseOver = true;
                mainBorder.Opacity = 0.8;
                _opacityTimer.Stop();
            };

            MouseLeave += (s, e) =>
            {
                _isMouseOver = false;
                _opacityTimer.Start();
            };

            // 添加文本变化事件
            inputTextBox.TextChanged += (s, e) =>
            {
                _autoSaveTimer.Stop();
                _autoSaveTimer.Start();
            };
        }

        private void RegisterGlobalHotKey()
        {
            var helper = new HotKeyHelper(this);
            helper.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Shift, Key.S, () =>
            {
                if (Visibility == Visibility.Visible)
                    Hide();
                else
                    Show();
            });
        }

        private void AutoSave(object sender, EventArgs e)
        {
            _autoSaveTimer.Stop();
            SaveText();
        }

        public void SaveText()
        {
            var textRange = new TextRange(inputTextBox.Document.ContentStart, inputTextBox.Document.ContentEnd);
            _fileManager.SaveText(textRange);
        }

        private void ExportText()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "富文本文件|*.rtf",
                Title = "导出文本",
                FileName = "我的笔记.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                var textRange = new TextRange(inputTextBox.Document.ContentStart, inputTextBox.Document.ContentEnd);
                _fileManager.ExportText(dialog.FileName, textRange);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterGlobalHotKey();
        }

        private void DragBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == dragBorder || e.Source is Grid)  // 允许通过Grid拖动
            {
                _isDragging = true;
                _lastPosition = e.GetPosition(this);
                CaptureMouse();
                e.Handled = true;
            }
        }

        private void InputTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            inputTextBox.Focus();
            e.Handled = true;  // 阻止事件冒泡
        }

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                ToggleBold();
                e.Handled = true;
            }
        }

        private void ToggleBold()
        {
            var selection = inputTextBox.Selection;
            if (selection.Start != selection.End)
            {
                var textRange = new TextRange(selection.Start, selection.End);
                var currentWeight = textRange.GetPropertyValue(TextElement.FontWeightProperty);
                
                if (currentWeight.Equals(FontWeights.Bold))
                    textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                else
                    textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }

        private void InputTextBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;
            var scrollViewer = FindChild<ScrollViewer>(richTextBox);
            if (scrollViewer != null)
            {
                if (e.Delta < 0)
                    scrollViewer.LineDown();
                else
                    scrollViewer.LineUp();
                e.Handled = true;
            }
        }

        // 辅助方法：查找控件
        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    return found;
                
                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                // 保存文本
                SaveText();
                
                // 视觉反馈
                button.Background = new SolidColorBrush(Color.FromRgb(204, 204, 204));
                button.Foreground = new SolidColorBrush(Colors.White);
                
                // 1秒后恢复原样
                await Task.Delay(1000);
                button.Background = new SolidColorBrush(Colors.Transparent);
                button.Foreground = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                // 视觉反馈 - 开始
                button.Background = new SolidColorBrush(Color.FromRgb(204, 204, 204));
                button.Foreground = new SolidColorBrush(Colors.White);

                // 导出操作
                ExportText();

                // 视觉反馈 - 结束
                await Task.Delay(1000);
                button.Background = new SolidColorBrush(Colors.Transparent);
                button.Foreground = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            }
        }

        private void DragHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _lastPosition = e.GetPosition(this);
            CaptureMouse();
            e.Handled = true;
        }

        public string GetRichTextBoxText()
        {
            var textRange = new TextRange(inputTextBox.Document.ContentStart, inputTextBox.Document.ContentEnd);
            return textRange.Text;
        }

        public void SetRichTextBoxText(string text)
        {
            var textRange = new TextRange(inputTextBox.Document.ContentStart, inputTextBox.Document.ContentEnd);
            textRange.Text = text;
        }

        private void ThemeToggle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDarkTheme = !_isDarkTheme;
            ApplyTheme(_isDarkTheme);
            _configManager.SaveThemeConfig(_isDarkTheme);
        }

        private void ApplyTheme(bool isDark)
        {
            var background = isDark ? _darkBackground : _lightBackground;
            var text = isDark ? _darkText : _lightText;
            var border = isDark ? _darkBorder : _lightBorder;

            // 更新背景
            var rectangle = (Rectangle)mainBorder.FindName("backgroundRect");
            rectangle.Fill = background;

            // 更新文本颜色
            inputTextBox.Foreground = text;

            // 更新边框颜色
            mainBorder.BorderBrush = border;
            foreach (var button in FindVisualChildren<Button>(this))
            {
                button.BorderBrush = border;
                button.Foreground = text;
            }
            dragHandle.BorderBrush = border;
            themeToggle.BorderBrush = border;
            themeToggle.Background = isDark ? border : new SolidColorBrush(Colors.Transparent);
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    yield return found;
                
                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
} 