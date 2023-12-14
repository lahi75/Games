using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace MonkeyMadness
{    
    #region application settings 
    /// <summary>
    /// ctor
    /// </summary>
    public class MMSettings
    {
        /// <summary>
        /// game difficulty
        /// </summary>
        public enum DifficultyType
        {
            easy,
            medium,
            hard
        }

        /// <summary>
        /// Player name
        /// </summary>
        public string Playername { get; set; }

        /// <summary>
        /// access the game difficulty
        /// </summary> 
        public DifficultyType Difficulty { get; set; }

        /// <summary>
        /// access the music option
        /// </summary>
        public bool Music { get; set; }

        /// <summary>
        /// enable / disable sound FX
        /// </summary>
        public bool Fx { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public MMSettings()
        {
            // Create our default settings
            Difficulty = DifficultyType.easy;
            Music = true;
            Fx = true;
            Playername = "";
        }
    }
    #endregion

    #region settings manager

    /// <summary>
    /// static class to acccess it from everywhere
    /// </summary>
    static class SettingsManager
    {
        private static string fileName = "settings.xml";

        /// <summary>
        /// create the one and only instance
        /// </summary>
        public static MMSettings Settings = new MMSettings();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings()
        {

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Settings = new MMSettings();

            //Obtain a virtual store for application

#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

            // Check if file is there
            if (fileStorage.FileExists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(Settings.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, fileStorage));

                try
                {
                    Settings = (MMSettings)serializer.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Settings = new MMSettings();

                    // Saving is optional - here we assume it works and the error is due to the file not being there.
                    SaveSettings();                    
                }
            }

            else
            {
                SaveSettings();
            }
        }

        public static void SaveSettings()
        {
            //Obtain a virtual store for application
#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif
            XmlSerializer serializer = new XmlSerializer(Settings.GetType());

            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, fileStorage));

            try
            {
                serializer.Serialize(stream, Settings);
            }
            catch
            {
             
            }

            stream.Close();
        }
    }

#endregion
}