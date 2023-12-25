using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using TrainTrouble.Properties;

namespace TGGameLibrary
{
    public class TGLevelStorage
    {
        public TGLevelStorage()
        {            
        }

        public TGLevelStorage(ContentManager content, Int32 level)
        {
            _level = level;

            _levelPath = string.Format("Content/levels/{0}.txt", level);

            switch (_level)
            {
                case 0:
                    _iconUnlocked = content.Load<Texture2D>("level_icons/0_unlocked");
                    _iconLocked = content.Load<Texture2D>("level_icons/0_locked");
                    break;
                case 1:
                    _iconUnlocked = content.Load<Texture2D>("level_icons/1_unlocked");
                    _iconLocked = content.Load<Texture2D>("level_icons/1_locked");
                    break;
                case 2:
                    _iconUnlocked = content.Load<Texture2D>("level_icons/2_unlocked");
                    _iconLocked = content.Load<Texture2D>("level_icons/2_locked");
                    break;
                case 3:
                    _iconUnlocked = content.Load<Texture2D>("level_icons/3_unlocked");
                    _iconLocked = content.Load<Texture2D>("level_icons/3_locked");
                    break;
                case 4:
                    _iconUnlocked = content.Load<Texture2D>("level_icons/4_unlocked");
                    _iconLocked = content.Load<Texture2D>("level_icons/4_locked");
                    break;                
            }
        }

        public TGLevelStorage(ContentManager content, Int32 level, TGLevelStorage serializedObject)
            : this(content, level)
        {
            this.IsUnlocked = serializedObject.IsUnlocked;
        }

        [XmlIgnoreAttribute]
        public String LevelPath
        {
            get
            {
                return _levelPath;
            }
        }

        [XmlIgnoreAttribute]
        public Texture2D Image
        {
            get
            {
                if (IsUnlocked)
                    return _iconUnlocked;
                else
                    return _iconLocked;
            }
        }                

        /// <summary>
        /// checks in which stage we should be according to the number of transported passengers
        /// </summary>        
        public TGColor Passenger2Stage(Int32 passengers)
        {
            if (passengers <= _redCap)
                return TGColor.Red;

            if (passengers > _redCap && passengers <= _blueCap)
                return TGColor.Blue;

            return TGColor.Green;
        }

        /// <summary>
        /// returns true if more than green cap passengers are transported
        /// </summary>        
        public Boolean LevelCleared(Int32 passengers)
        {
            return passengers > _greenCap;
        }

        public void SetRedCap(Int32 cap, Int32 spawnLikelyhood)
        {
            _redCap = cap;
            _redLikelyhood = spawnLikelyhood;
        }
        
        public void SetBlueCap(Int32 cap, Int32 spawnLikelyhood)
        {
            _blueCap = cap;
            _blueLikelyhood = spawnLikelyhood;
        }

        public void SetGreenCap(Int32 cap, Int32 spawnLikelyhood)
        {
            _greenCap = cap;
            _greenLikelyhood = spawnLikelyhood;
        }

        public Int32 AngryTime
        {
            get
            {
                return _angryTime;
            }
            set
            {
                _angryTime = value;
            }

        }

        [XmlIgnoreAttribute]
        public String LevelName
        {
            get
            {
                return _levelName;
            }
            set
            {
                _levelName = value;
            }
        }

        [XmlIgnoreAttribute]
        public Int32 Stops
        {
            get
            {
                return _stops;
            }
            set
            {
                _stops = value;
            }
        }

        [XmlIgnoreAttribute]
        public Int32 Boosts
        {
            get
            {
                return _boosts;
            }
            set
            {
                _boosts = value;
            }
        }        

        public bool IsUnlocked { get; set; }

        public void Unlock()
        {
            IsUnlocked = true;         
        }
        
        public Int32 SpawnLikelyHood( TGColor color ) 
        {
            switch (color)
            {
                default:
                case TGColor.Red:
                    return _redLikelyhood;
                case TGColor.Blue:
                    return _blueLikelyhood;
                case TGColor.Green:
                    return _greenLikelyhood;
            }
        }

