using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Content;

namespace TGGameLibrary
{
    /// <summary>
    /// class managing a train
    /// </summary>
    class TGTrain
    {
        ContentManager _content;

        TGColor _color;

        bool _centerCrossed = false;
        double _stopTime = 0;
        double _boostTime = 0;

        TGTile[,] _tiles = null;
        private float _speed = 2;                       // initial train speed
        private Texture2D _trainTextture;
        private Texture2D _wagonTextture;

        List<TGAnimatedTexture> _smoke = new List<TGAnimatedTexture>();
        List<Vector2> _smokePosition = new List<Vector2>();                
        int _smokeTime = 0;

        private TGTile.Rotation _trainRotation = TGTile.Rotation.LEFT;
        private TGTile.Rotation _wagon1Rotation = TGTile.Rotation.LEFT;
        private TGTile.Rotation _wagon2Rotation = TGTile.Rotation.LEFT;
        private Vector2 _trainPosition = new Vector2(24, 12); // initial train position on a tile
        private Vector2 _wagon1Position = new Vector2(24, 12); // initial wagon 1 position on a tile
        private Vector2 _wagon2Position = new Vector2(24, 12); // initial wagon 2 position on a tile        

        private Vector2 _increment = new Vector2(-1.0f, 0.0f);        // initial train direction
        private Vector2 _crashSite = new Vector2(0,0);

        private int _trainTileX = 0;     // the tile x,y where the train currently is
        private int _trainTileY = 0;
        private int _wagon1TileX = 0;     // the tile x,y where the wagon 1 currently is
        private int _wagon1TileY = 0;
        private int _wagon2TileX = 0;     // the tile x,y where the wagon 2 currently is
        private int _wagon2TileY = 0;

        Queue<Vector2> _position1Cache = new Queue<Vector2>();
        Queue<Vector2> _position2Cache = new Queue<Vector2>();
                                
        /// <summary>
        /// the direction of the train and the wagons
        /// </summary>
        private TGTile.Rotation TrainRotation
        {
            get
            {
                return _trainRotation;
            }
            set
            {
                _trainRotation = value;

                // a new rotation changes the movement increments
                switch (_trainRotation)
                {
                    case TGTile.Rotation.LEFT:
                        _increment.X = -_speed;
                        _increment.Y = 0;
                        _trainPosition.Y = TGTile.Size.Y / 2; // force the train in the middle of the tile 
                        break;
                    case TGTile.Rotation.RIGHT:
                        _increment.X = _speed;
                        _increment.Y = 0;
                        _trainPosition.Y = TGTile.Size.Y / 2; // force the train in the middle of the tile 
                        break;
                    case TGTile.Rotation.DOWN:
                        _increment.X = 0;
                        _increment.Y = _speed;
                        _trainPosition.X = TGTile.Size.X / 2; // force the train in the middle of the tile 
                        break;
                    case TGTile.Rotation.UP:
                        _increment.X = 0;
                        _increment.Y = -_speed;
                        _trainPosition.X = TGTile.Size.X / 2; // force the train in the middle of the tile 
                        break;
                    case TGTile.Rotation.DOWN_LEFT:
                        _increment.X = -_speed;               // diagonal movement here
                        _increment.Y = _speed;
                        break;
                    case TGTile.Rotation.DOWN_RIGHT:
                        _increment.X = _speed;                // diagonal movement here
                        _increment.Y = _speed;
                        break;
                    case TGTile.Rotation.UP_LEFT:
                        _increment.X = -_speed;               // diagonal movement here
                        _increment.Y = -_speed;
                        break;
                    case TGTile.Rotation.UP_RIGHT:
                        _increment.X = _speed;                // diagonal movement here
                        _increment.Y = -_speed;
                        break;
                }
            }
        }

        private TGTile.Rotation Wagon1Rotation
        {
            get
            {
                return _wagon1Rotation;
            }
            set
            {
                _wagon1Rotation = value;                
            }
        }

        private TGTile.Rotation Wagon2Rotation
        {
            get
            {
                return _wagon2Rotation;
            }
            set
            {
                _wagon2Rotation = value;
            }
        }       

        /// <summary>
        /// the x tile where the train is located
        /// </summary>
        public int TrainTileX
        {
            get
            {
                return _trainTileX;
            }
        }

