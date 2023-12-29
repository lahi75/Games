using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PoolPanic
{
    class PPWelcomePage
    {
        SpriteFont _largeFont;

        PPButton _play8;
        PPButton _play9;
        PPButton _exit;

        PPLabel _versionLabel;        

        Texture2D _background;
        SpriteFont _font;        
        
        ContentManager Content;
        
        public enum WelcomeResult
        {
            play8,
            play9,           
            exit,
            noresult
        }

        public PPWelcomePage(IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {
            Content = new ContentManager(serviceProvider, "Content");

            _play8 = new PPButton(Content, "Play 8", false,false);
            _play8.CenterPosition(new Vector2(screenRect.Width / 2 + 100, 400));
            _play9 = new PPButton(Content, "Play 9", false, false);
            _play9.CenterPosition(new Vector2(screenRect.Width / 2 + 100, 600));
            _exit = new PPButton(Content, "Exit", false, false);
            _exit.CenterPosition(new Vector2(screenRect.Width / 2 + 100, 800));

            _versionLabel = new PPLabel(Content);
            _versionLabel.CenterPosition(new Vector2(1650, 1100));

            _font = Content.Load<SpriteFont>("fonts/ButtonFont");            
            _largeFont = Content.Load<SpriteFont>("fonts/largeFont");
            _background = Content.Load<Texture2D>("background/main");
        }

        public void SetVersion(string s)
        {
            _versionLabel.SetText(s);
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
            _versionLabel.Draw(spriteBatch);
        }     
    }
}
