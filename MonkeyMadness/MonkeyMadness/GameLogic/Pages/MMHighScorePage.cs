using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class MMHighScorePage
    {
#if WINDOWS_PHONE || WINDOWS
        float _startTime = 0;           // starttime of the delay
        bool _delay = false;
#endif

        Texture2D _wp7logo;
        Texture2D _androidlogo;
        Texture2D _w7logo;

        MMButton _buttonExit;        

        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;
        //MMHighscore _weekhighscore;
        //MMHighscore _allhighscore;          

        SpriteFont _smallFont;
        SpriteFont _largeFont;

        MMButton _buttonEasy;
        MMButton _buttonMedium;
        MMButton _buttonHard;

        MMTransparentLabel _updateLabel;

        string _lastGameUser = "";
        int _lastGamePoints = 0;
        MMSettings.DifficultyType _lastGameDifficulty = MMSettings.DifficultyType.easy;

        public enum Result
        {
            exit,
            noresult
        }

        public MMHighScorePage(Game gameMain, Rectangle screenRect)
        {
             _screenRect = screenRect;

             _buttonEasy = new MMButton(gameMain, "Easy", true, true);
             _buttonMedium = new MMButton(gameMain, "Medium", true, true);
             _buttonHard = new MMButton(gameMain, "Hard", true, true);

            _buttonMedium.CenterPosition(new Vector2(_screenRect.Width / 2 , 130));             
            _buttonEasy.CenterPosition(new Vector2( _buttonMedium.GetRect().X - _buttonMedium.DefaultTexture.Width/2 - 50, 130));
            _buttonHard.CenterPosition(new Vector2(_buttonMedium.GetRect().X + _buttonMedium.DefaultTexture.Width + _buttonMedium.DefaultTexture.Width/2 + 50, 130));

            _updateLabel = new MMTransparentLabel(gameMain, "Updating");
            _updateLabel.CenterPosition(new Vector2(_screenRect.Width / 2, 270));

            _wp7logo = gameMain.Content.Load<Texture2D>("buttons/wp7");
            _androidlogo = gameMain.Content.Load<Texture2D>("buttons/android");
            _w7logo = gameMain.Content.Load<Texture2D>("buttons/w7");

            int y = 440;

#if XBOX
            y -= 30;
#endif


            _buttonExit = new MMButton(gameMain, "Back", false,false);
            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));

            _buttonExit.Hover = true;

            _font = gameMain.Content.Load<SpriteFont>("fonts/ButtonFont");
            _smallFont = gameMain.Content.Load<SpriteFont>("fonts/smallFont");
            _largeFont = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            
            _background = gameMain.Content.Load<Texture2D>("background/form_aux");

            //_weekhighscore = new MMHighscore();
            //_allhighscore = new MMHighscore();       
        }

        private void UpdateHighscore()
        {
            string mode = "1";

            switch (Difficulty)
            {
                case MMSettings.DifficultyType.easy:
                    mode = "1";
                    break;
                case MMSettings.DifficultyType.medium:
                    mode = "2";
                    break;
                case MMSettings.DifficultyType.hard:
                    mode = "3";
                    break;
            }

            //_weekhighscore.GetScores(mode, "TOP10WEEK");
            //_allhighscore.GetScores(mode, "TOP10ALL");           
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

            if (_buttonExit.Update(mousePosition, mouseDown))
                return Result.exit;

            if (_buttonEasy.Update(mousePosition, mouseDown))
            {
                _buttonMedium.Press = false;
                _buttonHard.Press = false;
                UpdateHighscore();
            }
            if (_buttonMedium.Update(mousePosition, mouseDown))
            {
                _buttonEasy.Press = false;
                _buttonHard.Press = false;
                UpdateHighscore();
            }

            if (_buttonHard.Update(mousePosition, mouseDown))
            {
                _buttonEasy.Press = false;
                _buttonMedium.Press = false;
                UpdateHighscore();
            }

            return Result.noresult;
        }        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);

            Vector2 position = new Vector2(50, 20);

#if XBOX
            position.Y += 20;
#endif

            // draw page caption
            DrawShadowedString(spriteBatch, _largeFont, "Highscore", position, Color.Gold);            

            _buttonEasy.Draw(spriteBatch,_font);
            _buttonMedium.Draw(spriteBatch,_font);
            _buttonHard.Draw(spriteBatch, _font);
            
            // Draw table headers 
            position.X = 20;
