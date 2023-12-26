using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AAGameLibrary
{
    /// <summary>
    /// class for drawing the vector to control the aircraft
    /// </summary>
    public class AALine
    {
        private Texture2D _cursorTextue;
        Point _p1 = new Point();
        Point _p2 = new Point();
        bool _clicked = false;

        /// <summary>
        /// ctor
        /// </summary>        
        public AALine(Texture2D cursorTextue)
        {            
            _cursorTextue = cursorTextue;   //Cursor texture
        }

        /// <summary>
        /// update the position from mouse pos and click events
        /// </summary>        
        public void Update(Point position, Boolean clickDown)
        {
            if (_clicked == false)
            {
                _p1 = position;
            
            }            
            _p2 = position;            

            _clicked = clickDown;
        }

        /// <summary>
        /// check if the line is visible and clicked
        /// </summary>
        public Boolean IsClicked
        {
            get
            {
                return _clicked;
            }
        }

        /// <summary>
        /// updates the origin of the line
        /// </summary>        
        public void SetOrigin(Point p)
        {
            _p1 = p;
        }

        /// <summary>
        /// returns the rotation of the line in radiant 
        /// </summary>        
        public float GetRotation()
        {
            float length = GetLength();
            float rot = 0.0f;            

            // prevent devide zero
            if (length > 0)
                rot = (float)Math.Asin((float)((float)_p1.Y - (float)_p2.Y) / (float)length);

            // quadrant 1 and 4
            if (_p2.X > _p1.X)
            {
                rot -= (float)Math.PI;
                rot *= -1;
            }
            // quadrant 4
            if (_p2.Y > _p1.Y && _p2.X <_p1.X)            
                rot += (float)(2*Math.PI);            

            return rot;
        }

        /// <summary>
        /// gets the length of the line p1,p2
        /// </summary>        
        public float GetLength()
        {
            // pythagoras
            return (float)Math.Sqrt((float)(_p1.X - _p2.X) * (float)(_p1.X - _p2.X) + (float)(_p1.Y - _p2.Y) * (float)(_p1.Y - _p2.Y));
        }

        /// <summary>
        /// draw the line from p1 to p2
        /// </summary>        
        public void Draw(SpriteBatch batch) 
        {
            // draw nothing when not clicked
            if (_clicked == false)
                return;

            // length of the line
            float length = GetLength();            
            // direction of the line
            float rot = GetRotation();
           
            batch.Draw(_cursorTextue, new Rectangle((int)_p1.X+1, (int)_p1.Y+1, (int)length, _cursorTextue.Width), null, Color.White, rot, new Vector2(_cursorTextue.Width, _cursorTextue.Height), SpriteEffects.None, 0);
        }
    }
}
