using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LLGameLibrary
{
    class LLWood : LLTool
    {
        LLAnimatedTexture _smoke;
        Vector2 _smokePos;

        // constructor of a prism
        public LLWood(ContentManager content, Point position, double rotation, bool fix)
            : base(content, position, rotation, fix)
        {
            _texture1 = fix ? content.Load<Texture2D>("objects/wood") : content.Load<Texture2D>("objects/wood");
            _texture2 = content.Load<Texture2D>("objects/wood_selected");
            _texture = _texture1;
            
            _smoke = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _smoke.Load(_content, "objects/smoke", 12, 12);
            _smoke.Pause();

            UpdateLines();
        }

        /// <summary>
        /// update the line of object with the obejct position
        /// </summary>
        protected override void UpdateLines()
        {
            _lines.Clear();

            Vector2 p1 = new Vector2(_position.X - _texture1.Width / 2, _position.Y + _texture1.Height/2);
            Vector2 p2 = new Vector2(_position.X - _texture1.Width / 2, _position.Y - _texture1.Height/2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X + _texture1.Width / 2, _position.Y + _texture1.Height / 2);
            p2 = new Vector2(_position.X + _texture1.Width / 2, _position.Y - _texture1.Height / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - _texture1.Width / 2, _position.Y - _texture1.Height / 2);
            p2 = new Vector2(_position.X + _texture1.Width / 2, _position.Y - _texture1.Height / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X + _texture1.Width / 2, _position.Y + _texture1.Height / 2);
            p2 = new Vector2(_position.X - _texture1.Width / 2, _position.Y + _texture1.Height / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X, _position.Y + _texture1.Height / 2);
            p2 = new Vector2(_position.X, _position.Y - _texture1.Height / 2);

            _lines.Add(new LLLine(p1, p2));

            p1 = new Vector2(_position.X - _texture1.Width/2, _position.Y);
            p2 = new Vector2(_position.X + _texture1.Width/2, _position.Y);

            _lines.Add(new LLLine(p1, p2));

        }

        /// <summary>
        /// check if the given line intersects with the mirror
        /// </summary>
        /// <param name="line">line to test</param>
        /// <param name="intersecPoint">return the intersection point if any</param>
        /// <param name="angle">return the intersection angle if any</param>
        /// <returns>absorb if hit from the back, mirror if hit from the front, or miss</returns>
        public override LLTool.Result Hittest(LLLine line, ref Point intersecPoint, ref double angle, ref bool inside)
        {                        
            foreach (LLLine l in _lines)
            {
                if (l.Intersec(line, ref intersecPoint, ref angle))
                {
                    Rectangle r = new Rectangle(_position.X - _texture1.Width / 2, _position.Y - _texture1.Height / 2, _texture1.Width, _texture1.Height);

                    inside = r.Contains(line.P2);

                    if (inside)
                    {
                        _smoke.Play();
                        _smokePos = new Vector2(line.P2.X, line.P2.Y - _smoke.Size.Y/2);
                    }
                    else
                        _smoke.Stop();

                    return Result.Wood;
                }
            }           
             
            return Result.Miss;
        }

        /// <summary>
        /// update logic
        /// </summary>        
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;            
            _smoke.UpdateFrame(elapsed);
        }

        /// <summary>
        /// draw the tool
        /// </summary>        
        public override void Draw(SpriteBatch spriteBatch)
        {
            // smoke is topmost on the wood tool
            if (_smoke.Is_paused == false)
                _smoke.DrawFrame(spriteBatch, _smokePos, SpriteEffects.None);
        }

        /// <summary>
        /// draw tool before anything else 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void DrawFirst(SpriteBatch spriteBatch)
        {            
            // create the effect of the laser going through the object
            spriteBatch.Draw(_texture, new Vector2(_position.X-_texture.Width/2, _position.Y - _texture.Height/2), Color.White);
        }
    }
}
