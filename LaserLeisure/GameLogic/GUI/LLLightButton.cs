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
    class LLLightButton
    {
        String _text;
        ContentManager _content;
        Rectangle _rectangle;
        Texture2D _texture;
        Texture2D _hoverTexture;

        SoundEffect _buttonNoise;
        SoundEffectInstance _buttonInstance;

        SpriteFont _font;

        bool _isHovered = false;
        bool _isPressed = false;

        public LLLightButton(ContentManager content, Rectangle rectangle, String text)
        {
            _content = content;
            _rectangle = rectangle;
            _text = text;
            _texture = _content.Load<Texture2D>("helper/white_2x2");
            _hoverTexture = _content.Load<Texture2D>("helper/sand_10x10");
            _font = content.Load<SpriteFont>("fonts/tycho_16");

            _buttonNoise = content.Load<SoundEffect>("sound/button-27");
            _buttonInstance = _buttonNoise.CreateInstance();
            _buttonInstance.IsLooped = false;            
        }

        public void Position(Point p)
        {
            _rectangle.Location = new Point(p.X - _rectangle.Width / 2, p.Y - _rectangle.Height / 2);
        }

        public Boolean Update(Point position, Boolean clickDown)
        {
            if (position.X != -1 && position.Y != -1)
                _isHovered = _rectangle.Contains(position);

            bool onMouseUp = _isPressed && !clickDown;

            //push button
            _isPressed = _isHovered && clickDown;

            //return true on Mouse up
            if (onMouseUp)
            {
                if (_buttonInstance.State != SoundState.Playing)
                    _buttonInstance.Play();
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw background            
            if (_isHovered)
            {
                for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _hoverTexture.Width)
                {
                    for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _hoverTexture.Height)
                    {
                        spriteBatch.Draw(_hoverTexture, new Vector2(x, y), Color.White);
                    }
                }
            }


            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _texture.Width)
                spriteBatch.Draw(_texture, new Vector2(x, _rectangle.Top), Color.White);

            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _texture.Width)
                spriteBatch.Draw(_texture, new Vector2(x, _rectangle.Bottom - _texture.Height), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _texture.Height)
                spriteBatch.Draw(_texture, new Vector2(_rectangle.Left, y), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _texture.Height)
                spriteBatch.Draw(_texture, new Vector2(_rectangle.Right - _texture.Width, y), Color.White);

            spriteBatch.DrawString(_font, _text, new Vector2(_rectangle.Center.X - _font.MeasureString(_text).X / 2, _rectangle.Center.Y - _font.MeasureString(_text).Y / 2), Color.White);

        }
    }
}
