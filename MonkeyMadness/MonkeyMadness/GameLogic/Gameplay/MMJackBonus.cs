using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class JJJackBonus
    {
        public JJJackBonus(Game gameMain)
        {
            _bonuses.Add(MMBonus.BonusType.Amor, 0);
            _bonuses.Add(MMBonus.BonusType.Drill, 0);
            _bonuses.Add(MMBonus.BonusType.Parachute, 0);
            _bonuses.Add(MMBonus.BonusType.Wings, 0);

            // create the animation
            _bonus = new MMAnimatedTexture(Vector2.Zero, 0, 0.5f, 0.5f, true);
            _gameMain = gameMain;
        }

        public Boolean CanDrill
        {
            get
            {
                return _bonuses[MMBonus.BonusType.Drill] > 0;
            }
        }

        public Boolean CanAmor
        {
            get
            {
                return _bonuses[MMBonus.BonusType.Amor] > 0;
            }
        }

        public Boolean CanWings
        {
            get
            {
                return _bonuses[MMBonus.BonusType.Wings] > 0;
            }
        }

        public Boolean CanParachute
        {
            get
            {
                return _bonuses[MMBonus.BonusType.Parachute] > 0;
            }
        }

        public void ResetDrill()
        {
            _bonuses[MMBonus.BonusType.Drill] = 0;
        }

        public void ResetParachute()
        {
            _bonuses[MMBonus.BonusType.Parachute] = 0;
        }

        public void Reset()
        {
            _bonuses[MMBonus.BonusType.Drill] = 0;
            _bonuses[MMBonus.BonusType.Amor] = 0;
            _bonuses[MMBonus.BonusType.Parachute] = 0;
            _bonuses[MMBonus.BonusType.Wings] = 0;            
        }

        public void Update(GameTime gameTime)
        {
            // each second decrement a second on all bonuses
            if (_currendSecond != (Int32)gameTime.TotalGameTime.TotalSeconds)
            {
                if( _bonuses[MMBonus.BonusType.Drill] > 0 )
                    _bonuses[MMBonus.BonusType.Drill]--;

                if (_bonuses[MMBonus.BonusType.Amor] > 0)
                    _bonuses[MMBonus.BonusType.Amor]--;

                if (_bonuses[MMBonus.BonusType.Parachute] > 0)
                    _bonuses[MMBonus.BonusType.Parachute]--;

                if (_bonuses[MMBonus.BonusType.Wings] > 0)
                    _bonuses[MMBonus.BonusType.Wings]--;

                _currendSecond = (Int32)gameTime.TotalGameTime.TotalSeconds;        
            }
        }

        public void Debug()
        {
            string bonus = "";
            if (_bonuses[MMBonus.BonusType.Wings] > 0)
                bonus += "w";
            if (_bonuses[MMBonus.BonusType.Amor] > 0)
                bonus += "a";
            if (_bonuses[MMBonus.BonusType.Drill] > 0)
                bonus += "d";
            if (_bonuses[MMBonus.BonusType.Parachute] > 0)
                bonus += "p";

            Console.WriteLine(bonus);
        }

        public void AddBonus(MMBonus.BonusType bonus)
        {
            Reset();

            switch (bonus)
            {
                case MMBonus.BonusType.Drill:
                    if( _bonuses[bonus] < 10 )
                        _bonuses[bonus] += 5;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_drill", 1, 1);
                    break;
                case MMBonus.BonusType.Amor:
                    if( _bonuses[bonus] < 25 )
                        _bonuses[bonus] += 15;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_armor", 1, 1);
                    break;
                case MMBonus.BonusType.Parachute:
                    if( _bonuses[bonus] < 30 )
                        _bonuses[bonus] += 20;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_parachute", 1, 1);
                    break;
                case MMBonus.BonusType.Wings:
                    if( _bonuses[bonus] < 25 )
                        _bonuses[bonus] += 15;
                    _bonus.Load(_gameMain.Content, "bonus/bonus_wings", 1, 1);
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (CanDrill || CanAmor || CanWings || CanParachute)
            {
                position.X -= 10;
                _bonus.DrawFrame(spriteBatch, position, SpriteEffects.None);
            }
        }

        private IDictionary<MMBonus.BonusType, Int32> _bonuses = new Dictionary<MMBonus.BonusType, Int32>();

        Int32 _currendSecond = 0;

        MMAnimatedTexture _bonus;
        Game _gameMain;
    }
}
