using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace TGGameLibrary
{
    public class TGButton
    {
        public Texture2D DefaultTexture
        {
            get { return _textureDefault; }
            set { _textureDefault = value; }
        }

        public Texture2D HoverTexture
        {
            get { return _textureHover; }
            set { _textureHover = value; }
        }

        public Texture2D PressedTexture
        {
            get { return _texturePressed; }
            set { _texturePressed = value; }
        }

        public Texture2D HoverPressedTexture
        {
            get { return _textureHoverPressed; }
            set { _textureHoverPressed = value; }
        }

        public TGButton(ContentManager Content, string text, bool checkButton, bool radioButton)
        {
            _textureDefault = Content.Load<Texture2D>("buttons/button_default");
            _textureHover = Content.Load<Texture2D>("buttons/button_default_hover");
            _texturePressed = Content.Load<Texture2D>("buttons/button_pressed");
            _textureHoverPressed = Content.Load<Texture2D>("buttons/button_pressed_hover");          
            _text = text;
            _checkButton = checkButton;
            _radioButton = radioButton;
            _position = new Vector2(0, 0);


            _buttonNoise = Content.Load<SoundEffect>("sound/button-27");
            _buttonInstance = _buttonNoise.CreateInstance();
            _buttonInstance.IsLooped = false;

            _hoverNoise = Content.Load<SoundEffect>("sound/beep-29");
            _hoverInstance = _hoverNoise.CreateInstance();
            _hoverInstance.IsLooped = false;
        }

        public void CenterPosition(Vector2 pos)
        {
            Vector2 p = new Vector2(pos.X - _textureDefault.Width / 2, pos.Y - _textureDefault.Height / 2);

            _position = p;
        }

        public Boolean Hover
        {
            get
            {
                return _isHovered;
            }
            set
            {
                _isHovered = value;
            }
        }

        public Boolean Press
        {
            get
            {
                return _isPressed;
            }
            set
            {
                _isPressed = value;
            }
        }

        private bool _mousePressed = false;

        public bool Update(Point mousePosition, bool mouseDown)
        {
            Rectangle pos = GetRect();
            
            // play noise on start hover
            //if (pos.Contains(mousePosition) && _isHovered == false)
            //        _hoverInstance.Play();                

            if (mousePosition.X != -1 && mousePosition.Y != -1)
                _isHovered = pos.Contains(mousePosition);


            if (_checkButton)
            {
                //checkbox button
                if (_isHovered && mouseDown)
                {
                    _mousePressed = true;                
                }

                if (!_isHovered && !mouseDown)
                {
                    _mousePressed = false;
                }

                if (_isHovered && !mouseDown && _mousePressed)
                {
                    if( _radioButton )
                        _isPressed = true; // can't unpress a radio by clicking it twice
                    else
                        _isPressed = !_isPressed; // can unlick a check button by pressing it twice
                    
                    _mousePressed = false;

                    if (_isPressed)
                    {                        
                        _buttonInstance.Play();
                        return true;
                    }
                }
            }
            else
            {
                bool onMouseUp = _isPressed && !mouseDown;

                //push button
                _isPressed = _isHovered && mouseDown;

                //return true on Mouse up
                if (onMouseUp)
                {                    
                    _buttonInstance.Play();
                    return true;
                }
            }

           

            return false;
        }

        public void SetText(string s)
        {
            _text = s;
        }

        public Rectangle GetRect()
        {
            Rectangle r = new Rectangle((int)_position.X, (int)_position.Y, (int)_textureDefault.Width, (int)_textureDefault.Height);
            return r;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            if (!_isHovered && !_isPressed)
            {
                //draw default texture
                spriteBatch.Draw(_textureDefault, _position, Color.White); 
            }
            else if (_isHovered && _isPressed)
            {
                //draw hoveredPressed texture
                spriteBatch.Draw(_textureHoverPressed, _position, Color.White);
            }
            else if (_isHovered)
            {
                //draw hovered texture
                spriteBatch.Draw(_textureHover, _position, Color.White);
            }
            else
            {
                //draw pressed texture
                spriteBatch.Draw(_texturePressed, _position, Color.White);
            }

            // center the text
            Vector2 fontSize = font.MeasureString(_text);
            Rectangle buttonPos = new Rectangle((int)_position.X, (int)_position.Y, _textureDefault.Width, _textureDefault.Height);
            Vector2 textPos = new Vector2(buttonPos.X + buttonPos.Width / 2 - fontSize.X / 2, buttonPos.Y + buttonPos.Height / 2 - fontSize.Y / 2);
            
            spriteBatch.DrawString(font, _text, textPos, Color.WhiteSmoke);
            
        }        
         
        private bool _isHovered = false;
        private bool _isPressed = false;

        private Vector2 _position;
        private Texture2D _textureDefault;
        private Texture2D _textureHover;
        private Texture2D _texturePressed;
        private Texture2D _textureHoverPressed;
        private string _text;
        private bool _checkButton;
        private bool _radioButton;

        SoundEffect _buttonNoise;
        SoundEffectInstance _buttonInstance;

        SoundEffect _hoverNoise;
        SoundEffectInstance _hoverInstance;
    }
}
