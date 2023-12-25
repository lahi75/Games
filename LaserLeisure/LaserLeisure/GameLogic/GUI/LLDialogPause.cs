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
    class LLDialogPause : LLDialogBox
    {
        SpriteFont _font18;
        SpriteFont _font21;

        LLLightButton _resumeBtn;
        LLLightButton _quitBtn;

        public enum Result
        {
            ongoing,
            quit,
            resume
        }

        public LLDialogPause(ContentManager content, Rectangle rectangle)
            : base(content, rectangle)
        {
            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _resumeBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnResume);
            _quitBtn = new LLLightButton(content, new Rectangle(0, 0, 100, 40), Resources.btnQuit);


            _resumeBtn.Position(new Point(_rectangle.Center.X - 100, _rectangle.Bottom - 50));
            _quitBtn.Position(new Point(_rectangle.Center.X + 100, _rectangle.Bottom - 50));
        }

        public Result Update(Point position, Boolean clickDown)
        {
            if (_resumeBtn.Update(position, clickDown))
                return Result.resume;

            if (_quitBtn.Update(position, clickDown))
                return Result.quit;

            return Result.ongoing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            String s = Resources.dlgPause;

            Vector2 p = new Vector2(_rectangle.Center.X - _font21.MeasureString(s).X / 2, _rectangle.Top + 30);

            spriteBatch.DrawString(_font21, s, p, Color.White);            

            _resumeBtn.Draw(spriteBatch);
            _quitBtn.Draw(spriteBatch);
        }
    }
}
