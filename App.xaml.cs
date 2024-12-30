using System.Windows;

namespace FloatingNotepad
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            if (MainWindow is MainWindow mainWindow)
            {
                mainWindow.SaveText();
            }
            base.OnExit(e);
        }
    }
} 