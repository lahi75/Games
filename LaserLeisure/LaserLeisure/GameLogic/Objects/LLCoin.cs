using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    /// <summary>
    /// class defining a coin, hitting a coin give you more money
    /// </summary>
    class LLCoin : LLObject
    {
        LLAnimatedTexture _texture;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLCoin(ContentManager content, Vector2 position, ObjectColor color)
            : base(content, position, 0)
        {
            _content = content;
            Reset();
            UpdateLines();
        }

        /// <summary>
        /// reset the bomb
        /// </summary>
        public override void Reset()
        {
            _status = ObjectStatus.alive;
            _texture = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            _texture.Load(_content, "objects/coins", 6, 12);
            _texture.Stop();
        }

        /// <summary>
        /// size of the bomb
        /// </summary>        
        public override Vector2 Size()
        {
            return new Vector2(41, 41);
        }

        /// <summary>
        /// remove the object from the game
        /// </summary>
        public override void Kill(Random r)
        {
            if (_status == ObjectStatus.alive)
                _texture.Play();

            _status = ObjectStatus.dead;
        }

        /// <summary>
        /// bomb reacts on a hit
        /// </summary>        
        protected override Boolean ObjectHit(ObjectColor color)
        {
            LLFxManager.Fx.PlayCoinsNoise();

            _texture.Play();
            _status = ObjectStatus.finished;
            return false; // this hit doesn't kill the beam
        }

        /// <summary>
        /// update the lines defining the object
        /// </summary>
        protected override void UpdateLines()
        {
            Vector2 p1 = new Vector2(_position.X, _position.Y - Size().Y / 2 + 5);
            Vector2 p2 = new Vector2(_position.X, _position.Y + Size().Y / 2 - 5);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - Size().X / 2 + 5, _position.Y);
            p2 = new Vector2(_position.X + Size().X / 2 - 5, _position.Y);

            _lines.Add(new LLLine(p1, p2));

            float y = (float)(Math.Sin(Math.PI / 4) * Size().X / 2 - 2);
            float x = (float)(Math.Cos(Math.PI / 4) * Size().Y / 2 - 2);

            p1 = new Vector2(_position.X - (int)x, _position.Y - (int)y);
            p2 = new Vector2(_position.X + (int)x, _position.Y + (int)y);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - (int)x, _position.Y + (int)y);
            p2 = new Vector2(_position.X + (int)x, _position.Y - (int)y);

            _lines.Add(new LLLine(p1, p2));
        }

        /// <summary>
        /// update logic
        /// </summary>        
        public override void Update(GameTime gameTime, LLTools tools, LLObjects objects)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _texture.UpdateFrame(elapsed);
        }

        /// <summary>
        /// draw the coin
        /// </summary>        
        public override void Draw(SpriteBatch spriteBatch)
        {            
            _texture.DrawFrame(spriteBatch, _position, SpriteEffects.None);
        }

        /// <summary>
        /// topmost drawing
        /// </summary>        
        public override void Draw2(SpriteBatch spriteBatch)
        {
        }
    }
}
