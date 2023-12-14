using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class MMBonuses
    {       
        /// <summary>
        /// constructor
        /// </summary>        
        public MMBonuses(Game gMain)
        {
            _gameMain = gMain;                        
        }

        /// <summary>
        /// initializes the monsters with game parameters
        /// </summary>        
        public void Init(Rectangle screenRect, Int32 gameSpeed, Int32 itemChance, Int32 maxLines, Int32 offset, float delta)
        {
            _screenRect = screenRect;
            _maxLines = maxLines;
            _gameSpeed = gameSpeed;
            _delta = delta;
            _offset = offset;
            _currendSecond = 0;
            _itemChance = itemChance;

            Clear();
        }

        /// <summary>
        /// creates a new bonus
        /// </summary>        
        private void AddBonus(Random r, Int32 level)
        {            
            MMBonus bonus = new MMBonus(_gameMain);

            bonus.Init(level, r, _screenRect, _gameSpeed, _maxLines, _offset, _delta);
            
            _bonuses.Add(bonus);
        }

        /// <summary>
        /// update the bonuses
        /// </summary>
        public void Update(GameTime gameTime, Int32 level)
        {
            // create a random new bonus item here
            if (_currendSecond != (Int32)gameTime.TotalGameTime.TotalSeconds)
            {
                Random r = new Random((int)DateTime.Now.Ticks);

                Int32 percentCreate = r.Next(1,100);

                // create the bonus with a likelyhood of N% per second
                if (percentCreate < _itemChance)
                    AddBonus(r,level);

                _currendSecond = (Int32)gameTime.TotalGameTime.TotalSeconds;               
            }

            foreach (MMBonus bonus in _bonuses)
            {
                if (bonus.Update(gameTime))
                {
                    // remove the item if it was shown long enough
                    _bonuses.Remove(bonus);
                    break;
                }
            }            
        }

        /// <summary>
        /// check if the position hits any bonus
        /// </summary>        
        public Boolean TestBonuses(Int32 line, Int32 position)
        {
            foreach (MMBonus bonus in _bonuses)
            {
                if (bonus.TestBonus(line, position))
                {
                    _lastBonus = bonus.Item; // store the last bonus that was catched by the avatar
                    _bonuses.Remove(bonus);                    
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// returns the last catched bonus
        /// </summary>
        public MMBonus.BonusType LastBonus
        {
            get
            {
                return _lastBonus;
            }
        }

        /// <summary>
        /// draw all bonuses
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MMBonus bonus in _bonuses)
            {
                bonus.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// clears all bonuses
        /// </summary>
        public void Clear()
        {
            _bonuses.Clear();
        }

        MMBonus.BonusType _lastBonus;
        List<MMBonus> _bonuses = new List<MMBonus>(); // bonuslist
        Game _gameMain;             // the game
        Rectangle _screenRect;      // client size of the game
        Int32 _maxLines;            // number of lines used in the game
        Int32 _gameSpeed;           // how fast object move        
        float _delta;               // distance between two lines
        Int32 _offset;              // offset of the first line to the top of the screen
        Int32 _itemChance = 5;      // chance of an item to appear in percent per second
        Int32 _currendSecond;      
    }
}
