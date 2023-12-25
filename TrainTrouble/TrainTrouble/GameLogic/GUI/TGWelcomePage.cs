using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Globalization;
using TrainTrouble.Properties;

#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace TGGameLibrary
{
    class TGWelcomePage
    {
        SpriteFont _largeFont;

        TGButton _buttonPlay;
        TGButton _buttonOptions;
        TGButton _buttonInfo;        
        TGButton _buttonHighscore;        
                
        Rectangle _screenRect;

        Texture2D _background;
        SpriteFont _font;
        SpriteFont _smallFont;

        TGTrain _train;
        ContentManager Content;
        

        public enum WelcomeResult
        {
            play,
            options,
            info,
            exit,
            highscore,            
            noresult
        }

        public TGWelcomePage(IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe, string language)
        {
            Content = new ContentManager(serviceProvider, "Content");

            _buttonPlay = new TGButton(Content, Resources.play, false,false);
            _buttonOptions = new TGButton(Content, Resources.options, false, false);
            
            _buttonInfo = new TGButton(Content, Resources.info, false, false);
            _buttonHighscore = new TGButton(Content, Resources.highscore, false, false);            
            
            _buttonPlay.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 120));
            _buttonOptions.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 200));            
            _buttonInfo.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 280));
            _buttonHighscore.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 360));            

            _buttonPlay.Hover = true;

            _smallFont = Content.Load<SpriteFont>("fonts/smallFont");
            _font = Content.Load<SpriteFont>("fonts/ButtonFont");
            _background = Content.Load<Texture2D>("background/main");
                                    
            _screenRect = screenRect;

            _largeFont = Content.Load<SpriteFont>("fonts/largeFont");

            _train = new TGTrain(Content, null, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_red"), 35, 17, TGColor.Red);            
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

            _train.Move(game,false);

            // wrap around of the train
            if (_train.Wagon2TileX < -1)
            {
                _train = new TGTrain(Content, null, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_red"), 35, 17, TGColor.Red);
            }

            return WelcomeResult.noresult;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            
            _buttonPlay.Draw(spriteBatch, _font);
            _buttonOptions.Draw(spriteBatch, _font);
            _buttonInfo.Draw(spriteBatch, _font);            
            _buttonHighscore.Draw(spriteBatch, _font);                        

            _train.Draw(spriteBatch);
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
                _buttonInfo.Hover = true;
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
            else
            {
                _buttonInfo.Hover = true;
                _buttonHighscore.Hover = false;
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
            else
            {
                _buttonPlay.Hover = true;
                _buttonHighscore.Hover = false;
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
                    return Content.Load<T>(localizedAssetName);
                }
                catch (ContentLoadException) { }
            }

            // If we didn't find any localized asset, fall back to the default name.
            return Content.Load<T>(assetName);
        }
    }
}
