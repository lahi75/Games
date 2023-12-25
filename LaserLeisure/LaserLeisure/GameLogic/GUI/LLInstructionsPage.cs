using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLInstructionsPage
    {
        double _debounceStart = 0;

        Texture2D _texture;

        Rectangle _screenRect;
        Texture2D _background;
        ContentManager _content;

        SpriteFont _fontContent;
        SpriteFont _font18;
        SpriteFont _font21;

        bool _back = false;
        bool _isTouched = false;

        LLButton _buttonGeneral;
        LLButton _buttonLaser;
        LLButton _buttonTools;
        LLButton _buttonObstacles;
        LLButton _buttonPoints;

        String _headerText = "";
        Texture2D _t1 = null;
        Texture2D _t2 = null;
        Texture2D _t3 = null;
        Texture2D _t4 = null;
        Texture2D _t5 = null;        
        String _line1 = "";
        String _line2 = "";
        String _line3 = "";
        String _line4 = "";
        String _line5 = "";
        String _line11 = null;
        String _line21 = null;
        String _line31 = null;
        String _line41 = null;
        String _line51 = null;

        InstructionsState _state = InstructionsState.content;

        public enum Result
        {
            exit,
            noresult
        }

        enum InstructionsState
        {
            content,
            general,
            laser,
            tools,
            obstacles,
            points
        }

        public LLInstructionsPage(ContentManager content, Rectangle screenRect)
        {
            _content = content;

            _background = content.Load<Texture2D>("backgrounds/default");

            _texture = content.Load<Texture2D>("icons/cap");

            _screenRect = screenRect;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");
            _fontContent = content.Load<SpriteFont>("fonts/tycho_18");
          //  _fontContent = content.Load<SpriteFont>("fonts/contentFont");


            _buttonGeneral = new LLButton(content, Resources.instGeneral, false, false);
            _buttonGeneral.CenterPosition(new Vector2(_screenRect.Center.X - 100, 140));

            _buttonLaser = new LLButton(content, Resources.instLaser, false, false);
            _buttonLaser.CenterPosition(new Vector2(_screenRect.Center.X - 50, 210));

            _buttonTools = new LLButton(content, Resources.instTools, false, false);
            _buttonTools.CenterPosition(new Vector2(_screenRect.Center.X, 280));

            _buttonObstacles = new LLButton(content, Resources.instObstacles, false, false);
            _buttonObstacles.CenterPosition(new Vector2(_screenRect.Center.X + 50, 350));

            _buttonPoints = new LLButton(content, Resources.instPoints, false, false);
            _buttonPoints.CenterPosition(new Vector2(_screenRect.Center.X + 100, 420));            
        }

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown)
        {            
            if (_back)
            {
                _back = false;

                if (_state == InstructionsState.content)
                    return Result.exit;
                else
                    _state = InstructionsState.content;
            }

            if (_state == InstructionsState.content)
            {
                if (_buttonGeneral.Update(mousePosition, mouseDown))
                {
                    _state = InstructionsState.general;
                    _t1 = _content.Load<Texture2D>("instructions/placeholder");
                    _t2 = _content.Load<Texture2D>("instructions/general_1");
                    _t3 = _content.Load<Texture2D>("instructions/placeholder");
                    _t4 = _content.Load<Texture2D>("instructions/placeholder");
                    _t5 = _content.Load<Texture2D>("instructions/placeholder");
                    _headerText = Resources.instGeneral;
                    _line1 = Resources.instGeneralLine1;
                    _line11 = TrimString(ref _line1, 600);
                    _line2 = Resources.instGeneralLine2;
                    _line21 = TrimString(ref _line2, 600);
                    _line3 = Resources.instGeneralLine3;
                    _line31 = TrimString(ref _line3, 600);
                    _line4 = Resources.instGeneralLine4;
                    _line41 = TrimString(ref _line4, 600);
                    _line5 = Resources.instGeneralLine5;
                    _line51 = TrimString(ref _line5, 600);
                }
                if (_buttonLaser.Update(mousePosition, mouseDown))
                {
                    _state = InstructionsState.laser;
                    _t1 = _content.Load<Texture2D>("instructions/laser_1");
                    _t2 = _content.Load<Texture2D>("instructions/laser_2");
                    _t3 = _content.Load<Texture2D>("instructions/laser_3");
                    _t4 = _content.Load<Texture2D>("instructions/laser_4");
                    _t5 = _content.Load<Texture2D>("instructions/placeholder");
                    _headerText = Resources.instLaser;
                    _line1 = Resources.instLaserLine1;
                    _line11 = TrimString(ref _line1, 600);
                    _line2 = Resources.instLaserLine2;
                    _line21 = TrimString(ref _line2, 600);
                    _line3 = Resources.instLaserLine3;
                    _line31 = TrimString(ref _line3, 600);
                    _line4 = Resources.instLaserLine4;
                    _line41 = TrimString(ref _line4, 600);
                    _line5 = "";
                    _line51 = TrimString(ref _line5, 600);
                }
                if (_buttonTools.Update(mousePosition, mouseDown))
                {
                    _state = InstructionsState.tools;
                    _t1 = _content.Load<Texture2D>("instructions/tool_1");
                    _t2 = _content.Load<Texture2D>("instructions/tool_2");
                    _t3 = _content.Load<Texture2D>("instructions/tool_3");
                    _t4 = _content.Load<Texture2D>("instructions/tool_4");
                    _t5 = _content.Load<Texture2D>("instructions/tool_5");
                    _headerText = Resources.instTools;
                    _line1 = Resources.instToolLine1;
                    _line11 = TrimString(ref _line1, 600);
                    _line2 = Resources.instToolLine2;
                    _line21 = TrimString(ref _line2, 600);
                    _line3 = Resources.instToolLine3;
                    _line31 = TrimString(ref _line3, 600);
                    _line4 = Resources.instToolLine4;
                    _line41 = TrimString(ref _line4, 600);
                    _line5 = Resources.instToolLine5;
                    _line51 = TrimString(ref _line5, 600);
                }
                if (_buttonObstacles.Update(mousePosition, mouseDown))
                {
                    _state = InstructionsState.obstacles;
                    _t1 = _content.Load<Texture2D>("instructions/obstacle_1");
                    _t2 = _content.Load<Texture2D>("instructions/obstacle_2");
                    _t3 = _content.Load<Texture2D>("instructions/obstacle_3");
                    _t4 = _content.Load<Texture2D>("instructions/obstacle_4");
                    _t5 = _content.Load<Texture2D>("instructions/obstacle_5");
                    _headerText = Resources.instObstacles;
                    _line1 = Resources.instObstaclesLine1;
                    _line11 = TrimString(ref _line1, 600);
                    _line2 = Resources.instObstaclesLine2;
                    _line21 = TrimString(ref _line2, 600);
                    _line3 = Resources.instObstaclesLine3;
                    _line31 = TrimString(ref _line3, 600);
                    _line4 = Resources.instObstaclesLine4;
                    _line41 = TrimString(ref _line4, 600);
                    _line5 = Resources.instObstaclesLine5;
                    _line51 = TrimString(ref _line5, 600);
                }
                if (_buttonPoints.Update(mousePosition, mouseDown))
                {
                    _state = InstructionsState.points;
                    _t1 = _content.Load<Texture2D>("instructions/placeholder");
                    _t2 = _content.Load<Texture2D>("instructions/points_2");
                    _t3 = _content.Load<Texture2D>("instructions/placeholder");
                    _t4 = _content.Load<Texture2D>("instructions/points_4");
                    _t5 = _content.Load<Texture2D>("instructions/points_5");
                    _headerText = Resources.instPoints;
                    _line1 = Resources.instPointsLine1;
                    _line11 = TrimString(ref _line1, 600);
                    _line2 = Resources.instPointsLine2;
                    _line21 = TrimString(ref _line2, 600);
                    _line3 = Resources.instPointsLine3;
                    _line31 = TrimString(ref _line3, 600);
                    _line4 = Resources.instPointsLine4;
                    _line41 = TrimString(ref _line4, 600);
                    _line5 = Resources.instPointsLine5;
                    _line51 = TrimString(ref _line5, 600);
                }
            }
            else if (mouseDown)
            {
                _isTouched = true;                
            }
            else if (_isTouched && !mouseDown)
            {
                _isTouched = false;
                _state = InstructionsState.content;
            }

            return Result.noresult;
        }

        public void Back()
        {
            _back = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with background tiles 
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            spriteBatch.Draw(_texture, new Vector2(250, 10), Color.White);

            Vector2 position = new Vector2(20, 20);

            // draw page caption
            spriteBatch.DrawString(_font21, Resources.instHeader, position, Color.White);


            if (_state == InstructionsState.content)
            {
                _buttonGeneral.Draw(spriteBatch, _font18);
                _buttonLaser.Draw(spriteBatch, _font18);
                _buttonTools.Draw(spriteBatch, _font18);
                _buttonObstacles.Draw(spriteBatch, _font18);
                _buttonPoints.Draw(spriteBatch, _font18);
            }
            else
            {
                Vector2 p = new Vector2(30, 90);
                spriteBatch.DrawString(_font21, _headerText, p, Color.White);

                p = new Vector2(30, 130);
                spriteBatch.Draw(_t1, p, Color.White);
                p.X += _t1.Width + 10;
                p.Y += 5;
                
                spriteBatch.DrawString(_fontContent, _line1, p, Color.White);

                if (_line11 != null)
                {
                    p.Y += 20;
                    spriteBatch.DrawString(_fontContent, _line11, p, Color.White);
                }                

                p = new Vector2(30, 200);
                spriteBatch.Draw(_t2, p, Color.White);
                p.X += _t2.Width + 10;
                p.Y += 5;

                spriteBatch.DrawString(_fontContent, _line2, p, Color.White);

                if (_line21 != null)
                {
                    p.Y += 25;
                    spriteBatch.DrawString(_fontContent, _line21, p, Color.White);
                }

                p = new Vector2(30, 270);
                spriteBatch.Draw(_t3, p, Color.White);
                p.X += _t3.Width + 10;
                p.Y += 5;

                spriteBatch.DrawString(_fontContent, _line3, p, Color.White);

                if (_line31 != null)
                {
                    p.Y += 25;
                    spriteBatch.DrawString(_fontContent, _line31, p, Color.White);
                }

                p = new Vector2(30, 340);
                spriteBatch.Draw(_t4, p, Color.White);
                p.X += _t4.Width + 10;
                p.Y += 5;

                spriteBatch.DrawString(_fontContent, _line4, p, Color.White);

                if (_line41 != null)
                {
                    p.Y += 25;
                    spriteBatch.DrawString(_fontContent, _line41, p, Color.White);
                }

                p = new Vector2(30, 410);
                p.Y += 5;

                spriteBatch.Draw(_t5, p, Color.White);
                p.X += _t5.Width + 10;
                spriteBatch.DrawString(_fontContent, _line5, p, Color.White);

                if (_line51 != null)
                {
                    p.Y += 25;
                    spriteBatch.DrawString(_fontContent, _line51, p, Color.White);
                }
            }
        }

        public String TrimString(ref String input, int width)
        {
            String tmp = null;

            while (_fontContent.MeasureString(input).X > width)
            {                
                tmp = input.Substring(input.LastIndexOf(' ')) + tmp;
                input = input.Remove(input.LastIndexOf(' '));
            }

            if (tmp != null)
                tmp = tmp.TrimStart(' ');
            
            return tmp;
        }

        private bool Debounce(GameTime time)
        {
            if (time.TotalGameTime.TotalMilliseconds - _debounceStart > 300)
            {
                _debounceStart = time.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
    }
}
