using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;


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
            _buttonPlay.CenterPosition(new Vector2(screenRect.Width/2, 100));

            _buttonOptions = new LLButton(content, Resources.optionHeader, false, false);
            _buttonOptions.CenterPosition(new Vector2(screenRect.Width/2, 170));

            _buttonInstructions = new LLButton(content, Resources.instHeader, false, false);
            _buttonInstructions.CenterPosition(new Vector2(screenRect.Width / 2, 240));

            _buttonHighscore = new LLButton(content, Resources.highscoreHeader, false, false);
            _buttonHighscore.CenterPosition(new Vector2(screenRect.Width/2,310));

            _buttonInfo = new LLButton(content, Resources.dlgInfo, false, false);
            _buttonInfo.CenterPosition(new Vector2(screenRect.Width / 2, 380));
                      
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
