using LaserLeisure.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LLGameLibrary
{
    class LLHighScorePage
    {
#if WINDOWS_PHONE || WINDOWS
        float _startTime = 0;           // starttime of the delay
        bool _delay = false;
#endif        

        Rectangle _screenRect;
        Texture2D _background;        
        //LLHighscore _weekhighscore;
        //LLHighscore _allhighscore;

        SpriteFont _font18;
        SpriteFont _font21;

        LLButton _buttonNovice;
        LLButton _buttonAdvanced;
        LLButton _buttonExpert;
        LLButton _buttonMaster;

        Vector2 _l0p;
        Vector2 _l1p;
        Vector2 _l2p;
        Vector2 _l3p;
        Vector2 _l4p;

        Texture2D _texture;

        Int32 _selectedLevel = 0;

        LLTransparentLabel _updateLabel;

        string _lastGameUser = "";
        int _lastGamePoints = 0;
        Int32 _lastLevel = 0;

        public enum Result
        {
            exit,
            noresult
        }

        public LLHighScorePage(IServiceProvider serviceProvider, Rectangle screenRect)
        {
            _screenRect = screenRect;

            ContentManager content = new ContentManager(serviceProvider, "Content");
             
            _buttonNovice = new LLButton(content, Resources.btnNovice, true, true);
            _buttonNovice.CenterPosition(new Vector2(125, 150));

            _buttonNovice.DefaultTexture = _buttonNovice.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonNovice.PressedTexture = _buttonNovice.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonAdvanced = new LLButton(content, Resources.btnAdvanced, true, true);
            _buttonAdvanced.CenterPosition(new Vector2(325, 150));

            _buttonAdvanced.DefaultTexture = _buttonAdvanced.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonAdvanced.PressedTexture = _buttonAdvanced.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonExpert = new LLButton(content, Resources.btnExpert, true, true);
            _buttonExpert.CenterPosition(new Vector2(525, 150));

            _buttonExpert.DefaultTexture = _buttonExpert.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonExpert.PressedTexture = _buttonExpert.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");

            _buttonMaster = new LLButton(content, Resources.btnMaster, true, true);
            _buttonMaster.CenterPosition(new Vector2(725, 150));

            _buttonMaster.DefaultTexture = _buttonMaster.HoverTexture = content.Load<Texture2D>("buttons/button_default_s");
            _buttonMaster.PressedTexture = _buttonMaster.HoverPressedTexture = content.Load<Texture2D>("buttons/button_pressed_s");


            _texture = content.Load<Texture2D>("icons/trophy");

            _l0p = new Vector2(80, 135);
            _l1p = new Vector2(240, 135);
            _l2p = new Vector2(400, 135);
            _l3p = new Vector2(560, 135);
            _l4p = new Vector2(720, 135);            

            _updateLabel = new LLTransparentLabel(content, Resources.highScoreUpdate);
            _updateLabel.CenterPosition(new Vector2(_screenRect.Width / 2, 330));
            
            _font18 = content.Load<SpriteFont>("fonts/tycho_18");
            _font21 = content.Load<SpriteFont>("fonts/tycho_21");

            _background = content.Load<Texture2D>("backgrounds/default");

            //_weekhighscore = new LLHighscore();
            //_allhighscore = new LLHighscore();       
        }

        private void UpdateHighscore()
        {           
            //_weekhighscore.GetScores(_selectedLevel.ToString(), "TOP10WEEK");
            //_allhighscore.GetScores(_selectedLevel.ToString(), "TOP10ALL");           
        }

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown)
        {

#if WINDOWS_PHONE || WINDOWS

            if (_delay && _startTime == 0)
                _startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            if (gameTime.TotalGameTime.TotalMilliseconds - _startTime > 500 && _delay)
            {
                _delay = false;
                UpdateHighscore(); // delayed update of the highscore to make sure previous send data is in there
            }
#endif            

            if (_buttonNovice.Update(mousePosition, mouseDown))
            {
                _buttonAdvanced.Press = false;
                _buttonExpert.Press = false;
                _buttonMaster.Press = false;
                _selectedLevel = 0;
                UpdateHighscore();
            }

            if (_buttonAdvanced.Update(mousePosition, mouseDown))
            {
                _buttonNovice.Press = false;
                _buttonExpert.Press = false;
                _buttonMaster.Press = false;
                _selectedLevel = 1;
                UpdateHighscore();
            }

            if (_buttonExpert.Update(mousePosition, mouseDown))
            {
                _buttonNovice.Press = false;
                _buttonAdvanced.Press = false;
                _buttonMaster.Press = false;
                _selectedLevel = 2;
                UpdateHighscore();
            }

            if (_buttonMaster.Update(mousePosition, mouseDown))
            {
                _buttonNovice.Press = false;
                _buttonAdvanced.Press = false;
                _buttonExpert.Press = false;                
                _selectedLevel = 3;
                UpdateHighscore();
            }

            return Result.noresult;
        }        

        public void Draw(SpriteBatch spriteBatch)
        {
            // fill the object with background tiles 
            for (int x = _screenRect.Left; x < _screenRect.Left + _screenRect.Width; x += _background.Width)
            {
                for (int y = (int)(_screenRect.Top); y < _screenRect.Top + _screenRect.Height; y += _background.Height)
                    spriteBatch.Draw(_background, new Vector2(x, y), Color.White);
            }

            spriteBatch.Draw(_texture, new Vector2(230, 20), Color.White);

            Vector2 position = new Vector2(15, 20);

            // draw page caption
            spriteBatch.DrawString(_font21, Resources.highScoreCaption, position, Color.White);

            _buttonNovice.Draw(spriteBatch, _font18);
            _buttonAdvanced.Draw(spriteBatch, _font18);
            _buttonExpert.Draw(spriteBatch, _font18);
            _buttonMaster.Draw(spriteBatch, _font18);         

            // Draw table headers 
            //position.X = 30;

            //position.Y = 240;
            //spriteBatch.DrawString(_font18, Resources.highScoreLocal, position, Color.White);

            //position.X = 280;
            //spriteBatch.DrawString(_font18, Resources.highScoreWeek, position, Color.White);

            //position.X = 540;
            //spriteBatch.DrawString(_font18, Resources.highScoreAll, position, Color.White);

            position.X = 300;
            position.Y = 180;

            position.Y += 20;

            // draw tables
            DrawTable(spriteBatch, LocalHighscoreManager.Highscore.GetScores(_selectedLevel), position);


#if WINDOWS_PHONE || WINDOWS
            position.X = 280;

         //   if(_weekhighscore.HighScore != null)
         //       DrawTable(spriteBatch, _weekhighscore.HighScore.Scores, position);

            position.X = 540;

         //   if(_allhighscore.HighScore != null)
         //       DrawTable(spriteBatch, _allhighscore.HighScore.Scores, position);

        //    if (_allhighscore.IsUpdating || _weekhighscore.IsUpdating)
         //       _updateLabel.Draw(spriteBatch, _font21);
#endif
            //spriteBatch.Draw(_accept, AcceptPosition(), Color.White);                                   
        }

    

        private void DrawTable(SpriteBatch spriteBatch, Score[] scores, Vector2 position)
        {
            if (scores != null)
            {
                Vector2 p = position;
                for (int i = 0; i < scores.Length; i++)
                {
                    bool currentUser = false;

                    if( _lastGameUser.Length != 0 )
                        if (_lastLevel == _selectedLevel && _lastGameUser == scores[i].Name && _lastGamePoints == scores[i].Points)
                            currentUser = true;

                    spriteBatch.DrawString(_font18, (i + 1).ToString() + ".", p, currentUser ? Color.DarkRed : Color.White);
                    //spriteBatch.DrawString(_smallFont, (i + 1).ToString() + ".", p, Color.White ); // draw rank

                    p.X += 30;

                    spriteBatch.DrawString(_font18, scores[i].Name, p, currentUser ? Color.DarkRed : new Color(25, 87, 168, 255)); // draw name

                    p.X += 130;

                    String helper = "000000";

                    int offset = (int)_font18.MeasureString(helper).X - (int)_font18.MeasureString(scores[i].Points.ToString()).X;

                    p.X += offset;

                    // draw 0 only if there is a name
                    if( scores[i].Name.Length != 0 )
                        spriteBatch.DrawString(_font18, scores[i].Points.ToString(), p, currentUser ? Color.DarkRed : new Color(25, 87, 168, 255)); // draw points

                    p.Y += 20;
                    p.X = position.X;
                }
            }
        }
        
        public Int32 LastLevel 
        {
            get
            {
                return _lastLevel;
            }
            set
            {
                _lastLevel = value;

                switch (_lastLevel)
                {
                    case 0:
                        _buttonNovice.Press = true;
                        _buttonAdvanced.Press = false;
                        _buttonExpert.Press = false;
                        _buttonMaster.Press = false;
                        break;
                    case 1:
                        _buttonNovice.Press = false;
                        _buttonAdvanced.Press = true;
                        _buttonExpert.Press = false;
                        _buttonMaster.Press = false;
                        break;
                    case 2:
                        _buttonNovice.Press = false;
                        _buttonAdvanced.Press = false;
                        _buttonExpert.Press = true;
                        _buttonMaster.Press = false;
                        break;
                    case 3:
                        _buttonNovice.Press = false;
                        _buttonAdvanced.Press = false;
                        _buttonExpert.Press = false;
                        _buttonMaster.Press = true;
                        break;

                }

#if WINDOWS_PHONE || WINDOWS
                _startTime = 0;
                _delay = true; // update the highscore a bit later             
#endif
            }
        }

        public void LastGame(string name, int points, Int32 level)
        {
            _lastGameUser = name;
            _lastGamePoints = points;
            _lastLevel = level;
            _selectedLevel = level;

            switch (_selectedLevel)
            {
                case 0:
                    _buttonNovice.Press = true;
                    _buttonAdvanced.Press = false;
                    _buttonExpert.Press = false;
                    _buttonMaster.Press = false;
                    break;
                case 1:
                    _buttonNovice.Press = false;
                    _buttonAdvanced.Press = true;
                    _buttonExpert.Press = false;
                    _buttonMaster.Press = false;
                    break;
                case 2:
                    _buttonNovice.Press = false;
                    _buttonAdvanced.Press = false;
                    _buttonExpert.Press = true;
                    _buttonMaster.Press = false;
                    break;
                case 3:
                    _buttonNovice.Press = false;
                    _buttonAdvanced.Press = false;
                    _buttonExpert.Press = false;
                    _buttonMaster.Press = true;
                    break;
            }
        }

        public void SetExitText(String s)
        {
            //_buttonExit.SetText(s);
        }      
    }
}
