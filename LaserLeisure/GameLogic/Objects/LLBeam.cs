using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LLGameLibrary
{
    /// <summary>
    /// the beam of a laser
    /// </summary>
    class LLBeam
    {
        Texture2D _textture;
        ContentManager _content;
        List<Vector2> _points = new List<Vector2>();
        Vector2 _origin;
        double _rotation = 0;
        double _initialRotation = 0;
        Vector2 _initialOrigin = new Vector2();        
        const float _maxSpeed = 10.0f;
        const float _woodSpeed = 0.5f;
        float _speed = _maxSpeed;
        LLLaser.ObjectColor _color = LLLaser.ObjectColor.red;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="content">content manager</param>
        /// <param name="origin">origin of the beam</param>
        /// <param name="rotation">rotation of the beam</param>
        public LLBeam(ContentManager content, Vector2 origin, double rotation, LLLaser.ObjectColor color, int startOffset)
        {
            _content = content;

            _initialOrigin = origin;

            float ry = (float)(Math.Sin(rotation) * startOffset);
            float rx = (float)(Math.Cos(rotation) * startOffset);
            _origin.X = origin.X - (int)rx;
            _origin.Y = origin.Y - (int)ry;

            _initialRotation = _rotation = rotation;
            _color = color;

            // load the texture
            switch (_color)
            {
                case LLLaser.ObjectColor.red:
                    _textture = content.Load<Texture2D>("objects/beam_red");
                    break;
                case LLLaser.ObjectColor.blue:
                    _textture = content.Load<Texture2D>("objects/beam_blue");
                    break;
                case LLLaser.ObjectColor.green:
                    _textture = content.Load<Texture2D>("objects/beam_green");
                    break;
            }

            // create the beam at it's origin
            _points.Add(_origin);
            _points.Add(_origin);
        }

        /// <summary>
        /// creates a new beam from this one
        /// </summary>        
        public LLBeam Reset()
        {
            return new LLBeam(_content, _initialOrigin, _initialRotation, _color,18);
        }

        /// <summary>
        /// checks if the beam is still active
        /// </summary>
        /// <returns>true if the beam is still moving</returns>
        public Boolean IsActive
        {
            get
            {
                return _speed > 0.0;
            }
        }

        /// <summary>
        /// draw the beam
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the path of the beam
            for (int i = 1; i < _points.Count; i++)
            {
                LLLine l = new LLLine(new Vector2(_points[i - 1].X, _points[i - 1].Y), new Vector2(_points[i].X, _points[i].Y));

                Point p1 = new Point((int)_points[i - 1].X, (int)_points[i - 1].Y);
                Point p2 = new Point((int)_points[i].X, (int)_points[i].Y);                

                double length = l.GetLength();
                double rot = l.GetRotation();

                int x = (int)p1.X;
                int y = (int)p1.Y;

                // correct rotation scaling issues
                if (rot <= Math.PI / 4 + 0.01 || rot > 7 * Math.PI / 4 - 0.01)
                    y += 3;
                if (rot >= Math.PI - 0.01 && rot < 5 * Math.PI / 4 + 0.01)
                    y -= 2;
                if (rot > 6 * Math.PI / 4 - 0.01 && rot < 6 * Math.PI / 4 + 0.01)
                    y += 1;
                if (rot > 2 * Math.PI / 4 - 0.01 && rot < 3 * Math.PI / 4 + 0.01)
                    x -= 2;
                if (rot > Math.PI - 0.01 && rot < Math.PI + 0.01)
                    x += 1;
                if (rot > 5 * Math.PI / 4 - 0.01 && rot < 5 * Math.PI / 4 + 0.01)
                    x += 2;
                if (rot > 6 * Math.PI / 4 - 0.01 && rot < 6 * Math.PI / 4 + 0.01)
                    x += 3;
                if (rot > 7 * Math.PI / 4 - 0.01 && rot < 7 * Math.PI / 4 + 0.01)
                    x += 1;

                spriteBatch.Draw(_textture, new Rectangle(x, y, (int)length, _textture.Width), null, Color.White, (float)rot, new Vector2(_textture.Width, _textture.Height), SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// move the beam
        /// </summary>
        /// <param name="objects">objects the beam can interact with</param>
        /// <returns>interaction can create a new beam</returns>
        public void Update(LLTools tools, LLBeams beams)
        {
            // return if the beam is dead
            if (IsActive == false)
                return;

            // calculate the x and y portion of the beam movement
            double y = (Math.Sin(_rotation) * _speed);
            double x = (Math.Cos(_rotation) * _speed);

            // the last point where the beam currently is
            Vector2 p = new Vector2(_points[_points.Count - 1].X, _points[_points.Count - 1].Y);

            // update that last point
            p.X -= (float)x;
            p.Y -= (float)y;

            _points[_points.Count - 1] = p;

            // intersection point with an object the beam will possibly hit
            Point intersecPoint = new Point();

            // line of the beam
            LLLine l = new LLLine(_points[_points.Count - 2], _points[_points.Count - 1]);

            // angle of the beam and a possibly hit object
            double angle = 0;
            bool inside = false;

            // now check if the beam hits an object and responde appropriately
            switch (tools.HitTest(l, ref intersecPoint, ref angle, ref inside))
            {                    
                case LLTool.Result.Absorb: // beam is absorbed
                    {
                        LLFxManager.Fx.PlayAbsorbNoise();

                        // revert position to the intersection point
                        float ry = (float)(Math.Sin(_rotation) * 1);
                        float rx = (float)(Math.Cos(_rotation) * 1);
                        intersecPoint.X += (int)rx;
                        intersecPoint.Y += (int)ry;

                        // update the last point of the beam to the intersection point
                        _points[_points.Count - 1] = new Vector2(intersecPoint.X, intersecPoint.Y);

                        // now the beam is dead
                        _speed = 0.0f;
                        break;
                    }
                case LLTool.Result.Mirror:
                    {
                        LLFxManager.Fx.PlayReflectNoise();

                        // revert the last beam step a bit to avoid another collision with the same object
                        float ry = (float)(Math.Sin(_rotation) * 2);
                        float rx = (float)(Math.Cos(_rotation) * 2);

                        intersecPoint.X += (int)rx;
                        intersecPoint.Y += (int)ry;

                        // update the last of the beam with the intersection point
                        _points[_points.Count - 1] = new Vector2(intersecPoint.X, intersecPoint.Y);
                        _points.Add(_points[_points.Count - 1]);

                        // reflect beam on 90° angle hit
                        if (angle <= Math.PI / 2 + 0.1 && angle >= Math.PI / 2 - 0.1)
                            angle = 0;

                        // update the rotation of the beam
                        _rotation += (double)(Math.PI - angle * 2);

                        // wrap around if rotation is greater than 2*PI
                        if (_rotation >= 2 * Math.PI)
                            _rotation -= (double)(2 * Math.PI);
                        if (_rotation < 0)
                            _rotation += 2 * Math.PI;                

                        _rotation = CleanAngle(_rotation);
                        
                        break;
                    }
                case LLTool.Result.Prism:
                    {
                        LLFxManager.Fx.PlayReflectNoise();

                        // revert position to the intersection point
                        float ry = (float)(Math.Sin(_rotation) * 1);
                        float rx = (float)(Math.Cos(_rotation) * 1);                        
                        // update the last point of the beam to the intersection point
                        _points[_points.Count - 1] = new Vector2(intersecPoint.X + (int)rx, intersecPoint.Y + (int)ry);

                        // now the beam is dead
                        _speed = 0.0f;
                        double r1;
                        double r2;
                        Vector2 origin1;
                        Vector2 origin2;
                                              
                        // create 2 new beams with 45° in each direction, if hit the prism with a 90° angle
                        if (angle <= Math.PI / 2 + 0.1 && angle >= Math.PI / 2 - 0.1)
                        {
                            // jump over the line to make sure the new beams don't immediatly intersect with the prism
                            ry = (float)(Math.Sin(_rotation) * 2);
                            rx = (float)(Math.Cos(_rotation) * 2);
                            origin2 = origin1 = new Vector2(intersecPoint.X - (int)rx, intersecPoint.Y - (int)ry);
                   
                            // add 45° in each new direction
                            r1 = _rotation + Math.PI / 4;
                            r2 = _rotation - Math.PI / 4;

                            // correct overflows
                            if (r1 >= 2 * Math.PI)
                                r1 -= 2 * Math.PI;
                            if (r1 < 0 * Math.PI)
                                r1 += 2 * Math.PI;
                            if (r2 < 0)
                                r2 += 2 * Math.PI;
                            if (r2 >= 2 * Math.PI)
                                r2 -= 2 * Math.PI;

                            r1 = CleanAngle(r1);
                            r2 = CleanAngle(r2);
                        }
                        else
                        {
                            // jump over the line to make sure the new beams don't immediatly intersect with the prism
                            ry = (float)(Math.Sin(_rotation) * 2);
                            rx = (float)(Math.Cos(_rotation) * 2);
                            origin1 = new Vector2(intersecPoint.X - (int)rx, intersecPoint.Y - (int)ry);
                            origin2 = new Vector2(intersecPoint.X + (int)rx, intersecPoint.Y + (int)ry);

                            r1 = _rotation; // one beam goes straigt through
                            r2 = _rotation + (double)(Math.PI - angle * 2); // other is reflected
                            
                            // wrap around if rotation is greater than 2*PI
                            if (r2 >= 2 * Math.PI)
                                r2 -= (double)(2 * Math.PI);
                            if (r2 < 0)
                                r2 += 2 * Math.PI;

                            r2 = CleanAngle(r2);
                        }

                        // create two new beams
                        beams.AddBeam(new LLBeam(_content, origin1, r1, _color,2));
                        beams.AddBeam(new LLBeam(_content, origin2, r2, _color,2));
                    }
                    break;
                case LLTool.Result.Splitter:
                    {
                        LLFxManager.Fx.PlayReflectNoise();

                        // revert position to the intersection point
                        float ry = (float)(Math.Sin(_rotation) * 1);
                        float rx = (float)(Math.Cos(_rotation) * 1);
                        // update the last point of the beam to the intersection point
                        _points[_points.Count - 1] = new Vector2(intersecPoint.X + (int)rx, intersecPoint.Y + (int)ry);

                        // now the beam is dead
                        _speed = 0.0f;
                        double r1;
                        double r2;
                        Vector2 origin1;
                        Vector2 origin2;

                        // jump over the line to make sure the new beams don't immediatly intersect with the prism
                        ry = (float)(Math.Sin(_rotation) * 2);
                        rx = (float)(Math.Cos(_rotation) * 2);
                        origin1 = new Vector2(intersecPoint.X - (int)rx, intersecPoint.Y - (int)ry);
                        origin2 = new Vector2(intersecPoint.X + (int)rx, intersecPoint.Y + (int)ry);                                              

                        r1 = _rotation; // one beam goes straigt through

                        // reflect beam on 90° angle hit
                        if (angle <= Math.PI / 2 + 0.1 && angle >= Math.PI / 2 - 0.1)
                            angle = 0;
                        
                        r2 = _rotation + (double)(Math.PI - angle * 2); // other is reflected                                                


                        // wrap around if rotation is greater than 2*PI
                        if (r2 >= 2 * Math.PI)
                            r2 -= 2 * Math.PI;

                        if (r2 < 0)
                            r2 += 2 * Math.PI;                    

                        r2 = CleanAngle(r2);
                        
                        // create two new beams
                        beams.AddBeam(new LLBeam(_content, origin1, r1, _color,2));
                        beams.AddBeam(new LLBeam(_content, origin2, r2, _color,2));
                    }
                    break;
                case LLTool.Result.Wood:
                    if (inside)
                    {
                        LLFxManager.Fx.PlayFireNoise(true);                        
                        _speed = _woodSpeed;
                    }
                    else
                    {
                        LLFxManager.Fx.PlayFireNoise(false);
                        _speed = 0.0f; // kill old beam
                        // create a new beam
                        beams.AddBeam(new LLBeam(_content, _points[_points.Count - 1], _rotation, _color, 1));
                    }

                    break;
                case LLTool.Result.Miss:
                    LLFxManager.Fx.PlayFireNoise(false);
                    break;
            }

            return;
        }

        /// <summary>
        /// forces the beam on a multiple of PI/4 to deal with rounding issues
        /// </summary>        
        private double CleanAngle(double angle)
        {
            if (angle < Math.PI / 4 + 0.2 && angle > Math.PI / 4 - 0.2)            
                return Math.PI / 4;            

            if(angle <  2 * Math.PI / 4 + 0.2 && angle > 2 * Math.PI / 4 - 0.2)
                return 2 * Math.PI / 4;

            if (angle < 3 * Math.PI / 4 + 0.2 && angle > 3 * Math.PI / 4 - 0.2)
                return 3 * Math.PI / 4;

            if (angle < 4 * Math.PI / 4 + 0.2 && angle > 4 * Math.PI / 4 - 0.2)
                return 4 * Math.PI / 4;

            if (angle < 5 * Math.PI / 4 + 0.2 && angle > 5 * Math.PI / 4 - 0.2)
                return 5 * Math.PI / 4;

            if (angle < 6 * Math.PI / 4 + 0.2 && angle > 6 * Math.PI / 4 - 0.2)
                return 6 * Math.PI / 4;

            if (angle < 7 * Math.PI / 4 + 0.2 && angle > 7 * Math.PI / 4 - 0.2)
                return 7 * Math.PI / 4;

            return 0;
        }

        /// <summary>
        /// turn off this beam
        /// </summary>
        public void Kill()
        {
            _speed = 0.0f;
        }

        /// <summary>
        /// check if the beam crashed into an object
        /// </summary>        
        public void Update(LLObjects objects)
        {
            // return if the beam is dead
            if (IsActive == false)
                return;

            // line of the beam
            LLLine l = new LLLine(_points[_points.Count - 2], _points[_points.Count - 1]);

            Point intersecPoint = new Point();

            if (objects.HitTest(l, ref intersecPoint, _color)) // kill the beam if the hittest tells us to do so
            {
                // revert position to the intersection point
                float ry = (float)(Math.Sin(l.GetRotation()) * 1);
                float rx = (float)(Math.Cos(l.GetRotation()) * 1);
                // update the last point of the beam to the intersection point
                _points[_points.Count - 1] = new Vector2(intersecPoint.X + (int)rx, intersecPoint.Y + (int)ry);
                _speed = 0.0f;
            }            
        }
    }   
}
