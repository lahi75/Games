using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SharpDX.DXGI;
using static MonkeyMadness.MMAchievement;

namespace MonkeyMadness
{
    class MMAchievementMonitor
    {
        public MMAchievementMonitor()
        {
        }

        /// <summary>
        /// resets everything at the beginning of a new game
        /// </summary>
        public void ResetGame()
        {
            _archievementMonsterRemoved = 0;
            ResetLevel();
        }

        /// <summary>
        /// resets variables when we proceed to a new level
        /// </summary> 
        public void ResetLevel()
        {
            _archievementMonsterCrashed = 0;
            _jumpRight = false;
            _pleasant = true;
            _collectedBonuses = 0;            
        }

        public void JumpRight()
        {
            _jumpRight = true;
        }

        public void MonsterRemoved()
        {
            if (AchievementsManager.Achievements.AchievementGhostBuster.IsUnlocked)
                return;

            _archievementMonsterRemoved++;

            if (_archievementMonsterRemoved == _monsterRemoveAward)
                AchievementsManager.Achievements.AchievementGhostBuster.Unlock();
        }

        public void MonsterCrash()
        {
            if (AchievementsManager.Achievements.AchievementTumbler.IsUnlocked)
                return;

            _archievementMonsterCrashed++;

            if (_archievementMonsterCrashed == _monsterCrashedAward)
                AchievementsManager.Achievements.AchievementTumbler.Unlock();
        }

        public void BonusCollect(MMBonus.BonusType bonus)
        {
            _collectedBonuses |= (int)bonus;

            if (AchievementsManager.Achievements.AchievementCheater.IsUnlocked)
                return;

            int a = (int)MMBonus.BonusType.Amor | (int)MMBonus.BonusType.Drill | (int)MMBonus.BonusType.Life | (int)MMBonus.BonusType.Parachute | (int)MMBonus.BonusType.Poison | (int)MMBonus.BonusType.Wings | (int)MMBonus.BonusType.Teleport;

            if (_collectedBonuses == a )
                AchievementsManager.Achievements.AchievementCheater.Unlock();
            
        }                

        public void ResetMonsterCrash()
        {
            _archievementMonsterCrashed = 0;
        }             

        public void LuckyDevilFall()
        {
            _linesFallen++;
        }

        public void LuckyDevilRecover(int maxLine, int recoverLine)
        {
            if (AchievementsManager.Achievements.AchievementLuckyDevil.IsUnlocked)
                return;

            if (recoverLine == maxLine - 1)
            {
                if (_linesFallen >= _fallLines)
                {
                    AchievementsManager.Achievements.AchievementLuckyDevil.Unlock();
                }
            }

            _linesFallen = 0;
        }

        public void SuperMonkey(MMSettings.DifficultyType difficulty, int level)
        {
            if (AchievementsManager.Achievements.AchievementSuper.IsUnlocked)
                return;

            if( level+1 == _superMonkey && difficulty == MMSettings.DifficultyType.hard)
                AchievementsManager.Achievements.AchievementSuper.Unlock();
        }

        public void FallLine()
        {
            if (AchievementsManager.Achievements.AchievementIcarus.IsUnlocked)
                return;

            _icarusFallen++;

            if (_icarusFallen == _fallIcarus)
            {
                AchievementsManager.Achievements.AchievementIcarus.Unlock();
            }
        }

        public void IcarusRecover()
        {
            _icarusFallen = 0;
        }

        public void StalkerCrash()
        {
            _stalkerCrash++;

            if (AchievementsManager.Achievements.AchievementStalker.IsUnlocked)
                return;

            if( _stalkerCrash == _maxStalkerCrash )
                AchievementsManager.Achievements.AchievementStalker.Unlock();
        }

        public void StalkerCrashRecover()
        {
            _stalkerCrash = 0;
        }

