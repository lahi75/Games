using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TrainTrouble.Properties;

namespace TGGameLibrary
{
    public class TGGameMain
    {
        enum GameState
        {
            Welcome,
            Selection,
            NextStage,
            GameOver,
            Game,
            Options,            
            HighScore,    
            Info,
            Pause
        }

        public enum GameResult
        {
            exit,
            noresult
        }
        static class MusicPlayer
        {
            public static Boolean IsExternalPlay = false;
        }

        double _debounceStart = 0;
        TGLevel _level;        
        TGWelcomePage _welcomePage;
        TGLevelSelection _levelSelection;
        TGOptionsPage _optionsPage;
        TGInfoPage _infoPage;
        TGHighScorePage _highScorePage;
        CultureInfo _oldCulture;

        Rectangle _screenRect;

        const Int32 _maxNameLength = 10;

        SpriteFont _tinyFont;
        SpriteFont _largeFont;
        SpriteFont _mediumFont;
        SpriteFont _smallFont;        

        Song _song;
        Int32 _currentSong = -2;

        TGAnimatedTexture _statusHighlight;        
        Texture2D _statusTextture;
        String _statusText;
        String _statusCaption;

        IServiceProvider Services;
        Game _gameMain;
        
        GameState _currentState = GameState.Welcome;
        GameState _followState = GameState.Welcome;

