using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace LLGameLibrary
{
    class LLInfoPage
    {
        Texture2D _texture;

        Rectangle _screenRect;
        Texture2D _background;
        ContentManager _content;

        SpriteFont _font18;
        SpriteFont _font21;

        String _version = "";

        public enum Result
        {
            exit,
            noresult
        }

        //LLLinkLabel _howtoLabel;
        //LLLinkLabel _emailLabel;
        //LLLinkLabel _soundjayLabel;

#if WINDOWS_PHONE
        LLButton _rateButton;
#endif


        public LLInfoPage(ContentManager content, Rectangle screenRect)
        {
            _content = content;

            _background = content.Load<Texture2D>("backgrounds/default");

            _texture = content.Load<Texture2D>("globe");

            _screenRect = screenRect;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            //_emailLabel = new LLLinkLabel(content, Resources.infoContactContent, Resources.infoContactContent, "Laser Leisure", Color.Yellow, Color.Yellow, LLLinkLabel.Task.Email);
            //_howtoLabel = new LLLinkLabel(content, Resources.infoHowtoContent, Resources.infoHowtoContent + "/laser", "", Color.Yellow, Color.Yellow, LLLinkLabel.Task.Webbrowser);
            //_soundjayLabel = new LLLinkLabel(content, Resources.infoAudioContent, Resources.infoAudioContent, "", Color.Yellow, Color.Yellow, LLLinkLabel.Task.Webbrowser);

        }

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown)
        {
            //_emailLabel.Update(mousePosition, mouseDown);
            //_howtoLabel.Update(mousePosition, mouseDown);
            //_soundjayLabel.Update(mousePosition, mouseDown);

            return Result.noresult;
        }

        public void SetVersion(String s)
        {
            _version = s;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with background tiles 
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            spriteBatch.Draw(_texture, new Vector2(230, 20), Color.White);

            Vector2 position = new Vector2(20, 20);

            // draw page caption
            spriteBatch.DrawString(_font21, Resources.dlgInfo, position, Color.White);
            
            int tabX = LargestLabel();

            // version label            
            position.X = 30;
            position.Y = 150;

            spriteBatch.DrawString(_font18, Resources.infoVersion, position, Color.White);

            position.X += tabX + 20;

            spriteBatch.DrawString(_font18, _version, position, Color.White);


            // contact label            
            //position.X = 30;
            //position.Y = 150;

            //spriteBatch.DrawString( _font18, Resources.infoContactLabel, position, Color.White);

            //position.X += tabX + 20;

            ////_emailLabel.SetPosition(position);
            ////_emailLabel.Draw(spriteBatch);            


            //// webpage label
            //position.X = 30;
            //position.Y = 230;

            //spriteBatch.DrawString(_font18, Resources.infoHowtoLabel, position, Color.White);

            //position.X += tabX + 20;

            ////_howtoLabel.SetPosition(position);
            ////_howtoLabel.Draw(spriteBatch);


            //// audio label            

            //position.X = 30;
            //position.Y = 310;

            //spriteBatch.DrawString(_font18, Resources.infoAudioLabel, position, Color.White);

            //position.X += tabX + 20;

            //_soundjayLabel.SetPosition(position);
            //_soundjayLabel.Draw(spriteBatch);
        }

        private int LargestLabel()
        {
            int max = 0;

            max = Math.Max(max, (int)_font18.MeasureString(Resources.infoContactLabel).X);            
            max = Math.Max(max, (int)_font18.MeasureString(Resources.infoAudioLabel).X);
            max = Math.Max(max, (int)_font18.MeasureString(Resources.infoVersion).X);
            max = Math.Max(max, (int)_font18.MeasureString(Resources.infoHowtoLabel).X);

            return max;
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
