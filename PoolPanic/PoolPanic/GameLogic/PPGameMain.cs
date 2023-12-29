using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        
        double _debounceStart = 0;
                                
        GameState _currentState = GameState.Menu;        

        PPCore _game;        

        PPWelcomePage _welcomePage;                        

        public PPGameMain(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {                                                              
            FxManager.LoadSettings(gameMain);
            
            _game = new PPCore(gameMain, serviceProvider, screenRect, titleSafe);
        
            _welcomePage = new PPWelcomePage(serviceProvider, screenRect, titleSafe);                        
        }
        
        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {           
            switch (_currentState)
            {
                default:
                case GameState.Menu:
                    {                        
                        switch (_welcomePage.Update(gameTime, position, clickDown))
                        {
                            case PPWelcomePage.WelcomeResult.exit:
                                return GameResult.exit;
                            case PPWelcomePage.WelcomeResult.play8:
                                _game.Init(PPCore.GameMode.Ball8);
                                _currentState = GameState.Play8;
                                break;
                            case PPWelcomePage.WelcomeResult.play9:
                                _game.Init(PPCore.GameMode.Ball9);
                                _currentState = GameState.Play9;
                                break;
                        }
                    }
                    break;
                case GameState.Play8:
                    {
                        if (_game.Update(gameTime, position, clickDown) == PPCore.GameResult.exit)
                            _currentState = GameState.Menu;
                    }
                    break;
                case GameState.Play9:
                    {
                        if (_game.Update(gameTime, position, clickDown) == PPCore.GameResult.exit)
                            _currentState = GameState.Menu;
                    }
                    break;                
            }

            return GameResult.noresult;
        }

        public void SetVersion(string s)
        {
            _welcomePage.SetVersion(s);
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
                    _game.Back(time);
                    break;
                case GameState.Play9:
                    _game.Back(time);
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
