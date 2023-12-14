using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class MMBonus
    {
        public enum BonusType
        {
            Wings = 1,
            Life = 2,
            Amor = 4,
            Drill = 8,
            Parachute = 16,
            Poison = 32,
            Teleport = 64
        }

        /// <summary>
        /// constructor
        /// </summary>
        public MMBonus(Game gameMain)
        {
            // create the animation
            _bonus = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f,true);

            // preload an image, this will be overwritten later!!!!!
            _bonus.Load(gameMain.Content, "bonus/bonus_parachute", 5, 5);            

            _targetTime = (float)gameMain.TargetElapsedTime.Milliseconds / 1000;

            _gameMain = gameMain;
        }

        /// <summary>
        /// initializes the bonus item with all game parameters
        /// </summary>        
        public void Init(Int32 level, Random r, Rectangle screenRect, Int32 gameSpeed, Int32 maxLines, Int32 offset, float delta)
        {
            _width = (int)_bonus.Size.X;
            _screenRect = screenRect;
            
            _maxLines = maxLines;
            _delta = delta;
            _offset = offset;
            _currentLevel = level;

            ShuffleStart(r);
        }

        /// <summary>
        /// update the bonus item game logic here
        /// </summary>        
        public bool Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _bonus.UpdateFrame(elapsed);

            if (_startTime == 0)
                _startTime = (float)gameTime.TotalGameTime.TotalSeconds;

            // return true if the item has to be removed
            if (gameTime.TotalGameTime.TotalSeconds - _startTime > _ttl)
                return true;


            // Bounce control constants
            const float BounceHeight = 0.18f;
            const float BounceRate = 3.0f;
            const float BounceSync = -0.75f;

            // Bounce along a sine curve over time.
            // Include the X coordinate so that neighboring items bounce in a nice wave pattern.            
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + _position.X * BounceSync;
            _bounce = (float)Math.Sin(t) * BounceHeight * _bonus.Size.Y - _bonus.Size.Y / 3;

            return false;
        }

        /// <summary>
        /// draw the texture on the sprite at the current position
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the bonus on the current line
            Vector2 p = _position;
            p.Y -= _bonus.Size.Y;

            p += new Vector2(0.0f, _bounce);

            _bonus.DrawFrame(spriteBatch, p, SpriteEffects.None);
        }

        /// <summary>
        /// check if the current position rans into the bonus
        /// </summary>        
        public Boolean TestBonus(Int32 line, Int32 position)
        {
            if (line == _currentLine)
            {
                if ((position > _position.X) && (position < _position.X + _width))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// read the current bonus item type
        /// </summary>
        public BonusType Item
        {
            get
            {
                return _currentItem;
            }
        }

        /// <summary>
        /// initialize the monster with random values
        /// </summary>
        private void ShuffleStart(Random r)
        {
            Int32 up = r.Next(0, 2) == 0 ? 1 : -1;
            
            _currentLine = r.Next(2, _maxLines);
            
            _position.X = r.Next(25, _screenRect.Width-25);

            // calculate the y position of the bonus            
            _position.Y = _currentLine * _delta - _offset;

            Int32 item;
            item = r.Next(1, 100);

            if (item < 10) // 10% change for the live item
            {
                _currentItem = BonusType.Life;
                _ttl = 10;
                
                _bonus.Load(_gameMain.Content, "bonus/bonus_life", 1, 1);
            }
            else if (item < 20) // 10% chance for monster removal
            {
                if (_currentLevel > 1)
                {
                    _currentItem = BonusType.Poison;
                    _ttl = 15;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_poison", 1, 1);
                }
                else
                {
                    _currentItem = BonusType.Wings;
                    _ttl = 20;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_wings", 1, 1);
                }
            }
            else if (item < 30) // 8% chance for the drill item
            {
                _currentItem = BonusType.Drill;
                _ttl = 15;

                _bonus.Load(_gameMain.Content, "bonus/bonus_drill", 1, 1);
            }
            else if (item < 50) // 20% chance for the amor item if level > 1, otherwise this is a parachute
            {
                if (_currentLevel > 1)
                {
                    _currentItem = BonusType.Amor;
                    _ttl = 20;

                    _bonus.Load(_gameMain.Content, "bonus/bonus_armor", 1, 1);
                }
                else
                {
                    _currentItem = BonusType.Parachute;
                    _ttl = 20;

                    _bonus.Load(_gameMain.Content, "bonus/bonus_parachute", 1, 1);
                }
            }
            else if (item < 75) // 25% chance for the wings
            {
                _currentItem = BonusType.Wings;
                _ttl = 20;

                _bonus.Load(_gameMain.Content, "bonus/bonus_wings", 1, 1);
            }
            else if (item < 90) // 15% chance for the wings
            {
                _currentItem = BonusType.Teleport;
                _ttl = 25;

                _bonus.Load(_gameMain.Content, "bonus/bonus_teleport", 1, 1);
            }
            else // rest for parachute
            {
                _currentItem = BonusType.Parachute;
                _ttl = 20;

                _bonus.Load(_gameMain.Content, "bonus/bonus_parachute", 1, 1);
            }
            
            _startTime = 0;

            _width = (int)_bonus.Size.X;
        }

        Game _gameMain;
        BonusType _currentItem;
        Int32 _ttl;                 // time to live for a bonus
        float _startTime;           // starttime of a bonus

        private float _bounce; // bounces along the y axis
        MMAnimatedTexture _bonus;
        Rectangle _screenRect;      // the size of the game area
        Int32 _currentLine;         // line in the the hole currently is
        Int32 _maxLines;            // maximum number of lines on the screen
        Vector2 _position;          // current screen position of the bonus        
        Int32 _width;
        float _delta;               // distance between two lines
        Int32 _offset;              // distance of the first line from the top of the screen
        float _targetTime;
        Int32 _currentLevel;
    }   
}
