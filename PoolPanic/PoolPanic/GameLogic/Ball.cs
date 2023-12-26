using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace PoolPanic
{
    /// <summary>
    /// class for ball drawing and simulation
    /// </summary>
    public class Ball
    {
        // stores all _collisions per shot to avoid double collision
        private List<Object> _collisions = new List<Object>();
        private Texture2D _texture;
        private TimeSpan _sunkTime;
        private Vector2 _holePosition;
        private bool _inHole = false;
        private int _inHoleTime = 1500;
              
        // create the ball using an initial position and give the ball a number from 0 to 15
        public Ball(ContentManager content, Vector2 position, Int32 number)
        {                       
            _texture = content.Load<Texture2D>("images/ball" + number);

            
            Radius = _texture.Width/2;
            Position = position;
            Speed = new Vector2(0, 0);
            Rotation = 0.0f;            
            Number = number;
            Active = true;
            _inHole = false;
        }

        /// <summary>
        /// number 0 is the white ball
        /// </summary>
        public Int32 Number { get; set; }

        /// <summary>
        /// check if the ball is a full ball
        /// </summary>
        public Boolean IsFull
        {
            get { return Number > 0 && Number < 8; }
        }

        /// <summary>
        /// check if the ball is a half ball
        /// </summary>
        public Boolean IsHalf
        {
            get { return Number > 8 && Number < 16; }
        }

        /// <summary>
        /// _radius of a ball
        /// </summary>
        public float Radius { get; set; }

        private Vector2 p = new Vector2();

        /// <summary>
        /// position of the ball on the _texture
        /// </summary>
        public Vector2 Position 
        { 
            get
            {
                return p;
            }
            set
            {
                p = value;

            }
        
        }

        /// <summary>
        /// speed vector of the ball
        /// </summary>
        public Vector2 Speed { get; set; }

        public float NormalizedSpeed()
        {
            return (float)Math.Sqrt((float)((Speed.X * Speed.X) + (Speed.Y * Speed.Y))) / 1000;
        }

        /// <summary>
        /// rotation of the _texture
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// set or get ball active 
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// start the ball in hole animation
        /// </summary>
        public void PutInHole(TimeSpan currentTime, Vector2 holePosition)
        {
            _sunkTime = currentTime;
            _inHole = true;
            _holePosition = holePosition;            
        }

        /// <summary>
        /// check if the ball is in a hole
        /// </summary> 
        public bool InHole
        {
            get { return _inHole; }
        }

        /// <summary>
        /// return the distance of the given point to the hole 
        /// </summary>        
        public double Distance(Vector2 p)
        {
            // Find the distance between the centers.
            float dx = p.X - Position.X;
            float dy = p.Y - Position.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// take the ball from the table
        /// </summary>
        public void RemoveFromTable(GameTime UpdateTime)
        {           
            // animate how the ball falls into the hole
            if ((Position.X != (int)_holePosition.X) || (Position.Y != _holePosition.Y))
            {
                // desired distance to move
                double deltaD = 2;

                double step = Math.Sqrt((_holePosition.X - Position.X) * (_holePosition.X - Position.X) + (_holePosition.Y - Position.Y) * (_holePosition.Y - Position.Y));
                double deltaX = (deltaD * (_holePosition.X - Position.X)) / step;
                double deltaY = (deltaD * (_holePosition.Y - Position.Y)) / step;

                if (step > deltaD)
                {
                    // jump towards the final position             
                    Position = new Vector2((float)(Position.X + deltaX), (float)(Position.Y + deltaY));
                }
                else
                {
                    // jump to the new position if the the step would overshoot the final position
                    Position = _holePosition;
                }
            }

            // remove the ball after sitting some time in the hole
            if ((UpdateTime.TotalGameTime - _sunkTime).TotalMilliseconds > _inHoleTime)
                _inHole = false;
        }

        /// <summary>
        /// check if the given _balls collide
        /// </summary>        
        public bool IsCollision(Ball b)
        {
            Vector2 i1, i2;

            if (FindCircleCircleIntersections(Position.X, Position.Y, Radius, b.Position.X, b.Position.Y, b.Radius, out i1, out i2) != 0)
            {
                return true;
            }

            return false;
        }
      
        /// <summary>
        /// calculate the elastic collision and update both _balls
        /// </summary>        
        public void EllasticCollisionPhysics(Ball b)
        {
            //find normal vector
            Vector2 normal = new Vector2(b.Position.X - Position.X, b.Position.Y - Position.Y);

            //find normal vector's modulus, i.e. length
            float normalmod = (float)Math.Sqrt(Math.Pow(normal.X, 2) + Math.Pow(normal.Y, 2));

            //find unitnormal vector
            Vector2 unitnormal = new Vector2((b.Position.X - Position.X) / normalmod, (b.Position.Y - Position.Y) / normalmod);

            //find tangent vector
            Vector2 unittan = new Vector2(-1 * unitnormal.Y, unitnormal.X);

            //first ball normal speed before collision
            float inormalspeedb = unitnormal.X * Speed.X + unitnormal.Y * Speed.Y;

            //first ball tangential speed 
            float itanspeed = unittan.X * Speed.X + unittan.Y * Speed.Y;
            
            //second ball normal speed before collision
            float ynormalspeedb = unitnormal.X * b.Speed.X + unitnormal.Y * b.Speed.Y;

            //second ball tangential speed
            float ytanspeed = unittan.X * b.Speed.X + unittan.Y * b.Speed.Y;

            //tangential speeds don'_texture change whereas normal speeds do

            float mass = 1;
            float mass1 = 1;

            //Calculate normal speeds after the collision
            float inormalspeeda = (inormalspeedb * (mass - mass1) + 2 * mass1 * ynormalspeedb) / (mass + mass1);
            float ynormalspeeda = (ynormalspeedb * (mass1 - mass) + 2 * mass * inormalspeedb) / (mass + mass1);

            //Calculate first ball Velocity vector components (tangential and normal)
            Vector2 inormala = new Vector2(unitnormal.X * inormalspeeda, unitnormal.Y * inormalspeeda);
            Vector2 itana = new Vector2(unittan.X * itanspeed, unittan.Y * itanspeed);

            //Calculate second ball Velocity vector components (tangential and normal)
            Vector2 ynormala = new Vector2(unitnormal.X * ynormalspeeda, unitnormal.Y * ynormalspeeda);
            Vector2 ytana = new Vector2(unittan.X * ytanspeed, unittan.Y * ytanspeed);

            //Add Vector components to each _balls' Velocity
            Speed = Vector2.Add(inormala, itana);
            b.Speed = Vector2.Add(ynormala, ytana);
        }

        /// <summary>
        /// save the current collision to avoid double _collisions per update
        /// </summary>       
        public void AddCollision(Object o)
        {
            _collisions.Add(o);
        }

        /// <summary>
        /// reset all saved collision per update
        /// </summary>
        public void ClearCollision()
        {
            _collisions.Clear();
        }

        /// <summary>
        /// check if the ball already collided with the given ball
        /// </summary>        
        public bool IsCollided(Object o)
        {
            foreach (Object h in _collisions)
            {
                if (h.Equals(o))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// draw ball
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch, GameTime DrawTime)
        {
            //float speed = (float)Speed.Length();            
            //Rotation += (speed / 3 * 2.0f) / 1000;            
            spriteBatch.Draw(_texture, Position, null, Color.White, Rotation, new Vector2(Radius, Radius), 0.5f, SpriteEffects.None, 1.0f);
            
          
        }

        /// <summary>
        /// damp the speed of the ball
        /// </summary>        
        public void Damp(float percent)
        {
            //float length = (float)Math.Sqrt( (float) ((Speed.X * Speed.X) + (Speed.Y * Speed.Y)) );
          
            float x = Speed.X;
            float y = Speed.Y;

            x = (100-percent) * x / 100;
            y = (100-percent) * y / 100;

            // stop it completely if we are slow enough
            if (x < 5 && x > -5)
                x = 0;
            if (y < 5 && y > -5)
                y = 0;

            Speed = new Vector2(x, y);            
        }
                       
        /// <summary>
        /// Find the points where the two circles intersect.
        /// </summary>        
        private int FindCircleCircleIntersections( float cx0, float cy0, float radius0, float cx1, float cy1, float radius1, out Vector2 intersection1, out Vector2 intersection2)
        {
            // Find the distance between the centers.
            float dx = cx0 - cx1;
            float dy = cy0 - cy1;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // See how many solutions there are.
            if (dist > radius0 + radius1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if (dist < Math.Abs(radius0 - radius1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if ((dist == 0) && (radius0 == radius1))
            {
                // No solutions, the circles coincide.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else
            {
                // Find a and h.
                double a = (radius0 * radius0 - radius1 * radius1 + dist * dist) / (2 * dist);
                double h = Math.Sqrt(radius0 * radius0 - a * a);

                // Find P2.
                double cx2 = cx0 + a * (cx1 - cx0) / dist;
                double cy2 = cy0 + a * (cy1 - cy0) / dist;

                // Get the points P3.
                intersection1 = new Vector2((float)(cx2 + h * (cy1 - cy0) / dist),(float)(cy2 - h * (cx1 - cx0) / dist));
                intersection2 = new Vector2((float)(cx2 - h * (cy1 - cy0) / dist),(float)(cy2 + h * (cx1 - cx0) / dist));

                // See if we have 1 or 2 solutions.
                if (dist == radius0 + radius1) 
                    return 1;
                return 2;
            }
        }          
    }
}
