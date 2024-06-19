using System;

namespace Blocks
{
    public class FileManager
    {

        private static Lazy<FileManager> instance = new Lazy<FileManager>(() => new FileManager());
        public static FileManager Instance { get { return instance.Value; } }


        const string SETTINGS_FILE = "settings.txt";

        public void SaveSettings(Level level)
        {
            using System.IO.StreamWriter file = new System.IO.StreamWriter(SETTINGS_FILE, false);
            file.WriteLine(level);
        }

        public Level LoadSettings()
        {
            if (!System.IO.File.Exists(SETTINGS_FILE))
            {
                return Level.Easy;
            }

            using System.IO.StreamReader file = new System.IO.StreamReader(SETTINGS_FILE);

            string line = file.ReadLine();
            return (Level)Enum.Parse(typeof(Level), line);
        }

    }


}
