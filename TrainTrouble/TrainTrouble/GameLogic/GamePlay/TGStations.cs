using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGGameLibrary
{
    /// <summary>
    /// collection of all station in a level
    /// </summary>
    class TGStations
    {
        List<TGStation> _stations = new List<TGStation>();
        Int32 _currendSecond;        
        const Int32 _maxPassengers = 6;
        Vector2 _overflowSite = new Vector2(0, 0);

        /// <summary>
        /// constructs the stations
        /// </summary>
        public TGStations()
        {
        }

        /// <summary>
        /// adds a station tile to the stations
        /// </summary>        
        public void AddTile(int x, int y)
        {
            foreach (TGStation s in _stations)
            {
                if (s.AddPosition(x, y))
                    return;
            }

            // create a new station of the other once are full
            TGStation station = new TGStation();
            station.AddPosition(x, y);
            _stations.Add(station);           
        }

        /// <summary>
        /// create new random passenger at any station
        /// </summary>
        public Boolean SpawnPassenger(GameTime gameTime, ContentManager content, TGColor[] colors,Int32 angryTime, Int32 spawnLikelyhood)
        {
            if (_currendSecond != (Int32)gameTime.TotalGameTime.TotalSeconds)
            {
                Random r = new Random((int)DateTime.Now.Ticks);

                Int32 percent = r.Next(1, 100);                

                // create the passenger with a likelyhood of % per second
                if (percent < spawnLikelyhood)
                {
                    // create random station
                    Int32 i = r.Next(0, _stations.Count);

                    // create random color
                    Int32 c = r.Next(0, colors.Length);
                    TGColor color = colors[c];

                    // create passengers if there are below 6
                    if (_stations[i].Passengers.Count < 6)
                        _stations[i].Passengers.AddPassenger(color, content,angryTime);
                    else
                    {
                        _overflowSite = _stations[i].Position;
                        return false;
                    }
                }

                _currendSecond = (Int32)gameTime.TotalGameTime.TotalSeconds;
            }

            return true;
        }

        public void SetPauseOffset(double time)
        {
            // add the offset on each passenger to cover pauses
            foreach (TGStation s in _stations)
                s.Passengers.AddSpawnTime(time);
        }

        /// <summary>
        /// clears all passengers from the stations
        /// </summary>
        public void ClearPassengers()
        {
            foreach (TGStation s in _stations)            
                s.Passengers.Clear();            
        }

        /// <summary>
        /// get the station that overflowed recentyl
        /// </summary>
        public Vector2 OverflowSite
        {
            get
            {
                return _overflowSite;
            }
        }

        /// <summary>
        /// update passengers waiting at the stations
        /// </summary>        
        public void Update(GameTime gameTime)
        {
            foreach (TGStation s in _stations)
                s.Passengers.Update(gameTime);
        }

        /// <summary>
        /// draw the stations
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TGStation s in _stations)
            {
                s.Draw(spriteBatch);                
            }            
        }

        /// <summary>
        /// check the given train is inside any station
        /// </summary>        
        public TGStation TrainInStation(TGTrain train)
        {
            foreach (TGStation s in _stations)
                if (s.TrainInStation(train))
                    return s;

            return null;
        }

        /// <summary>
        /// position of the station
        /// </summary>        
        public Vector2 StationPosition(Int32 i)
        {
            return _stations[i].Position;
        }
    }
}
