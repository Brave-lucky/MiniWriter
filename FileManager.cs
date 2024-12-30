using System;
using System.IO;
using System.Windows.Documents;

namespace FloatingNotepad
{
    public class FileManager
    {
        private readonly string _notesPath;
        private const string NOTES_FILENAME = "notes.rtf";

        public FileManager()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MyNoteApp"
            );
            
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _notesPath = Path.Combine(appDataPath, NOTES_FILENAME);
        }

        public void SaveText(TextRange textRange)
        {
            using (FileStream fs = new FileStream(_notesPath, FileMode.Create))
            {
                textRange.Save(fs, System.Windows.DataFormats.Rtf);
            }
        }

        public void LoadText(TextRange textRange)
        {
            if (File.Exists(_notesPath))
            {
                using (FileStream fs = new FileStream(_notesPath, FileMode.Open))
                {
                    textRange.Load(fs, System.Windows.DataFormats.Rtf);
                }
            }
        }

        public void ExportText(string path, TextRange textRange)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                textRange.Save(fs, System.Windows.DataFormats.Rtf);
            }
        }
    }
} 