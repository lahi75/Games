using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PoolPanic
{
    /// <summary>
    /// defines a line
    /// </summary>
    class Line
    {
        float _x1 = 0;
        float _y1 = 0;
        float _x2 = 0;
        float _y2 = 0;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        public Line(Vector2 p1, Vector2 p2)
        {
            _x1 = p1.X;
            _y1 = p1.Y;

            _x2 = p2.X;
            _y2 = p2.Y;
        }

        public Line(Point p1, Point p2)
        {
            _x1 = p1.X;
            _y1 = p1.Y;
            _x2 = p2.X;
            _y2 = p2.Y;
        }

        public void UpdateP2(Point p2)
        {
            _x2 = p2.X;
            _y2 = p2.Y;
        }

        /// <summary>
        /// calulate the intersection point and the angle between 2 lines
        /// http://local.wasp.uwa.edu.au/~pbourke/geometry/lineline2d
        /// </summary>
        /// <param name="line">line to test with</param>
        /// <param name="result">intersection point</param>
        /// <param name="angle">angle between the two lines</param>
        /// <returns>true if they intersect, false if not</returns>
        public Boolean Intersec(Line line, ref Point result, ref double angle)
        {
            // denominator
            double d = ((line._y2 - line._y1) * (_x2 - _x1) - (line._x2 - line._x1) * (_y2 - _y1));

            if (d == 0)
                return false; // lines are paralell

            double ua = ((line._x2 - line._x1) * (_y1 - line._y1) - (line._y2 - line._y1) * (_x1 - line._x1)) / d;
            double ub = ((_x2 - _x1) * (_y1 - line._y1) - (_y2 - _y1) * (_x1 - line._x1)) / d;

            // lines intersect if ua and ub between 0 and 1
            //  if ((ua > 0 && ua < 1) && (ub > 0 && ub < 1))
            {
                // calulate the intersection point
                result.X = (int)(_x1 + ua * (_x2 - _x1));
                result.Y = (int)(_y1 + ua * (_y2 - _y1));

                // calculate the triangle between intersection point and starting point of each line
                double a = GetLength(new Vector2(_x1, _y1), new Vector2(line._x1, line._y1));
                double b = GetLength(new Vector2(line._x1, line._y1), new Vector2(result.X, result.Y));
                double c = GetLength(new Vector2(result.X, result.Y), new Vector2(_x1, _y1));

                // caluculate the angle between the two lines
                angle = Math.Acos((b * b + c * c - a * a) / (2 * b * c));

                return true;
            }
           // return false;
        }

        /// <summary>
        /// gets the length of the line p1,p2
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns>lenght</returns>
        private double GetLength(Vector2 p1, Vector2 p2)
        {
            // pythagoras
            return Math.Sqrt((double)(p1.X - p2.X) * (double)(p1.X - p2.X) + (double)(p1.Y - p2.Y) * (double)(p1.Y - p2.Y));
        }

        /// <summary>
        /// get lenght of this line
        /// </summary>
        /// <returns>lenght</returns>
        public double GetLength()
        {
            return GetLength(new Vector2(_x1, _y1), new Vector2(_x2, _y2));
        }

        /// <summary>
        /// gets the rotation of the line
        /// </summary>
        /// <returns>angle in radiant</returns>
        public double GetRotation()
        {
            return GetRotation(new Vector2(_x1, _y1), new Vector2(_x2, _y2));
        }

        /// <summary>
        /// gets the rotation of the given line 
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns>angle in radiant</returns>
        private double GetRotation(Vector2 p1, Vector2 p2)
        {
            double length = GetLength(p1, p2);
            double rot = 0.0f;

            // prevent devide zero
            if (length > 0)
                rot = Math.Asin(((double)p1.Y - (double)p2.Y) / length);

            // quadrant 1 and 4
            if (p2.X > p1.X)
            {
                rot -= Math.PI;
                rot *= -1;
            }
            // quadrant 4
            if (p2.Y > p1.Y && p2.X < p1.X)
                rot += 2 * Math.PI;

            if (rot < 0)
                rot += 2 * Math.PI;
            if (rot > 2 * Math.PI)
                rot -= 2 * Math.PI;

            return rot;
        }

        /// <summary>
        /// last point of the line
        /// </summary>
        public Point P2
        {
            get
            {
                return new Point((int)_x2, (int)_y2);
            }
        }

        /// <summary>
        /// first point of the line
        /// </summary>
        public Point P1
        {
            get
            {
                return new Point((int)_x1, (int)_y1);
            }
        }
    }
}





