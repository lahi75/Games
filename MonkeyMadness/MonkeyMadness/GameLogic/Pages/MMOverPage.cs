using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyMadness
{
    class MMOverPage
    {
        MMButton _buttonContinue;
        SpriteFont _medium;
        SpriteFont _font;
        Rectangle _screenRect;
        Rectangle _titleSafe;

        float _startTime;           // starttime of a bonus
        int _currentTime;

        MMPoints _points;
        int _cumulatedPoints = 0;    

        public enum Result
        {
            exit,
            noresult
        }

        public MMOverPage(Game gameMain, Rectangle screenRect, Rectangle titleSafe)
        {
            _buttonContinue = new MMButton(gameMain, "Go", false, false);

            _buttonContinue.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 380));

            _font = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            _medium = gameMain.Content.Load<SpriteFont>("fonts/mediumFont");

            _screenRect = screenRect;
            _titleSafe = titleSafe;
        }

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown, MMPoints points, int cumulatedPoints)
        {
            _points = points;
            _cumulatedPoints = cumulatedPoints;

            if (_startTime == 0)
            {
                _startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
                MMFxManager.Fx.PlayGameOver();
            }

            _currentTime = (int)(gameTime.TotalGameTime.TotalMilliseconds - _startTime);

            if (_buttonContinue.Update(mousePosition, mouseDown))
                return Result.exit;            

            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawShadowedString(spriteBatch, _font, "Game Over", new Vector2(_screenRect.Width / 2 - _font.MeasureString("Game Over").X / 2, 100), Color.Red);

            string s = "Points" + " " + (_cumulatedPoints + _points.Points);
            string sHelper = "Points" + " ?????";

            DrawShadowedString(spriteBatch, _medium, s, new Vector2(_screenRect.Width / 2 - _medium.MeasureString(sHelper).X / 2, 200), Color.Red);

            if (_currentTime > 2000)
            {
                _buttonContinue.Hover = true;
                _buttonContinue.Draw(spriteBatch, _medium);
            }
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
