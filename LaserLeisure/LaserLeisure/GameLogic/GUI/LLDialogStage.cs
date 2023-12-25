using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLDialogStage : LLDialogBox
    {
        SpriteFont _font18;
        SpriteFont _font21;
        
        LLLightButton _quitBtn;

        Texture2D _texture;

        int _bonus;
        int _tools;
        int _current;
        int _time;

        public enum Result
        {
            ongoing,
            quit
        }

        public LLDialogStage(ContentManager content, Rectangle rectangle, int bonus, int tools, int current, int time)
            : base(content, rectangle)
        {
            _bonus = bonus;
            _tools = tools;
            _current = current;
            _time = time;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _texture = _content.Load<Texture2D>("icons/trophy");
            
            _quitBtn = new LLLightButton(content, new Rectangle(0, 0, 120, 40), Resources.btnQuit);
            
            _quitBtn.Position(new Point(_rectangle.Center.X, _rectangle.Bottom - 30));
        }

        public Result Update(Point position, Boolean clickDown)
        {          
            if (_quitBtn.Update(position, clickDown))
                return Result.quit;

            return Result.ongoing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(_texture, new Vector2(_rectangle.Right - 100, _rectangle.Center.Y - _texture.Height/2), Color.White);

            String s = Resources.dlgAllDone;

            // header
            Vector2 p = new Vector2(_rectangle.Center.X - _font21.MeasureString(s).X / 2, _rectangle.Top + 10);
            spriteBatch.DrawString(_font21, s, p, Color.White);

            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 50);
            spriteBatch.DrawString(_font18, Resources.dlgBonus, p, Color.White);

            p = new Vector2(_rectangle.Left + 280, _rectangle.Top + 50);
            spriteBatch.DrawString(_font21, _bonus.ToString(), p, Color.White);

            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 90);
            spriteBatch.DrawString(_font18, Resources.dlgTools, p, Color.White);

            p = new Vector2(_rectangle.Left + 280, _rectangle.Top + 90);
            spriteBatch.DrawString(_font21, "-" + _tools.ToString(), p, Color.White);

            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 130);
            spriteBatch.DrawString(_font18, Resources.dlgTime, p, Color.White);

            p = new Vector2(_rectangle.Left + 280, _rectangle.Top + 130);
            spriteBatch.DrawString(_font21, _time.ToString(), p, Color.White);

            p = new Vector2(_rectangle.Left + 30, _rectangle.Top + 170);
            spriteBatch.DrawString(_font18, Resources.dlgTotal, p, Color.White);

            p = new Vector2(_rectangle.Left + 280, _rectangle.Top + 170);
            spriteBatch.DrawString(_font21, _current.ToString(), p, Color.White);
            
            _quitBtn.Draw(spriteBatch);
        }
    }
}