        public TGGameMain(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {
            _oldCulture = Thread.CurrentThread.CurrentUICulture;            

            if (MediaPlayer.State == MediaState.Playing)
                MusicPlayer.IsExternalPlay = true;

            Services = serviceProvider;
            ContentManager content = new ContentManager(serviceProvider, "Content");            

            _gameMain = gameMain;

            _screenRect = screenRect;

            // load settings from isolated storage
            LevelManager.LoadSettings(Services);

            // level 0 is always unlocked
            LevelManager.Levels.GetLevel(0).Unlock();

            // load settings from isolated storage
            SettingsManager.LoadSettings();            
            TGFxManager.LoadSettings(content);
            LocalHighscoreManager.LoadSettings();

            _welcomePage = new TGWelcomePage(Services, screenRect, titleSafe, _oldCulture.Parent.TwoLetterISOLanguageName);
            _optionsPage = new TGOptionsPage(Services, screenRect, titleSafe);
            _infoPage = new TGInfoPage(Services, screenRect);
            _highScorePage = new TGHighScorePage(Services, screenRect);

            _levelSelection = new TGLevelSelection(Services, screenRect);

            _largeFont = content.Load<SpriteFont>("fonts/largeFont");
            _smallFont = content.Load<SpriteFont>("fonts/smallFont");
            _tinyFont = content.Load<SpriteFont>("fonts/tinyFont");
            _mediumFont = content.Load<SpriteFont>("fonts/mediumFont");            

            _statusTextture = content.Load<Texture2D>("buttons/gameover");
            _statusHighlight = new TGAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _statusHighlight.Load(content, "buttons/highlight", 20, 20);
            Init();
        }
        void Init()
        {
            // work around for ad, make sure that language is english during startup
            _optionsPage.Music = SettingsManager.Settings.Music;
            _optionsPage.FX = SettingsManager.Settings.Fx;
            _optionsPage.Playername = SettingsManager.Settings.Playername;
            
            // in debug, unlock all levels
#if DEBUG
            LevelManager.Levels.GetLevel(1).Unlock();
            LevelManager.Levels.GetLevel(2).Unlock();
            LevelManager.Levels.GetLevel(3).Unlock();
            LevelManager.Levels.GetLevel(4).Unlock();            
#endif

            LevelManager.Levels.Level0.SetRedCap(30,13);
            LevelManager.Levels.Level0.SetBlueCap(30,13);
            LevelManager.Levels.Level0.SetGreenCap(30,13);
            LevelManager.Levels.Level0.AngryTime = 50;

            LevelManager.Levels.Level1.SetRedCap(20,13);
            LevelManager.Levels.Level1.SetBlueCap(50,16);
            LevelManager.Levels.Level1.SetGreenCap(50,19);
            LevelManager.Levels.Level1.AngryTime = 80;

            LevelManager.Levels.Level2.SetRedCap(20,12);
            LevelManager.Levels.Level2.SetBlueCap(60,15);
            LevelManager.Levels.Level2.SetGreenCap(90,18);
            LevelManager.Levels.Level2.AngryTime = 90;

            LevelManager.Levels.Level3.SetRedCap(20,14);
            LevelManager.Levels.Level3.SetBlueCap(60,18);
            LevelManager.Levels.Level3.SetGreenCap(100,21);
            LevelManager.Levels.Level3.AngryTime = 80;

            LevelManager.Levels.Level4.SetRedCap(30, 16);
            LevelManager.Levels.Level4.SetBlueCap(70, 20);
            LevelManager.Levels.Level4.SetGreenCap(120, 25);
            LevelManager.Levels.Level4.AngryTime = 80;

            LevelManager.Levels.Level0.Stops = 0;
            LevelManager.Levels.Level1.Stops = 3;
            LevelManager.Levels.Level2.Stops = 6;
            LevelManager.Levels.Level3.Stops = 12;
            LevelManager.Levels.Level4.Stops = 15;

            LevelManager.Levels.Level0.Boosts = 3;
            LevelManager.Levels.Level1.Boosts = 5;
            LevelManager.Levels.Level2.Boosts = 8;
            LevelManager.Levels.Level3.Boosts = 15;
            LevelManager.Levels.Level4.Boosts = 20;
        }

        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            UpdateMusic();            

            switch (_currentState)
            {
                case GameState.Welcome:
                    {                        
                        switch (_welcomePage.Update(gameTime, position, clickDown))
                        {
                            case TGWelcomePage.WelcomeResult.play:
                                _currentState = GameState.Selection;                                
                                break;                                
                            case TGWelcomePage.WelcomeResult.options:
                                _currentState = GameState.Options;
                                break;
                            case TGWelcomePage.WelcomeResult.exit:
                                return GameResult.exit;                                                            
                            case TGWelcomePage.WelcomeResult.highscore:
                                _currentState = GameState.HighScore;
                                _followState = GameState.Welcome;
                                _highScorePage.LastLevel = 0;
                                // reset last game
                                _highScorePage.LastGame("", 0, 0);
                                _highScorePage.SetExitText(Resources.back);
                                break;
                            case TGWelcomePage.WelcomeResult.info:
                                _currentState = GameState.Info;
                                break;
                            default:
                                // TODO
                                break;
                        }
                    }
                    break;

                case GameState.Selection:
                    {
                        switch (_levelSelection.Update(gameTime, position, clickDown))
                        {
                            case TGLevelSelection.Result.Level_0:
                                _currentState = GameState.Game;
                                LoadLevel(0);                                
                                break;
                            case TGLevelSelection.Result.Level_1:
                                _currentState = GameState.Game;
                                LoadLevel(1);                                
                                break;
                            case TGLevelSelection.Result.Level_2:
                                _currentState = GameState.Game;
                                LoadLevel(2);
                                break;
                            case TGLevelSelection.Result.Level_3:
                                _currentState = GameState.Game;
                                LoadLevel(3);
                                break;
                            case TGLevelSelection.Result.Level_4:
                                _currentState = GameState.Game;
                                LoadLevel(4);
                                break;
                            case TGLevelSelection.Result.back:
                                _currentState = GameState.Welcome;
                                break;
                        }
                    }
                    break;
                case GameState.Game:
                    if (clickDown)
                        _level.Click(gameTime, position);

                    switch (_level.Update(gameTime))
                    {
                        case TGLevel.Result.TrainCrash:
                            _followState = GameState.Selection;
                            _currentState = GameState.GameOver;
                            _statusText = Resources.trainCollision;
                            _statusCaption = Resources.gameOver;
                            _statusHighlight.Play();                            
                            break;
                        case TGLevel.Result.StationOverflow:
                            _followState = GameState.Selection;
                            _currentState = GameState.GameOver;
                            _statusText = Resources.stationFull;
                            _statusCaption = Resources.gameOver;
                            _statusHighlight.Play();
                            break;
                        case TGLevel.Result.NewStage:
                            _currentState = GameState.NextStage;
                            _statusCaption = Resources.nextStage;
                            _statusText = Resources.trainAdded;
                            _statusHighlight.Stop();                      
                            break;
                        case TGLevel.Result.LevelCleared:
                            _followState = GameState.Selection;
                            _currentState = GameState.GameOver;
                            _statusCaption = Resources.levelAccomplished;

                            if (_level.LevelIndex == LevelManager.Levels.Count - 1)
                                _statusText = Resources.allDone;
                            else
                            {
                                _statusText = Resources.levelUnlocked;
                                LevelManager.Levels.GetLevel(_level.LevelIndex + 1).Unlock();
                                LevelManager.SaveSettings();
                            }

                            _statusHighlight.Stop();
                            break;
                        case TGLevel.Result.Pause:
                            _currentState = GameState.Pause;
                            _statusCaption = Resources.pause;
                            _statusText = Resources.tapContinue;
                            _statusHighlight.Stop();                      
                            break;
                        default:
                            break;
                    }                    
                    break;  
                case GameState.GameOver:
                    if (clickDown)
                    {                                                
                        _currentState = GameState.HighScore;

                        _highScorePage.LastLevel = _level.LevelIndex;
                        // highlight the last game in the list
                        _highScorePage.LastGame(SettingsManager.Settings.Playername, _level.Points, _level.LevelIndex);

                        _highScorePage.SetExitText(Resources.goon);

                        LocalHighscoreManager.SaveSettings();
                    }
             
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _statusHighlight.UpdateFrame(elapsed);
                    break;
                case GameState.Pause:
                    if (clickDown)
                    {
                        _level.Continue(gameTime);
                        _currentState = GameState.Game;
                    }
                    break;
                case GameState.NextStage:
                    if (clickDown)
                        _currentState = GameState.Game;
                    break;
                case GameState.Options:

                    switch (_optionsPage.Update(position, clickDown))
                    {
                        case TGOptionsPage.Result.exit:
                            _currentState = GameState.Welcome;

                            // save the new options in the isolated storage                            
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

                    break;
                case GameState.Info:

                    switch (_infoPage.Update(position, clickDown))
                    {
                        case TGInfoPage.Result.exit:
                            _currentState = GameState.Welcome;
                            break;
                        default:
                            break;
                    }

                    break;                
                case GameState.HighScore:
                    {                     
                        switch (_highScorePage.Update(gameTime, position, clickDown))
                        {
                            case TGHighScorePage.Result.exit:
                                _currentState = _followState;
                                break;
                            default:
                                break;
                        }
                    }
                    break;                
                default:
                    break;
            }

            return GameResult.noresult;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.GameOver:
                    _level.Draw(spriteBatch);
                    DrawStatus(spriteBatch);
                    break;
                case GameState.Game:
                    _level.Draw(spriteBatch);
                    break;
                case GameState.Pause:
                case GameState.NextStage:
                    _level.Draw(spriteBatch);
                    DrawStatus(spriteBatch);
                    break;
                case GameState.Selection:
                    _levelSelection.Draw(spriteBatch);
                    break;
                case GameState.Welcome:
                    _welcomePage.Draw(spriteBatch);
                    break;
                case GameState.Options:
                    _optionsPage.Draw(spriteBatch);
                    break;                                    
                case GameState.HighScore:
                    _highScorePage.Draw(spriteBatch);
                    break;
                case GameState.Info:
                    _infoPage.Draw(spriteBatch);
                    break;
            }
         
            spriteBatch.End();            
        }

