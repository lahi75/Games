using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LaserLeisure.Properties;

namespace LLGameLibrary
{
    class LLDialogBomb : LLDialogBox
    {
        SpriteFont _font18;
        SpriteFont _font21;

        LLLightButton _resetBtn;
        LLLightButton _quitBtn;

        LLAnimatedTexture _texture;

        public enum Result
        {
            ongoing,
            quit,
            reset
        }

        public LLDialogBomb(ContentManager content, Rectangle rectangle)
            : base(content, rectangle)
        {
            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _resetBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnReset);
            _quitBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnQuit);

            _texture = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            _texture.Load(_content, "objects/bomb", 16, 16);            

            _resetBtn.Position(new Point(_rectangle.Center.X - 100, _rectangle.Bottom - 30));
            _quitBtn.Position(new Point(_rectangle.Center.X + 100, _rectangle.Bottom - 30));
        }

        public Result Update(Point position, Boolean clickDown)
        {
            if (_resetBtn.Update(position, clickDown))
                return Result.reset;
            if (_quitBtn.Update(position, clickDown))
                return Result.quit;

            return Result.ongoing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            _texture.DrawFrame(spriteBatch, new Vector2(_rectangle.Right - 80, _rectangle.Center.Y - 30), SpriteEffects.None);

            String s = Resources.dlgNotCompleted;

            Vector2 p = new Vector2(_rectangle.Center.X - _font21.MeasureString(s).X / 2, _rectangle.Top + 10);
            spriteBatch.DrawString(_font21, s, p, Color.White);
            
            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 70);

            spriteBatch.DrawString(_font18, Resources.dlgBomb, p, Color.White);
            
            _resetBtn.Draw(spriteBatch);
            _quitBtn.Draw(spriteBatch);
        }
    }
}
