using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using TrainTrouble.Properties;


namespace TGGameLibrary
{
    class TGOptionsPage
    {
        TGButton _buttonExit;        

        TGButton _buttonMusic;
        TGButton _buttonFX;

        TGButton _buttonEnterName;

        SpriteFont _largeFont;
        SpriteFont _mediumFont;
        SpriteFont _smallFont;
        Rectangle _screenRect;

        Texture2D _background;
        SpriteFont _font;

        PlayerIndex _playerIndex = PlayerIndex.One;

        State _currentState = State.OptionsPage;

        public enum State
        {
            OptionsPage,
            EnterName
        }

        public enum Result
        {            
            exit,
            noresult
        }

        public TGOptionsPage(IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");

            _buttonExit = new TGButton(content, Resources.back, false, false);
            _buttonMusic = new TGButton(content, Resources.music, true, false);
            _buttonFX = new TGButton(content, Resources.fx, true, false);
            _buttonEnterName = new TGButton(content, "Enter Name", false, false);

            int y = 400;
            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            _buttonMusic.CenterPosition(new Vector2(320, 200));
            _buttonFX.CenterPosition(new Vector2(510, 200));
            _buttonEnterName.CenterPosition(new Vector2(320 , 290));

            _font = content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = content.Load<SpriteFont>("fonts/largeFont");
            _mediumFont = content.Load<SpriteFont>("fonts/mediumFont");
            _smallFont = content.Load<SpriteFont>("fonts/smallFontL");

            _background = content.Load<Texture2D>("background/small");

            _screenRect = screenRect;
        }

        public Result Update(Point mousePosition, bool mouseDown)
        {
        
            if (_buttonExit.Update(mousePosition, mouseDown))
                return Result.exit;
           
            if (_buttonEnterName.Update(mousePosition, mouseDown))
                _currentState = State.EnterName;

            if (_currentState == State.EnterName)
            {                
                #if WINDOWS_PHONE    || XBOX             

                if (!Guide.IsVisible)
                    Guide.BeginShowKeyboardInput(_playerIndex, Resources.username, Resources.enteruser, Playername, delegate(IAsyncResult result) 
                { 
                    string name = Guide.EndShowKeyboardInput(result);

                    if (name != null && name.Length > 0)
                    {
                        if( name.Length > 14 )
                            Playername = name.Remove(14);
                        else
                            Playername = name;
                    }
                    _currentState = State.OptionsPage; 
                }, null);
                
                #endif
            }

                     
            _buttonMusic.Update(mousePosition, mouseDown);

            _buttonFX.Update(mousePosition, mouseDown);            

            return Result.noresult;
        }

        public PlayerIndex GamePadIndex
        {
            get
            {
                return _playerIndex;
            }
            set
            {
                _playerIndex = value;
            }
        }
        
        public string Playername { get; set; }                           

        public Boolean Music
        {
            get
            {
                return _buttonMusic.Press;
            }
            set
            {
                _buttonMusic.Press = value;
            }
        }

        public Boolean FX
        {
            get
            {
                return _buttonFX.Press;
            }
            set
            {
                _buttonFX.Press = value;
            }
        }
        /*
        public void Down()
        {
            if (_buttonEasy.Hover || _buttonMedium.Hover || _buttonHard.Hover)
            {
                Unhover();
                _buttonMusic.Hover = true;
            }
            else if (_buttonMusic.Hover || _buttonFX.Hover)
            {
                Unhover();
                _buttonEnterName.Hover = true;
            }
            else if (_buttonEnterName.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
            else
            {
                Unhover();
                _buttonExit.Hover = true;
            }

        }

        public void Up()
        {
            if (_buttonEasy.Hover || _buttonMedium.Hover || _buttonHard.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
            else if (_buttonMusic.Hover || _buttonFX.Hover)
            {
                Unhover();
                _buttonEasy.Hover = true;
            }
            else if (_buttonEnterName.Hover)
            {
                Unhover();
                _buttonMusic.Hover = true;
            }
            else
            {
                Unhover();
                _buttonEnterName.Hover = true;
            }
        }
         */ 

        public void Unhover()
        {
            //_buttonExit.Hover = false;           
            _buttonMusic.Hover = false;
            _buttonFX.Hover = false;
            _buttonEnterName.Hover = false;
        }

        /*
        public void Left()
        {
            if (_buttonEasy.Hover )
            {
                Unhover();
                _buttonHard.Hover = true;
            }
            else if (_buttonMedium.Hover)
            {
                Unhover();
                _buttonEasy.Hover = true;
            }
            else if (_buttonHard.Hover)
            {
                Unhover();
                _buttonMedium.Hover = true;
            }
            else if ( _buttonFX.Hover )
            {
                Unhover();
                _buttonMusic.Hover = true;
            }
            else if (_buttonMusic.Hover)
            {
                Unhover();
                _buttonFX.Hover = true;
            }
        }

        public void Right()
        {
            if (_buttonEasy.Hover)
            {
                Unhover();
                _buttonMedium.Hover = true;
            }
            else if (_buttonMedium.Hover)
            {
                Unhover();
                _buttonHard.Hover = true;
            }
            else if (_buttonHard.Hover)
            {
                Unhover();
                _buttonEasy.Hover = true;
            }
            else if (_buttonFX.Hover)
            {
                Unhover();
                _buttonMusic.Hover = true;
            }
            else if (_buttonMusic.Hover)
            {
                Unhover();
                _buttonFX.Hover = true;
            }
        }
        */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);   

            Vector2 position = new Vector2(50, 20);

            // draw page caption
            DrawShadowedString(spriteBatch, _largeFont, Resources.options, position, new Color(25,87,168,255));
            
               
            _buttonExit.Draw(spriteBatch, _font);

            String s = Resources.audio;

            position.X = 30;
            position.Y = 180;

            DrawShadowedString(spriteBatch, _mediumFont, s, position, new Color(25,87,168,255));

            _buttonMusic.Draw(spriteBatch, _font);
            _buttonFX.Draw(spriteBatch, _font);            

            position.X = 30;
            position.Y = 270;

            s = Resources.username;

            DrawShadowedString(spriteBatch, _mediumFont, s, position, new Color(25,87,168,255));

            _buttonEnterName.SetText(Playername);
            _buttonEnterName.Draw(spriteBatch, _smallFont);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