        public void CheckPleasant()
        {
            if (_pleasant)
            {
                if (AchievementsManager.Achievements.AchievementPleasant.IsUnlocked)
                    return;

                AchievementsManager.Achievements.AchievementPleasant.Unlock();
            }
        }

        public void UnPleasant()
        {
            _pleasant = false;
        }

        public void CheckLefty()
        {
            if (_jumpRight == false)
            {
                if (AchievementsManager.Achievements.AchievementLefty.IsUnlocked)
                    return;

                AchievementsManager.Achievements.AchievementLefty.Unlock();
            }
        }

        public void Score(int score)
        {
            if (score  < 100)
            {
                if (AchievementsManager.Achievements.AchievementNewbie.IsUnlocked)
                    return;

                AchievementsManager.Achievements.AchievementNewbie.Unlock();
            }
        }

        public void UpperLineDrilled()
        {
            if (AchievementsManager.Achievements.AchievementDrillerKiller.IsUnlocked)
                return;

            AchievementsManager.Achievements.AchievementDrillerKiller.Unlock();
        }

        public void LevelTime(Int32 seconds)
        {
            if (seconds < _speedyTime)
            {
                if (AchievementsManager.Achievements.AchievementSpeedyGonzales.IsUnlocked)
                    return;

                AchievementsManager.Achievements.AchievementSpeedyGonzales.Unlock();
            }            
        }

        // remove 3 monster in the whole game
        const Int32 _monsterRemoveAward = 3;
        // recover from 3 monster crashes
        const Int32 _monsterCrashedAward = 3;
        // run through a level in 30 seconds
        const Int32 _speedyTime = 30;
        //fall that many lines and recover on the last line
        const Int32 _fallLines = 2;
        // fall N lines without getting up
        const Int32 _fallIcarus = 4;
        // get stunned by 5 monsters in a row
        const Int32 _maxStalkerCrash = 3;
        // reach level at difficulty hard
        const Int32 _superMonkey = 5;

        Int32 _archievementMonsterRemoved = 0;
        Int32 _archievementMonsterCrashed = 0;
        
        Int32 _linesFallen = 0;
        Int32 _icarusFallen = 0;
        Int32 _collectedBonuses = 0;
        bool _jumpRight = false;
        bool _pleasant = true;
        Int32 _stalkerCrash = 0;       
    }

    static class MonitorManager
    {
        public static MMAchievementMonitor Monitor = new MMAchievementMonitor();
    }

    public class MMAchievement
    {
        public enum Achievement
        {
            DrillerKiller,
            GhostBuster,
            Newbie,
            SpeedyGonzales,
            LuckyDevil,
            Tumbler,
            Lefty,
            Pleasant,
            Icarus,
            Cheater,
            Stalker,
            Super
        }

        [XmlIgnoreAttribute]
        public String Name 
        {
            get
            {
                switch (_achievment)
                {
                    default:
                    case Achievement.DrillerKiller:
                        return Properties.Resources.AchievementDrillerKiller;
                    case Achievement.GhostBuster:
                        return Properties.Resources.AchievementGhostBuster;
                    case Achievement.Newbie:
                        return Properties.Resources.AchievementNewbie;
                    case Achievement.SpeedyGonzales:
                        return Properties.Resources.AchievementSpeedyGonzales;
                    case Achievement.LuckyDevil:
                        return Properties.Resources.AchievmentLuckyDevil;
                    case Achievement.Tumbler:
                        return Properties.Resources.AchievementTumbler;
                    case Achievement.Lefty:
                        return Properties.Resources.AchievementLefty;
                    case Achievement.Pleasant:
                        return Properties.Resources.AchievmentPleasantJourney;
                    case Achievement.Icarus:
                        return Properties.Resources.AchievementIcarus;
                    case Achievement.Cheater:
                        return Properties.Resources.AchievementCheater;
                    case Achievement.Stalker:
                        return Properties.Resources.AchievementStalker;
                    case Achievement.Super:
                        return Properties.Resources.AchievementSuper;
                }
            }

        }

