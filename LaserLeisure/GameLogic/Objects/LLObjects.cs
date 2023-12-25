using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LLGameLibrary
{
    /// <summary>
    /// storage class for all non moveable game objects
    /// </summary>
    class LLObjects
    {
        List<LLObject> _objects = new List<LLObject>();
        ContentManager _content;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLObjects(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// reset all objects to the state of the begin of a level
        /// </summary>
        public void Reset()
        {
            foreach (LLObject o in _objects)
                o.Reset();
        }

        /// <summary>
        /// clears the objects
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
        }

        /// <summary>
        /// creates a new object
        /// </summary>        
        public void AddObject(LLObject obj)
        {
            _objects.Add(obj);
        }

        /// <summary>
        /// check if any objects contains the given point
        /// </summary>        
        public Boolean Contains(Point p)
        {
            foreach (LLObject o in _objects)
            {
                if (o.Contains(p, false))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// checks if the given rectangle intersects with any object
        /// </summary>        
        public Boolean Intersects(Rectangle r)
        {
            foreach (LLObject o in _objects)
            {
                if (o.Intersecs(r))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// checks if the beams hits any object
        /// </summary>
        /// <param name="line">beam</param>        
        /// <returns>hit or miss</returns>
        public Boolean HitTest(LLLine line, ref Point intersectionPoint, LLObject.ObjectColor color)
        {            
            // go through all obejcts
            foreach (LLObject o in _objects)
            {
                if( o.Status == LLObject.ObjectStatus.alive )
                    if (o.Hittest(line,ref intersectionPoint, color))
                        return true;
            }
            return false;
        }

        /// <summary>
        /// checks if we clicked on a laser
        /// </summary>
        /// <param name="p">point to test</param>
        /// <returns>true if a laser was clicked</returns>        
        public Boolean LaserContain(Point p)
        {
            foreach (LLObject o in _objects)
            {
                // go through all laser
                if( o.GetType() == typeof(LLLaser))
                    if (o.Contains(p,true))
                        return true;
            }
            return false;
        }

        /// <summary>
        /// check if there are dead laser objects
        /// </summary>
        /// <returns>true if all laser are inactive</returns>
        public Boolean LaserDead()
        {
            foreach (LLObject o in _objects)
            {
                // go through all laser
                if( o.GetType() == typeof(LLLaser))
                    if( o.Status != LLObject.ObjectStatus.dead )
                        return false;            
            }
            return true;
        }

        /// <summary>
        /// checks if a beam has hit any bomb
        /// </summary>
        /// <returns></returns>
        public Boolean Bombhit()
        {
            foreach (LLObject o in _objects)
            {
                // go through all bombs
                if (o.GetType() == typeof(LLBomb))
                    if (o.Status == LLObject.ObjectStatus.dead)
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Kill coins
        /// </summary>
        public void KillCoins()
        {
            Boolean found = true;

            while (found)
            {
                found = false;
                foreach (LLObject o in _objects)
                {
                    // go through all coins and kill them
                    if (o.GetType() == typeof(LLCoin))
                    {
                        _objects.Remove(o);
                        found = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// check if any points just scored points
        /// </summary>
        /// <returns></returns>
        public Boolean CoinsScored()
        {
            foreach (LLObject o in _objects)
            {
                // go through all coins and check if they scored points
                if (o.GetType() == typeof(LLCoin))
                {
                    if (o.Status == LLObject.ObjectStatus.finished)
                    {
                        o.Kill(new Random((int)DateTime.Now.Ticks));
                        return true; // this coins just scored points
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// kills the object externaly
        /// </summary>        
        public void Kill()
        {
            Random r = new Random((int)DateTime.Now.Ticks);

            // kill all objects
            foreach (LLObject o in _objects)
            {
                o.Kill(r);
            }
        }

        /// <summary>
        /// checks if there are unfinished targets
        /// </summary>
        /// <returns>true if all targets are hit by the right laser</returns>
        public Boolean TargetFinish()
        {
            foreach (LLObject o in _objects)
            {
                // go through all targets
                if (o.GetType() == typeof(LLTarget))
                    if (o.Status != LLObject.ObjectStatus.finished)
                        return false;
            }
            return true;
        }

        /// <summary>
        /// update logic of the opjects
        /// </summary>        
        public void Update(GameTime gameTime, LLTools tools)
        {
            foreach (LLObject o in _objects)
            {
                o.Update(gameTime,tools,this);
            }
        }

        /// <summary>
        /// draw all the objects
        /// </summary>         
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (LLObject o in _objects)
            {
                o.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// do the topmost drawing
        /// </summary>         
        public void Draw2(SpriteBatch spriteBatch)
        {
            foreach (LLObject o in _objects)
            {
                o.Draw2(spriteBatch);
            }
        }        
    }          
}
