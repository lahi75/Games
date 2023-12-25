using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace LLGameLibrary
{
    #region application settings 
    /// <summary>
    /// ctor
    /// </summary>
    public class LLSettings
    {
        public enum Difficulty
        {
            novice = 0,
            advanced = 1,
            expert = 2,
            master = 3
        }

        public enum Mode
        {
            freeStyle,
            ladder
        }

        /// <summary>
        /// Player name
        /// </summary>
        public string Playername { get; set; }
        
        public Mode GameMode { get; set; }
       
        /// <summary>
        /// access the music option
        /// </summary>
        public bool Music { get; set; }

        /// <summary>
        /// enable / disable sound FX
        /// </summary>
        public bool Fx { get; set; }

        /// <summary>
        /// get the current selected difficulty
        /// </summary>
        public Difficulty LevelStage { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public LLSettings()
        {
            // Create our default settings            
            Music = true;
            Fx = true;
            Playername = "";
            LevelStage = Difficulty.novice;
            GameMode = Mode.ladder;
        }
    }
    #endregion

    #region level settings
    
    /// <summary>
    /// a single level object
    /// </summary>
    public class LLLevelStorage
    {                        
        /// <summary>
        /// check if the level was unlocked
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// get the points made in this level
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public LLLevelStorage()
        {
            Points = 0;
            IsUnlocked = false;
        }

        /// <summary>
        /// ctor for deserialization
        /// </summary>        
        public LLLevelStorage(LLLevelStorage serializedObject)            
        {
            this.IsUnlocked = serializedObject.IsUnlocked;
            this.Points = serializedObject.Points;
        }        
    }
    
    /// <summary>
    /// storage class for all levels
    /// </summary>
    public class LLLevelStages
    {     
        private const int _maxLevel = 10;
        private const int _currentCap = 10; // currently just 10 levels are used

        public List<LLLevelStorage> Novice = new List<LLLevelStorage>();
        public List<LLLevelStorage> Advanced = new List<LLLevelStorage>();
        public List<LLLevelStorage> Expert = new List<LLLevelStorage>();
        public List<LLLevelStorage> Master = new List<LLLevelStorage>();
                        
        /// <summary>
        /// empty ctor for xml serialization
        /// </summary>
        public LLLevelStages()
        {                    
        }

        /// <summary>
        /// return the sum of all points per given stage
        /// </summary>        
        public Boolean IsActive(LLSettings.Difficulty diff)
        {
            switch (diff)
            {
                case LLSettings.Difficulty.novice:
                    foreach (LLLevelStorage l in Novice)
                        if (l.IsUnlocked)
                            return true;
                    break;
                case LLSettings.Difficulty.advanced:
                    foreach (LLLevelStorage l in Advanced)
                        if (l.IsUnlocked)
                            return true;
                    break;
                case LLSettings.Difficulty.master:
                    foreach (LLLevelStorage l in Master)
                        if (l.IsUnlocked)
                            return true;
                    break;
                case LLSettings.Difficulty.expert:
                    foreach (LLLevelStorage l in Expert)
                        if (l.IsUnlocked)
                            return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// return the sum of all points per given stage
        /// </summary>        
        public Int32 TotalPoints(LLSettings.Difficulty diff)
        {
            Int32 points = 0;
            switch (diff)
            {
                case LLSettings.Difficulty.novice:
                    foreach (LLLevelStorage l in Novice)
                        points += l.Points;
                    break;
                case LLSettings.Difficulty.advanced:
                    foreach (LLLevelStorage l in Advanced)
                        points += l.Points;
                    break;
                case LLSettings.Difficulty.master:
                    foreach (LLLevelStorage l in Master)
                        points += l.Points;
                    break;
                case LLSettings.Difficulty.expert:
                    foreach (LLLevelStorage l in Expert)
                        points += l.Points;
                    break;
            }

            return points;
        }

        /// <summary>
        /// how many levels are available per stage
        /// </summary>
        public Int32 MaxLevel
        {
            get
            {
                return _currentCap;
            }
        }

        /// <summary>
        /// initialize the obejcts just in case deserialization failed
        /// </summary>
        public void Create()
        {
            Novice.Clear();
            Advanced.Clear();
            Expert.Clear();
            Master.Clear();

            for (int i = 0; i < _maxLevel; i++)
            {
                Novice.Add(new LLLevelStorage());
                Advanced.Add(new LLLevelStorage());
                Expert.Add(new LLLevelStorage());
                Master.Add(new LLLevelStorage());
            }

            Reset(LLSettings.Difficulty.novice);
            Reset(LLSettings.Difficulty.advanced);
            Reset(LLSettings.Difficulty.expert);
            Reset(LLSettings.Difficulty.master);
        }

        /// <summary>
        /// resets the achievements per given stage
        /// </summary>        
        public void Reset(LLSettings.Difficulty diff)
        {
            switch (diff)
            {
                case LLSettings.Difficulty.novice:
                    for (int i = 0; i < _maxLevel; i++)
                    {
                        Novice[i].IsUnlocked = i == 0 ? true : false; // level 1 is always unlocked
                        Novice[i].Points = 0;
                    }
                    break;
                case LLSettings.Difficulty.advanced:
                    for (int i = 0; i < _maxLevel; i++)
                    {
                        Advanced[i].IsUnlocked = i == 0 ? true : false; // level 1 is always unlocked
                        Advanced[i].Points = 0;
                    }
                    break;
                case LLSettings.Difficulty.expert:
                    for (int i = 0; i < _maxLevel; i++)
                    {
                        Expert[i].IsUnlocked = i == 0 ? true : false; // level 1 is always unlocked
                        Expert[i].Points = 0;
                    }
                    break;
                case LLSettings.Difficulty.master:
                    for (int i = 0; i < _maxLevel; i++)
                    {
                        Master[i].IsUnlocked = i == 0 ? true : false; // level 1 is always unlocked
                        Master[i].Points = 0;
                    }
                    break;
            }
        }
            
        /// <summary>
        /// ctor the deserialize the object 
        /// </summary>        
        public LLLevelStages(LLLevelStages serializedObject)
        {
            for (int i = 0; i < _maxLevel; i++)
            {                
                Novice.Add(serializedObject.Novice[i]);
                Advanced.Add(serializedObject.Advanced[i]);
                Expert.Add(serializedObject.Expert[i]);
                Master.Add(serializedObject.Master[i]);
            }                          
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
        public static LLSettings Settings = new LLSettings();        

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings()
        {

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Settings = new LLSettings();            

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
                    Settings = (LLSettings)serializer.Deserialize(stream);                    
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Settings = new LLSettings();                    

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

    #region level manager

    /// <summary>
    /// static class to acccess it from everywhere
    /// </summary>
    static class LevelManager
    {
        private static string fileName = "level.xml";

        /// <summary>
        /// create the one and only instance
        /// </summary>
        public static LLLevelStages Level = new LLLevelStages();        

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings()
        {
            // Create our exposed settings class. This class gets serialized to load/save the settings.    
            Level = new LLLevelStages();            

            //Obtain a virtual store for application
#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

            // Check if file is there
            if (fileStorage.FileExists(fileName))
            {

                XmlSerializer serializer = new XmlSerializer(Level.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, fileStorage));

                try
                {                 
                    Level = (LLLevelStages)serializer.Deserialize(stream);                    
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();                    
                    Level = new LLLevelStages();
                    Level.Create();
                    // Saving is optional - here we assume it works and the error is due to the file not being there.
                    SaveSettings();
                }
            }

            else
            {
                Level.Create();
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
            XmlSerializer serializer = new XmlSerializer(Level.GetType());

            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, fileStorage));

            try
            {                
                serializer.Serialize(stream, Level);                
            }
            catch
            {

            }
            stream.Close();
        }
    }

    #endregion
}

