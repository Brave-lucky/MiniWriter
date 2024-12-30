using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace MiniWriter
{
    public class ConfigManager
    {
        private readonly string _configPath;
        private const string CONFIG_FILENAME = "config.json";

        public ConfigManager()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MiniWriter"
            );
            
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _configPath = Path.Combine(appDataPath, CONFIG_FILENAME);
        }

        public Point LoadWindowPosition()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    var config = JsonSerializer.Deserialize<WindowConfig>(json);
                    return new Point(config.X, config.Y);
                }
                catch
                {
                    return new Point(100, 100);
                }
            }
            return new Point(100, 100);
        }

        public void SaveWindowPosition(double x, double y)
        {
            var config = new WindowConfig { X = x, Y = y };
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(_configPath, json);
        }

        public bool LoadThemeConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    var config = JsonSerializer.Deserialize<WindowConfig>(json);
                    return config.IsDarkTheme;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public void SaveThemeConfig(bool isDarkTheme)
        {
            var config = new WindowConfig
            {
                X = LoadWindowPosition().X,
                Y = LoadWindowPosition().Y,
                IsDarkTheme = isDarkTheme
            };
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(_configPath, json);
        }

        private class WindowConfig
        {
            public double X { get; set; }
            public double Y { get; set; }
            public bool IsDarkTheme { get; set; }
        }
    }
} 