        [XmlIgnoreAttribute]
        public String Description 
        { 
            get
            {
            switch (_achievment)
            {
                default:
                case Achievement.DrillerKiller:
                    return Properties.Resources.AchievementDrillerKillerDescription;
                case Achievement.GhostBuster:
                    return Properties.Resources.AchievementGhostBusterDescription;
                case Achievement.Newbie:
                    return Properties.Resources.AchievementNewbieDescription;
                case Achievement.SpeedyGonzales:
                    return Properties.Resources.AchievementSpeedyGonzalesDescription;
                case Achievement.LuckyDevil:
                    return Properties.Resources.AchievementLuckyDevilDescription;
                case Achievement.Tumbler:
                    return Properties.Resources.AchievementTumblerDescription;
                case Achievement.Lefty:
                    return Properties.Resources.AchievementLeftyDescription;
                case Achievement.Pleasant:
                    return Properties.Resources.AchievmentPleasantJourneyDescription;
                case Achievement.Icarus:
                    return Properties.Resources.AchievementIcarusDescription;
                case Achievement.Cheater:
                    return Properties.Resources.AchievementCheaterDescription;
                case Achievement.Stalker:
                    return Properties.Resources.AchievementStalkerDescription;
                case Achievement.Super:
                    return Properties.Resources.AchievementSuperDescription;
            }

        }
    }

        public bool IsUnlocked { get; set; }

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

        public MMAchievement()
        {
            //default ctor is required for xml serialization                 
        }

        public MMAchievement(Game gameMain, Achievement archievement)
        {
            _achievment = archievement;            
            _gameMain = gameMain;

            switch (_achievment)
            {
                case Achievement.DrillerKiller:                    
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/driller_killer_unlocked");                    
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/driller_killer_locked");
                    break;
                case Achievement.GhostBuster:                    
                        _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/ghost_buster_unlocked");                    
                        _iconLocked = _gameMain.Content.Load<Texture2D>("awards/ghost_buster_locked");
                    break;
                case Achievement.Newbie:
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/newbie_unlocked");
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/newbie_locked");
                    break;
                case Achievement.SpeedyGonzales:
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/speedy_unlocked");
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/speedy_locked");
                    break;
                case Achievement.LuckyDevil:
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/lucky_unlocked");
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/lucky_locked");
                    break;
                case Achievement.Tumbler:
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/tumbler_unlocked");
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/tumbler_locked");
                    break;   
                case Achievement.Lefty:
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/lefty_unlocked");
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/lefty_locked");
                    break;   
                case Achievement.Pleasant:
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/pleasant_locked");
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/pleasant_unlocked");
                    break;
                case Achievement.Icarus:
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/icarus_locked");
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/icarus_unlocked");
                    break;
                case Achievement.Cheater:
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/cheater_locked");
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/cheater_unlocked");
                    break;
                case Achievement.Stalker:
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/stalker_locked");
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/stalker_unlocked");
                    break;
                case Achievement.Super:
                    _iconLocked = _gameMain.Content.Load<Texture2D>("awards/super_locked");
                    _iconUnlocked = _gameMain.Content.Load<Texture2D>("awards/super_unlocked");
                    break;

            }
        }    
    
        public MMAchievement(Game gameMain,  Achievement archievement, MMAchievement serializedObject)
            :this(gameMain, archievement)
        {
            this.IsUnlocked = serializedObject.IsUnlocked;
        }     

        public void Unlock()
        {
            IsUnlocked = true;

            MMFxManager.Fx.PlayAwardNoise();

            // 5 seconds showtime of the award
            _showTime = 5;
        }

        public void Update(GameTime time)
        {
            if (_showTime > 0)
            {
                if (_currendSecond != (Int32)time.TotalGameTime.TotalSeconds)
                {
                    _showTime--;
                    _currendSecond = (Int32)time.TotalGameTime.TotalSeconds;
                }
            }
        }        

