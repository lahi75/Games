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
    class LLDialogRepeat : LLDialogBox
    {
        SpriteFont _font18;
        SpriteFont _font21;

        LLLightButton _repeatBtn;
        LLLightButton _quitBtn;

        Texture2D _texture;

        public enum Result
        {
            ongoing,
            quit,
            repeat
        }

        public LLDialogRepeat(ContentManager content, Rectangle rectangle)
            : base(content, rectangle)
        {
            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _texture = _content.Load<Texture2D>("icons/deny");

            _repeatBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnRepeat);
            _quitBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnQuit);


            _repeatBtn.Position(new Point(_rectangle.Center.X - 100, _rectangle.Bottom - 30));
            _quitBtn.Position(new Point(_rectangle.Center.X + 100, _rectangle.Bottom - 30));
        }

        public Result Update(Point position, Boolean clickDown)
        {
            if( _repeatBtn.Update(position,clickDown))
                return Result.repeat;

            if( _quitBtn.Update(position,clickDown))
                return Result.quit;

            return Result.ongoing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(_texture, new Vector2(_rectangle.Right - 100, _rectangle.Center.Y - _texture.Height / 2), Color.White);

            String s = Resources.dlgNotCompleted;

            Vector2 p = new Vector2( _rectangle.Center.X - _font21.MeasureString(s).X/2, _rectangle.Top + 10);

            spriteBatch.DrawString(_font21, s, p, Color.White);

            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 110);

            spriteBatch.DrawString(_font18, Resources.dlgPenalty, p, Color.White);

            p = new Vector2(_rectangle.Left + 280, _rectangle.Top + 110);

            spriteBatch.DrawString(_font21, "-10", p, Color.White);

            _repeatBtn.Draw(spriteBatch);
            _quitBtn.Draw(spriteBatch);
        }
    }
}
