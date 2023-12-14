using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    /// <summary>
    /// class encapsulating a hole in a line
    /// </summary>
    class JJHole
    {              
        /// <summary>
        /// constructor
        /// </summary>
        public JJHole( Game gameMain )
        {
            _hole = gameMain.Content.Load<Texture2D>("misc/hole");

            _targetTime = (float)gameMain.TargetElapsedTime.Milliseconds / 1000;            
        }  

        public void Init(Random r, Rectangle screenRect, Rectangle titleSafe, Int32 gameSpeed, Int32 maxLines, Int32 width, Int32 offset, float delta, bool start)
        {
            _titleSafe = titleSafe;
            _screenRect = screenRect;
            
            // increments depends on the game target speed step
            _increment = ((_titleSafe.Width / gameSpeed) * _targetTime);

            _maxLines = maxLines;
            
            _width = width;

            _delta = delta;

            _offset = offset;

            // create random initial values
            ShuffleStart(r,start);
        }

        /// <summary>
        /// draw the texture on the sprite at the current position
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch, Texture2D background)
        {            
            spriteBatch.Draw(background, new Rectangle((int)_position.X, (int)_position.Y, _width, _hole.Height), new Rectangle((int)_position.X, (int)_position.Y, _width, _hole.Height), Color.White);
        }

        public bool IsLeft()
        {
            return _increment > 0;
        }

        /// <summary>
        /// moves the hole
        /// </summary>
        public void Move()
        {
            // shift it on x
            _position.X += _increment;

            // check for horizontal wraop around
            if (_position.X < _titleSafe.X - _width / 2)
            {
                _position.X = _titleSafe.Width + _titleSafe.X - _width / 2;
                _currentLine -= 1; // move one line up

                // vertical wrap around of the top
                if (_currentLine < 1)
                    _currentLine = _maxLines;
            }
            else if (_position.X + _width / 2 > _titleSafe.Width + _titleSafe.X)
            {
                _position.X = _titleSafe.X - _width / 2;
                _currentLine += 1;

                // vertical wrap around
                if (_currentLine > _maxLines)
                    _currentLine = 1;
            }                        
              
            // calculate the position of a new line            
            _position.Y = _currentLine * _delta-_offset;
        }

        public Boolean TestHole(Int32 line, Int32 position, Boolean sameLine)
        {
            if (line == _currentLine)
            {                             
                if( !sameLine )
                {
                    if (_increment < 0)
                        position += 10;
                    else
                        position -= 10;
                }

                if ((position > _position.X) && (position < _position.X + _width))
                    return true;
            }
            
            return false;
        }

        public Boolean Intersects(JJHole hole)
        {
            if (_currentLine == hole._currentLine)
            {
                if( (_position.X - _width - 15 < hole._position.X) && (_position.X + _width + 15 > hole._position.X ))
                    return true;                
            }
            return false;
        }

        /// <summary>
        /// initialize the hole with random values
        /// </summary>
        private void ShuffleStart(Random r,bool start)
        {                        
            Int32 up = r.Next(0, 2) == 0 ? 1 : -1;


            if (start)
            {
                // first hole starts in line 0 traveling upwards
                _currentLine = 1;
                _increment *= -1;
            }
            else
            {
                _increment *= up;
                _currentLine = r.Next(1, _maxLines + 1);
            }

            _position.X = r.Next(_titleSafe.X, _titleSafe.Width + _titleSafe.X);            

            _position.Y = _currentLine * _delta-_offset;            
        }

        Texture2D _hole;            // texture to draw the hole
        Rectangle _titleSafe;
        Rectangle _screenRect;      // the size of the game area
        Int32 _currentLine;         // line in the the hole currently is
        Int32 _maxLines;            // maximum number of lines on the screen
        Vector2 _position;          // current screen position of the hole        
        Int32 _width;               // width of the hole        
        float _increment;           // number of pixels the hole moves with each gamestep
        float _delta;               // distance between two lines
        Int32 _offset;              // distance of the first line from the top of the screen
        float _targetTime;
    }
}
