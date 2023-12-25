using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace LLGameLibrary
{
    public class LLGameMain
    {
        public enum GameState
        {
            Welcome,
            Selection,
            LevelIntro,            
            Game,
            Options,
            Instructions,
            HighScore,    
            Info,            
        }

        public enum GameResult
        {
            exit,
            noresult
        }

        public static class MusicPlayer
        {
            public static Boolean IsExternalPlay = false;
        }

        int _songIndex = 0;
        double _debounceStart = 0;
        
        Rectangle _screenRect;        
        IServiceProvider Services;
        ContentManager _content;
        Game _gameMain;
        GameState _currentState = GameState.Welcome;

        const Int32 _maxNameLength = 12;

        LLLevel _game;
        LLLevelIntro _intro;

        LLWelcomePage _welcomePage;
        LLLevelPage _levelPage;
        LLOptionsPage _optionsPage;
        LLHighScorePage _highScorePage;
        LLInfoPage _infoPage;
        LLInstructionsPage _instructionsPage;
        CultureInfo _oldCulture;

        public void UpdateMusic(GameState gameState, LLLevel.LevelState levelState)
        {
            if (SettingsManager.Settings.Music == false)
            {
                if (MediaPlayer.State != MediaState.Stopped)
                {
                    if (MediaPlayer.Volume > 0)
                        MediaPlayer.Volume -= 0.01f;
                    else
                    {
                        MediaPlayer.Stop();
                        _songIndex = 0;
                    }
                }
            }

            if (levelState == LLLevel.LevelState.edit && gameState == GameState.Game )
            {
                if (SettingsManager.Settings.Music)                
                    PlaySong(1);                
            }
            else if (gameState == GameState.HighScore ||
                    gameState == GameState.Info ||
                    gameState == GameState.Selection ||
                    gameState == GameState.Welcome ||
                    gameState == GameState.Options ||
                    gameState == GameState.Instructions)
            {
                if (SettingsManager.Settings.Music)
                    PlaySong(2);
            }
            else
            {
                if (MediaPlayer.State != MediaState.Stopped)
                {
                    if (MediaPlayer.Volume > 0)
                        MediaPlayer.Volume -= 0.01f;
                    else
                    {
                        MediaPlayer.Stop();
                        _songIndex = 0;
                    }
                }
            }
        }
        private void PlaySong(int number)
        {
            if( _songIndex != number )
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    try
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Volume = 0.3f;
                        MediaPlayer.Play(_content.Load<Song>("music/song" + number));
                        _songIndex = number;
                    }
                    catch
                    {
                        SettingsManager.Settings.Music = false;
                    }
                }
            }
        }

        public LLGameMain(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {
            _oldCulture = Thread.CurrentThread.CurrentUICulture;

            if (MediaPlayer.State == MediaState.Playing)
                MusicPlayer.IsExternalPlay = true;            

            Services = serviceProvider;
            _content = new ContentManager(serviceProvider, "Content");            

            SettingsManager.LoadSettings();
            LevelManager.LoadSettings();
            LLFxManager.LoadSettings(_content);
            LocalHighscoreManager.LoadSettings();            

            _gameMain = gameMain;
            _screenRect = screenRect;

            _game = new LLLevel(gameMain, Services, screenRect, titleSafe);
            _intro = new LLLevelIntro(gameMain, serviceProvider, screenRect, titleSafe);

            _welcomePage = new LLWelcomePage(_content, screenRect, _oldCulture.Parent.TwoLetterISOLanguageName);
            _levelPage = new LLLevelPage(_content, screenRect);
            _optionsPage = new LLOptionsPage(_content, screenRect);
            _highScorePage = new LLHighScorePage(Services, screenRect);
            _infoPage = new LLInfoPage(_content, screenRect);
            _instructionsPage = new LLInstructionsPage(_content, screenRect);

            _optionsPage.GameMode = SettingsManager.Settings.GameMode;
            _optionsPage.Music = SettingsManager.Settings.Music;
            _optionsPage.FX = SettingsManager.Settings.Fx;
            _optionsPage.Playername = SettingsManager.Settings.Playername;
        }

        public void Activate(IDictionary<string, object> dict)
        {         
            _currentState = (GameState)dict["GameState"];

            // activating gamestate not ready yet
            if (_currentState == GameState.Game)
                _currentState = GameState.Welcome;

            //_gameLogic.Activate(dict);
        }

        public void Deactivate(IDictionary<string, object> dict)
        {         
            dict["GameState"] = _currentState;
            //_gameLogic.Deactivate(dict);
        }

        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            UpdateMusic(_currentState, _game.State);

            switch (_currentState)
            {
                case GameState.Welcome:
                    {
                        switch (_welcomePage.Update(gameTime, position, clickDown))
                        {
                            case LLWelcomePage.WelcomeResult.exit:
                                return GameResult.exit;
                            case LLWelcomePage.WelcomeResult.play:
                                _levelPage.UpdateLevelButtons();
                                _currentState = GameState.Selection;                                
                                break;
                            case LLWelcomePage.WelcomeResult.options:                                
                                _currentState = GameState.Options;
                                break;
                            case LLWelcomePage.WelcomeResult.info:
                                _currentState = GameState.Info;
                                break;
                            case LLWelcomePage.WelcomeResult.highscore:
                                _currentState = GameState.HighScore;                                
                                _highScorePage.LastLevel = (int)SettingsManager.Settings.LevelStage;
                                // reset last game
                                _highScorePage.LastGame("", 0, (int)SettingsManager.Settings.LevelStage);
                                _highScorePage.SetExitText(Resources.back);
                                break;
                            case LLWelcomePage.WelcomeResult.instructions:
                                _currentState = GameState.Instructions;
                                break;
                        }
                    }
                    break;
                case GameState.Selection:
                    {
                        switch (_levelPage.Update(gameTime, position, clickDown))
                        {
                            case LLLevelPage.LevelResult.exit:
                                _currentState = GameState.Welcome;
                                break;
                            case LLLevelPage.LevelResult.noresult:
                                break;
                            case LLLevelPage.LevelResult.game:
                                _intro.Init(SettingsManager.Settings.LevelStage,_levelPage.SelectedLevel);
                                _currentState = GameState.LevelIntro;                               

                                //temp
                                //_game.Init(SettingsManager.Settings.LevelStage, _levelPage.SelectedLevel);
                                //_currentState = GameState.Game;
                                break;
                        }
                    }

                    break;
                case GameState.LevelIntro:
                    {
                        if (_intro.Update(gameTime, position, clickDown) == LLLevelIntro.IntroResult.exit)
                        {
                            _game.Init(SettingsManager.Settings.LevelStage, _levelPage.SelectedLevel);
                            _currentState = GameState.Game;
                        }
                    }
                    break;
                case GameState.Game:
                    switch (_game.Update(gameTime, position, clickDown))
                    {
                        case LLLevel.GameResult.next:
                            StorePoints(_game.CurrentLevel - 1, _game.CurrentPoints);
                            UnlockLevel(_game.CurrentLevel, _game.Stage);                           
                            _currentState = GameState.Selection;
                            _levelPage.UpdateLevelButtons();
                            break;
                        case LLLevel.GameResult.exit_next:
                            StorePoints(_game.CurrentLevel - 1, _game.CurrentPoints);
                            UnlockLevel(_game.CurrentLevel, _game.Stage);                           
                            _currentState = GameState.Welcome;
                            _levelPage.UpdateLevelButtons();
                            break;
                        case LLLevel.GameResult.finish:
                            StorePoints(_game.CurrentLevel - 1, _game.CurrentPoints);
                            UnlockLevel(_game.CurrentLevel, _game.Stage);
                            _levelPage.UpdateLevelButtons();
                            // finished the stage -> show highscore list
                            SendScore(_game.Stage, LevelManager.Level.TotalPoints(_game.Stage));
                            _currentState = GameState.HighScore;
                            _highScorePage.LastLevel = (int)_game.Stage;
                            // highlight the last game in the list
                            _highScorePage.LastGame(SettingsManager.Settings.Playername, LevelManager.Level.TotalPoints(_game.Stage), (int)_game.Stage);
                            break;
                        case LLLevel.GameResult.exit:                                                        
                            _currentState = GameState.Welcome;
                            break;
                    }
                    break;                  
                case GameState.Options:        
                    switch (_optionsPage.Update(position, clickDown))
                    {
                            case LLOptionsPage.OptionsResult.exit:
                                _currentState = GameState.Welcome;
                                // save the new settings in the isolated storage
                                SettingsManager.Settings.GameMode = _optionsPage.GameMode;                                
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
                    {
                        switch (_infoPage.Update(gameTime, position, clickDown))
                        {
                            case LLInfoPage.Result.exit:
                                _currentState = GameState.Welcome;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case GameState.HighScore:
                    {
                        switch (_highScorePage.Update(gameTime, position, clickDown))
                        {
                            case LLHighScorePage.Result.exit:
                                _currentState = GameState.Welcome;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case GameState.Instructions:
                    {
                        switch (_instructionsPage.Update(gameTime, position, clickDown))
                        {
                            case LLInstructionsPage.Result.exit:
                                _currentState = GameState.Welcome;
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

        private void StorePoints(Int32 level, Int32 points)
        {
            // don't save points in freestyle mode
            if (SettingsManager.Settings.GameMode == LLSettings.Mode.freeStyle) 
                return; 

            switch (SettingsManager.Settings.LevelStage)
            {
                case LLSettings.Difficulty.novice:
                    LevelManager.Level.Novice[level].Points = points;
                    break;
                case LLSettings.Difficulty.advanced:
                    LevelManager.Level.Advanced[level].Points = points;
                    break;
                case LLSettings.Difficulty.expert:
                    LevelManager.Level.Expert[level].Points = points;
                    break;
                case LLSettings.Difficulty.master:
                    LevelManager.Level.Master[level].Points = points;
                    break;
            }
            LevelManager.SaveSettings();
        }

        private void UnlockLevel(Int32 level, LLSettings.Difficulty stage)
        {            
            if (SettingsManager.Settings.GameMode != LLSettings.Mode.ladder)
                return;

            switch (stage)
            {
                case LLSettings.Difficulty.novice:
                    LevelManager.Level.Novice[level - 1].IsUnlocked = false;    // close current level
                    if (level < LevelManager.Level.MaxLevel)
                        LevelManager.Level.Novice[level].IsUnlocked = true;     // open the successor
                    break;
                case LLSettings.Difficulty.advanced:
                    LevelManager.Level.Advanced[level - 1].IsUnlocked = false;  // close current level
                    if (level < LevelManager.Level.MaxLevel)
                        LevelManager.Level.Advanced[level].IsUnlocked = true;
                    break;
                case LLSettings.Difficulty.expert:
                    LevelManager.Level.Expert[level - 1].IsUnlocked = false;    // close current level
                    if (level < LevelManager.Level.MaxLevel)
                        LevelManager.Level.Expert[level].IsUnlocked = true;
                    break;
                case LLSettings.Difficulty.master:
                    LevelManager.Level.Master[level - 1].IsUnlocked = false;    // close current level
                    if (level < LevelManager.Level.MaxLevel)
                        LevelManager.Level.Master[level].IsUnlocked = true;
                    break;
            }
            LevelManager.SaveSettings();         
        }

        public bool DrawAd()
        {                        
            // don't draw ad on first page
            return _currentState != GameState.Welcome;
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
            switch (_currentState)
            {
                case GameState.Game:
                    _game.Draw(spriteBatch);
                    break;
                case GameState.Welcome:
                    _welcomePage.Draw(spriteBatch);
                    break;
                case GameState.Selection:
                    _levelPage.Draw(spriteBatch);
                    break;
                case GameState.Options:
                    _optionsPage.Draw(spriteBatch);
                    break;
                case GameState.LevelIntro:
                    _intro.Draw(spriteBatch);
                    break;
                case GameState.HighScore:
                    _highScorePage.Draw(spriteBatch);
                    break;
                case GameState.Info:
                    _infoPage.Draw(spriteBatch);
                    break;
                case GameState.Instructions:
                    _instructionsPage.Draw(spriteBatch);
                    break;
                default:
                    break;
            }
        }                

        public Boolean Back(GameTime time)
        {
            if (!Debounce(time))
                return false;
            

            switch (_currentState)
            {
                case GameState.Welcome:
                    return true;
                case GameState.Options:
                    _optionsPage.Back();
                    break;
                case GameState.Selection:
                    _levelPage.Back();
                    break;
                case GameState.Game:
                    _game.Back(time);
                    break;
                case GameState.LevelIntro:
                    _levelPage.UpdateLevelButtons();
                    _currentState = GameState.Selection; // back to selection from level intro page
                    _intro.Back();
                    break;
                case GameState.HighScore:
                    _currentState = GameState.Welcome;
                    break;
                case GameState.Info:
                    _currentState = GameState.Welcome;
                    break;
                case GameState.Instructions:
                    _instructionsPage.Back();
                    break;
            }

            return false;               
        }

        void SendScore(LLSettings.Difficulty stage, Int32 points)
        {            
            LLHighscore hs = new LLHighscore();
                       
            string info = "1";
            int level = (int)stage;            

            Score score = new Score(level.ToString(), SettingsManager.Settings.Playername, info, points, "DE");

            hs.SendScore(score);

            LocalHighscoreManager.Highscore.AddScore(level, score);
            LocalHighscoreManager.SaveSettings();
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
