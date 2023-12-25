using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LLGameLibrary
{
    /// <summary>
    /// count how many tools are within a given rectangle
    /// </summary>
    class LLCounter
    {
        SpriteFont _font25;
        ContentManager _content;
        Rectangle _rect;
        int _count = 0;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLCounter(ContentManager content, Point p)
        {
            _content = content;
            _rect = new Rectangle( p.X - 10, p.Y - 10, 20, 20 );
            _font25 = _content.Load<SpriteFont>("fonts/tycho_25");
        }

        /// <summary>
        /// return the number of coundet items
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// update the number according to how many tools are withing the rectangle
        /// </summary>        
        public void Update(LLTools tools)
        {
            _count = tools.Count(_rect);
        }

        /// <summary>
        /// draw the number
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            String s = _count.ToString();
            spriteBatch.DrawString(_font25, s, new Vector2(_rect.Center.X - _font25.MeasureString(s).X / 2, _rect.Center.Y - _font25.MeasureString(s).Y / 2), Color.White);
        }
    }
}
