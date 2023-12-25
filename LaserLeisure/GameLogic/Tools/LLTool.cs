using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LLGameLibrary
{
    abstract class LLTool
    {
        /// <summary>
        /// this is what a object can do with beams
        /// </summary>
        public enum Result
        {
            Absorb,  // beam can end here
            Miss,    // beam doesn't hit the object
            Mirror,  // beam is mirrors
            Prism,   // does a prism beam split
            Splitter,// does a half mirror split 
            Wood     // slows the beams
        }

        protected Texture2D _texture;
        protected Texture2D _texture1;
        protected Texture2D _texture2;
        protected Point _position = new Point(0, 0);
        protected Point _lastGoodPosition = new Point(0, 0);
        protected Boolean _clicked = false;
        protected ContentManager _content;
        protected double _rotation = 0;
        //protected LLLine _line;
        protected List<LLLine> _lines = new List<LLLine>();
        protected Vector2 _clickOffset;
        private bool _fix = false;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLTool(ContentManager content, Point position, double rotation, bool fix)
        {
            _content = content;
            _lastGoodPosition = _position = position;
            _rotation = rotation;
            _fix = fix;
        }

        /// <summary>
        /// check if the object is clicked by the user
        /// </summary>
        /// <returns></returns>
        public Boolean IsClicked
        {
            get
            {
                return _clicked;
            }
        }

        /// <summary>
        /// this functions depends on the real object, wether it's a mirror or prism etc.
        /// </summary>
        /// <param name="line">line/beam to test</param>
        /// <param name="intersecPoint">if hit, this is the intersection point</param>
        /// <param name="angle">this will be the angle between object and beam</param>
        /// <returns>return the type of the action which is to be applied to the beam</returns>
        public abstract Result Hittest(LLLine line, ref Point intersecPoint, ref double angle, ref bool inside);

        /// <summary>
        /// position of the object
        /// </summary>
        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _position.X += (int)_clickOffset.X;
                _position.Y += (int)_clickOffset.Y; // obdate object position

                UpdateLines(); // update the object line
            }
        }               

        /// <summary>
        /// update the line of object with the obejct position
        /// </summary>
        protected abstract void UpdateLines();        

        /// <summary>
        /// check if the given point is within the object, inflates the object a bit to allow easier clicking
        /// </summary>
        /// <param name="p">point</param>
        /// <returns>true, point is inside the object</returns>
        public Boolean Contains(Point p, Boolean inflate)
        {
            // can't click tools that are fixed
            if (_fix)
                return false;

            Rectangle r = new Rectangle((int)(_position.X - _texture.Width / 2), (int)(_position.Y - _texture.Height / 2), _texture.Width, _texture.Height);

            if (inflate)
            {
                // make the rectangle a bit larger to allow easier clicking
                r.Inflate(30, 30);
            }

            return r.Contains(p);
        }

        /// <summary>
        /// click the object, change texture, allow repositioning of the object
        /// </summary>
        /// <param name="position">click position</param>
        /// <param name="click">true if a click was detected</param>
        public void Click(Point position, Boolean click)
        {
            // can't click tools that are fixed
            if (_fix)
                return;

            if (click)
            {
                // call this when toggeling from not clicked to clicked
                if (!IsClicked)
                {
                    // load new texture
                    _texture = _texture2;
                    // remember the last good position just in case we have to revert the position
                    _lastGoodPosition = position;
                    // remember the click offset
                    _clickOffset = new Vector2(_position.X - position.X, _position.Y - position.Y);
                }
                _clicked = true;
            }
            else
            {
                if (IsClicked)
                {
                    // load the standard texture
                    _texture = _texture1;
                    _clicked = false;                 
                }
            }
        }

        /// <summary>
        /// revert the position of the tool to the last known good one
        /// </summary>
        public void RevertPosition()
        {            
            Position = _lastGoodPosition;

            // play revert noise
            LLFxManager.Fx.PlayAbsorbNoise();
        }

        /// <summary>
        /// for those tools who need an update
        /// </summary>        
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draw the current texture
        /// </summary>
        /// <param name="spriteBatch">spritebatch</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 p = new Vector2(_position.X, _position.Y);

            float rotation = (float)(_rotation + Math.PI);

            if (rotation >= 2 * Math.PI)
                rotation -= (float)(2 * Math.PI);

            int x = (int)p.X;
            int y = (int)p.Y;

            // correct x, y according to rotation            
            if (rotation > 0 && rotation <= 5 * Math.PI / 4)
                x += 1;
            if (rotation > Math.PI - 0.0001 && rotation < 2 * Math.PI)
                y += 1;

            spriteBatch.Draw(_texture, new Rectangle(x, y, _texture.Width, _texture.Height), null, Color.White, (float)(_rotation + Math.PI), new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 0);
        }
                
        public virtual void DrawFirst(SpriteBatch spriteBatch)
        {
        }
    }
}
