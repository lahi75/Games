using Microsoft.Xna.Framework;
using System;

namespace PoolPanic
{
    /// <summary>
    /// class simulating a whole on the _texture
    /// </summary>
    class Hole
    {
        // _radius of the hole
        const int _radius = 50;

        // a hole is defined by 2 lines
        Vector2 _point1;
        Vector2 _point2;
        Vector2 _point3;
                  
        /// <summary>
        /// hole 0 - 5
        /// </summary>        
        public Hole(Vector2 position, Int32 number)
        {
            Number = number;

            switch (number)
            {
                case 0: // upper left
                    _point1 = new Vector2(position.X, position.Y + _radius);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X + _radius, position.Y);
                    break;
                case 1: // upper center
                    _point1 = new Vector2(position.X-_radius, position.Y);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X + _radius, position.Y);
                    break;
                case 2: // upper right
                    _point1 = new Vector2(position.X-_radius, position.Y);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X, position.Y+_radius);
                    break;
                case 3: // lower left
                    _point1 = new Vector2(position.X, position.Y - _radius);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X + _radius, position.Y);
                    break;
                case 4: // lower center
                    _point1 = new Vector2(position.X-_radius, position.Y);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X + _radius, position.Y);
                    break;
                case 5: // lower right
                    _point1 = new Vector2(position.X-_radius, position.Y);
                    _point2 = new Vector2(position.X, position.Y);
                    _point3 = new Vector2(position.X, position.Y-_radius);
                    break;                    
            }          
        }

        /// <summary>
        /// the number of the hole
        /// </summary>
        public Int32 Number { get; set;}

        public Vector2 Offset
        {
            get
            {
                if (Number == 1)
                    return new Vector2(0, -20);
                if (Number == 4)
                    return new Vector2(0, 20);

                return new Vector2(0, 0);
            }
        }

        /// <summary>
        /// the position of the hole on the table
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _point2;
            }
        }

        /// <summary>
        /// check if the ball is in the hole
        /// </summary>        
        public bool IsInHole(Ball ball)
        {           
            Vector2 depth;

            // test _cue segment 1 of the hole
            if( SolveCircleSegmentCollision(ball, _point1, _point2, out depth) )
            {          
                return true;
            }
            // test _cue segment 2 of the hole
            if (SolveCircleSegmentCollision(ball, _point2, _point3, out depth))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// find the closest between circle and a point on the given _cue segment
        /// </summary>        
        private void ClosestPointFromCircle(Ball b, Vector2 p1, Vector2 p2, out Vector2 closest)
        {
            closest = Vector2.Zero;
            Vector2 segmentVector = p1 - p2;
            Vector2 circleRelative = b.Position - p2;

            Vector2 segmentUnit;
            Vector2.Normalize(ref segmentVector, out segmentUnit);

            float projection = Vector2.Dot(circleRelative, segmentUnit);
            if (projection <= 0)
            {
                closest = p2;
                return;
            }
            if (projection >= segmentVector.Length())
            {
                closest = p1;
                return;
            }

            Vector2 projectionVector = segmentUnit * projection;
            closest = projectionVector + p2;
        }

        /// <summary>
        /// check if the _balls hits the given _cue segment
        /// </summary>        
        private bool SolveCircleSegmentCollision(Ball ball, Vector2 p1, Vector2 p2, out Vector2 depth)
        {
            depth = Vector2.Zero;

            Vector2 closest;
            ClosestPointFromCircle(ball, p1, p2, out closest);

            Vector2 distanceVector = ball.Position - closest;
            if (distanceVector.Length() > ball.Radius)
            {
                return false;
            }
            
            Vector2 unitDist;
            Vector2.Normalize(ref distanceVector, out unitDist);
            depth = unitDist * (ball.Radius - distanceVector.Length());

            return true;
        }  

          /*
          // Find the points of intersection.
          private int FindLineCircleIntersections( float cx, float cy, float radius, Vector2 point1, Vector2 point2, out Vector2 intersection1, out Vector2 intersection2)
          {
              float dx, dy, A, B, C, det, t;

              dx = point2.X - point1.X;
              dy = point2.Y - point1.Y;

              A = dx * dx + dy * dy;
              B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
              C = (point1.X - cx) * (point1.X - cx) +
                  (point1.Y - cy) * (point1.Y - cy) -
                  radius * radius;

              det = B * B - 4 * A * C;
              if ((A <= 0.0000001) || (det < 0))
              {
                  // No real solutions.
                  intersection1 = new Vector2(float.NaN, float.NaN);
                  intersection2 = new Vector2(float.NaN, float.NaN);
                  return 0;
              }
              else if (det == 0)
              {
                  // One solution.
                  t = -B / (2 * A);
                  intersection1 =
                      new Vector2(point1.X + t * dx, point1.Y + t * dy);
                  intersection2 = new Vector2(float.NaN, float.NaN);
                  return 1;
              }
              else
              {
                  // Two solutions.
                  t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                  intersection1 =
                      new Vector2(point1.X + t * dx, point1.Y + t * dy);
                  t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                  intersection2 =
                      new Vector2(point1.X + t * dx, point1.Y + t * dy);
                  return 2;
              }
          }*/    
    }
}
