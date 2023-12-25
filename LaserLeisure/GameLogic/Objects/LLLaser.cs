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
    /// laser, the origin of a beam
    /// </summary>
    class LLLaser: LLObject         
    {
        private Texture2D _texture;
        private LLAnimatedTexture _smoke;
        private LLBeams _beams;
        private ObjectColor _color;
        
        /// <summary>
        /// ctor
        /// </summary>        
        public LLLaser(ContentManager content, Vector2 position, double rotation, ObjectColor color)
            : base(content, position, rotation)
        {
            _color = color;
            LoadLaserTexture();
            _smoke = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _smoke.Load(_content, "objects/smoke", 12, 12);
            _smoke.Pause();

            UpdateLines();
            _beams = new LLBeams(content, _position, rotation,color);          
        }

        /// <summary>
        /// reset the laser
        /// </summary>
        public override void Reset()
        {
            _status = ObjectStatus.alive;
            _beams.Reset();
            _smoke.Pause();
            LoadLaserTexture();
        }

        /// <summary>
        /// load the right laser color
        /// </summary>
        private void LoadLaserTexture()
        {
            switch (_color)
            {
                case ObjectColor.blue:
                    _texture = _content.Load<Texture2D>("objects/laser_blue");
                    break;
                case ObjectColor.green:
                    _texture = _content.Load<Texture2D>("objects/laser_green");
                    break;
                case ObjectColor.red:
                    _texture = _content.Load<Texture2D>("objects/laser_red");
                    break;
            }
        }

        /// <summary>
        /// size of the laser object
        /// </summary>        
        public override Vector2 Size()
        {            
            return new Vector2(_texture.Width, _texture.Height);            
        }

        /// <summary>
        /// kill the object (i.e. after the bomb exlode)
        /// </summary>
        public override void Kill(Random r)
        {
            _texture = _content.Load<Texture2D>("objects/laser_dead");
            // shutdown all beams
            _beams.ShutDown();
            _smoke.SeekTo(r.Next(0, 11)); // seek to a random position in the animation
            _smoke.Play();
            // laser is dead after hit by a beam
            _status = ObjectStatus.dead;
        }

        /// <summary>
        /// react if the laser is hit by a beam
        /// </summary>        
        protected override Boolean ObjectHit(ObjectColor color)
        {
            LLFxManager.Fx.PlayFailBell();

            Kill(new Random((int)DateTime.Now.Ticks));
            return true; // kill the beam
        }

        /// <summary>
        /// update logic of the laser
        /// </summary>        
        public override void Update(GameTime gameTime, LLTools tools, LLObjects objects)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _beams.Update(tools,objects);
            _smoke.UpdateFrame(elapsed);

            // laser is dead if all beams are dead
            if (_beams.AllDead())
                _status = ObjectStatus.dead;
        }        

        /// <summary>
        /// draw the laser and the beams of a laser
        /// </summary>        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 p = new Vector2(_position.X, _position.Y);

            float rotation = (float)(_rotation + Math.PI);

            // wrap around
            if (rotation >= 2 * Math.PI)
                rotation -= (float)(2 * Math.PI);

            int x = (int)p.X;
            int y = (int)p.Y;

            // correct x, y according to rotation            
            if (rotation > 0 && rotation <= 5 * Math.PI / 4)
                x += 1;
            if (rotation > Math.PI - 0.0001 && rotation < 2 * Math.PI)
                y += 1;

            spriteBatch.Draw(_texture, new Rectangle(x, y, _texture.Width, _texture.Height), null, Color.White, rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 0);

            _beams.Draw(spriteBatch);

            
        }        

        /// <summary>
        /// topmost drawing
        /// </summary>        
        public override void Draw2(SpriteBatch spriteBatch)
        {
            if (_smoke.Is_paused == false)            
                _smoke.DrawFrame(spriteBatch, new Vector2(_position.X - _smoke.Size.X / 2, _position.Y - _smoke.Size.Y / 2), SpriteEffects.None);            
        }
     
        /// <summary>
        /// create the lines enclosing the laser from the laser position
        /// </summary>
        protected override void UpdateLines()
        {            
            // 3 lines
            // 2 left and right and one vertical through the middle

            float y = (float)(Math.Sin(_rotation) * Size().X / 2);
            float x = (float)(Math.Cos(_rotation) * Size().Y / 2);

            Vector2 p1 = new Vector2(_position.X - (int)x, _position.Y - (int)y - Size().Y / 5);
            Vector2 p2 = new Vector2(_position.X + (int)x, _position.Y + (int)y - Size().Y / 5);
            
            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - (int)x, _position.Y - (int)y + Size().Y / 5);
            p2 = new Vector2(_position.X + (int)x, _position.Y + (int)y + Size().Y / 5);

            _lines.Add(new LLLine(p1, p2));

            y = (float)(Math.Sin(_rotation + Math.PI/2) * Size().X / 2);
            x = (float)(Math.Cos(_rotation + Math.PI/2) * Size().Y / 2);

            p1 = new Vector2(_position.X - (int)x, _position.Y - (int)y);
            p2 = new Vector2(_position.X + (int)x, _position.Y + (int)y);

            _lines.Add(new LLLine(p1, p2));           
        } 
    }
}
