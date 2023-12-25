using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LLGameLibrary
{
    /// <summary>
    /// base class for all objects
    /// </summary>
    abstract class LLObject
    {        
        protected Vector2 _position = new Vector2(0, 0);
        protected ContentManager _content;
        protected double _rotation = 0;      
        protected ObjectStatus _status = ObjectStatus.alive;
        protected List<LLLine> _lines = new List<LLLine>();

        /// <summary>
        /// an object can have a color
        /// </summary>
        public enum ObjectColor
        {
            red,
            green,
            blue
        }

        /// <summary>
        /// objectstatus
        /// </summary>
        public enum ObjectStatus
        {
            dead,
            alive,
            finished
        }

        /// <summary>
        /// ctor
        /// </summary>        
        public LLObject(ContentManager content, Vector2 position, double rotation)
        {
            _content = content;
            _position = position;
            _rotation = rotation;            
        }

        /// <summary>
        /// get the object status
        /// </summary>
        public ObjectStatus Status
        {
            get
            {
                return _status;
            }
        }

        /// <summary>
        /// reset the status of the object
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// kill the object (i.e. after the bomb exlode)
        /// </summary>
        public abstract void Kill(Random r);        

        /// <summary>
        /// get the size of the object
        /// </summary>        
        public abstract Vector2 Size();

        /// <summary>
        /// check if the given point falls within the obejct
        /// </summary>        
        public Boolean Contains(Point p, Boolean inflate)
        {
            Rectangle r = new Rectangle((int)(_position.X - Size().X/2), (int)(_position.Y - Size().Y/2), (int)Size().X, (int)Size().Y);

            if( inflate )
                r.Inflate(20, 20);

            return r.Contains(p);
        }

        /// <summary>
        /// checks if the given rectangle intersects with the object
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Boolean Intersecs(Rectangle r)
        {
            Rectangle o = new Rectangle((int)(_position.X - Size().X / 2), (int)(_position.Y - Size().Y / 2), (int)Size().X, (int)Size().Y);

            return o.Intersects(r);
        }

        /// <summary>
        /// check if a given line hits the object
        /// </summary>
        /// <param name="line">line to test</param>
        /// <param name="intersectionPoint"> the point in which the line has entered the object</param>
        /// <param name="color">the color of the line hitting the object</param>
        /// <returns>true if object hit</returns>
        public Boolean Hittest(LLLine line, ref Point intersectionPoint, ObjectColor color)
        {            
            double angle = 0;

            foreach (LLLine l in _lines)
            {
                if (l.Intersec(line, ref intersectionPoint, ref angle))
                {
                    return ObjectHit(color);                    
                }
            }
            return false;
        }

        /// <summary>
        /// each object can react if hit
        /// </summary>
        /// <param name="color">color of the beam that hit the object</param>        
        /// <param name="speed">object may alter beam speed or can even stop the beam</param>
        protected abstract Boolean ObjectHit(ObjectColor color);
        
        /// <summary>
        /// update logic
        /// </summary>        
        public abstract void Update(GameTime gameTime, LLTools tools, LLObjects objects);

        /// <summary>
        /// update the lines of object with the obejct position
        /// </summary>
        protected abstract void UpdateLines();        

        /// <summary>
        /// draw the object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// is called after the first draw to draw topmost objects
        /// </summary>        
        public abstract void Draw2(SpriteBatch spriteBatch);        
    }
}
