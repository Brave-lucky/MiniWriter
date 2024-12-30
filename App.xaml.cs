using System.Windows;

namespace MiniWriter
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