using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PoolPanic
{
    /// <summary>
    /// class simulating a billiard texture
    /// </summary>
    class Table
    {
        // the _holes of the _texture
        List<Hole> _holes = new List<Hole>();
        private Texture2D _texture;
        // where is the upper left corner of the table on the screen
        private Point _offset = new Point(150, 210);
        // how large is the border of the table
        private int _border = 60;
        // dimension and location of the table
        private Rectangle _rectangle;        

        /// <summary>
        /// creates the table
        /// </summary>        
        public Table(ContentManager content)
        {
            _texture = content.Load<Texture2D>("images/table");            
            _rectangle = new Rectangle(_offset.X + _border, _offset.Y + _border, _texture.Width - 2 * _border, _texture.Height - 2 * _border);
            CreateHoles();
        }

        /// <summary>
        /// create the holes at the corner positions and upper center and lower center
        /// </summary>
        private void CreateHoles()
        {
            Hole h;

            h = new Hole(new Vector2(0 + _border + _offset.X, 0 + _border + _offset.Y), 0);
            _holes.Add(h);

            h = new Hole(new Vector2(_texture.Width / 2 + _offset.X, 0 + _border + _offset.Y), 1);
            _holes.Add(h);

            h = new Hole(new Vector2(_texture.Width + _offset.X - _border, 0 + _border + _offset.Y), 2);
            _holes.Add(h);

            h = new Hole(new Vector2(0 + _border + _offset.X, _texture.Height - _border + _offset.Y), 3);
            _holes.Add(h);

            h = new Hole(new Vector2(_texture.Width / 2 + _offset.X, _texture.Height - _border + _offset.Y), 4);
            _holes.Add(h);

            h = new Hole(new Vector2(_texture.Width + _offset.X - _border, _texture.Height - _border + _offset.Y), 5);
            _holes.Add(h);
        }

        /// <summary>
        /// check if the given ball is in a hole
        /// </summary>        
        /// <returns>-1 if not in a hole</returns>
        public Hole BallInHole(Ball ball)
        {
            foreach (Hole h in _holes)
            {
                if (h.IsInHole(ball))
                {                    
                    return h;
                }
            }
            return null;
        }

        /// <summary>
        /// clip the line at the borders of the table
        /// </summary>        
        public void Clip(ref Line input)
        {
            Point p1,p2;

            p1 = new Point(_rectangle.Left,_rectangle.Top);
            p1.Y += 2;
            p2 = new Point(_rectangle.Right, _rectangle.Top);
            p2.Y += 2;

            Line upper = new Line(p1, p2);

            p1 = new Point(_rectangle.Left, _rectangle.Top);
            p1.X += 2;            
            p2 = new Point(_rectangle.Left, _rectangle.Bottom);
            p2.X += 2;            

            Line left = new Line(p1, p2);

            p1 = new Point(_rectangle.Left, _rectangle.Bottom);
            p1.Y -= 2;
            p2 = new Point(_rectangle.Right, _rectangle.Bottom); 
            p2.Y -= 2;

            Line lower = new Line(p1, p2);

            p1 = new Point(_rectangle.Right, _rectangle.Top);
            p1.X -= 2;            
            p2 = new Point(_rectangle.Right, _rectangle.Bottom); 
            p2.X -= 2;            

            Line right = new Line(p1, p2);

            Point result = new Point(0,0);
            double angle = 0;

            if (input.Intersec(upper, ref result, ref angle))
            {
                if (InBetween(result, input.P1, input.P2)) 
                {
                    if (_rectangle.Contains(result))
                    {
                        input.UpdateP2(result);
                        return;
                    }
                }
            }
            if (input.Intersec(left, ref result, ref angle))
            {
                if (InBetween(result, input.P1, input.P2))
                
                {
                    if (_rectangle.Contains(result))    
                    {
                        input.UpdateP2(result);
                        return;
                    }
                }
            }
            if (input.Intersec(lower, ref result, ref angle))
            {
                if (InBetween(result, input.P1, input.P2))
                
                {
                    if (_rectangle.Contains(result))    
                    {
                        input.UpdateP2(result);
                        return;
                    }
                }
            }
            if (input.Intersec(right, ref result, ref angle))
            {
                if (InBetween(result, input.P1, input.P2))
                {                    
                    if (_rectangle.Contains(result))
                    {
                        input.UpdateP2(result);
                        return;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("no solution");
        }

        /// <summary>
        /// checks if middle is in between p1 and p2
        /// </summary>        
        private bool InBetween(Point p1, Point middle, Point p2)
        {          
            if( (((Math.Abs(p1.X) < Math.Abs(middle.X)) && (Math.Abs(middle.X) < Math.Abs(p2.X))) ||
                ((Math.Abs(p1.Y) < Math.Abs(middle.Y)) && (Math.Abs(middle.Y) < Math.Abs(p2.Y)))) ||                
                (((Math.Abs(p1.X) > Math.Abs(middle.X)) && (Math.Abs(middle.X) > Math.Abs(p2.X))) ||
                ((Math.Abs(p1.Y) > Math.Abs(middle.Y)) && (Math.Abs(middle.Y) > Math.Abs(p2.Y)))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// bounce the given ball at the table borders
        /// </summary>        
        public void BounceBorder(Ball ball)
        {
            if (ball.Position.X - _rectangle.X < ball.Radius)
            {
                ball.Position = new Vector2(ball.Radius + (ball.Radius - ball.Position.X + _rectangle.X) + _rectangle.X, ball.Position.Y);
                ball.Speed = new Vector2(-ball.Speed.X, ball.Speed.Y);
            }
            if (ball.Position.X > _rectangle.X + _rectangle.Width - ball.Radius)
            {
                ball.Position = new Vector2(2 * (_rectangle.X + _rectangle.Width - ball.Radius) - ball.Position.X, ball.Position.Y);
                ball.Speed = new Vector2(-ball.Speed.X, ball.Speed.Y);
            }
            if (ball.Position.Y - _rectangle.Y < ball.Radius)
            {
                ball.Position = new Vector2(ball.Position.X, ball.Radius + (ball.Radius - ball.Position.Y + _rectangle.Y) + _rectangle.Y);
                ball.Speed = new Vector2(ball.Speed.X, -ball.Speed.Y);
            }
            if (ball.Position.Y > _rectangle.Y + _rectangle.Height - ball.Radius)
            {
                ball.Position = new Vector2(ball.Position.X, 2 * (_rectangle.Y + _rectangle.Height - ball.Radius) - ball.Position.Y);
                ball.Speed = new Vector2(ball.Speed.X, -ball.Speed.Y);
            }
        }        

        public void ClipYPosition(Ball b)
        {
            if (b.Position.Y - b.Radius < _rectangle.Top + 50)
                b.Position = new Vector2(b.Position.X, _rectangle.Top + b.Radius + 50);

            if (b.Position.Y + b.Radius > _rectangle.Bottom - 50)
                b.Position = new Vector2(b.Position.X, _rectangle.Bottom - b.Radius - 50);
        }

        /// <summary>
        /// get the location and size of the _table
        /// </summary>        
        public Rectangle GetBounds()
        {
            return _rectangle;
        }
    
        /// <summary>
        /// draw the _table
        /// </summary>        
        public void DrawTable(SpriteBatch spriteBatch, GameTime DrawTime)
        {            
            spriteBatch.Draw(_texture, new Vector2(_offset.X, _offset.Y), Color.White);            
        }
    }
}
