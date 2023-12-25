using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLOptionsPage
    {
        SpriteFont _font18;
        SpriteFont _font21;

        LLButton _ladderMode;
        LLButton _freestyleMode;

        LLButton _buttonMusic;
        LLButton _buttonFX;

        LLButton _buttonEnterName;

        Texture2D _texture;

        Rectangle _screenRect;
        Texture2D _background;
        ContentManager _content;   

        Boolean _back = false;

        public enum OptionsResult
        {
            exit,
            noresult
        }

        State _currentState = State.OptionsPage;

        public enum State
        {
            OptionsPage,
            EnterName
        }

        public LLOptionsPage(ContentManager content, Rectangle screenRect)
        {
            _content = content;

            _background = content.Load<Texture2D>("backgrounds/default");
            
            _screenRect = screenRect;

            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _ladderMode = new LLButton(content, Resources.optionLadder, true, true);
            _ladderMode.CenterPosition(new Vector2(300, 170));

            _freestyleMode = new LLButton(content, Resources.optionFreestyle, true, true);
            _freestyleMode.CenterPosition(new Vector2(540, 170));

            _buttonMusic = new LLButton(content, Resources.optionMusic, true, false);
            _buttonMusic.CenterPosition(new Vector2(300, 270));

            _buttonFX = new LLButton(content, Resources.optionFX, true, false);            
            _buttonFX.CenterPosition(new Vector2(540, 270));

            _buttonEnterName = new LLButton(content, Resources.optionName, false, false);
            _buttonEnterName.CenterPosition(new Vector2(300, 370));

            _texture = content.Load<Texture2D>("icons/settings");
        }

        public OptionsResult Update(Point mousePosition, bool mouseDown)
        {
            if (_back)
            {
                _back = false;
                return OptionsResult.exit;
            }

            if (_buttonEnterName.Update(mousePosition, mouseDown))
                _currentState = State.EnterName;          

            if (_ladderMode.Update(mousePosition, mouseDown))
                _freestyleMode.Press = false;

            if (_freestyleMode.Update(mousePosition, mouseDown))
                _ladderMode.Press = false;

            _buttonMusic.Update(mousePosition, mouseDown);

            _buttonFX.Update(mousePosition, mouseDown);

            return OptionsResult.noresult;
        }

        public void Back()
        {
            _back = true;
        }

        public LLSettings.Mode GameMode
        {
            get
            {
                if (_ladderMode.Press == true)
                    return LLSettings.Mode.ladder;

                return LLSettings.Mode.freeStyle;
            }
            set
            {
                if (value == LLSettings.Mode.ladder)
                {
                    _ladderMode.Press = true;
                    _freestyleMode.Press = false;
                }
                else
                {
                    _ladderMode.Press = false;
                    _freestyleMode.Press = true;
                }
            }
        }

        public string Playername { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with background tiles 
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            spriteBatch.Draw(_texture, new Vector2(230, 20), Color.White);

            Vector2 position = new Vector2(50, 20);

            // draw page caption
            spriteBatch.DrawString( _font21,  Resources.optionHeader, position, Color.White);

            position.X = 50;
            position.Y = 160;

            spriteBatch.DrawString(_font18, Resources.optionsMode, position, Color.White);

            position.X = 50;
            position.Y = 260;

            spriteBatch.DrawString(_font18, Resources.optionsAudio, position, Color.White);

            position.X = 50;
            position.Y = 360;

            spriteBatch.DrawString(_font18, Resources.optionsName, position, Color.White);


            _ladderMode.Draw(spriteBatch, _font18);
            _freestyleMode.Draw(spriteBatch, _font18);
            _buttonMusic.Draw(spriteBatch, _font18);
            _buttonFX.Draw(spriteBatch, _font18);

            _buttonEnterName.SetText(Playername);
            _buttonEnterName.Draw(spriteBatch, _font18);
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
    }
}
