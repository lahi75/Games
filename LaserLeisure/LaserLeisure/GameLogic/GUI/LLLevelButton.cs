using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLLevelButton
    {
        String _text;
        ContentManager _content;
        Rectangle _rectangle;
        Texture2D _frameTexture;
        Texture2D _backgroundTexture;
        Texture2D _hoverTexture;
        SpriteFont _font16;
        SpriteFont _font21;
        SpriteFont _font25;
        bool _enabled = false;
        int _levelNumber;
        int _points = 0;

        SoundEffect _buttonNoise;
        SoundEffectInstance _buttonInstance;

        bool _isHovered = false;
        bool _isPressed = false;

        public LLLevelButton(ContentManager content, Rectangle rectangle, int levelNumber)
        {
            _content = content;
            _rectangle = rectangle;
            _text = levelNumber.ToString();
            _levelNumber = levelNumber;
            _frameTexture = _content.Load<Texture2D>("helper/gray_2x2");
            _backgroundTexture = _content.Load<Texture2D>("helper/gray_10x10");
            _hoverTexture = _content.Load<Texture2D>("helper/sand_10x10");
            _font16 = content.Load<SpriteFont>("fonts/tycho_16");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");
            _font25 = content.Load<SpriteFont>("fonts/tycho_25");

            _buttonNoise = content.Load<SoundEffect>("sound/button-27");
            _buttonInstance = _buttonNoise.CreateInstance();
            _buttonInstance.IsLooped = false;            
        }

        public Boolean Enable
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                _frameTexture = _enabled ? _content.Load< Texture2D > ("helper/white_2x2") : _content.Load< Texture2D > ("helper/gray_2x2");
                _backgroundTexture = _enabled ? _content.Load<Texture2D>("helper/red_10x10") : _content.Load<Texture2D>("helper/gray_10x10");
                _isHovered = false;
                _isPressed = false;
            }
        }

        public Boolean ShowPoints { get; set; }

        public void SetPoints(int points)
        {
            _points = points;
        }

        public int LevelNumber
        {
            get
            {
                return _levelNumber;
            }
        }

        public void Position(Point p)
        {
            _rectangle.Location = new Point(p.X - _rectangle.Width / 2, p.Y - _rectangle.Height / 2);
        }

        public Boolean Update(Point position, Boolean clickDown)
        {
            if (!_enabled)
                return false;

            if (position.X != -1 && position.Y != -1 && clickDown)
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
            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _backgroundTexture.Width)
            {
                for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _backgroundTexture.Height)
                {
                    if( _isHovered )
                        spriteBatch.Draw(_hoverTexture, new Vector2(x, y), Color.White);
                    else
                        spriteBatch.Draw(_backgroundTexture, new Vector2(x, y), Color.White);
                }
            }

            // draw fame 
            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _frameTexture.Width)
                spriteBatch.Draw(_frameTexture, new Vector2(x, _rectangle.Top), Color.White);

            for (int x = _rectangle.Left; x < _rectangle.Left + _rectangle.Width; x += _frameTexture.Width)
                spriteBatch.Draw(_frameTexture, new Vector2(x, _rectangle.Bottom - _frameTexture.Height), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _frameTexture.Height)
                spriteBatch.Draw(_frameTexture, new Vector2(_rectangle.Left, y), Color.White);

            for (int y = _rectangle.Top; y < _rectangle.Top + _rectangle.Height; y += _frameTexture.Height)
                spriteBatch.Draw(_frameTexture, new Vector2(_rectangle.Right - _frameTexture.Width, y), Color.White);

            spriteBatch.DrawString(_font25, _text, new Vector2(_rectangle.Center.X - _font25.MeasureString(_text).X / 2, _rectangle.Center.Y - _font25.MeasureString(_text).Y / 2), Color.White);

            if (ShowPoints)
            {
                // draw the points
                string t = _points.ToString();
                spriteBatch.DrawString(_font16, t, new Vector2(_rectangle.Right - _font16.MeasureString(t).X - 5, _rectangle.Top), Color.White);
            }
        }
    }
}
