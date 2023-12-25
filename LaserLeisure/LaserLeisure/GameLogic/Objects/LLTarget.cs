using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    /// <summary>
    /// class defining a target for a laser
    /// </summary>
    class LLTarget : LLObject
    {
        LLAnimatedTexture _smoke;
        LLAnimatedTexture _texture;
        ObjectColor _color;

        /// <summary>
        /// ctor
        /// </summary>        
        public LLTarget(ContentManager content, Vector2 position, ObjectColor color)
            : base(content, position, 0)
        {                        
            _color = color;
            _content = content;
            _smoke = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _smoke.Load(_content, "objects/smoke", 12, 12);
            _smoke.Pause();
            Reset();            
            UpdateLines();
        }

        /// <summary>
        /// reset the target
        /// </summary>
        public override void Reset()
        {
            _smoke.Pause();
            _status = ObjectStatus.alive;

            _texture = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            switch (_color)
            {
                case ObjectColor.blue:
                    _texture.Load(_content, "objects/target_blue", 32, 32);
                    break;
                case ObjectColor.green:
                    _texture.Load(_content, "objects/target_green", 32, 32);
                    break;
                case ObjectColor.red:
                    _texture.Load(_content, "objects/target_red", 32, 32);
                    break;
            }

            _texture.Stop();
        }

        /// <summary>
        /// size of the target
        /// </summary>        
        public override Vector2 Size()
        {
            return _texture.Size;
        }

        /// <summary>
        /// kill the object
        /// </summary>
        public override void Kill(Random r)
        {
            // target is dead if hit a wrong laser
            _status = ObjectStatus.dead;
            _texture.Load(_content, "objects/target_dead", 1, 1);
            _smoke.SeekTo(r.Next(0, 11)); // seek to a random position in the animation
            _smoke.Play();
        }


        /// <summary>
        /// target reacts on a hit
        /// </summary>        
        protected override Boolean ObjectHit(ObjectColor color)
        {
            if (color != _color) // wrong color, kill the object
            {
                LLFxManager.Fx.PlayFailBell();
                
                Kill(new Random((int)DateTime.Now.Ticks));
                return true; // kill the beam
            }

            if (_status == ObjectStatus.alive)
            {
                LLFxManager.Fx.PlaySuccessBell();

                _texture.Play();
                _status = ObjectStatus.finished;
            }
            return true; // kill the beam
        }

        /// <summary>
        /// update the lines defining the object
        /// </summary>
        protected override void UpdateLines()
        {            
            Vector2 p1 = new Vector2(_position.X, _position.Y - Size().Y / 2 + 5);
            Vector2 p2 = new Vector2(_position.X, _position.Y + Size().Y / 2 - 5);

            _lines.Add(new LLLine(p1, p2));            

            p1 = new Vector2(_position.X - Size().X / 2 + 5, _position.Y );
            p2 = new Vector2(_position.X + Size().X / 2 - 5, _position.Y );

            _lines.Add(new LLLine(p1, p2));

            float y = (float)(Math.Sin(Math.PI/4) * Size().X / 2 - 2);
            float x = (float)(Math.Cos(Math.PI/4) * Size().Y / 2 - 2);

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
            _smoke.UpdateFrame(elapsed);
        }

        /// <summary>
        /// draw the target
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
            if (_smoke.Is_paused == false)
                _smoke.DrawFrame(spriteBatch, new Vector2(_position.X - _smoke.Size.X / 2, _position.Y - _smoke.Size.Y / 2), SpriteEffects.None);            
        }      
    }
}
