using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MonkeyMadness
{
    class MMLevelPage
    {
        MMButton _buttonContinue;

        TimeSpan _levelTime = TimeSpan.Zero;
        SpriteFont _medium;
        SpriteFont _font;
        float _startTime;         
        int _currentTime;
        int _lastTime;
        Rectangle _screenRect;
        Rectangle _titleSafe;      
        Int32 _bonusPoints = 100;        
        MMPoints _points;
        int _cumulatedPoints = 0;
        int _level;

        public int Score { get { return _cumulatedPoints + _points.Points; } }

        public enum Result
        {
            exit,
            noresult
        }

        public MMLevelPage(Game gameMain, Rectangle screenRect, Rectangle titleSafe)
        {
            _buttonContinue = new MMButton(gameMain, Properties.Resources.strLevelGo, false, false);

            _buttonContinue.CenterPosition(new Vector2(titleSafe.Width / 2 + titleSafe.X, 360));

            _font = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            _medium = gameMain.Content.Load<SpriteFont>("fonts/mediumFont");
         //   _background = gameMain.Content.Load<Texture2D>("info_background");
            _screenRect = screenRect;
            _titleSafe = titleSafe;
            Reset(1);            
        }

        public Boolean Counting()
        {
            return _bonusPoints > 0;
        }

        public void Finish()
        {
            _points.Points += _bonusPoints;
            MMFxManager.Fx.PlayPointsNoise(false);
        }

        public void Reset(Int32 level)
        {            
            _startTime = 0;
            _lastTime = 0;
            _bonusPoints = 100 + level * 20; // each level gives more bonus points
            _level = level;
            _levelTime = TimeSpan.Zero;
        }        

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown,  MMPoints points, int cumulatedPoints, TimeSpan levelTime)
        {            
            _points = points;
            _cumulatedPoints = cumulatedPoints;

            if (_startTime == 0)
                _startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            if (_levelTime == TimeSpan.Zero)
                _levelTime = levelTime;

            _currentTime = (int)(gameTime.TotalGameTime.TotalMilliseconds - _startTime);

            if (_buttonContinue.Update(mousePosition, mouseDown))
                return Result.exit;

            if (_bonusPoints > 0)
            {                
                if (_currentTime > 2000)
                {
                    if (_currentTime > _lastTime + 50)
                    {
                        MMFxManager.Fx.PlayPointsNoise(true);

                        int increment = (int)(_bonusPoints / 100) + 1;

                        _points.Points += increment;
                        _bonusPoints -= increment;
                        _lastTime = _currentTime;
                    }
                }
            }
            else
            {
                MMFxManager.Fx.PlayPointsNoise(false);
            }

            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch, bool trial)
        {
            //spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            if (trial)
            {
                _bonusPoints = 0;

                //DrawShadowedString(spriteBatch, _medium, "More Levels", new Vector2(_screenRect.Width / 2 - _medium.MeasureString("More Levels").X / 2, 90), Color.Red);
                //DrawShadowedString(spriteBatch, _medium, "More Monsters", new Vector2(_screenRect.Width / 2 - _medium.MeasureString("More Monsters").X / 2, 140), Color.Red);
                //DrawShadowedString(spriteBatch, _medium, "Zero Ads", new Vector2(_screenRect.Width / 2 - _medium.MeasureString("Zero Ads").X / 2, 190), Color.Red);
                //DrawShadowedString(spriteBatch, _medium, "Buy Monkey Madness on Marketplace", new Vector2(_screenRect.Width / 2 - _medium.MeasureString("Buy Monkey Madness on Marketplace").X / 2, 270), Color.Red);                
            }
            else
            {
                int x = 100;

                String s = Properties.Resources.strConcrats;

                DrawShadowedString(spriteBatch, _font, s, new Vector2(_screenRect.Width / 2 - _font.MeasureString(s).X / 2, x), Color.Red);

                x += 80;

                s = Properties.Resources.strLevel + " " + _level;

                DrawShadowedString(spriteBatch, _medium, s, new Vector2(_screenRect.Width / 2 - _medium.MeasureString(s).X / 2, x), Color.Red);

                x += 40;

                String t = "";

                // draw the minutes only when required
                if (_levelTime.Minutes > 0)
                    t = _levelTime.Minutes.ToString("00") + ":";
                
                t += _levelTime.Seconds.ToString("00");
                
                s = Properties.Resources.strTime + " " + t;

                DrawShadowedString(spriteBatch, _medium, s, new Vector2(_screenRect.Width / 2 - _medium.MeasureString(s).X / 2, x), Color.Red);

                x += 40;

                s = Properties.Resources.strPoints + " " + (_cumulatedPoints + _points.Points);
                String sHelper = Properties.Resources.strPoints + " ????";

                DrawShadowedString(spriteBatch, _medium, s, new Vector2((int)(_screenRect.Width / 2 - _medium.MeasureString(sHelper).X / 2), x), Color.Red);
            }

            if (_bonusPoints == 0)
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