        void DrawStatus(SpriteBatch spriteBatch)
        {
            Vector2 status =  _level.StatusPosition * TGTile.Size;

            status.X -= _statusHighlight.Size.X / 2 - TGTile.Size.X/2;
            status.Y -= _statusHighlight.Size.Y / 2 - TGTile.Size.Y/2;

            if( _statusHighlight.Is_paused == false )
                _statusHighlight.DrawFrame(spriteBatch, status, SpriteEffects.None);

            Vector2 p = new Vector2(_screenRect.Width / 2 - _statusTextture.Width / 2, _screenRect.Height / 2 - _statusTextture.Height / 2);
            
            //draw default texture
            spriteBatch.Draw(_statusTextture, p, Color.White);

            // center the text
            String text = _statusCaption;
            Vector2 fontSize = _largeFont.MeasureString(text);

            Rectangle buttonPos = new Rectangle((int)p.X, (int)p.Y, _statusTextture.Width, _statusTextture.Height);
            Vector2 textPos = new Vector2(buttonPos.X + buttonPos.Width / 2 - fontSize.X / 2, buttonPos.Y + buttonPos.Height / 3 - fontSize.Y / 2);

            spriteBatch.DrawString(_largeFont, text, textPos, Color.WhiteSmoke);

            text = _statusText;
            fontSize = _smallFont.MeasureString(text);
            textPos = new Vector2(buttonPos.X + buttonPos.Width / 2 - fontSize.X / 2, buttonPos.Y + buttonPos.Height * 2 / 3 - fontSize.Y / 2);

            spriteBatch.DrawString(_smallFont, text, textPos, Color.WhiteSmoke);
        }

