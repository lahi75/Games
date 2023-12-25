using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGGameLibrary
{
    /// <summary>
    /// collection of all passengers waiting at a station
    /// </summary>
    class TGPassengers
    {
        List<TGPassenger> _passangers = new List<TGPassenger>();

        /// <summary>
        /// clear all passengers
        /// </summary>
        public void Clear()
        {
            _passangers.Clear();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TGPassengers()
        {
        }

        /// <summary>
        /// adds a new passengers to the list
        /// </summary>        
        public void AddPassenger(TGColor color, ContentManager content,Int32 angryTime)
        {
            _passangers.Add(new TGPassenger(color,content,angryTime));
        }

        /// <summary>
        /// add time on the spawntime to deal with game pauses
        /// </summary>        
        public void AddSpawnTime(double time)
        {
            foreach (TGPassenger p in _passangers)
                p.AddSpawnTime(time);
        }

        /// <summary>
        /// removes all passengers of the given color from the list
        /// </summary>        
        public Boolean RemovePassengers(TGColor color, ref TGPoints points, bool fastForward)
        {
            bool removed = false;
            bool cont = true;

            while (cont)
            {
                cont = false;

                foreach (TGPassenger p in _passangers)
                {
                    if (p.GetColor == color)
                    {
                        points.AddPassenger(p.PassengerValue);
                        points.PickedupPassengers++;

                        _passangers.Remove(p);
                        removed = true;
                        break;
                    }
                }
                for (int i = 0; i < _passangers.Count; i++)
                {
                    // go on with the iteration if the are passengers of the given color left
                    if (_passangers[i].GetColor == color)
                        cont = true;
                }
            }
            return removed;
        }

        /// <summary>
        /// how many passengers are in the list
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _passangers.Count;
            }
        }

        /// <summary>
        /// updates all passengers in the list
        /// </summary>        
        public void Update(GameTime gameTime)
        {
            foreach (TGPassenger p in _passangers)
                p.Update(gameTime);
        }

        /// <summary>
        /// draw all passengers at the given location
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            position.X -= 3;

            foreach (TGPassenger p in _passangers )
            {
                p.Draw(spriteBatch, position);
                position.X += p.Width;                
            }
        }
    }
}
