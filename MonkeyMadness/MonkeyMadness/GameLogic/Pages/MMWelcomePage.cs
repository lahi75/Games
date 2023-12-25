using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Microsoft.Xna.Framework.Content;
#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace MonkeyMadness
{
    class MMWelcomePage
    {
        SpriteFont _largeFont;

        MMButton _buttonPlay;
        MMButton _buttonOptions;
        MMButton _buttonInfo;
        //MMButton _buttonExit;
        MMButton _buttonHighscore;
        MMButton _buttonAchievements;

        //MMButton _buttonMore;
        //JJNewsTicker _newsTicker;

        int _currentSecond = 0;
        //bool _showTrial = false;

        float _idleEnd;
        bool _idle = false;        
        Game _gameMain;
        MMAnimatedTexture _monkey;
        Vector2 _monkeyPosition;
        float _increment;
        Rectangle _screenRect;

        Texture2D _background;
        SpriteFont _font;
        SpriteFont _smallFont;
        

        public enum WelcomeResult
        {
            play,
            options,
            info,
            exit,
            highscore,
            achievements,
            noresult
        }
      
        public MMWelcomePage(Game gameMain, Rectangle screenRect, Rectangle titleSafe, string language)
        {
            _gameMain = gameMain;

            _buttonPlay = new MMButton(gameMain, Properties.Resources.strPlay, false,false);
            _buttonOptions = new MMButton(gameMain, Properties.Resources.strOptions, false, false);
            _buttonInfo = new MMButton(gameMain, Properties.Resources.strInfo, false, false);            
            _buttonHighscore = new MMButton(gameMain, Properties.Resources.strHighScore, false, false);
            _buttonAchievements = new MMButton(gameMain, Properties.Resources.strAward, false, false);

            _buttonPlay.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 80));
            _buttonOptions.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 160));
            _buttonInfo.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 240));
            _buttonHighscore.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 320));
            _buttonAchievements.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 400));

            //_buttonMore = new JJButton(gameMain, "", false, false);
            //_buttonMore.DefaultTexture = _buttonMore.HoverTexture = _buttonMore.PressedTexture = _buttonMore.HoverPressedTexture = LoadLocalizedAsset<Texture2D>("buttons/more_games");
            //_buttonMore.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X + 250, 320));

            _buttonPlay.Hover = true;
            
            _smallFont = gameMain.Content.Load<SpriteFont>("fonts/smallFont");
            _font = gameMain.Content.Load<SpriteFont>("fonts/ButtonFont");
            _background = gameMain.Content.Load<Texture2D>("background/form_main");
            
            _increment = ((screenRect.Width / 10) * (float)gameMain.TargetElapsedTime.Milliseconds / 1000);

            _monkey = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _monkey.Load(gameMain.Content, "monkey/run", 16, 16);

            _monkeyPosition = new Vector2( titleSafe.X + +titleSafe.Width + 50, 415);

            //_newsTicker = new JJNewsTicker(gameMain.Content, new Point(0, 0), language);

            _screenRect = screenRect;
            _largeFont = gameMain.Content.Load<SpriteFont>("fonts/largeFont");                        
        }
                       
        public WelcomeResult Update(GameTime game, Point mousePosition, bool mouseDown)
        {            
            if (_buttonPlay.Update(mousePosition, mouseDown))
                return WelcomeResult.play;

            if (_buttonOptions.Update(mousePosition, mouseDown))
                return WelcomeResult.options;

            if (_buttonInfo.Update(mousePosition, mouseDown))
                return WelcomeResult.info;

            if (_buttonHighscore.Update(mousePosition, mouseDown))
                return WelcomeResult.highscore;

            if (_buttonAchievements.Update(mousePosition, mouseDown))
                return WelcomeResult.achievements;

            //_newsTicker.Update(game,mousePosition,mouseDown);

            //if (_buttonMore.Update(mousePosition, mouseDown))
            {
                // open website
                /*
#if WINDOWS_PHONE
                try
                {
                    //var wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
                    //wbt.URL = Resources.moreGamesURL;
                    //wbt.Show();
                    MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
                    marketplaceSearchTask.SearchTerms = "Phoebit";
                    marketplaceSearchTask.Show();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
#endif
#if WINDOWS
                    System.Diagnostics.Process.Start(Resources.moreGamesURL);                                
#endif          */
            }        
                
            UpdateMonkey(game);

            return WelcomeResult.noresult;
        }

        private void UpdateMonkey(GameTime game)
        {
            float elapsed = (float)game.ElapsedGameTime.TotalSeconds;
           _monkey.UpdateFrame(elapsed);

            if (!_idle)
            {
                _monkeyPosition.X -= _increment;
                if (_monkeyPosition.X < 600)
                {
                    
                    _idle = true;
                    _idleEnd = game.TotalGameTime.Seconds + 4;
                    _monkey.Load(_gameMain.Content, "monkey/idle", 16, 16);
                }
            }
            else
            {
                if (game.TotalGameTime.Seconds > _idleEnd)
                {
                    _increment *= -1;
                    _idle = false;
                    _monkey.Load(_gameMain.Content, "monkey/run", 16, 16);
                }
            }            

            if (_monkeyPosition.X > _screenRect.Width + 80)
                _increment *= -1;

            if (_currentSecond != game.TotalGameTime.Seconds)
            {
                _currentSecond = game.TotalGameTime.Seconds;
                //_showTrial = !_showTrial;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Boolean trial)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            
            _buttonPlay.Draw(spriteBatch, _font);
            _buttonOptions.Draw(spriteBatch, _font);
            _buttonInfo.Draw(spriteBatch, _font);
            //_buttonExit.Draw(spriteBatch, _font);
            _buttonHighscore.Draw(spriteBatch, _font);
            _buttonAchievements.Draw(spriteBatch, _font);

          //  _buttonMore.Draw(spriteBatch, _font);

            SpriteEffects e = _increment > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            _monkey.DrawFrame(spriteBatch, _monkeyPosition, e);

            //_newsTicker.Draw(spriteBatch);

            if( trial )
                DrawShadowedString(spriteBatch, _largeFont, "Trial", new Vector2(600, 20), Color.Red);                    
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        public void Up()
        {
            if (_buttonPlay.Hover)
            {
                _buttonAchievements.Hover = true;
                _buttonPlay.Hover = false;
            }
            else if (_buttonOptions.Hover)
            {
                _buttonPlay.Hover = true;
                _buttonOptions.Hover = false;
            }
            else if (_buttonInfo.Hover)
            {
                _buttonOptions.Hover = true;
                _buttonInfo.Hover = false;
            }
            else if (_buttonHighscore.Hover)
            {
                _buttonInfo.Hover = true;
                _buttonHighscore.Hover = false;
            }
            else
            {
                _buttonAchievements.Hover = false;
                _buttonHighscore.Hover = true;
            }            
        }

        public void Down()
        {
            if (_buttonPlay.Hover)
            {
                _buttonOptions.Hover = true;
                _buttonPlay.Hover = false;
            }
            else if (_buttonOptions.Hover)
            {
                _buttonInfo.Hover = true;
                _buttonOptions.Hover = false;
            }
            else if (_buttonInfo.Hover)
            {
                _buttonHighscore.Hover = true;
                _buttonInfo.Hover = false;
            }
            else if (_buttonHighscore.Hover)
            {
                _buttonAchievements.Hover = true;
                _buttonHighscore.Hover = false;
            }
            else
            {
                _buttonPlay.Hover = true;
                _buttonAchievements.Hover = false;
            }            
        }


        T LoadLocalizedAsset<T>(string assetName)
        {
            string[] cultureNames =
            {
                CultureInfo.CurrentCulture.Name,                        // eg. "en-US"
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName     // eg. "en"
            };

            // Look first for a specialized language-country version of the asset,
            // then if that fails, loop back around to see if we can find one that
            // specifies just the language without the country part.
            foreach (string cultureName in cultureNames)
            {
                string localizedAssetName = assetName + '.' + cultureName;

                try
                {
                    return _gameMain.Content.Load<T>(localizedAssetName);
                }
                catch (ContentLoadException) { }
            }

            // If we didn't find any localized asset, fall back to the default name.
            return _gameMain.Content.Load<T>(assetName);
        }
    }
}