        public bool Draw(SpriteBatch spriteBatch, Vector2 position, bool showAlways)
        {
            if (_showTime > 0 || showAlways)
            {
                spriteBatch.Draw(Image, new Rectangle((int)position.X, (int)position.Y, Image.Width, Image.Height), Color.White);
            }

            // return a true if the award is shown right after unlocking
            if (_showTime > 0)
                return true;

            return false;
        }
        
        Int32 _showTime = 0;
        Int32 _currendSecond = 0;
        Game _gameMain;
        Achievement _achievment;
        Texture2D _iconLocked;            // texture to draw the arward
        Texture2D _iconUnlocked;          // texture to draw the arward
    }

    public class MMAchievements
    {
        public MMAchievement AchievementGhostBuster { get; set; }
        public MMAchievement AchievementDrillerKiller { get; set; }
        public MMAchievement AchievementNewbie { get; set; }
        public MMAchievement AchievementSpeedyGonzales { get; set; }
        public MMAchievement AchievementLuckyDevil { get; set; }
        public MMAchievement AchievementTumbler { get; set; }
        public MMAchievement AchievementLefty { get; set; }
        public MMAchievement AchievementPleasant { get; set; }
        public MMAchievement AchievementIcarus { get; set; }
        public MMAchievement AchievementCheater { get; set; }
        public MMAchievement AchievementStalker { get; set; }
        public MMAchievement AchievementSuper { get; set; }

        List<MMAchievement> _achievements = new List<MMAchievement>();

        public MMAchievements()
        {
            //default ctor is required for xml serialization
        }