        /// <summary>
        /// the y tile where the train is located
        /// </summary>
        public int TrainTileY
        {
            get
            {
                return _trainTileY;
            }
        }

        /// <summary>
        /// the x tile where the wagon is located
        /// </summary>
        public int Wagon1TileX
        {
            get
            {
                return _wagon1TileX;
            }
        }

        /// <summary>
        /// the y tile where the wagon is located
        /// </summary>
        public int Wagon1TileY
        {
            get
            {
                return _wagon1TileY;
            }
        }

        /// <summary>
        /// the x tile where the wagon 2 is located
        /// </summary>
        public int Wagon2TileX
        {
            get
            {
                return _wagon2TileX;
            }
        }

        /// <summary>
        /// the y tile where the wagon 2 is located
        /// </summary>
        public int Wagon2TileY
        {
            get
            {
                return _wagon2TileY;
            }
        }

        /// <summary>
        /// create the train a the given tile
        /// </summary>        
        public TGTrain(ContentManager content, TGTile[,] tiles, Texture2D train, Texture2D wagon, int x, int y, TGColor color)
        {
            _content = content;

            // create some smoke animations
            for (int i = 0; i < 4; i++)
            {
                _smoke.Add(new TGAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false));
                _smoke[i].Load(content, "items/smoke", 17, 17);
                _smoke[i].Pause();
                _smokePosition.Add(new Vector2());
            }

            _color = color;
            _tiles = tiles;
            _trainTextture = train;
            _wagonTextture = wagon;            
            _trainTileY = y;
            _trainTileX = x;
            _wagon1TileX = x; // wagon is always on the right tile of the train
            _wagon1TileY = y;
            _wagon2TileX = x; // wagon is always on the right tile of the train
            _wagon2TileY = y;

            while (MoveTrain() == false);

            while (Move1Wagon() == false);

            while (MoveTrain() == false);
        }

        /// <summary>
        /// returns the color of the train
        /// </summary>
        public TGColor GetColor
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// checks if the given train crashes into this one
        /// </summary>        
        public bool CheckTrainCrash(TGTrain train2)
        {
            if (_trainTileX == train2._trainTileX && _trainTileY == train2._trainTileY ||
                _trainTileX == train2._wagon1TileX && _trainTileY == train2._wagon1TileY ||
                _trainTileX == train2._wagon2TileX && _trainTileY == train2._wagon2TileY)
            {

                _crashSite.X = _trainTileX;
                _crashSite.Y = _trainTileY;
                return true;
            }

            if (train2._trainTileX == _trainTileX && train2._trainTileY == _trainTileY ||
                 train2._trainTileX == _wagon1TileX && train2._trainTileY == _wagon1TileY ||
                 train2._trainTileX == _wagon2TileX && train2._trainTileY == _wagon2TileY)
            {
                _crashSite.X = train2._trainTileX;
                _crashSite.Y = train2._trainTileY;
                return true;
            }

            return false;
        }

        /// <summary>
        /// gets the position of the last traincrash
        /// </summary>
        public Vector2 CrashSite
        {
            get
            {
                return _crashSite;
            }
        }       

        /// <summary>
        /// draw the train
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            // calculate the absolute position of the train on the screen
            Vector2 p = new Vector2(_trainTileX, _trainTileY) * TGTile.Size + _trainPosition;          

            spriteBatch.Draw(_trainTextture, p, null, Color.White, Rotation2Rad(_trainRotation), new Vector2(12, 12), 1.0f, SpriteEffects.None, 0);

            p = new Vector2(_wagon1TileX, _wagon1TileY) * TGTile.Size + _wagon1Position;
            
            spriteBatch.Draw(_wagonTextture, p, null, Color.White, Rotation2Rad(_wagon1Rotation), new Vector2(12, 12), 1.0f, SpriteEffects.None, 0);

            p = new Vector2(_wagon2TileX, _wagon2TileY) * TGTile.Size + _wagon2Position;

            spriteBatch.Draw(_wagonTextture, p, null, Color.White, Rotation2Rad(_wagon2Rotation), new Vector2(12, 12), 1.0f, SpriteEffects.None, 0);


