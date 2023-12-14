using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonkeyMadness
{
    /// <summary>
    /// collection of all holes in the game
    /// </summary>
    class JJHoles
    {                                
        /// <summary>
        /// constructor
        /// </summary>        
        public JJHoles(Microsoft.Xna.Framework.Game gMain)
        {
            gameMain = gMain;                        
        }

        public void Init(Rectangle screenRect, Rectangle titleSafe, Int32 gameSpeed, Int32 maxLines, Int32 width, Int32 offset, float delta, Texture2D background)
        {
            _titleSafe = titleSafe;
            _screenRect = screenRect;
            _maxLines = maxLines;
            _gameSpeed = gameSpeed;
            _width = width;

            _offset = offset;
            _delta = delta;
            _background = background;

            Reset();
        }

        public void SetBackground(Texture2D background)
        {
            _background = background;
        }

        public void Reset()
        {
            _holes.Clear();

            Random r = new Random((int)DateTime.Now.Ticks);

            // start with 3 holes
            AddHole(r,true);
            AddHole(r,false);
            AddHole(r,false);
        }

        /// <summary>
        /// creates a new hole
        /// </summary>        
        public void AddHole(Random r, bool start)
        {
            JJHole hole = new JJHole(gameMain);

            hole.Init(r, _screenRect, _titleSafe, _gameSpeed, _maxLines, _width, _offset, _delta, start);

            // make sure the new hole doesn't intersect with an existing one
            while (TestInterference(hole))
                hole.Move();

            _holes.Add(hole);            
        }

        /// <summary>
        /// draw all wholes
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (JJHole hole in _holes)
            {
                hole.Draw(spriteBatch,_background);
            }
        }

        /// <summary>
        /// update the position of all holes
        /// </summary>
        public void Move()
        {
            foreach (JJHole hole in _holes)
            {
                hole.Move();
            }
        }

        /// <summary>
        /// clears all holes
        /// </summary>
        public void Clear()
        {
            _holes.Clear();
        }

        /// <summary>
        /// number of holes
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _holes.Count;
            }
        }

        public Boolean TestHole(Int32 line, Int32 position, Boolean sameLine, out bool leftHole)
        {
            foreach (JJHole hole in _holes)
            {
                if (hole.TestHole(line, position, sameLine))
                {
                    leftHole = hole.IsLeft();
                    return true;
                }
            }
            leftHole = false;
            return false;
        }

        protected Boolean TestInterference(JJHole newHole)
        {
            foreach (JJHole hole in _holes)
            {
                if (hole.Intersects(newHole))
                    return true;
            }
            return false;
        }

        List<JJHole> _holes = new List<JJHole>();
        Game gameMain;        
        Rectangle _screenRect;
        Rectangle _titleSafe;
        Int32 _maxLines;
        Int32 _gameSpeed;
        Int32 _width;
        float _delta;
        Int32 _offset;

        Texture2D _background;
    }
}
