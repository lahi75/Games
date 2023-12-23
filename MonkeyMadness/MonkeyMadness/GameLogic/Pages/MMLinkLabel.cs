using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonkeyMadness
{
    class MMLinkLabel
    {
        public enum Task
        {
            Webbrowser,
            Email
        }

        Vector2 _position;
        String _action;
        String _param;
        String _text;
        Color _defaultColor;
        Color _hoverColor; 

        SoundEffect _buttonNoise;
        SoundEffectInstance _buttonInstance;        

        SpriteFont _font;

        bool _isHovered = false;
        bool _isPressed = false;

        Task _task;
        
        public MMLinkLabel(Game gameMain, String text, String action, String param, Color defaultColor, Color hoverColor, Task task)
        {
            _font = gameMain.Content.Load<SpriteFont>("fonts/LLTexture");

            _action = action;
            _param = param;
            _text = text;
            _defaultColor = defaultColor;
            _hoverColor = hoverColor;
            _position = Vector2.Zero;

            _buttonNoise = gameMain.Content.Load<SoundEffect>("sound/button-27");
            _buttonInstance = _buttonNoise.CreateInstance();
            _buttonInstance.IsLooped = false;

            _task = task;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void Update(Point mousePosition, bool mouseDown)
        {
            Rectangle r = new Rectangle((int)_position.X, (int)_position.Y, (int)_font.MeasureString(_text).X, (int)_font.MeasureString(_text).Y);

            // make it easier to hit the label
            r.Inflate(20, 20);

            _isHovered = r.Contains(mousePosition);

            bool onMouseUp = _isPressed && !mouseDown;

            // reset the pressed state on mouse up
            if (onMouseUp)
                _isPressed = false;

            // do the click on the link label
            if (_isHovered && mouseDown && _isPressed == false)
            {
                _isPressed = true;
                _buttonInstance.Play();
                Navigate();
            }            
        }

        public void Draw(SpriteBatch spriteBatch)
        {                     
            if( _isHovered )
                spriteBatch.DrawString(_font, _text, _position, _hoverColor);
            else
                spriteBatch.DrawString(_font, _text, _position, _defaultColor);            
        }

        private void Navigate()
        {
            #if WINDOWS_PHONE
            try
            {
                if (_task == Task.Webbrowser)
                {
                    var wbt = new Microsoft.Phone.Tasks.WebBrowserTask();
                    wbt.Uri = new Uri(_action);
                    wbt.Show();
                }
                else
                {
                    var et = new Microsoft.Phone.Tasks.EmailComposeTask();
                    et.To = _action;
                    et.Subject = _param;
                    et.Show();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            #endif
            #if WINDOWS

            switch (_task)
            {
                case Task.Webbrowser:
                    System.Diagnostics.Process.Start(_action);
                    break;
                case Task.Email:
                    System.Diagnostics.Process.Start("mailto:" + _action + "?Subject=" + _param);
                    break;
            }
            #endif
        }        
    }         
}
