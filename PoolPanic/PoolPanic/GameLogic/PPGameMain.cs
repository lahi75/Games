using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.Globalization;
using System.Threading;

namespace PoolPanic
{
    public class PPGameMain
    {
        public enum GameState
        {
            Menu,
            Play8,
            Play9,
            GameOver,
            About
        }

        public enum GameResult
        {
            exit,
            noresult
        }

        int _songIndex = 0;

        double _debounceStart = 0;
        
        Rectangle _screenRect;        
        IServiceProvider Services;
        ContentManager _content;
        Game _gameMain;
        GameState _currentState = GameState.Menu;

        const Int32 _maxNameLength = 12;

        PPCore _game;
        

        PPWelcomePage _welcomePage;
        
        CultureInfo _oldCulture;        

        public PPGameMain(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {
            _oldCulture = Thread.CurrentThread.CurrentUICulture;
       
            Services = serviceProvider;
            _content = new ContentManager(serviceProvider, "Content");            
                        
            FxManager.LoadSettings(gameMain);
            

            _gameMain = gameMain;
            _screenRect = screenRect;

            _game = new PPCore(gameMain, Services, screenRect, titleSafe);
        

            _welcomePage = new PPWelcomePage(Services, screenRect, titleSafe, _oldCulture.Parent.TwoLetterISOLanguageName);
                        
        }
        

        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {           
            switch (_currentState)
            {
                case GameState.Menu:
                    {                        
                        switch (_welcomePage.Update(gameTime, position, clickDown))
                        {
                            case PPWelcomePage.WelcomeResult.exit:
                                return GameResult.exit;
                            case PPWelcomePage.WelcomeResult.play8:
                                _currentState = GameState.Play8;
                                break;
                            case PPWelcomePage.WelcomeResult.play9:
                                _currentState = GameState.Play9;
                                break;
                        }
                    }
                    break;
                case GameState.Play8:
                    {
                        _game.Update(gameTime, position, clickDown);
                    }

                    break;

                case GameState.Play9:
                    {
                        _game.Update(gameTime, position, clickDown);
                    }

                    break;

                default:
                    break;
            }

            return GameResult.noresult;
        }

          

        
        

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (_currentState)
            {
                case GameState.Menu:
                    _welcomePage.Draw(spriteBatch);
                    break;
                case GameState.Play8:                    
                    _game.Draw(spriteBatch,gameTime);
                    break;
                case GameState.Play9:
                    _game.Draw(spriteBatch, gameTime);
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
                case GameState.Menu:
                    return true;
                case GameState.Play8:
                    //_optionsPage.Back();
                    break;                
            }

            return false;               
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
