using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MonkeyMadness
{
    static class MusicPlayer
    {
        public static Boolean IsExternalPlay = false;
    }

    public class MMGameMain
    {
        enum GameState
        {
            Welcome,
            Game,
            Options,            
            Info,
            HighScore,
            Achievements
        }

        public enum GameResult
        {
            exit,
            noresult
        }

        double _debounceBtnStart = 0;
        double _debounceBackStart = 0;
        SpriteBatch _spriteBatch;
        GameState _currentState = GameState.Welcome;
        Song _song;
        Int32 _currentSong = -2;
        Boolean _trial = false;
        const Int32 _maxNameLength = 10;

        

        Game _gameMain;

        

        //Only for button testing on windows
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public void SetTrial( Boolean trial )
        {
            _trial = trial;                                    
        }

        //Rectangle titleSafe;        

        MMGameLogic _gameLogic;
        MMWelcomePage _welcomePage;
        MMInfoPage _infoPage;
        MMOptionsPage _optionsPage;
        MMHighScorePage _highScorePage;
        MMAchievementsPage _achievementsPage;
        CultureInfo _oldCulture;
      

        public MMGameMain(Game gameMain, Rectangle screenRect, Rectangle titleSafe)
        {
            _oldCulture = Thread.CurrentThread.CurrentUICulture;

            if (MediaPlayer.State == MediaState.Playing)
                MusicPlayer.IsExternalPlay = true;

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(gameMain.GraphicsDevice);

           
            _gameLogic = new MMGameLogic(gameMain, screenRect, titleSafe);

           _welcomePage = new MMWelcomePage(gameMain, screenRect, titleSafe,_oldCulture.Parent.TwoLetterISOLanguageName);

            _infoPage = new MMInfoPage(gameMain, screenRect);

              _optionsPage = new MMOptionsPage(gameMain, screenRect, titleSafe);

            _highScorePage = new MMHighScorePage(gameMain, screenRect);
          
            _gameMain = gameMain;

            // load settings from isolated storage
            SettingsManager.LoadSettings();
            AchievementsManager.LoadSettings(gameMain);
            LocalHighscoreManager.LoadSettings();

            _achievementsPage = new MMAchievementsPage(gameMain, screenRect);
            
            MMFxManager.LoadSettings(gameMain);

            SetName("Cheeta");



            Init();           
        }        
               
        public void SetVersion(string s)
        {
            _infoPage.SetVersion(s);
        }

        public void SetName(string s)
        {
            // if not set...set the user name
            if (SettingsManager.Settings.Playername.Length == 0)
            {                                
                // limit to _maxNameLength letter        
                SettingsManager.Settings.Playername = s.Substring(0, s.Length >= _maxNameLength ? _maxNameLength : s.Length);
                SettingsManager.SaveSettings();
                _optionsPage.Playername = SettingsManager.Settings.Playername;            
            }
        }

        public bool IsFrontPageVisible()
        {
            return _currentState == GameState.Welcome;
        }

        public PlayerIndex GamePadIndex
        {
            get
            {
                return 0; // _optionsPage.GamePadIndex;
            }
            set
            {
                //_optionsPage.GamePadIndex = value;
            }            
        }

        void Init()
        {
            _optionsPage.Difficulty = SettingsManager.Settings.Difficulty;
            _optionsPage.Music = SettingsManager.Settings.Music;
            _optionsPage.FX = SettingsManager.Settings.Fx;
            _optionsPage.Playername = SettingsManager.Settings.Playername;

            // in debug reset achievments
            #if DEBUG
              //  AchievementsManager.Achievements.Reset();
            #endif
        }

        public void Activate(IDictionary<string, object> dict)
        {
            _currentSong = (Int32)dict["CurrentSong"];
            _currentState = (GameState)dict["GameState"];

            // activating gamestate not ready yet
            if (_currentState == GameState.Game)
                _currentState = GameState.Welcome;

            //-no   _gameLogic.Activate(dict);
        }

        public void Deactivate(IDictionary<string, object> dict)
        {            
            dict["CurrentSong"] = _currentSong;
            dict["GameState"] = _currentState;

            //-no _gameLogic.Deactivate(dict);
        }

        void UpdateMusic()
        {
            if (_currentState == GameState.Game)
            {
                
                if ( SettingsManager.Settings.Music)
                {
                    try
                    {
                        MediaPlayer.IsRepeating = true;

                        int level = _gameLogic.CurrentLevel % 4 + 1;                        

                        if (_currentSong != level)
                        {
                            String s = "sound/music" + level;
                            _song = _gameMain.Content.Load<Song>(s);
                            _currentSong = level;

                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(_song);                            
                        }
                    }
                    catch 
                    {
                     //   SettingsManager.Settings.Music = false;
                        _currentSong = -2;
                    }
                }
                
            }
            else
            {                
                if (SettingsManager.Settings.Music)
                {
                    if (_currentSong != -1)
                    {                        
                        try
                        {
                            MediaPlayer.IsRepeating = true;
                            _song = _gameMain.Content.Load<Song>("sound/jungle");
                            _currentSong = -1;
                            MediaPlayer.Volume = 0.8f;
                            MediaPlayer.Play(_song);
                                                        
                        }
                        catch 
                        { 
                            SettingsManager.Settings.Music = false;
                            _currentSong = -2;
                        }
                    }
                
                }
                else
                {
                    if (MediaPlayer.State != MediaState.Stopped)
                    {
                        MediaPlayer.Stop();
                        _currentSong = -2;
                    }
                    
                }
                
            }            
        }

        private bool _showMouseCursor = true;

        public bool ShowMouseCursor { get { return _showMouseCursor; } }

        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            UpdateMusic();            

            switch (_currentState)
            {
                case GameState.Welcome:
                    {
                        _showMouseCursor = true;
                        
                        switch (_welcomePage.Update(gameTime, position,clickDown))
                        {
                            case MMWelcomePage.WelcomeResult.play:
                                _currentState = GameState.Game;                                
                                _gameLogic.Init(SettingsManager.Settings.Difficulty, _trial );
                                _gameLogic.Start();
                                break;
                            case MMWelcomePage.WelcomeResult.options:
                                _currentState = GameState.Options;
                                break;
                            case MMWelcomePage.WelcomeResult.exit:
                                return GameResult.exit;
                            case MMWelcomePage.WelcomeResult.info:
                                _currentState = GameState.Info;
                                break;
                            case MMWelcomePage.WelcomeResult.achievements:
                                _currentState = GameState.Achievements;
                                break;
                            case MMWelcomePage.WelcomeResult.highscore:                                
                                _currentState = GameState.HighScore;
                                _highScorePage.Difficulty = SettingsManager.Settings.Difficulty; // initialize the view with the current difficulty
                                // reset last game
                                _highScorePage.LastGame("",0, SettingsManager.Settings.Difficulty);
                                break;
                            default:
                                // TODO
                                break;
                        
                        }  
                        
                    }
                    break;
                    
                case GameState.Game:
                    {
                        switch (_gameLogic.Update(gameTime, position, clickDown))
                        {
                            case MMGameLogic.GameResult.exit:

                                // submit the score to the online ladder, save in a local file
                                SendScore();

                                if( MusicPlayer.IsExternalPlay == false)
                                    MediaPlayer.Stop();

                                // initial view of on the highscorelist
                                _highScorePage.Difficulty = SettingsManager.Settings.Difficulty;
                                // highlight the last game in the list
                                _highScorePage.LastGame(SettingsManager.Settings.Playername, _gameLogic.CumulatedPoints, SettingsManager.Settings.Difficulty);
                                _currentState = GameState.HighScore;                                
                                _showMouseCursor = true;
                                AchievementsManager.SaveSettings();
                                LocalHighscoreManager.SaveSettings();
                                break;
                            default:
                                _showMouseCursor = _gameLogic.ShowMouseCursor;
                                break;
                        }
                    }
                    break;
                    
                case GameState.Info:
                    {
                        _showMouseCursor = true;

                        switch (_infoPage.Update(position, clickDown))
                        {                            
                            case MMInfoPage.Result.exit:
                                _currentState = GameState.Welcome;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case GameState.Options:
                    {
                        _showMouseCursor = true;

                        switch (_optionsPage.Update(position, clickDown))
                        {
                            case MMOptionsPage.Result.exit:
                                _currentState = GameState.Welcome;

                                // save the new difficulty in the isolated storage
                                SettingsManager.Settings.Difficulty = _optionsPage.Difficulty;
                                SettingsManager.Settings.Music = _optionsPage.Music;
                                SettingsManager.Settings.Fx = _optionsPage.FX;                                
                                String s = _optionsPage.Playername;
                                // limit the size to _maxNameLength letter
                                SettingsManager.Settings.Playername = s.Substring(0, s.Length >= _maxNameLength ? _maxNameLength : s.Length);

                                SettingsManager.SaveSettings();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case GameState.HighScore:
                    {
                        _showMouseCursor = true;

                        switch (_highScorePage.Update(gameTime, position, clickDown))
                        {
                            case MMHighScorePage.Result.exit:
                                _currentState = GameState.Welcome;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                    
                case GameState.Achievements:
                    switch (_achievementsPage.Update(position, clickDown))
                    {
                        case MMAchievementsPage.Result.exit:
                            _currentState = GameState.Welcome;
                            break;
                        default:
                            break;
                    }
                    break;
                    
                default:
                    break;
            }

         
            return GameResult.noresult;
        }      

        public void Draw()
        {            

            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.Game:
                    _gameLogic.Draw(_spriteBatch);
                    break;
                case GameState.Welcome:
                    _welcomePage.Draw(_spriteBatch,_trial);
                    break;
                case GameState.Options:
                    _optionsPage.Draw(_spriteBatch);
                    break;
                case GameState.Info:
                    _infoPage.Draw(_spriteBatch);
                    break;
                case GameState.Achievements:
                    _achievementsPage.Draw(_spriteBatch);
                    break;
                case GameState.HighScore:
                    _highScorePage.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

        }

        public void Left(GameTime time)
        {
            switch (_currentState)
            {
                case GameState.Game:
                    _gameLogic.Left();
                    break;
                case GameState.Options:
                    if (Debounce(time, ref _debounceBtnStart))
                        _optionsPage.Left();                    
                    break;
                case GameState.HighScore:
                    if( Debounce(time, ref _debounceBtnStart) )
                        _highScorePage.Left();                    
                    break;
                case GameState.Achievements:
                    if (Debounce(time, ref _debounceBtnStart))
                       _achievementsPage.Left();
                    break;
            }                       
        }

        public void Right(GameTime time)
        {            
            switch (_currentState)
            {
                case GameState.Game:
                    _gameLogic.Right();
                    break;
                case GameState.Options:
                    if (Debounce(time, ref _debounceBtnStart))
                        _optionsPage.Right();
                    break;
                case GameState.HighScore:
                    if (Debounce(time, ref _debounceBtnStart))
                        _highScorePage.Right();
                    break;
                case GameState.Achievements:
                    if (Debounce(time, ref _debounceBtnStart))
                       _achievementsPage.Right();
                    break;
            }                       
        }

        public void Jump(GameTime time)
        {            
            switch (_currentState)
            {
                case GameState.Game:
                    if (Debounce(time, ref _debounceBtnStart))
                        _gameLogic.Up();
                    break;                
            }                       
        }

        public void Up(GameTime time)
        {            
            switch (_currentState)
            {                
                case GameState.Welcome:
                    if (Debounce(time, ref _debounceBtnStart))
                        _welcomePage.Up();
                    break;
                case GameState.Options:
                    if (Debounce(time, ref _debounceBtnStart))
                        _optionsPage.Up();
                    break;
                case GameState.HighScore:
                    if (Debounce(time, ref _debounceBtnStart))
                        _highScorePage.Up();
                    break;
                case GameState.Achievements:
                    if (Debounce(time, ref _debounceBtnStart))
                        _achievementsPage.Up();
                    break;
            }           
        }

        public void Down(GameTime time)
        {
            switch (_currentState)
            {
                case GameState.Welcome:
                    if (Debounce(time, ref _debounceBtnStart))
                        _welcomePage.Down();
                    break;
                case GameState.Options:
                    if (Debounce(time, ref _debounceBtnStart))
                        _optionsPage.Down();
                    break;
                case GameState.HighScore:
                    if (Debounce(time, ref _debounceBtnStart))
                        _highScorePage.Down();
                    break;
                case GameState.Achievements:
                    if (Debounce(time, ref _debounceBtnStart))
                        _achievementsPage.Down();
                    break;
            }
        }

        public Boolean Back(GameTime time)
        {            
            if (!Debounce(time, ref _debounceBackStart))
                return false;            

            if (_currentState == GameState.Welcome)
                return true;

            // make sure to save the option when going back
            if (_currentState == GameState.Options)
            {
                // save the new difficulty in the isolated storage
                SettingsManager.Settings.Difficulty = _optionsPage.Difficulty;
                SettingsManager.Settings.Music = _optionsPage.Music;
                SettingsManager.Settings.Fx = _optionsPage.FX;
                SettingsManager.SaveSettings();
            }

            if (_currentState == GameState.Achievements)
            {                
                if (_achievementsPage.Back() == false) // stay in this page when we return from details page to overview page
                    return false;
            }


            MMFxManager.Fx.StopAll();

            if( _currentState == GameState.Game )
                AchievementsManager.SaveSettings();
            
            if( MusicPlayer.IsExternalPlay == false )
                MediaPlayer.Stop();

            _currentSong = -2;

            _currentState = GameState.Welcome;

            return false;
        }

        private bool Debounce(GameTime time, ref double start)
        {
            if (time.TotalGameTime.TotalMilliseconds - start > 300.0)
            {
                start = time.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }

        public void Start()
        {            
            switch (_currentState)
            {
                case GameState.Game:                                        
                    _gameLogic.Start();
                    break;
                case GameState.Welcome:

                    break;
            }                       
        }

        void SendScore()
        {
            
            MMHighscore hs = new MMHighscore();

            string mode = "1";

            switch (SettingsManager.Settings.Difficulty)
            {
                case MMSettings.DifficultyType.easy:
                    mode = "1";
                    break;
                case MMSettings.DifficultyType.medium:
                    mode = "2";
                    break;
                case MMSettings.DifficultyType.hard:
                    mode = "3";
                    break;
            }

            string info = "1";

#if WINDOWS
            info = "1";
#elif WINDOWS_PHONE
            info = "2";
#elif XBOX
            info = "3";
#endif
            // 4 is for Android

            Score score = new Score(mode, SettingsManager.Settings.Playername, info, _gameLogic.CumulatedPoints, "DE");

            hs.SendScore(score);
            
            LocalHighscoreManager.Highscore.AddScore(SettingsManager.Settings.Difficulty, score);

            
        }     
            
    }
}
