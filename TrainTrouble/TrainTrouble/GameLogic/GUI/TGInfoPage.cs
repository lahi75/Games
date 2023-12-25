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
    class TGInfoPage
    {
        TGButton _buttonExit;
        ContentManager Content;

        TGLinkLabel _howtoLabel;
        TGLinkLabel _emailLabel;
        TGLinkLabel _soundjayLabel;
        TGLinkLabel _banjrLabel;

        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;

        SpriteFont _smallFont;
        SpriteFont _largeFont;
        SpriteFont _mediumFont;

        String _version;

        public enum Result
        {            
            exit,
            noresult
        }

        public TGInfoPage(IServiceProvider serviceProvider, Rectangle screenRect)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");

            Content = content;

            _screenRect = screenRect;         

            _buttonExit = new TGButton(content, Resources.back, false,false);

            int y = 430;
                

            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            _font = content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = content.Load<SpriteFont>("fonts/largeFont");
            _mediumFont = content.Load<SpriteFont>("fonts/mediumFont");
            _smallFont = content.Load<SpriteFont>("fonts/smallFont");
            
            _background = content.Load<Texture2D>("background/small");
        }

        public void SetVersion(String s)
        {
            _version = s;
        }

        public Result Update(Point mousePosition, bool mouseDown)
        {            
            if (_buttonExit.Update(mousePosition, mouseDown))
            {        
                return Result.exit;
            }
            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);   

            Vector2 position = new Vector2(80, 20);

            DrawShadowedString(spriteBatch, _largeFont, Resources.infoHeader, position, new Color(25,87,168,255));

            int tabX = LargestLabel();

            position.X = 10;
            position.Y = 140;

            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowtoLabel, position, Color.Crimson);

            
            position.X += tabX + 10;

            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowToPassenger, position, new Color(25,87,168,255));            
            position.Y = 170;
            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowToMoney, position, new Color(25,87,168,255));
            position.Y = 200;
            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowToSwitch, position, new Color(25,87,168,255));
            position.Y = 230;
            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowToStop, position, new Color(25,87,168,255));            
            position.Y = 260;
            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowtoBoost, position, new Color(25,87,168,255));
            position.Y = 290;
            DrawShadowedString(spriteBatch, _smallFont, Resources.infoHowtoEnd, position, new Color(25,87,168,255));                       

            position.X = 10;
            position.Y = 330;

            DrawShadowedString(spriteBatch, _smallFont, Resources.infoVersion, position, Color.Crimson);
            position.X += tabX + 10;
            DrawShadowedString(spriteBatch, _smallFont, _version, position, new Color(25,87,168,255));
                                               
            _buttonExit.Draw(spriteBatch, _font);            
        }

        private int LargestLabel()
        {
            int max = 0;
            
            max = Math.Max(max, (int)_smallFont.MeasureString(Resources.infoContactLabel).X);
            max = Math.Max(max, (int)_smallFont.MeasureString(Resources.infoHowtoLabel).X);
            max = Math.Max(max, (int)_smallFont.MeasureString(Resources.infoAudioLabel).X);
            max = Math.Max(max, (int)_smallFont.MeasureString(Resources.infoVersion).X);
            max = Math.Max(max, (int)_smallFont.MeasureString(Resources.infoMusicLabel).X);

            return max;
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
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
