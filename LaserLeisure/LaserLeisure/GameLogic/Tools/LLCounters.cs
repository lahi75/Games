using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LLGameLibrary
{
    /// <summary>
    /// collection of all counters
    /// </summary>
    class LLCounters
    {
        List<LLCounter> _counter = new List<LLCounter>();

        /// <summary>
        /// clear the list
        /// </summary>
        public void Clear()
        {
            _counter.Clear();
        }

        /// <summary>
        /// add new counter to list
        /// </summary>        
        public void Add(LLCounter counter)
        {
            _counter.Add(counter);
        }

        /// <summary>
        /// count all items 
        /// </summary>        
        public int CountItems()
        {
            int count = 0;
            foreach (LLCounter c in _counter)
                count += c.Count;

            return count;
        }

        /// <summary>
        /// count all tools in this counter
        /// </summary>
        public void Update(LLTools tools)
        {
            foreach (LLCounter c in _counter)            
                c.Update(tools);            
        }

        /// <summary>
        /// draw them
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (LLCounter c in _counter)
                c.Draw(spriteBatch);
        }
    }
}
