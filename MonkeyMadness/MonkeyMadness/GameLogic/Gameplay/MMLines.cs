using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonkeyMadness
{
    /// <summary>
    /// class handling the lines of the game
    /// </summary>
    class MMLines
    {
        /// <summary>
        /// Ctor
        /// </summary>        
        public MMLines(Game gameMain)
        {
            _line = gameMain.Content.Load<Texture2D>("misc/line");            
        }        

        public void Init(Rectangle screenRect, Rectangle titleSafe, Int32 maxLines)
        {
            _maxLines = maxLines;
            _screenRect = screenRect;
            _titleSafe = titleSafe;
        }

        /// <summary>
        /// draw the lines on the given batch
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            int offset = LinesOffset;

            for (int i = 0; i < _maxLines; i++)
            {
                float delta = LinesDelta;
                delta *= i + 1;
                Rectangle r = new Rectangle(0, (int)delta - offset, _screenRect.Width, _line.Height);

                spriteBatch.Draw(_line, r, Color.White);
            }                
        }

        /// <summary>
        /// the distance of the first line from the top of the screen
        /// </summary>
        public Int32 LinesOffset
        {
            get
            {
                // offset is 90% of a single line spacing
                Int32 o = (int)((_screenRect.Height) / (float)(_maxLines + 1) * 0.9);

#if WINDOWS_PHONE
                o -= 80;
#endif
                return o;
            }            
        }

        /// <summary>
        /// the distance between two lines
        /// </summary>
        public float LinesDelta
        {
            get
            {
                // calculate the delta between two lines to distribute them evenly
                float d = (_screenRect.Height + LinesOffset) / (float)(_maxLines + 1);

                return d;
            }
        }

        Int32 _maxLines;
        Texture2D _line;
        Rectangle _screenRect;
        Rectangle _titleSafe;
    }
}
