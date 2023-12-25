using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLPrism : LLTool
    { 
        // constructor of a prism
        public LLPrism(ContentManager content, Point position, double rotation,bool fix)
            : base(content, position, rotation,fix)
        {
            _texture1 = fix ? content.Load<Texture2D>("objects/splitter_fixed") : content.Load<Texture2D>("objects/splitter");
            _texture2 = content.Load<Texture2D>("objects/splitter_selected");
            _texture = _texture1;
            UpdateLines();
        }

        /// <summary>
        /// update the line of object with the obejct position
        /// </summary>
        protected override void UpdateLines()
        {
            _lines.Clear();

            float y = (float)(Math.Sin(_rotation) * _texture1.Width / 2);
            float x = (float)(Math.Cos(_rotation) * _texture1.Width / 2);

            Vector2 p1 = new Vector2(_position.X - (int)x, _position.Y - (int)y);
            Vector2 p2 = new Vector2(_position.X + (int)x, _position.Y + (int)y);
            
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
                    // check if we hit the mirror from the back
                    double hitDirection = _rotation - line.GetRotation();

                    // wrap around to make sure we are within 0 and 2PI
                    if (hitDirection < 0)
                        hitDirection += 2 * Math.PI;

                    if (hitDirection <= Math.PI)
                        return Result.Absorb;

                    return Result.Prism;
                }
            }
            return Result.Miss;
        }
    }
}
