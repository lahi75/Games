using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class JJMonsters
    {
         /// <summary>
        /// constructor
        /// </summary>        
        public JJMonsters(Game gMain)
        {
            _gameMain = gMain;                        
        }

        /// <summary>
        /// initializes the monsters with game parameters
        /// </summary>        
        public void Init(Rectangle screenRect, Rectangle titleSafe, Int32 gameSpeed, Int32 maxLines, Int32 width, Int32 offset, float delta)
        {
            _titleSafe = titleSafe;
            _screenRect = screenRect;
            _maxLines = maxLines;
            _gameSpeed = gameSpeed;            
            _delta = delta;
            _offset = offset;

            Clear();
        }

        public void Remove()
        {
            if (_monsters.Count > 0)
                _monsters.RemoveAt(0);
        }

        /// <summary>
        /// creates a new monster
        /// </summary>        
        public void AddMonster(Random r)
        {
            List<Int32> numbers = new List<Int32>(); // monsterlist

            for (int i = 1; i <= _maxMonster; i++)
                numbers.Add(i);
            
            // remove all monster numbers that are already there
            foreach (JJMonster m in _monsters)            
                numbers.Remove(m.Number);            
           
            int number;

            // create a new random monster
            if (numbers.Count != 0)
            {
                int s = r.Next(1, numbers.Count+1);
                number = numbers[s-1];
            }
            else
                number = r.Next(1, _maxMonster);


            Boolean ghost= false;
            Boolean vulture = false;

            // every third monster is a ghost
            if (_monsters.Count % 3 == 2)
            {              
                ghost = true;
            }
            if (_monsters.Count % 3 == 1)
            {
                vulture = true;
            }

            JJMonster monster = new JJMonster(_gameMain,number, ghost,vulture);

            monster.Init(r, _screenRect, _titleSafe, _gameSpeed, _maxLines, _offset, _delta);

            // make sure the new hole doesn't intersect with an existing one
            while (TestInterference(monster))
            {
                // move the monster until it doesn't interfere with another monster
                GameTime t = new GameTime();
                monster.Move(t);
            }

            _monsters.Add(monster);            
        }

        /// <summary>
        /// draw all monsters
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (JJMonster monster in _monsters)
            {
                monster.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// update the position of all holes
        /// </summary>
        public void Move(GameTime gameTime)
        {
            foreach (JJMonster monster in _monsters)
            {
                monster.Move(gameTime);
            }
        }

        /// <summary>
        /// clears all monsters
        /// </summary>
        public void Clear()
        {
            _monsters.Clear();
        }

        /// <summary>
        /// number of monster
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _monsters.Count;
            }
        }

        /// <summary>
        /// check if the position hits any monster
        /// </summary>        
        public Boolean TestMonster(Int32 line, Int32 position)
        {
            foreach (JJMonster monster in _monsters)
            {
                if (monster.TestMonster(line,position))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// checks a monster sits on top of an other monster
        /// </summary>        
        protected Boolean TestInterference(JJMonster newMonster)
        {
            foreach (JJMonster monster in _monsters)
            {
                if (monster.Intersects(newMonster))
                    return true;
            }
            return false;
        }

        List<JJMonster> _monsters = new List<JJMonster>(); // monsterlist
        Game _gameMain;             // the game
        Rectangle _screenRect;      // client size of the game
        Rectangle _titleSafe;
        Int32 _maxLines;            // number of lines used in the game
        Int32 _gameSpeed;           // how fast object move        
        float _delta;               // distance between two lines
        Int32 _offset;              // offset of the first line to the top of the screen

        Int32 _maxMonster = 4;
    }    
}
