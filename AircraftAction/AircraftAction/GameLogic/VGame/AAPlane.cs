using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace AAGameLibrary
{
    public class AAPlane
    {
        private Texture2D _texture;
        private Texture2D _texture_selected;
        Boolean _highLight = false;        

        private float _speed = 0.38f;
        float _rotation = 2.0f;

        float _rotationSpeed = 0.007f;

        float _destRotation = 2.0f;

        Vector2 _position = new Vector2( 300,300);

        AALevel.Destination _destination;

        public AAPlane(ContentManager content, AALevel.Destination destination)
        {            
            _destination = destination;

            _texture = content.Load<Texture2D>("sprites/plane");

            switch (_destination)
            {
                default:
                case AALevel.Destination.blue:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_blue");
                    break;
                case AALevel.Destination.red:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_red");
                    break;
                case AALevel.Destination.yellow:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_yellow");
                    break;
                case AALevel.Destination.green:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_green");
                    break;
                case AALevel.Destination.purple:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_purple");
                    break;
                case AALevel.Destination.orange:
                    _texture_selected = content.Load<Texture2D>("sprites/plane_orange");
                    break;
            }            
        }

        public void Update()
        {
            float y = (float)(Math.Sin(_rotation) * _speed);
            float x = (float)(Math.Cos(_rotation) * _speed);

            _position.X -= x;
            _position.Y -= y;


            if (_rotation > _destRotation + 0.01f || _rotation < _destRotation - 0.01f)
            {
                //Debug.WriteLine(_rotation - _destRotation);

                double a1 = _rotation;
                double a2 = _destRotation;

                //Debug.WriteLine("A1: " + _rotation + "   A2: " + _destRotation);
                

                if (a1 > a2)
                {
                    if (a1 - a2 > Math.PI)
                    
                        _rotation += _rotationSpeed;
                    else
                        _rotation -= _rotationSpeed;
                }
                else
                {
                    if (a2 - a1 > Math.PI)
                        _rotation -= _rotationSpeed;
                    else
                        _rotation += _rotationSpeed;
                }

                

                if (_rotation > 2 * Math.PI)
                    _rotation = 0;
                if (_rotation < 0)
                    _rotation = 2 * (float)Math.PI;

            }

        }

        public void SetRotation(float rot)
        {
            _destRotation = rot;
            
        }

        public Point Position()
        {
            return new Point((int)_position.X, (int)_position.Y);
        }

        public Boolean Hittest(Point p)
        {
            Rectangle r = new Rectangle((int)_position.X - _texture.Width / 2, (int)_position.Y - _texture.Height / 2, _texture.Width, _texture.Height);

            return r.Contains(p);            
        }

        public Boolean Selected
        {
            get
            {
                return _highLight;
            }
            set
            {
                _highLight = value;
            }
        }

        public void Draw(SpriteBatch batch) //SpriteBatch to use.
        {
            

            //int length = (int)Math.Sqrt((_p1.X - _p2.X) * (_p1.X - _p2.X) + (_p1.Y - _p2.Y) * (_p1.Y - _p2.Y));
            
           

            //if (length > 0)
            //    rot = (float)Math.Asin((float)(_p1.Y - _p2.Y) / (float)length);



            Texture2D t = Selected ? _texture_selected : _texture;

            batch.Draw(t, _position, null, Color.White, _rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            
        }

    }
}
