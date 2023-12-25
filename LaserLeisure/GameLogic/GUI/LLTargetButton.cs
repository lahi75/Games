using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LLGameLibrary
{
    class LLTargetButton
    {
        Texture2D _texture;
        Texture2D _textture_hover;
        Texture2D _textture_pressed;
        Point _position;
        Rectangle _rectangle;
        bool _hovered = false;

        SoundEffect _buttonNoise;
        SoundEffectInstance _buttonInstance;

        public LLTargetButton(ContentManager content, Point position)
        {
            _texture = content.Load<Texture2D>("buttons/button_target");
            _textture_hover = content.Load<Texture2D>("buttons/button_target_pressed");
            _textture_pressed = content.Load<Texture2D>("buttons/button_pressed_red");
            _position = position;
            _rectangle = new Rectangle(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2, _texture.Width, _texture.Height);

            _buttonNoise = content.Load<SoundEffect>("sound/button-27");
            _buttonInstance = _buttonNoise.CreateInstance();
            _buttonInstance.IsLooped = false;     
        }

        public Boolean Pressed { get; set; }
        public Boolean IsHovered() { return _hovered; }

        public Boolean Update(Point position, Boolean clickDown)
        {
            if (Pressed == false)
            {
                if (clickDown)
                {
                    if (_rectangle.Contains(position))
                    {
                        _hovered = true;
                        return false;
                    }
                }
                else
                {
                    if (_hovered && _rectangle.Contains(position))
                    {
                        if (_buttonInstance.State != SoundState.Playing)
                            _buttonInstance.Play();
                        
                        _hovered = false;
                        Pressed = true;
                        return true;
                    }
                }
                _hovered = false;
            }
            else
            {
                if( clickDown)
                {
                    if (_rectangle.Contains(position))
                    {
                        _hovered = true;
                        return false;
                    }
                }
                else
                {
                    if (_hovered && _rectangle.Contains(position))
                    {
                        if (_buttonInstance.State != SoundState.Playing)
                            _buttonInstance.Play();

                        _hovered = false;
                        Pressed = false;
                        return true;
                    }
                }
                _hovered = false;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Pressed)
            {
                if (_hovered)
                    spriteBatch.Draw(_textture_hover, new Vector2(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2), Color.White);   
                else
                    spriteBatch.Draw(_textture_pressed, new Vector2(_position.X - _textture_pressed.Width / 2, _position.Y - _textture_pressed.Height / 2), Color.White);
            }
            else
            {
                if (_hovered)
                    spriteBatch.Draw(_textture_hover, new Vector2(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2), Color.White);
                else
                    spriteBatch.Draw(_texture, new Vector2(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2), Color.White);
            }
        }
    }
}
