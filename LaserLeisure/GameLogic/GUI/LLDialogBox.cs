using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LLGameLibrary
{
    class LLDialogBox
    {
        private Texture2D _texture;
        protected ContentManager _content;
        protected Rectangle _rectangle;

        public LLDialogBox(ContentManager content, Rectangle rect)
        {
            _content = content;
            _rectangle = rect;
            _texture = content.Load<Texture2D>("redbackground");            
        }

        /// <summary>
        /// draw the wall
        /// </summary>        
        public virtual void Draw(SpriteBatch spriteBatch)
        {            
            // fill the object with tiles
            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _texture.Width)
            {
                for (int y = (int)(_rectangle.Top); y < _rectangle.Top + _rectangle.Height; y += _texture.Height)
                    spriteBatch.Draw(_texture, new Vector2(x, y), Color.White);
            }
        }
    }
}
