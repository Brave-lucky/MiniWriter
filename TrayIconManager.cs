using System.Windows.Forms;
using System.Drawing;
using System;
using System.IO;

namespace FloatingNotepad
{
    public class TrayIconManager
    {
        private readonly NotifyIcon _trayIcon;
        private readonly MainWindow _mainWindow;

        public TrayIconManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _trayIcon = new NotifyIcon
            {
                Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "app.ico")),
                Visible = true
            };

            InitializeContextMenu();
            InitializeEvents();
        }

        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            
            var showItem = new ToolStripMenuItem("显示");
            showItem.Click += (s, e) => _mainWindow.Show();
            
            var exitItem = new ToolStripMenuItem("退出");
            exitItem.Click += (s, e) => System.Windows.Forms.Application.Exit();

            contextMenu.Items.Add(showItem);
            contextMenu.Items.Add(exitItem);

            _trayIcon.ContextMenuStrip = contextMenu;
        }

        private void InitializeEvents()
        {
            _trayIcon.DoubleClick += (s, e) => _mainWindow.Show();
        }

        public void Dispose()
        {
            _trayIcon.Dispose();
        }
    }
} 