        Int32 _level;
        Texture2D _iconLocked;            // texture to draw the award
        Texture2D _iconUnlocked;          // texture to draw the award
        String _levelPath;

        Int32 _redLikelyhood = 15;
        Int32 _greenLikelyhood = 15;
        Int32 _blueLikelyhood = 15;

        Int32 _redCap = 5;
        Int32 _blueCap = 10;
        Int32 _greenCap = 15;
        Int32 _angryTime = 30;
        String _levelName;
        Int32 _stops = 0;
        Int32 _boosts = 0;
    }


    public class TGLevelStorageList
    {
        public TGLevelStorage Level0 { get; set; }
        public TGLevelStorage Level1 { get; set; }
        public TGLevelStorage Level2 { get; set; }
        public TGLevelStorage Level3 { get; set; }
        public TGLevelStorage Level4 { get; set; }

        List<TGLevelStorage> _levels = new List<TGLevelStorage>();

        public TGLevelStorageList()
        {
            //default ctor is required for xml serialization
        }

        public TGLevelStorage GetLevel(Int32 level)
        {
            return _levels[level];
        }

        public Int32 Count
        {
            get
            {
                return _levels.Count;
            }
        }

        /// <summary>
        /// for test purposes make the levels resetable
        /// </summary>
        public void Reset()
        {
            foreach (TGLevelStorage l in _levels)
                l.IsUnlocked = false;
        }

        public TGLevelStorageList(ContentManager content)
        {
            Level0 = new TGLevelStorage(content, 0);
            Level1 = new TGLevelStorage(content, 1);
            Level2 = new TGLevelStorage(content, 2);
            Level3 = new TGLevelStorage(content, 3);
            Level4 = new TGLevelStorage(content, 4);

            Level0.LevelName = Resources.level1Name;
            Level1.LevelName = Resources.level2Name;
            Level2.LevelName = Resources.level3Name;
            Level3.LevelName = Resources.level4Name;
            Level4.LevelName = Resources.level5Name;

            _levels.Add(Level0);
            _levels.Add(Level1);
            _levels.Add(Level2);
            _levels.Add(Level3);
            _levels.Add(Level4);
        }

        public TGLevelStorageList(ContentManager content, TGLevelStorageList serializedObject)
        {
            Level0 = new TGLevelStorage(content, 0, serializedObject.Level0);
            Level1 = new TGLevelStorage(content, 1, serializedObject.Level1);
            Level2 = new TGLevelStorage(content, 2, serializedObject.Level2);
            Level3 = new TGLevelStorage(content, 3, serializedObject.Level3);
            Level4 = new TGLevelStorage(content, 4, serializedObject.Level4);

            Level0.LevelName = Resources.level1Name;
            Level1.LevelName = Resources.level2Name;
            Level2.LevelName = Resources.level3Name;
            Level3.LevelName = Resources.level4Name;
            Level4.LevelName = Resources.level5Name;

            _levels.Add(Level0);
            _levels.Add(Level1);
            _levels.Add(Level2);
            _levels.Add(Level3);
            _levels.Add(Level4);
        }
    }

    static class LevelManager
    {
        private static string fileName = "levels.xml";

        /// <summary>
        /// create the one and only instance
        /// </summary>
        public static TGLevelStorageList Levels = new TGLevelStorageList();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings(IServiceProvider serviceProvider)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Levels = new TGLevelStorageList(content);

            //Obtain a virtual store for application

#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

            // Check if file is there
            if (fileStorage.FileExists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(Levels.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, fileStorage));

                try
                {
                    Levels = new TGLevelStorageList(content, (TGLevelStorageList)serializer.Deserialize(stream));
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Levels = new TGLevelStorageList(content);

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
            XmlSerializer serializer = new XmlSerializer(Levels.GetType());

            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, fileStorage));

            try
            {
                serializer.Serialize(stream, Levels);
            }
            catch
            {

            }

            stream.Close();
        }
    }
}
