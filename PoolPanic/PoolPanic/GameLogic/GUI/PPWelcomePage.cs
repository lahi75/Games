using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;



namespace PoolPanic
{
    class PPWelcomePage
    {
        SpriteFont _largeFont;

        PPButton _play8;
        PPButton _play9;
        PPButton _exit;        
                        
        Rectangle _screenRect;

        Texture2D _background;
        SpriteFont _font;
        SpriteFont _smallFont;
        
        ContentManager Content;
        

        public enum WelcomeResult
        {
            play8,
            play9,           
            exit,
            noresult
        }

        public PPWelcomePage(IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe, string language)
        {
            Content = new ContentManager(serviceProvider, "Content");

            _play8 = new PPButton(Content, "Play 8", false,false);
            _play8.CenterPosition(new Vector2(screenRect.Width / 2, 100));
            _play9 = new PPButton(Content, "Play 9", false, false);
            _play9.CenterPosition(new Vector2(screenRect.Width / 2, 200));
            _exit = new PPButton(Content, "Exit", false, false);
            _exit.CenterPosition(new Vector2(screenRect.Width / 2, 300));

            _smallFont = Content.Load<SpriteFont>("fonts/smallFont");
            _font = Content.Load<SpriteFont>("fonts/ButtonFont");
            _background = Content.Load<Texture2D>("background/main");
                                    
            _screenRect = screenRect;

            _largeFont = Content.Load<SpriteFont>("fonts/largeFont");

        }               
        

        public WelcomeResult Update(GameTime game, Point mousePosition, bool mouseDown)
        {            
            if (_play8.Update(mousePosition, mouseDown))
                return WelcomeResult.play8;

            if (_play9.Update(mousePosition, mouseDown))
                return WelcomeResult.play9;

            if (_exit.Update(mousePosition, mouseDown))
                return WelcomeResult.exit;
           
            return WelcomeResult.noresult;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            _play8.Draw(spriteBatch, _font);
            _play9.Draw(spriteBatch, _font);
            _exit.Draw(spriteBatch, _font);                                    
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }        
    }
}
