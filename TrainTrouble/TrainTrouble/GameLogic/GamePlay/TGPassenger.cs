using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGGameLibrary
{
    /// <summary>
    /// class managing a passenger waiting at a station
    /// </summary>
    class TGPassenger
    {
        private Texture2D _texture;
        TGColor _color;
        ContentManager _content;
        double _spawnTime = 0;
        Int32 _angryTime;
                
        double _passengerValue = 100;
        bool _angry = false;

        /// <summary>
        /// constructor
        /// </summary>        
        public TGPassenger(TGColor color, ContentManager content, Int32 angryTime)
        {            
            _color = color;

            if (color == TGColor.Red)
                _texture = content.Load<Texture2D>("items/passenger_red");
            else if( color == TGColor.Blue )
                _texture = content.Load<Texture2D>("items/passenger_blue");
            else
                _texture = content.Load<Texture2D>("items/passenger_green");
            
            _content = content;

            _angryTime = angryTime;
        }

        /// <summary>
        /// add some time on the spawn time to handle game pauses
        /// </summary>        
        public void AddSpawnTime(double time)
        {
            _spawnTime += time;
        }

        /// <summary>
        /// the color of the passenger
        /// </summary>
        public TGColor GetColor
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// 1 -> if the passengers waited 0sex, 0 -> if the passengers waited ANGRYTIME seconds
        /// </summary>
        public double PassengerValue
        {
            get
            {
                return _passengerValue;
            }
        }

        /// <summary>
        /// indicates that the passenger is angry
        /// </summary>
        public Boolean IsAngry
        {
            get
            {
                return _angry;
            }
        }

        /// <summary>
        /// the width of the passenger texture
        /// </summary>
        public int Width
        {
            get
            {
                return _texture.Width;
            }
        }

        /// <summary>
        /// updates the passenger
        /// </summary>        
        public void Update(GameTime gameTime)
        {
            if (_spawnTime == 0)
                _spawnTime = gameTime.TotalGameTime.TotalSeconds;

            Int32 elapsed = (Int32)(gameTime.TotalGameTime.TotalSeconds - _spawnTime);

            if (elapsed > _angryTime)
                _passengerValue = 0;
            else
                _passengerValue = (100 - (100 * (elapsed) / (double)_angryTime)) / 100;

            if (gameTime.TotalGameTime.TotalSeconds - _spawnTime > _angryTime && _angry == false)
            {
                _angry = true;
                
                // passenger gets black after 15 secs not being picked up                
                if (_color == TGColor.Red)
                    _texture = _content.Load<Texture2D>("items/passenger_red_angry");
                else if (_color == TGColor.Blue)
                    _texture = _content.Load<Texture2D>("items/passenger_blue_angry");
                else
                    _texture = _content.Load<Texture2D>("items/passenger_green_angry");
            }
        }

        /// <summary>
        /// draw the passenger
        /// </summary>                
        public void Draw(SpriteBatch spriteBatch, Vector2 p)
        {
            if (_texture != null)
            {
                // Draw it in screen space.                    
                spriteBatch.Draw(_texture, p, Color.White);
            }
        }
    }
}