        public void Update(GameTime time)
        {
            foreach (MMAchievement a in _achievements)
                a.Update(time);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {            
            foreach (MMAchievement a in _achievements)
            {
                if( a.Draw(spriteBatch, position, false) )
                    position.X += 80;
            }
        }

        /// <summary>
        /// for test purposes make the awards resetable
        /// </summary>
        public void Reset()
        {
            foreach (MMAchievement a in _achievements)
                a.IsUnlocked = false;
        }

        public MMAchievements(Game gameMain)
        {
            AchievementGhostBuster = new MMAchievement(gameMain, MMAchievement.Achievement.GhostBuster);
            AchievementDrillerKiller = new MMAchievement(gameMain, MMAchievement.Achievement.DrillerKiller);
            AchievementNewbie = new MMAchievement(gameMain, MMAchievement.Achievement.Newbie);
            AchievementSpeedyGonzales = new MMAchievement(gameMain, MMAchievement.Achievement.SpeedyGonzales);
            AchievementLuckyDevil = new MMAchievement(gameMain, MMAchievement.Achievement.LuckyDevil);
            AchievementTumbler = new MMAchievement(gameMain, MMAchievement.Achievement.Tumbler);
            AchievementLefty = new MMAchievement(gameMain, MMAchievement.Achievement.Lefty);
            AchievementPleasant = new MMAchievement(gameMain, MMAchievement.Achievement.Pleasant);
            AchievementIcarus = new MMAchievement(gameMain, MMAchievement.Achievement.Icarus);
            AchievementCheater = new MMAchievement(gameMain, MMAchievement.Achievement.Cheater);
            AchievementStalker = new MMAchievement(gameMain, MMAchievement.Achievement.Stalker);
            AchievementSuper = new MMAchievement(gameMain, MMAchievement.Achievement.Super);

            _achievements.Add(AchievementGhostBuster);
            _achievements.Add(AchievementDrillerKiller);
            _achievements.Add(AchievementNewbie);
            _achievements.Add(AchievementSpeedyGonzales);
            _achievements.Add(AchievementLuckyDevil);
            _achievements.Add(AchievementTumbler);
            _achievements.Add(AchievementLefty);
            _achievements.Add(AchievementPleasant);
            _achievements.Add(AchievementIcarus);
            _achievements.Add(AchievementCheater);
            _achievements.Add(AchievementStalker);
            _achievements.Add(AchievementSuper);
        }

    public MMAchievements(Game gameMain, MMAchievements serializedObject)
        {
            AchievementGhostBuster = new MMAchievement(gameMain, MMAchievement.Achievement.GhostBuster, serializedObject.AchievementGhostBuster);
            AchievementDrillerKiller = new MMAchievement(gameMain, MMAchievement.Achievement.DrillerKiller, serializedObject.AchievementDrillerKiller);
            AchievementNewbie = new MMAchievement(gameMain, MMAchievement.Achievement.Newbie, serializedObject.AchievementNewbie);
            AchievementSpeedyGonzales = new MMAchievement(gameMain, MMAchievement.Achievement.SpeedyGonzales, serializedObject.AchievementSpeedyGonzales);
            AchievementLuckyDevil = new MMAchievement(gameMain, MMAchievement.Achievement.LuckyDevil, serializedObject.AchievementLuckyDevil);
            AchievementTumbler = new MMAchievement(gameMain, MMAchievement.Achievement.Tumbler, serializedObject.AchievementTumbler);
            AchievementLefty = new MMAchievement(gameMain, MMAchievement.Achievement.Lefty, serializedObject.AchievementLefty);
            AchievementPleasant = new MMAchievement(gameMain, MMAchievement.Achievement.Pleasant, serializedObject.AchievementPleasant);
            AchievementIcarus = new MMAchievement(gameMain, MMAchievement.Achievement.Icarus, serializedObject.AchievementIcarus);
            AchievementCheater = new MMAchievement(gameMain, MMAchievement.Achievement.Cheater, serializedObject.AchievementCheater);
            AchievementStalker = new MMAchievement(gameMain, MMAchievement.Achievement.Stalker, serializedObject.AchievementStalker);
            AchievementSuper = new MMAchievement(gameMain, MMAchievement.Achievement.Super, serializedObject.AchievementSuper);

            _achievements.Add(AchievementGhostBuster);
            _achievements.Add(AchievementDrillerKiller);
            _achievements.Add(AchievementNewbie);
            _achievements.Add(AchievementSpeedyGonzales);
            _achievements.Add(AchievementLuckyDevil);
            _achievements.Add(AchievementTumbler);
            _achievements.Add(AchievementLefty);
            _achievements.Add(AchievementPleasant);
            _achievements.Add(AchievementIcarus);
            _achievements.Add(AchievementCheater);
            _achievements.Add(AchievementStalker);
            _achievements.Add(AchievementSuper);
        }
    }

    #region settings manager

    /// <summary>
    /// static class to acccess it from everywhere
    /// </summary>
    static class AchievementsManager
    {
        private static string fileName = "achievements.xml";

        /// <summary>
        /// create the one and only instance
        /// </summary>
        public static MMAchievements Achievements = new MMAchievements();

        /// <summary>
        /// load settings from isolates storage
        /// </summary>
        public static void LoadSettings(Game gameMain)
        {

            // Create our exposed settings class. This class gets serialized to load/save the settings.
            Achievements = new MMAchievements(gameMain);            

            //Obtain a virtual store for application

#if WINDOWS
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForDomain();
#else
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

            // Check if file is there
            if (fileStorage.FileExists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(Achievements.GetType());
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, fileStorage));

                try
                {
                    Achievements = new MMAchievements(gameMain, (MMAchievements)serializer.Deserialize(stream));
                    stream.Close();
                }
                catch
                {
                    // An error occurred so let's use the default settings.
                    stream.Close();
                    Achievements = new MMAchievements(gameMain);

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
            XmlSerializer serializer = new XmlSerializer(Achievements.GetType());

            StreamWriter stream = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, fileStorage));

            try
            {
                serializer.Serialize(stream, Achievements);
            }
            catch
            {

            }

            stream.Close();
        }
    }

    #endregion
}

