using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGGameLibrary
{
    /// <summary>
    /// station management
    /// </summary>
    class TGStation
    {
        /// <summary>
        /// constructs a stations
        /// </summary>
        public TGStation()
        {
            Clear();
        }

        /// <summary>
        /// add a tile to a station, a station needs 3 adjacent tiles in line
        /// </summary>        
        public bool AddPosition(int x, int y)
        {
            for (int i = 0; i < 3; i++)
            {
                // fill an empty tile with a tile position
                if (_position[i].X == 0 && _position[i].Y == 0)
                {                    
                    _position[i].X = x;
                    _position[i].Y = y;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// the position of the station ( center tile )
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position[1];
            }
        }

        /// <summary>
        /// returns true if the given train is in the station
        /// </summary>        
        public bool TrainInStation( TGTrain train )
        {
            if( (train.TrainTileX == _position[0].X && train.TrainTileY == _position[0].Y + 1 ) &&
                (train.Wagon2TileX == _position[2].X && train.TrainTileY == _position[2].Y + 1 ))
            return true;

            if ((train.TrainTileX == _position[2].X && train.TrainTileY == _position[2].Y + 1) &&
                (train.Wagon2TileX == _position[0].X && train.TrainTileY == _position[0].Y + 1))
            return true;

            return false;
        }

        /// <summary>
        /// clears the station
        /// </summary>
        void Clear()
        {
            for( int i = 0; i < 3; i++ )
            {
                _position[i].X = 0;
                _position[i].Y = 0;                
            }
        }        
       
        /// <summary>
        /// list of passengers waiting at the station
        /// </summary>
        public TGPassengers Passengers
        {
            get
            {
                return _passengers;
            }
        }

        /// <summary>
        /// draw the station
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {            
           _passengers.Draw(spriteBatch,new Vector2(_position[0].X, _position[0].Y) * TGTile.Size);                
        }

        Vector2[] _position = new Vector2[3];
        TGPassengers _passengers = new TGPassengers();
    }
}
