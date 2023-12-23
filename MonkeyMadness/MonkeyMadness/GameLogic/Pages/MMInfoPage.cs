using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;

#if WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace MonkeyMadness
{
    class MMInfoPage
    {
        MMButton _buttonExit;

        //MMLinkLabel _howtoLabel;
        //MMLinkLabel _emailLabel;
        //MMLinkLabel _soundjayLabel;

        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;

        SpriteFont _smallFont;
        SpriteFont _largeFont;
        SpriteFont _mediumFont;

        Game _gameMain;

        String _version;

        public enum Result
        {            
            exit,
            noresult
        }

        public MMInfoPage(Game gameMain, Rectangle screenRect)
        {
             _screenRect = screenRect;

            //_emailLabel = new MMLinkLabel(gameMain, "Contact", "void@void.com", "Monkey Madness", Color.Gold, Color.Red, MMLinkLabel.Task.Email); ;
            // _howtoLabel = new MMLinkLabel(gameMain, "Howto", "Howtocontent" + "/monkey", "", Color.Gold, Color.Red, MMLinkLabel.Task.Webbrowser);
            // _soundjayLabel = new MMLinkLabel(gameMain, "Audio", "Audiolink", "", Color.Gold, Color.Red, MMLinkLabel.Task.Webbrowser);

            _buttonExit = new MMButton(gameMain, Properties.Resources.strBack, false,false);

            _gameMain = gameMain;

            int y = 420;

            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            _font = gameMain.Content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            _mediumFont = gameMain.Content.Load<SpriteFont>("fonts/mediumFont");
            _smallFont = gameMain.Content.Load<SpriteFont>("fonts/smallFont");
            
            _background = gameMain.Content.Load<Texture2D>("background/form_aux");


#if WINDOWS_PHONE
            _rateButton = new JJButton(gameMain, "", false, false);
            _rateButton.DefaultTexture = _rateButton.HoverTexture = _rateButton.PressedTexture = _rateButton.HoverPressedTexture = LoadLocalizedAsset<Texture2D>("buttons/rate_game");
            _rateButton.CenterPosition(new Vector2(700, 400));
#endif
        }

        public void SetVersion(String s)
        {
            _version = s;
        }

        public Result Update(Point mousePosition, bool mouseDown)
        {
            //_emailLabel.Update(mousePosition, mouseDown);
            //_howtoLabel.Update(mousePosition, mouseDown);
            //_soundjayLabel.Update(mousePosition, mouseDown);

            if (_buttonExit.Update(mousePosition, mouseDown))
            {        
                return Result.exit;
            }

//#if WINDOWS_PHONE
//            if (_rateButton.Update(mousePosition, mouseDown))
//            {
//                try
//                {
//                    MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
//                    marketplaceReviewTask.Show();
//                }
//                catch (Exception e)
//                {
//                    Console.Write(e.Message);
//                }
//            }
//#endif

            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);   

            Vector2 position = new Vector2(80, 20);

            DrawShadowedString(spriteBatch, _largeFont, Properties.Resources.strInfoHeader, position, Color.Gold);

            int tabX = LargestLabel();            

            // contact label            
            position.X = 50; 
            position.Y = 100;

            //DrawShadowedString(spriteBatch, _smallFont, "Contact", position, Color.LightBlue);

            position.X += tabX;            

            //_emailLabel.SetPosition(position);
            //_emailLabel.Draw(spriteBatch);

//#if WINDOWS_PHONE
//            _rateButton.Draw(spriteBatch, _font);
//#endif

            // hotwo label            
            position.X = 50; 
            position.Y = 140;

           // DrawShadowedString(spriteBatch, _smallFont, "Howto", position, Color.LightBlue);

            position.X += tabX;

//#if WINDOWS_PHONE
//            DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoTilt, position, Color.Gold);            
//#elif XBOX
//            DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoXBoxRun, position, Color.Gold);               
//#endif
//            position.Y = 170;

//            DrawShadowedString(spriteBatch, _smallFont, "Leave", position, Color.Gold);

//            position.Y = 200;

//#if WINDOWS_PHONE
//            DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoTap, position, Color.Gold);
//#elif XBOX
//            DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoXboxJump, position, Color.Gold);            
//#endif
//            position.Y = 230;

//            DrawShadowedString(spriteBatch, _smallFont, "Hint", position, Color.Gold);            

//            position.Y = 260;

            //DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoHowtoContent, position, Color.Gold);

            //_howtoLabel.SetPosition(position);
            //_howtoLabel.Draw(spriteBatch);


            // version label
            //s = Resource1.strInfoGraphicsLabel + Resource1.strInfoGraphicsContent;            

            position.X = 50;
            position.Y = 150;

            

            DrawShadowedString(spriteBatch, _smallFont, Properties.Resources.strInfoVersion, position, Color.LightBlue);

            position.X += tabX;

            DrawShadowedString(spriteBatch, _smallFont, _version, position, Color.Gold);

            // audio label            

            position.X = 50;
            position.Y = 340;

            //DrawShadowedString(spriteBatch, _smallFont, "Audio", position, Color.LightBlue);

            position.X += tabX;

            //_soundjayLabel.SetPosition(position);
            //_soundjayLabel.Draw(spriteBatch);

            //DrawShadowedString(spriteBatch, _smallFont, Resource1.strInfoAudioContent, position, Color.Gold);
           
            _buttonExit.Draw(spriteBatch, _font);            
        }

        private int LargestLabel()
        {
            int max = 0;
            
            max = Math.Max(max, (int)_smallFont.MeasureString(Properties.Resources.strInfoVersion).X);
            //max = Math.Max(max, (int)_smallFont.MeasureString("Contact").X);
            //max = Math.Max(max, (int)_smallFont.MeasureString("Contact").X);
            // max = Math.Max(max, (int)_smallFont.MeasureString("Contact").X);

            return max + 10;
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
                    return _gameMain.Content.Load<T>(localizedAssetName);
                }
                catch (ContentLoadException) { }
            }

            // If we didn't find any localized asset, fall back to the default name.
            return _gameMain.Content.Load<T>(assetName);
        }

    }
}
