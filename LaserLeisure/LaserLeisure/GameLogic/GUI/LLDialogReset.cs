using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLDialogReset : LLDialogBox
    {
        SpriteFont _font18;
        SpriteFont _font25;

        LLLightButton _resetBtn;
        LLLightButton _dontBtn;

        Texture2D _texture;

        public enum Result
        {
            ongoing,
            reset,
            dont
        }

        public LLDialogReset(ContentManager content, Rectangle rectangle)
            : base(content, rectangle)
        {

            _texture = _content.Load<Texture2D>("icons/warning");

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font25 = content.Load<SpriteFont>("fonts/tycho_25");

            _resetBtn = new LLLightButton(content, new Rectangle(0, 0, 180, 40), Resources.btnReset);
            _dontBtn = new LLLightButton(content, new Rectangle(0, 0, 180, 40), Resources.btnDontReset);

            _resetBtn.Position(new Point(_rectangle.Center.X - 100, _rectangle.Bottom - 130));
            _dontBtn.Position(new Point(_rectangle.Center.X + 100, _rectangle.Bottom - 130));
        }

        public Result Update(Point position, Boolean clickDown)
        {
            if (_resetBtn.Update(position, clickDown))
                return Result.reset;

            if (_dontBtn.Update(position, clickDown))
                return Result.dont;

            return Result.ongoing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(_texture, new Vector2(_rectangle.Right - 100, _rectangle.Center.Y - _texture.Height / 2 - 20), Color.White);
            spriteBatch.Draw(_texture, new Vector2(_rectangle.Left + 60, _rectangle.Center.Y - _texture.Height / 2 - 20), Color.White);

            String s = Resources.dlgResetLadder;

            Vector2 p = new Vector2(_rectangle.Center.X - _font25.MeasureString(s).X / 2, _rectangle.Top + 200);

            spriteBatch.DrawString(_font25, s, p, Color.White);

            _resetBtn.Draw(spriteBatch);
            _dontBtn.Draw(spriteBatch);
        }
    }
}
