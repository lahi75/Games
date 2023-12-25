using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Globalization;
using LaserLeisure.Properties;

#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace LLGameLibrary
{
    class LLWelcomePage
    {
        SpriteFont _font18;
        
        LLButton _buttonPlay;
        LLButton _buttonOptions;
        LLButton _buttonInstructions;        
        LLButton _buttonHighscore;
        LLButton _buttonInfo;

        LLButton _buttonMore;

        //LLNewsTicker _newsTicker;        
                                  
        Rectangle _screenRect;
        Texture2D _background;        
        ContentManager _content;
        
        public enum WelcomeResult
        {
            play,
            options,
            info,
            instructions,
            exit,
            highscore,            
            noresult
        }

        public LLWelcomePage(ContentManager content, Rectangle screenRect, string language)
        {
            _content = content;

            
            _buttonPlay = new LLButton(content, Resources.welcomePlay, false,false);
            _buttonPlay.CenterPosition(new Vector2(screenRect.Width/2, 140));

            _buttonOptions = new LLButton(content, Resources.optionHeader, false, false);
            _buttonOptions.CenterPosition(new Vector2(screenRect.Width/2, 210));

            _buttonInstructions = new LLButton(content, Resources.instHeader, false, false);
            _buttonInstructions.CenterPosition(new Vector2(screenRect.Width / 2, 280));

            _buttonHighscore = new LLButton(content, Resources.highscoreHeader, false, false);
            _buttonHighscore.CenterPosition(new Vector2(screenRect.Width/2,350));

            _buttonInfo = new LLButton(content, Resources.dlgInfo, false, false);
            _buttonInfo.CenterPosition(new Vector2(screenRect.Width / 2, 420));

            //_newsTicker = new LLNewsTicker(content, new Point(0, 0), language);
            
            _buttonMore = new LLButton(content,"", false, false);
            _buttonMore.DefaultTexture = _buttonMore.HoverTexture = _buttonMore.PressedTexture = _buttonMore.HoverPressedTexture = LoadLocalizedAsset<Texture2D>("buttons/more_games");                                                                        
            _buttonMore.CenterPosition(new Vector2(screenRect.Width / 2 + screenRect.X + 270, 400));
            
            _background = content.Load<Texture2D>("backgrounds/welcome");
            _screenRect = screenRect;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
        }               
        

        public WelcomeResult Update(GameTime game, Point mousePosition, bool mouseDown)
        {                        
            if (_buttonPlay.Update(mousePosition, mouseDown))
                return WelcomeResult.play;
            
            if (_buttonOptions.Update(mousePosition, mouseDown))
                return WelcomeResult.options;

            if (_buttonInstructions.Update(mousePosition, mouseDown))
                return WelcomeResult.instructions;

            if (_buttonInfo.Update(mousePosition, mouseDown))
                return WelcomeResult.info;

            if (_buttonHighscore.Update(mousePosition, mouseDown))
                return WelcomeResult.highscore;

            //_newsTicker.Update(game,mousePosition,mouseDown);

            if (_buttonMore.Update(mousePosition, mouseDown))
            {
                // open website
                #if WINDOWS_PHONE
                try
                {
                    MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
                    marketplaceSearchTask.SearchTerms = "Phoebit";
                    marketplaceSearchTask.Show();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                #endif
                #if WINDOWS
                   System.Diagnostics.Process.Start(Resources.moreGamesURL);                                
                #endif
            }    

           
            return WelcomeResult.noresult;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            _buttonPlay.Draw(spriteBatch, _font18);            
            _buttonOptions.Draw(spriteBatch, _font18);
            _buttonInfo.Draw(spriteBatch, _font18);            
            _buttonHighscore.Draw(spriteBatch, _font18);
            _buttonInstructions.Draw(spriteBatch, _font18);            

            //_newsTicker.Draw(spriteBatch);            
            _buttonMore.Draw(spriteBatch, _font18);                     
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
                    return _content.Load<T>(localizedAssetName);
                }
                catch (ContentLoadException) { }
            }

            // If we didn't find any localized asset, fall back to the default name.
            return _content.Load<T>(assetName);
        }         
    }
}