        private void LoadLevel(Int32 level)
        {            
            // Unloads the content for the current level before loading the next one.
            if (_level != null)
                _level.Dispose();

            // Load the level.
            string levelPath = LevelManager.Levels.GetLevel(level).LevelPath;
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                _level = new TGLevel(Services, fileStream, level);
        }

        public Boolean Back(GameTime time)
        {
            if (!Debounce(time))
                return false;

            if (_currentState == GameState.Welcome)
                return true;

            // make sure to save the option when going back
            if (_currentState == GameState.Options)
            {
                // save the options in the isolated storage                
                SettingsManager.Settings.Music = _optionsPage.Music;
                SettingsManager.Settings.Fx = _optionsPage.FX;
                SettingsManager.Settings.Playername = _optionsPage.Playername;
                SettingsManager.SaveSettings();
            }
                        
            if (_currentState == GameState.Game)
                if (_level.Back(time) == false)
                    return false;

            _currentState = GameState.Welcome;

            return false;
        }       

        void UpdateMusic()
        {
            ContentManager content = new ContentManager(Services, "Content");

            if (_currentState == GameState.Game || _currentState == GameState.NextStage || _currentState == GameState.Pause || _currentState == GameState.GameOver)
            {
                if (SettingsManager.Settings.Music)
                {
                    try
                    {
                        MediaPlayer.IsRepeating = true;

                        int level = _level.LevelIndex + 1;

                        if (_currentSong != level)
                        {
                            String s = "music/tune" + level;
                            _song = content.Load<Song>(s);
                            _currentSong = level;

                            MediaPlayer.Volume = 0.3f;
                            MediaPlayer.Play(_song);
                        }
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
                if (SettingsManager.Settings.Music)
                {
                    if (_currentSong != -1)
                    {
                        try
                        {
                            MediaPlayer.IsRepeating = true;
                            _song = content.Load<Song>("music/tune0");
                            _currentSong = -1;
                            MediaPlayer.Volume = 0.2f;
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
        private bool Debounce(GameTime time)
        {
            if (time.TotalGameTime.TotalMilliseconds - _debounceStart > 300)
            {
                _debounceStart = time.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
    }
}
