using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyMadness
{
    class JJPoints
    {       
        public void Init(Int32 level)
        {
            _points = 0;
            _lastUpdate = 0;
            _startTime = 0;
            _level = level;
        }

        public void Update(GameTime gameTime)
        {
            if( gameTime.TotalGameTime.TotalMilliseconds - _lastUpdate > 1000 )
            {
                if( _points > 0 )
                    _points--;
                _lastUpdate = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (_lastUpdate == 0)
                _lastUpdate = gameTime.TotalGameTime.TotalMilliseconds;

            if (_startTime == 0)
                _startTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Up()
        {
            _points += (int)(20 * LevelModifier());
        }

        public void Fall()
        {

            if (_points > 10 * LevelModifier())
                _points -= (int)(10 * LevelModifier());
            else
                _points = 0;
        }

        public void HeadCrash()
        {
            if (_points > 5 * LevelModifier())
                _points -= (int)(5 * LevelModifier());
            else
                _points = 0;
        }

        public void BonusCatched()
        {
            _points += (int)(15 * LevelModifier());
        }

        public void MonsterCrash()
        {
            if (_points > 15 * LevelModifier())
                _points -= (int)(15* LevelModifier());
            else
                _points = 0;
        }

        public Int32 Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        public double LevelModifier()
        {
            return 0.5 + _level*0.5;
        }        

        double _startTime = 0;        
        double _lastUpdate = 0;

        Int32 _points = 0;
        Int32 _level = 1;


    }
}
