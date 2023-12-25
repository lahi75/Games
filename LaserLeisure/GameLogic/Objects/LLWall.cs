using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LLGameLibrary
{
    /// <summary>
    /// wall which will simply absorb a beam
    /// </summary>
    class LLWall : LLObject
    {
        private Texture2D _texture;
        private Vector2 _size;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLWall(ContentManager content, Vector2 position, Vector2 size, ObjectColor color)
            : base(content, position, 0)
        {
            _size = size;
            _texture = content.Load<Texture2D>("brick");
            UpdateLines();            
        }

        /// <summary>
        /// reset the wall
        /// </summary>
        public override void Reset()
        {
            // nothing to reset here
        }

        /// <summary>
        /// this object is not killable
        /// </summary>
        public override void Kill(Random r)
        {
        }

        /// <summary>
        /// size of the wall, 
        /// </summary>        
        public override Vector2 Size()
        {
            return _size;
        }

        /// <summary>
        /// react on a beam hit
        /// </summary>        
        protected override Boolean ObjectHit(ObjectColor color)
        {
            LLFxManager.Fx.PlayAbsorbNoise();            
            return true; // this one kills a beam
        }

        /// <summary>
        /// creates lines from the given position
        /// </summary>
        protected override void UpdateLines()
        {
            // Span a rectangle of 4 lines around the center point
            // this line is not at the outer border, it's at the start point
            Vector2 p1 = new Vector2(_position.X - (int)Size().X / 2, _position.Y - Size().Y / 2);
            Vector2 p2 = new Vector2(_position.X + (int)Size().X / 2, _position.Y - Size().Y / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - (int)Size().X / 2, _position.Y + Size().Y / 2);
            p2 = new Vector2(_position.X + (int)Size().X / 2, _position.Y + Size().Y / 2);

            _lines.Add(new LLLine(p1, p2));            

            p1 = new Vector2(_position.X - Size().X / 2, _position.Y - Size().Y / 2);
            p2 = new Vector2(_position.X - Size().X / 2, _position.Y + Size().Y / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X + Size().X / 2, _position.Y - Size().Y / 2);
            p2 = new Vector2(_position.X + Size().X / 2, _position.Y + Size().Y / 2);

            _lines.Add(new LLLine(p1, p2));   
        } 

        /// <summary>
        /// update logic of a wall
        /// </summary>        
        public override void Update(GameTime gameTime, LLTools tools, LLObjects objects)
        {            
        }

        /// <summary>
        /// draw the wall
        /// </summary>        
        public override void Draw(SpriteBatch spriteBatch)
        {                       
            // fill the object with tiles
            for (int x = (int)(_position.X - Size().X / 2); x < _position.X + Size().X / 2; x += _texture.Width)
            {
                for( int y = (int)(_position.Y - Size().Y / 2); y < _position.Y + Size().Y / 2; y += _texture.Height)
                    spriteBatch.Draw(_texture, new Vector2(x, y), Color.White);
            }
        }

        /// <summary>
        /// topmost drawing
        /// </summary>        
        public override void Draw2(SpriteBatch spriteBatch)
        {
        }

     
    }
}