            // draw the smoke textures
            for (int i = 0; i < 4; i++)
            {
                if (_smoke[i].Is_paused == false && (_smokePosition[i].X != 0 && _smokePosition[i].Y != 0))
                    _smoke[i].DrawFrame(spriteBatch, _smokePosition[i], SpriteEffects.None);
            }
        }

        /// <summary>
        /// stops the train for n ms
        /// </summary>        
        public void Stop(double stopTime)
        {
            _stopTime = stopTime;
            _boostTime = 0.0;
        }

        public void Boost(double boostTime)
        {
            _boostTime = boostTime;
            _stopTime = 0.0;
        }


        /// <summary>
        /// calculates the radiant from the given rotation
        /// </summary>        
        private float Rotation2Rad(TGTile.Rotation rot)
        {
            float rad;

            switch (rot)
            {
                case TGTile.Rotation.LEFT:
                    rad = 0;
                    break;
                case TGTile.Rotation.RIGHT:
                    rad = (float)(Math.PI);
                    break;
                case TGTile.Rotation.UP:
                    rad = (float)(Math.PI / 2);
                    break;
                case TGTile.Rotation.DOWN:
                    rad = (float)(2 * Math.PI * 3 / 4);
                    break;

                case TGTile.Rotation.DOWN_LEFT:
                    rad = (float)(2 * Math.PI * 7 / 8);
                    break;
                case TGTile.Rotation.DOWN_RIGHT:
                    rad = (float)(2 * Math.PI * 5 / 8);
                    break;
                case TGTile.Rotation.UP_LEFT:
                    rad = (float)(2 * Math.PI * 1 / 8);
                    break;
                case TGTile.Rotation.UP_RIGHT:
                    rad = (float)(2 * Math.PI * 3 / 8);
                    break;
                default:
                    rad = 0;
                    break;
            }
            return rad;
        }

        public bool IsStopped
        {
            get
            {
                return _stopTime != 0.0;
            }
        }

        public bool IsBoosted
        {
            get
            {
                return _boostTime != 0.0;
            }
        }

        /// <summary>
        /// move the train into the current direction, with the current speed
        /// </summary>        
        public void Move(GameTime gameTime,bool fastForward)
        {         
            if (gameTime.TotalGameTime.TotalMilliseconds > _stopTime)
            {
                // noise when starting the train again
                if (_stopTime > 0.0)
                {
                    TGFxManager.Fx.PlayWhistleNoise();
                    _stopTime = 0.0;
                }

                MoveTrain();
                Move1Wagon();
                Move2Wagon();

                if (fastForward)
                {
                    MoveTrain();
                    Move1Wagon();
                    Move2Wagon();
                }
            }

            if (gameTime.TotalGameTime.TotalMilliseconds < _boostTime)
            {
                MoveTrain();
                Move1Wagon();
                Move2Wagon();
            }
            else
            {
                _boostTime = 0.0;
            }

            // animate the smoke textures
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int msec = (int)gameTime.TotalGameTime.TotalMilliseconds;

            for (int i = 0; i < 4; i++)            
                _smoke[i].UpdateFrame(elapsed);            
            
            if( msec > _smokeTime + 300 )
            {
                _smokeTime = msec;
                for ( int i = 0; i < 4; i++ )
                {
                    // pick a paused animation and place it where the train is, animation will run for a second
                    if (_smoke[i].Is_paused)
                    {
                        Vector2 p = new Vector2(_trainTileX, _trainTileY) * TGTile.Size;
                        p.X += _trainPosition.X / 8;
                        _smokePosition[i] = p;
                        _smoke[i].Play();
                        break;
                    }
                }                
            }
        }
       
        /// <summary>
        /// returns true if the train just passed the center of a tile
        /// </summary>        
        public bool CheckCenterCrossing()
        {
            if (_centerCrossed == false)
            {
                if ( _trainRotation == TGTile.Rotation.LEFT)
                {
                    if (_trainPosition.X < TGTile.Size.X / 2)
                    {
                        _centerCrossed = true;
                        return true;
                    }
                }
                if (_trainRotation == TGTile.Rotation.RIGHT)
                {
                    if (_trainPosition.X > TGTile.Size.X / 2)
                    {
                        _centerCrossed = true;
                        return true;
                    }
                }
                if (_trainRotation == TGTile.Rotation.UP)
                {
                    if (_trainPosition.Y < TGTile.Size.Y / 2)
                    {
                        _centerCrossed = true;
                        return true;
                    }
                }
                if (_trainRotation == TGTile.Rotation.DOWN)
                {
                    if (_trainPosition.Y > TGTile.Size.Y / 2)
                    {
                        _centerCrossed = true;
                        return true;
                    }
                }            
            }
            return false;
        }

        /// <summary>
        /// moves the first wagon
        /// </summary>        
        private bool Move1Wagon()
        {
            _wagon1Position = (Vector2)_position1Cache.Dequeue();            

            _position2Cache.Enqueue(_wagon1Position);

            Boolean tileChange = false;

            // check tile overflow                        
            if (_wagon1Position.X < 0)
            {
                _wagon1TileX -= 1;
                _wagon1Position.X = TGTile.Size.X - 1;
                tileChange = true;
            }
            if (_wagon1Position.X > TGTile.Size.X - 1)
            {
                _wagon1TileX += 1;
                _wagon1Position.X = 0;
                tileChange = true;
            }
            if (_wagon1Position.Y > TGTile.Size.Y - 1)
            {
                _wagon1TileY += 1;
                _wagon1Position.Y = 0;
                tileChange = true;
            }
            if (_wagon1Position.Y < 0)
            {
                _wagon1TileY -= 1;
                _wagon1Position.Y = TGTile.Size.Y - 1;
                tileChange = true;
            }

            // change the wagon rotation if the track tells us to do so        
            if (_tiles != null)        
                Wagon1Rotation = _tiles[Wagon1TileX, Wagon1TileY].UpdateRotation(_wagon1Position, Wagon1Rotation, tileChange);

            return tileChange;
        }

        /// <summary>
        /// moves the second wagon
        /// </summary>        
        private bool Move2Wagon()
        {
            _wagon2Position = (Vector2)_position2Cache.Dequeue();            

            Boolean tileChange = false;

            // check tile overflow                        
            if (_wagon2Position.X < 0)
            {
                _wagon2TileX -= 1;
                _wagon2Position.X = TGTile.Size.X - 1;
                tileChange = true;
            }
            if (_wagon2Position.X > TGTile.Size.X - 1)
            {
                _wagon2TileX += 1;
                _wagon2Position.X = 0;
                tileChange = true;
            }
            if (_wagon2Position.Y > TGTile.Size.Y - 1)
            {
                _wagon2TileY += 1;
                _wagon2Position.Y = 0;
                tileChange = true;
            }
            if (_wagon2Position.Y < 0)
            {
                _wagon2TileY -= 1;
                _wagon2Position.Y = TGTile.Size.Y - 1;
                tileChange = true;
            }

            // change the wagon rotation if the track tells us to do so   
            if (_tiles != null)        
                Wagon2Rotation = _tiles[Wagon2TileX, Wagon2TileY].UpdateRotation(_wagon2Position, Wagon2Rotation, tileChange);

            return tileChange;
        }

        /// <summary>
        /// move the train
        /// </summary>        
        private bool MoveTrain()
        {
            _trainPosition += _increment;

            _position1Cache.Enqueue(_trainPosition);

            Boolean tileChange = false;

            // check tile overflow                        
            if (_trainPosition.X < 0)
            {
                _trainTileX -= 1;
                _trainPosition.X = TGTile.Size.X - 1;
                tileChange = true;
                _centerCrossed = false;
            }
            if (_trainPosition.X > TGTile.Size.X - 1)
            {
                _trainTileX += 1;
                _trainPosition.X = 0;
                tileChange = true;
                _centerCrossed = false;
            }
            if (_trainPosition.Y > TGTile.Size.Y - 1)
            {
                _trainTileY += 1;
                _trainPosition.Y = 0;
                tileChange = true;
                _centerCrossed = false;
            }
            if (_trainPosition.Y < 0)
            {
                _trainTileY -= 1;
                _trainPosition.Y = TGTile.Size.Y - 1;
                tileChange = true;
                _centerCrossed = false;
            }

            // change the train rotation if the track tells us to do so      
            if(_tiles != null)  
                TrainRotation = _tiles[TrainTileX, TrainTileY].UpdateRotation(_trainPosition, TrainRotation, tileChange);            

            return tileChange;
        }
    }
}

