using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TrainTrouble.Properties;

namespace TGGameLibrary
{
    class TGHighScorePage
    {
#if WINDOWS_PHONE || WINDOWS
        float _startTime = 0;           // starttime of the delay
        bool _delay = false;
#endif

        TGButton _buttonExit;        
        Texture2D _accept;

        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;        

        SpriteFont _smallFont;
        SpriteFont _largeFont;

        TGLevelButton _level0;
        TGLevelButton _level1;
        TGLevelButton _level2;
        TGLevelButton _level3;
        TGLevelButton _level4;

        Vector2 _l0p;
        Vector2 _l1p;
        Vector2 _l2p;
        Vector2 _l3p;
        Vector2 _l4p;

        Int32 _selectedLevel = 0;

        TGTransparentLabel _updateLabel;

        string _lastGameUser = "";
        int _lastGamePoints = 0;
        Int32 _lastLevel = 0;

        public enum Result
        {
            exit,
            noresult
        }

        public TGHighScorePage(IServiceProvider serviceProvider, Rectangle screenRect)
        {
             _screenRect = screenRect;

             ContentManager content = new ContentManager(serviceProvider, "Content");

             _level0 = new TGLevelButton(content, LevelManager.Levels.GetLevel(0));
             _level1 = new TGLevelButton(content, LevelManager.Levels.GetLevel(1));
             _level2 = new TGLevelButton(content, LevelManager.Levels.GetLevel(2));
             _level3 = new TGLevelButton(content, LevelManager.Levels.GetLevel(3));
             _level4 = new TGLevelButton(content, LevelManager.Levels.GetLevel(4));

             _l0p = new Vector2(80, 135);
             _l1p = new Vector2(240, 135);
             _l2p = new Vector2(400, 135);
             _l3p = new Vector2(560, 135);
             _l4p = new Vector2(720, 135);

             _level0.CenterPosition(_l0p);
             _level1.CenterPosition(_l1p);
             _level2.CenterPosition(_l2p);
             _level3.CenterPosition(_l3p);
             _level4.CenterPosition(_l4p);

            _updateLabel = new TGTransparentLabel(content, Resources.highScoreUpdate);
            _updateLabel.CenterPosition(new Vector2(_screenRect.Width / 2, 330));

            _accept = content.Load<Texture2D>("level_icons/accept");

            int y = 440;

            _buttonExit = new TGButton(content, Resources.back, false,false);
            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            _font = content.Load<SpriteFont>("fonts/ButtonFont");
            _smallFont = content.Load<SpriteFont>("fonts/smallFont");
            _largeFont = content.Load<SpriteFont>("fonts/largeFont");
            
            _background = content.Load<Texture2D>("background/small");            
        }        

        public Result Update(GameTime gameTime, Point mousePosition, bool mouseDown)
        {

#if WINDOWS_PHONE || WINDOWS

            if (_delay && _startTime == 0)
                _startTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            if (gameTime.TotalGameTime.TotalMilliseconds - _startTime > 500 && _delay)
            {
                _delay = false;              
            }
#endif

            if (_buttonExit.Update(mousePosition, mouseDown))
                return Result.exit;

            if (_level0.Update(mousePosition, mouseDown))
            {             
                _selectedLevel = 0;                
            }
            if (_level1.Update(mousePosition, mouseDown))
            {             
                _selectedLevel = 1;
            }

            if (_level2.Update(mousePosition, mouseDown))
            {                
                _selectedLevel = 2;                
            }

            if (_level3.Update(mousePosition, mouseDown))
            {                
                _selectedLevel = 3;                
            }
            if (_level4.Update(mousePosition, mouseDown))
            {                
                _selectedLevel = 4;                
            }

            return Result.noresult;
        }        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);

            Vector2 position = new Vector2(5, 20);


            // draw page caption
            DrawShadowedString(spriteBatch, _largeFont, Resources.highScoreCaption, position, new Color(25,87,168,255));            

            _level0.Draw(spriteBatch,_font);
            _level1.Draw(spriteBatch, _font);
            _level2.Draw(spriteBatch, _font);
            _level3.Draw(spriteBatch, _font);
            _level4.Draw(spriteBatch, _font);

            DrawShadowedString(spriteBatch, _smallFont, AcceptedName(), new Vector2( _screenRect.Width/2 - _smallFont.MeasureString(AcceptedName()).X/2, 200), new Color(25,87,168,255));
            
            position.X = 30;
            position.Y = 250;
            
            // draw tables
            DrawTable(spriteBatch, LocalHighscoreManager.Highscore.GetScores(_selectedLevel), position);

            spriteBatch.Draw(_accept, AcceptPosition(), Color.White);
                       
            _buttonExit.Draw(spriteBatch, _font);
        }

        private Vector2 AcceptPosition()
        {
            switch (_selectedLevel)
            {
                default:
                case 0:
                    return _l0p - new Vector2(_accept.Width/2,_accept.Height/2);
                case 1:
                    return _l1p - new Vector2(_accept.Width / 2, _accept.Height / 2);
                case 2:
                    return _l2p - new Vector2(_accept.Width / 2, _accept.Height / 2);
                case 3:
                    return _l3p - new Vector2(_accept.Width / 2, _accept.Height / 2);
                case 4:
                    return _l4p - new Vector2(_accept.Width / 2, _accept.Height / 2);
            }
        }

        private String AcceptedName()
        {
            switch (_selectedLevel)
            {
                default:
                case 0:
                    return LevelManager.Levels.Level0.LevelName;
                case 1:
                    return LevelManager.Levels.Level1.LevelName;
                case 2:
                    return LevelManager.Levels.Level2.LevelName;
                case 3:
                    return LevelManager.Levels.Level3.LevelName;
                case 4:
                    return LevelManager.Levels.Level4.LevelName;
            }
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

                    DrawShadowedString(spriteBatch, _smallFont, (i + 1).ToString() + ".", p, currentUser? Color.DarkRed : Color.Crimson);
                    //spriteBatch.DrawString(_smallFont, (i + 1).ToString() + ".", p, Color.Crimson ); // draw rank

                    p.X += 30;

                    spriteBatch.DrawString(_smallFont, scores[i].Name, p, currentUser ? Color.DarkRed : new Color(25,87,168,255)); // draw name

                    p.X += 130;

                    String helper = "000000";

                    int offset = (int)_smallFont.MeasureString(helper).X - (int)_smallFont.MeasureString(scores[i].Points.ToString()).X;

                    p.X += offset;

                    // draw 0 only if there is a name
                    if( scores[i].Name.Length != 0 )
                        spriteBatch.DrawString(_smallFont, scores[i].Points.ToString(), p, currentUser ? Color.DarkRed : new Color(25,87,168,255)); // draw points

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
        }

        public void SetExitText(String s)
        {
            _buttonExit.SetText(s);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }        
    }
}