#if XBOX 
            position.X += 100;
#endif

            position.Y = 170;
            DrawShadowedString(spriteBatch, _smallFont, "Local", position, Color.LightBlue);

#if WINDOWS_PHONE || WINDOWS

            position.X = 280;
            DrawShadowedString(spriteBatch, _smallFont, "Week", position, Color.LightBlue);

            position.X = 540;
            DrawShadowedString(spriteBatch, _smallFont, "Total", position, Color.LightBlue);

            position.X = 20;
            position.Y = 200;
#endif

#if XBOX 
            position.X = 20;
            position.X += 100;
            position.Y = 200;
#endif

            // draw tables
            DrawTable(spriteBatch, LocalHighscoreManager.Highscore.GetScores(Difficulty), position);


#if WINDOWS_PHONE || WINDOWS
            position.X = 280;

            //if(_weekhighscore.HighScore != null)
              //  DrawTable(spriteBatch, _weekhighscore.HighScore.Scores, position);

            position.X = 540;

            //if(_allhighscore.HighScore != null)
              //  DrawTable(spriteBatch, _allhighscore.HighScore.Scores, position);

           // if (_allhighscore.IsUpdating || _weekhighscore.IsUpdating)
           //     _updateLabel.Draw(spriteBatch, _largeFont);
#endif
           
            _buttonExit.Draw(spriteBatch, _font);
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
                        if (_lastGameDifficulty == Difficulty && _lastGameUser == scores[i].Name && _lastGamePoints == scores[i].Points)
                            currentUser = true;

                    DrawShadowedString(spriteBatch, _smallFont, (i + 1).ToString() + ".", p, currentUser? Color.DarkRed : Color.LightBlue);
                    //spriteBatch.DrawString(_smallFont, (i + 1).ToString() + ".", p, Color.LightBlue ); // draw rank

                    p.X += 30;

                    spriteBatch.DrawString(_smallFont, scores[i].Name, p, currentUser ? Color.DarkRed : Color.Gold); // draw name

                    p.X += 130;


                    // draw icon somewhere
                    if (scores[i].Info == "2")
                        spriteBatch.Draw(_wp7logo, new Vector2(p.X + 65, p.Y), Color.White);
                    else if (scores[i].Info == "4")
                        spriteBatch.Draw(_androidlogo, new Vector2(p.X + 65, p.Y), Color.White);
                    else if (scores[i].Info == "1")
                        spriteBatch.Draw(_w7logo, new Vector2(p.X + 65, p.Y), Color.White);                  

                    String helper = "00000";

                    int offset = (int)_smallFont.MeasureString(helper).X - (int)_smallFont.MeasureString(scores[i].Points.ToString()).X;

                    p.X += offset;

                    // draw 0 only if there is a name
                    if( scores[i].Name.Length != 0 )
                        spriteBatch.DrawString(_smallFont, scores[i].Points.ToString(), p, currentUser ? Color.DarkRed : Color.Gold); // draw points                    

                    p.Y += 20;
                    p.X = position.X;
                }
            }
        }

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

#if WINDOWS_PHONE || WINDOWS
                _startTime = 0;
                _delay = true; // update the highscore a bit later             
#endif
            }
        }

        public void LastGame(string name, int points, MMSettings.DifficultyType difficulty)
        {
            _lastGameUser = name;
            _lastGamePoints = points;
            _lastGameDifficulty = difficulty;
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        public void Down()
        {
            if (_buttonEasy.Hover || _buttonMedium.Hover || _buttonHard.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }            
            else
            {
                Unhover();
                _buttonEasy.Hover = true;
            }

        }

        public void Up()
        {
            if (_buttonEasy.Hover || _buttonMedium.Hover || _buttonHard.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }            
            else
            {
                Unhover();
                _buttonEasy.Hover = true;
            }
        }

        public void Unhover()
        {
            _buttonExit.Hover = false;
            _buttonEasy.Hover = false;
            _buttonMedium.Hover = false;
            _buttonHard.Hover = false;            
        }

        public void Left()
        {
            if (_buttonEasy.Hover)
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
        }
    }
}
