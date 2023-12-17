using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;



namespace MonkeyMadness
{
    class MMOptionsPage
    {
        MMButton _buttonExit;

        MMButton _buttonEasy;
        MMButton _buttonMedium;
        MMButton _buttonHard;

        MMButton _buttonMusic;
        MMButton _buttonFX;

        MMButton _buttonEnterName;

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

        public MMOptionsPage(Game gameMain, Rectangle screenRect, Rectangle titleSafe)
        {
            _buttonExit = new MMButton(gameMain, "Back", false,false);
            _buttonMusic = new MMButton(gameMain, "Music", true, false);
            _buttonFX = new MMButton(gameMain, "Fx", true, false);
            _buttonEasy = new MMButton(gameMain, "Easy", true,true);
            _buttonMedium = new MMButton(gameMain, "Medium", true,true);
            _buttonHard = new MMButton(gameMain, "Hard", true,true);
            _buttonEnterName = new MMButton(gameMain, "Enter Name", false, false);

            int y = 420;

#if XBOX
            y -= 20;
#endif

            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            _buttonMusic.CenterPosition(new Vector2(320, 230));
            _buttonFX.CenterPosition(new Vector2(510, 230));

            _buttonEasy.CenterPosition(new Vector2(320, 140));
            _buttonMedium.CenterPosition(new Vector2(510, 140));
            _buttonHard.CenterPosition(new Vector2(700, 140));

            _buttonEnterName.CenterPosition(new Vector2(320 , 320));

            _font = gameMain.Content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            _mediumFont = gameMain.Content.Load<SpriteFont>("fonts/mediumFont");
            _smallFont = gameMain.Content.Load<SpriteFont>("fonts/smallFontL");

            _background = gameMain.Content.Load<Texture2D>("background/form_aux");

            _screenRect = screenRect;
        }

        public Result Update(Point mousePosition, bool mouseDown)
        {
        
            if (_buttonExit.Update(mousePosition, mouseDown))
                return Result.exit;


            if (_buttonEasy.Update(mousePosition, mouseDown))
            {
                _buttonMedium.Press= false;
                _buttonHard.Press = false;
            }
            if( _buttonMedium.Update(mousePosition, mouseDown))
            {
                _buttonEasy.Press = false;
                _buttonHard.Press = false;
            }

            if (_buttonHard.Update(mousePosition, mouseDown))
            {
                _buttonEasy.Press = false;
                _buttonMedium.Press = false;
            }

            if (_buttonEnterName.Update(mousePosition, mouseDown))
                _currentState = State.EnterName;

            if (_currentState == State.EnterName)
            {                
                #if WINDOWS_PHONE    || XBOX             

                if (!Guide.IsVisible)
                    Guide.BeginShowKeyboardInput(_playerIndex, Resource1.strUserName, Resource1.strEnterUserName, Playername, delegate(IAsyncResult result) 
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
                   
        public MMSettings.DifficultyType Difficulty
        {
            get
            {
                if (_buttonEasy.Press == true)
                    return MMSettings.DifficultyType.easy;
                if (_buttonMedium.Press == true)
                    return MMSettings.DifficultyType.medium;

                return MMSettings.DifficultyType.hard;
            }
            set
            {
                if (value == MMSettings.DifficultyType.easy)
                {
                    _buttonMedium.Press = false;
                    _buttonHard.Press = false;
                    _buttonEasy.Press = true;
                }
                else if (value == MMSettings.DifficultyType.medium)
                {
                    _buttonMedium.Press = true;
                    _buttonHard.Press = false;
                    _buttonEasy.Press = false;
                }
                else if (value == MMSettings.DifficultyType.hard)
                {
                    _buttonMedium.Press = false;
                    _buttonHard.Press = true;
                    _buttonEasy.Press = false;
                }                
            }
        }

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

        public void Unhover()
        {
            _buttonExit.Hover = false;
            _buttonEasy.Hover = false;
            _buttonMedium.Hover = false;
            _buttonHard.Hover = false;
            _buttonMusic.Hover = false;
            _buttonFX.Hover = false;
            _buttonEnterName.Hover = false;
        }


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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);   

            Vector2 position = new Vector2(50, 20);

#if XBOX
            position.Y += 20;
#endif

            // draw page caption
            DrawShadowedString(spriteBatch, _largeFont, "Options", position, Color.Gold);


            // draw difficulty string
            String s = "Difficulty";                 
            position = new Vector2(10, 120);
            DrawShadowedString(spriteBatch, _mediumFont, s, position, Color.Gold);
               
            _buttonExit.Draw(spriteBatch, _font);


            s = "Audio";

            position.X = 30;
            position.Y = 210;

            DrawShadowedString(spriteBatch, _mediumFont, s, position, Color.Gold);

            _buttonMusic.Draw(spriteBatch, _font);
            _buttonFX.Draw(spriteBatch, _font);

            _buttonEasy.Draw(spriteBatch, _font);
            _buttonMedium.Draw(spriteBatch, _font);
            _buttonHard.Draw(spriteBatch, _font);

            position.X = 30;
            position.Y = 300;

            s = "Name";

            DrawShadowedString(spriteBatch, _mediumFont, s, position, Color.Gold);

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
