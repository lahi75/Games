using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace TGGameLibrary
{
    public enum TGColor
    {
        Red,
        Blue,
        Green
    }    

    public class TGLevel : IDisposable
    {
        private double _debounceStart = 0;
        
        private TGTile[,] _tiles;
        private Int32 _levelNumber;
        private List<TGTrain> _trains = new List<TGTrain>();

        private Texture2D _metalFrame;
        private Texture2D _coins;
        private Texture2D _passenger;
        private Texture2D _stop;
        private Texture2D _boost;
        private Texture2D _selection;
        private Texture2D _background;
        private Texture2D _switch;
        private Texture2D _clock;

        private Random _r;

        private double _pauseStart = 0;

        private TGColor _currentStage = TGColor.Red;

        private SpriteFont _smallFont;
        private Vector2 _statusPosition = new Vector2();

        private TGStations _stations = new TGStations();
        private TGPoints _points = new TGPoints();

        private TapAction _tapAction = TapAction.Stop;

        private bool _fastForward = false;

        public enum Result
        {
            TrainCrash,
            LevelCleared,
            StationOverflow,
            NewStage,
            Pause,
            NoResult
        }

        private enum TapAction
        {
            Boost,
            Stop
        }

        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        /// <summary>
        /// get the current level number
        /// </summary>
        public Int32 LevelIndex
        {
            get
            {
                return _levelNumber;
            }
        }

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public TGLevel(IServiceProvider serviceProvider, Stream fileStream, Int32 levelNumber)
        {
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");

            _levelNumber = levelNumber;

            _smallFont = content.Load<SpriteFont>("fonts/smallFont");

            _metalFrame = content.Load<Texture2D>("buttons/metal_frame");
            _coins = content.Load<Texture2D>("buttons/coins");
            _passenger = content.Load<Texture2D>("items/passenger_white");
            _stop = content.Load<Texture2D>("buttons/stop");
            _boost = content.Load<Texture2D>("buttons/boost");
            _selection = content.Load<Texture2D>("buttons/selection");
            _switch = content.Load<Texture2D>("tiles/switch_highlight");
            _clock = content.Load<Texture2D>("buttons/clock");

            switch (levelNumber)
            {
                default:
                case 0:
                    _background = content.Load<Texture2D>("tiles/gras");
                    _points.Stops = LevelManager.Levels.Level0.Stops;
                    _points.Boosts = LevelManager.Levels.Level0.Boosts;
                    break;
                case 1:
                    _background = content.Load<Texture2D>("tiles/sand");
                    _points.Stops = LevelManager.Levels.Level1.Stops;
                    _points.Boosts = LevelManager.Levels.Level1.Boosts;
                    break;
                case 2:
                    _background = content.Load<Texture2D>("tiles/snow");
                    _points.Stops = LevelManager.Levels.Level2.Stops;
                    _points.Boosts = LevelManager.Levels.Level2.Boosts;
                    break;
                case 3:
                    _background = content.Load<Texture2D>("tiles/ashes");
                    _points.Stops = LevelManager.Levels.Level3.Stops;
                    _points.Boosts = LevelManager.Levels.Level3.Boosts;
                    break;
                case 4:
                    _background = content.Load<Texture2D>("tiles/summer");
                    _points.Stops = LevelManager.Levels.Level4.Stops;
                    _points.Boosts = LevelManager.Levels.Level4.Boosts;
                    break;
            }
            LoadTiles(fileStream);

            CreateTrains();
        }

        /// <summary>
        /// clean up this level on destruction
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        #region TileLoading
        /// <summary>
        /// initialize the tiles from the level configuration file
        /// </summary>        
        private void LoadTiles(Stream fileStream)
        {
            _r = new Random((int)DateTime.Now.Ticks);

            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // allocate the tile grid.
            _tiles = new TGTile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    _tiles[x, y] = LoadTile(tileType, x, y);
                }
            }            
        }

        /// <summary>
        /// Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private TGTile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                // Blank space
                case '.':
                    if( y > 4)
                        return LoadRandomTile();
                    else
                        return new TGTile(null,null,TGTile.TrackType.empty);
                case '-':
                    return LoadTrackTile(TGTile.TrackType.horizontal);
                case '|':
                    return LoadTrackTile(TGTile.TrackType.vertical);
                case '1':
                    return LoadTrackTile(TGTile.TrackType.turn_1);                
                case '2':
                    return LoadTrackTile(TGTile.TrackType.turn_2);                
                case '3':
                    return LoadTrackTile(TGTile.TrackType.turn_3);      
                case '4':
                    return LoadTrackTile(TGTile.TrackType.turn_4);                      
                case 'A':
                    return LoadSwitchTile(TGTile.TrackType.switch_A);                    
                case 'B':
                    return LoadSwitchTile(TGTile.TrackType.switch_B);                    
                case 'C':
                    return LoadSwitchTile(TGTile.TrackType.switch_C);
                case 'D':
                    return LoadSwitchTile(TGTile.TrackType.switch_D);
                case 'E':
                    return LoadSwitchTile(TGTile.TrackType.switch_E);
                case 'F':
                    return LoadSwitchTile(TGTile.TrackType.switch_F);
                case 'G':
                    return LoadSwitchTile(TGTile.TrackType.switch_G);
                case 'H':
                    return LoadSwitchTile(TGTile.TrackType.switch_H);
                case 'X':
                    return LoadTrackTile(TGTile.TrackType.cross);
                case 's':
                    LoadStation(x, y);
                    return LoadStationTile();

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private TGTile LoadRandomTile()
        {
            
            Int32 percent = _r.Next(1, 100);

            if (percent < 10)
            {
                int type = _r.Next(0, 3);
                return new TGTile(Content.Load<Texture2D>("tiles/" + _levelNumber.ToString() + type.ToString()), null, TGTile.TrackType.empty);
            }
            

            return new TGTile(null, null, TGTile.TrackType.empty);

        }

        private void LoadStation(int x, int y)
        {
            _stations.AddTile(x, y);
        }

        private TGTile LoadGrasTile()
        {
            return new TGTile(Content.Load<Texture2D>("tiles/gras"), null, TGTile.TrackType.empty);
        }

        /// <summary>
        /// create a standard track tile
        /// </summary>        
        private TGTile LoadTrackTile(TGTile.TrackType tt)
        {
            return new TGTile(Content.Load<Texture2D>("tiles/" + tt.ToString()), null, tt);
        }

        /// <summary>
        /// create a switch tile
        /// </summary>        
        private TGTile LoadSwitchTile(TGTile.TrackType tt)
        {
            return new TGTile(Content.Load<Texture2D>("tiles/" + tt.ToString() + "a"), Content.Load<Texture2D>("tiles/" + tt.ToString() + "b"),tt);
        }

        /// <summary>
        /// load a station tile
        /// </summary>        
        private TGTile LoadStationTile()
        {
            return new TGTile(Content.Load<Texture2D>("items/station"), null, TGTile.TrackType.empty);
        }
        #endregion

        /// <summary>
        /// create train according to the current stage
        /// </summary>
        void CreateTrains()
        {
            _trains.Clear();

            _fastForward = false;

            if (_currentStage == TGColor.Red)
            {
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_red"), (Int32)_stations.StationPosition(0).X, (Int32)_stations.StationPosition(0).Y + 1, TGColor.Red));
            }
            else if (_currentStage == TGColor.Blue)
            {
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_red"), (Int32)_stations.StationPosition(0).X, (Int32)_stations.StationPosition(0).Y + 1, TGColor.Red));
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_blue"), (Int32)_stations.StationPosition(1).X, (Int32)_stations.StationPosition(1).Y + 1, TGColor.Blue));             
            }
            else
            {
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_red"), (Int32)_stations.StationPosition(0).X, (Int32)_stations.StationPosition(0).Y + 1, TGColor.Red));
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_blue"), (Int32)_stations.StationPosition(1).X, (Int32)_stations.StationPosition(1).Y + 1, TGColor.Blue));
                _trains.Add(new TGTrain(Content, _tiles, Content.Load<Texture2D>("items/train"), Content.Load<Texture2D>("items/wagon_green"), (Int32)_stations.StationPosition(2).X, (Int32)_stations.StationPosition(2).Y + 1, TGColor.Green));
            }
        }

        /// <summary>
        /// do a click in a level
        /// </summary>        
        public void Click(GameTime gameTime, Point p)
        {
            if (!Debounce(gameTime))
                return;

            // check click in menu bar

            if (p.X > 90 && p.X < 145 && p.Y > 20 && p.Y < 50)
            {
                _tapAction = TapAction.Stop;
                TGFxManager.Fx.PlaySwitchNoise();
            }
            else if (p.X > 145 && p.X < 200 && p.Y > 20 && p.Y < 50)
            {
                _tapAction = TapAction.Boost;
                TGFxManager.Fx.PlaySwitchNoise();
            }
            else if (p.X > 200 && p.X < 260 && p.Y > 20 && p.Y < 50)
            {
                _fastForward = !_fastForward;
                TGFxManager.Fx.PlaySwitchNoise();
            }            

            //////////////////////
            // get tile at given position
            int y = p.Y / (int)TGTile.Size.Y;
            int x = p.X / (int)TGTile.Size.X;

            // check if we want to speed or stop the train                        
            // check each train if the click is on the train
            foreach (TGTrain t in _trains)
            {
                if (TrainClick(x, y, t))
                {
                    if (_tapAction == TapAction.Stop && _points.Stops > 0 && !t.IsStopped)
                    {
                        t.Stop(gameTime.TotalGameTime.TotalMilliseconds + 3000);
                        _points.Stops--;
                        TGFxManager.Fx.PlayBreakNoise();
                    }
                    else if( _tapAction == TapAction.Boost && _points.Boosts > 0 && !t.IsBoosted)
                    {
                        t.Boost(gameTime.TotalGameTime.TotalMilliseconds + 3000);
                        _points.Boosts--;
                        TGFxManager.Fx.PlayWhistleNoise();
                    }
                }                    
            }            
            
            // check if the click hits the switch tile
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                // switch the tile on click
                if (_tiles[x, y].IsSwitchable())
                    SwitchTrack(x, y);
                else
                {
                    // Search neighbourghs, if there is a clickable tile
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            int nx = x + a;
                            int ny = y + b;
                            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
                            {
                                // click neightbour it its switchable
                                if (_tiles[nx, ny].IsSwitchable())
                                {
                                    SwitchTrack(nx, ny);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        bool TrainClick(int x, int y, TGTrain t)
        {
            if (x == t.Wagon1TileX && y == t.Wagon1TileY)
                return true;

            // Search neighbourghs, if there is a clickable tile
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    int nx = x + a;
                    int ny = y + b;
                    if (nx >= 0 && nx < Width && ny >= 0 && ny < Height)
                    {
                        if (nx == t.Wagon1TileX && ny == t.Wagon1TileY)
                        {                            
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // switch the tile 
        private bool SwitchTrack(int x, int y)
        {
            // only switch if no train is on the tile            
            foreach (TGTrain t in _trains)
            {
                if ((t.TrainTileX == x && t.TrainTileY == y) ||
                    (t.Wagon1TileX == x && t.Wagon1TileY == y) ||
                    (t.Wagon2TileX == x && t.Wagon2TileY == y))
                    return false; 
            }

            _tiles[x, y].Switch();

            TGFxManager.Fx.PlaySwitchNoise();

            // switching costs one point
            _points.Points--;

            return true;
        }

        /// <summary>
        /// debounce a click on the user interface to prevent double clicks
        /// </summary>        
        private bool Debounce(GameTime time)
        {            
            if (time.TotalGameTime.TotalMilliseconds - _debounceStart > 300)
            {
                _debounceStart = time.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }

        #region Update

        public Result Update(GameTime gameTime)
        {
            // do nothing in pause mode
            if (_pauseStart != 0.0)
                return Result.Pause;

            foreach( TGTrain t in _trains )
            {             
                t.Move(gameTime,_fastForward);

                // check if a trains is inside a station, pick up passengers there
                if( t.CheckCenterCrossing() )
                {
                    TGStation s = _stations.TrainInStation(t);
                    if (s != null)
                    {
                        t.Stop(gameTime.TotalGameTime.TotalMilliseconds + 1000);
                        if( s.Passengers.RemovePassengers(t.GetColor, ref _points, _fastForward) )                        
                            TGFxManager.Fx.PlayCoinNoise();
                    }
                }
            }

            if (LevelManager.Levels.GetLevel(_levelNumber).LevelCleared(_points.PickedupPassengers))
            {
                TGFxManager.Fx.PlayLevelClearedNoise();
                return Result.LevelCleared;
            }
            
            if ( _currentStage != LevelManager.Levels.GetLevel(_levelNumber).Passenger2Stage( _points.PickedupPassengers ) )
            {                
                _currentStage++;
                CreateTrains();
                _stations.ClearPassengers();
                return Result.NewStage;
            }

            // check train crashes
            for (int i = 0; i < _trains.Count; i++)
            {
                for (int j = 0; j < _trains.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (_trains[i].CheckTrainCrash(_trains[j]))
                    {
                        _statusPosition = _trains[i].CrashSite;

                        TGFxManager.Fx.PlayCrashNoise();

                        return Result.TrainCrash;
                    }
                }
            }           

            // create random passengers somewhere
            TGColor[] colors;
            
            if( _currentStage == TGColor.Red )
                colors = new TGColor[1]{TGColor.Red};
            else if( _currentStage == TGColor.Blue )            
                colors = new TGColor[2] { TGColor.Red, TGColor.Blue };
            else
                colors = new TGColor[3] { TGColor.Red, TGColor.Blue, TGColor.Green };

            int spawnLikelyhood = LevelManager.Levels.GetLevel(_levelNumber).SpawnLikelyHood(_currentStage);

            // in fast forward passengers spawn more often
            if (_fastForward)
                spawnLikelyhood *= 2;

            int angryTime = LevelManager.Levels.GetLevel(_levelNumber).AngryTime;

            // in fast forward passengers get angry faster
            if (_fastForward)
                angryTime /= 2;

            if (_stations.SpawnPassenger(gameTime, content, colors, angryTime, spawnLikelyhood) == false)
            {
                _statusPosition = _stations.OverflowSite;

                TGFxManager.Fx.PlayStationNoise();

                return Result.StationOverflow;
            }

            _stations.Update(gameTime);

            return Result.NoResult;
        }

        #endregion

        public Int32 Points
        {
            get
            {
                return _points.Points;
            }
        }

        /// <summary>
        /// get the position of the last incedent (station full or train crash)
        /// </summary>
        public Vector2 StatusPosition
        {
            get
            {
                return _statusPosition;
            }
        }

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {            
            DrawTiles(spriteBatch);                        

            // draw the trains
            foreach (TGTrain t in _trains)
                t.Draw(spriteBatch);

            // and the stations
            _stations.Draw(spriteBatch);

            // and the game status and points
            spriteBatch.Draw(_metalFrame, new Vector2(0,0), Color.White);
            spriteBatch.Draw(_coins, new Vector2(20, 20), Color.White);
            spriteBatch.Draw(_passenger, new Vector2(265, 20), Color.White); // 275

            spriteBatch.Draw(_stop, new Vector2(95, 20), Color.White);
            spriteBatch.Draw(_boost, new Vector2(160, 20), Color.White);
            spriteBatch.Draw(_clock, new Vector2(220, 20), Color.White);            
            
            DrawShadowedString(spriteBatch, _smallFont, _points.Points.ToString(), new Vector2(50, 22), Color.White);
            DrawShadowedString(spriteBatch, _smallFont, _points.PickedupPassengers.ToString(), new Vector2(280, 22), Color.White);
            DrawShadowedString(spriteBatch, _smallFont, _points.Stops.ToString(), new Vector2(125, 22), Color.White);
            DrawShadowedString(spriteBatch, _smallFont, _points.Boosts.ToString(), new Vector2(185, 22), Color.White);


            if (_tapAction == TapAction.Stop)
            {
                spriteBatch.Draw(_selection, new Vector2(95, 47), Color.White);
            }
            else
            {
                spriteBatch.Draw(_selection, new Vector2(155, 47), Color.White);
            }


            if (_fastForward)
            {
                spriteBatch.Draw(_selection, new Vector2(210, 47), Color.White);
            }
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        /// <summary>
        /// handle the back button
        /// </summary>        
        public bool Back(GameTime gameTime)
        {
            if (_pauseStart != 0.0)
                return true;
            else
                _pauseStart = gameTime.TotalGameTime.TotalSeconds;                

            return false;
        }

        /// <summary>
        /// continue after a pause
        /// </summary>        
        public void Continue(GameTime gameTime)
        {
            _stations.SetPauseOffset(gameTime.TotalGameTime.TotalSeconds - _pauseStart);

            _pauseStart = 0.0;
        }

        /// <summary>
        /// Draws each tile in the level.
        /// </summary>
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // For each tile position, draw a background tile
            for (int y = 0; y <= Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Vector2 p = new Vector2(x, y) * TGTile.Size;
                    spriteBatch.Draw(_background, p, Color.White);                
                }
            }

            // For each tile position, draw all other tiles
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (_tiles[x, y].IsSwitchable())
                    {
                        Vector2 p = new Vector2(x, y) * TGTile.Size;
                        spriteBatch.Draw(_switch, p, Color.White);
                    }
                    //else
                    _tiles[x, y].Draw(spriteBatch, new Vector2(x, y));                                        
                }
            }
        }

        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        private int Width
        {
            get { return _tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        private int Height
        {
            get { return _tiles.GetLength(1); }
        }
    }
}
