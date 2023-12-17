using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    public class MMAchievementsPage
    {
        MMButton _buttonExit;

        MMAchievementButton _buttonAchievement0;
        MMAchievementButton _buttonAchievement1;
        MMAchievementButton _buttonAchievement2;
        MMAchievementButton _buttonAchievement3;
        MMAchievementButton _buttonAchievement4;
        MMAchievementButton _buttonAchievement5;
        MMAchievementButton _buttonAchievement6;

        MMAchievementButton _buttonAchievement7;
        MMAchievementButton _buttonAchievement8;
        MMAchievementButton _buttonAchievement9;
        MMAchievementButton _buttonAchievement10;
        MMAchievementButton _buttonAchievement11;

        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;

        SpriteFont _tinyFont;
        SpriteFont _largeFont;
        SpriteFont _mediumFont;
        SpriteFont _smallFont;

        volatile State _currentState;

        private enum State
        {
            Overview,
            Achievement0Detail,
            Achievement1Detail,
            Achievement2Detail,
            Achievement3Detail,
            Achievement4Detail,
            Achievement5Detail,
            Achievement6Detail,
            Achievement7Detail,
            Achievement8Detail,
            Achievement9Detail,
            Achievement10Detail,
            Achievement11Detail,
        }

        public enum Result
        {
            exit,
            noresult
        }

        public Boolean Back()
        {            
            if (_currentState == State.Overview)
                return true;

            _currentState = State.Overview;            

            return false;
        }


        public MMAchievementsPage(Game gameMain, Rectangle screenRect)
        {
             _screenRect = screenRect;

            _buttonExit = new MMButton(gameMain, "Back", false,false);
            _buttonExit.Hover = true;            
            
            _buttonAchievement0 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementNewbie);
            _buttonAchievement1 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementGhostBuster);
            _buttonAchievement2 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementDrillerKiller);
            _buttonAchievement3 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementSpeedyGonzales);
            _buttonAchievement4 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementLuckyDevil);
            _buttonAchievement5 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementTumbler);
            _buttonAchievement6 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementLefty);
            _buttonAchievement7 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementPleasant);
            _buttonAchievement8 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementIcarus);
            _buttonAchievement9 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementCheater);
            _buttonAchievement10 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementSuper);
            _buttonAchievement11 = new MMAchievementButton(gameMain, AchievementsManager.Achievements.AchievementStalker);

            int y = 420;

#if XBOX
            y -= 20;
#endif

            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));

            _buttonAchievement0.CenterPosition(new Vector2(100, 200));
            _buttonAchievement1.CenterPosition(new Vector2(220, 200));
            _buttonAchievement2.CenterPosition(new Vector2(340, 200));
            _buttonAchievement3.CenterPosition(new Vector2(460, 200));
            _buttonAchievement4.CenterPosition(new Vector2(580, 200));
            _buttonAchievement5.CenterPosition(new Vector2(700, 200));

            _buttonAchievement6.CenterPosition(new Vector2(100, 300));
            _buttonAchievement7.CenterPosition(new Vector2(220, 300));
            _buttonAchievement8.CenterPosition(new Vector2(340, 300));
            _buttonAchievement9.CenterPosition(new Vector2(460, 300));
            _buttonAchievement10.CenterPosition(new Vector2(580, 300));
            _buttonAchievement11.CenterPosition(new Vector2(700, 300));                        

            _font = gameMain.Content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = gameMain.Content.Load<SpriteFont>("fonts/largeFont");
            _smallFont = gameMain.Content.Load<SpriteFont>("fonts/smallFont");
            _tinyFont = gameMain.Content.Load<SpriteFont>("fonts/tinyFont");
            _mediumFont = gameMain.Content.Load<SpriteFont>("fonts/mediumFont");
            
            _background = gameMain.Content.Load<Texture2D>("background/form_aux");
            
            _currentState = State.Overview;
        }

        public Result Update(Point mousePosition, bool mouseDown)
        {
            if (_currentState == State.Overview)
            {
                //update buttons
                if (_buttonAchievement0.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement0Detail;

                if (_buttonAchievement1.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement1Detail;

                if (_buttonAchievement2.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement2Detail;

                if (_buttonAchievement3.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement3Detail;

                if (_buttonAchievement4.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement4Detail;

                if (_buttonAchievement5.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement5Detail;
                
                if (_buttonAchievement6.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement6Detail;

                if (_buttonAchievement7.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement7Detail;

                if (_buttonAchievement8.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement8Detail;

                if (_buttonAchievement9.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement9Detail;

                if (_buttonAchievement10.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement10Detail;

                if (_buttonAchievement11.Update(mousePosition, mouseDown))
                    _currentState = State.Achievement11Detail;
            }

            if (_buttonExit.Update(mousePosition, mouseDown))
            {
                if (_currentState == State.Overview)
                    return Result.exit;
                else
                    _currentState = State.Overview;
            }

            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect, Color.White);   

            Vector2 position;            

            switch (_currentState)
            {
                case State.Overview:                      
                    
                    position = new Vector2(80, 20);         
                    
           #if XBOX
                    position.Y += 20;
           #endif

                    DrawShadowedString(spriteBatch, _largeFont, "Award", position, Color.Gold);

                    _buttonAchievement0.Draw(spriteBatch, _font);
                    Rectangle r = _buttonAchievement0.GetRect();
                    Vector2 pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Newbie").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Newbie", pos, _buttonAchievement0.Hover ? Color.Red : Color.Gold);                    

                    _buttonAchievement1.Draw(spriteBatch, _font);
                    r = _buttonAchievement1.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;                        
                    pos.X -= _tinyFont.MeasureString("Ghost Buster").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Ghost Buster", pos, _buttonAchievement1.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement2.Draw(spriteBatch, _font);
                    r = _buttonAchievement2.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;                        
                    pos.X -= _tinyFont.MeasureString("Driller Killer").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Driller Killer", pos, _buttonAchievement2.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement3.Draw(spriteBatch, _font);
                     r = _buttonAchievement3.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Speedy Gonzsales").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Speedy Gonzsales", pos, _buttonAchievement3.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement4.Draw(spriteBatch, _font);
                    r = _buttonAchievement4.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Lucky Devil").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Lucky Devil", pos, _buttonAchievement4.Hover ? Color.Red : Color.Gold);
                   
                    _buttonAchievement5.Draw(spriteBatch, _font);
                    r = _buttonAchievement5.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Tumbler").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Tumbler", pos, _buttonAchievement5.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement6.Draw(spriteBatch, _font);
                    r = _buttonAchievement6.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Lefty").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Lefty", pos, _buttonAchievement6.Hover ? Color.Red : Color.Gold);

                    ///////////////////// new line //////////////////////7

                    _buttonAchievement7.Draw(spriteBatch, _font);
                    r = _buttonAchievement7.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Pleasant").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Pleasant", pos, _buttonAchievement7.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement8.Draw(spriteBatch, _font);
                    r = _buttonAchievement8.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Icarus").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Icarus", pos, _buttonAchievement8.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement9.Draw(spriteBatch, _font);
                    r = _buttonAchievement9.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Cheater").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Cheater", pos, _buttonAchievement9.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement10.Draw(spriteBatch, _font);
                    r = _buttonAchievement10.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Super").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Super", pos, _buttonAchievement10.Hover ? Color.Red : Color.Gold);

                    _buttonAchievement11.Draw(spriteBatch, _font);
                    r = _buttonAchievement11.GetRect();
                    pos = new Vector2( r.X, r.Y + r.Height + 5 );
                    pos.X += r.Width / 2;
                    pos.X -= _tinyFont.MeasureString("Stalker").X / 2;
                    spriteBatch.DrawString(_tinyFont, "Stalker", pos, _buttonAchievement11.Hover ? Color.Red : Color.Gold);

                    break;
                    /*
                case State.Achievement0Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement0.Achievement);
                    break;

                case State.Achievement1Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement1.Achievement);
                    break;

                case State.Achievement2Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement2.Achievement);
                    break;

                case State.Achievement3Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement3.Achievement);
                    break;

                case State.Achievement4Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement4.Achievement);
                    break;

                case State.Achievement5Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement5.Achievement);
                    break;

                case State.Achievement6Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement6.Achievement);
                    break;

                case State.Achievement7Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement7.Achievement);
                    break;

                case State.Achievement8Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement8.Achievement);
                    break;

                case State.Achievement9Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement9.Achievement);
                    break;

                case State.Achievement10Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement10.Achievement);
                    break;

                case State.Achievement11Detail:
                    DrawAchievementDetail(spriteBatch, _buttonAchievement11.Achievement);
                    break;       */             
            }
     
            _buttonExit.Draw(spriteBatch, _font);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        private void DrawAchievementDetail(SpriteBatch spriteBatch, MMAchievement achievement)
        {
            Vector2 position;
            String s;

            position = new Vector2(80, 20);

#if XBOX
            position.Y += 20;
#endif

            DrawShadowedString(spriteBatch, _largeFont, "Award", position, Color.Gold);

            position.X = 80;
            position.Y = 170;

            achievement.Draw(spriteBatch, position, true);

            //Draw Achievement
            s = achievement.Name;
            position.X += 80;
            position.Y += achievement.Image.Height / 2;
            position.Y -= _mediumFont.MeasureString(s).Y/2;

            DrawShadowedString(spriteBatch, _mediumFont, s, position, Color.Gold);

            //Draw Description            
            position.Y += 80;
            s = achievement.Description;

            DrawShadowedString(spriteBatch, _tinyFont, s, position, Color.Gold);
            Unhover();
            _buttonExit.Hover = true;
        }

        public void Right()
        {
            if (_buttonExit.Hover)
            {
                Unhover();
                _buttonAchievement0.Hover = true;
            }
            else if (_buttonAchievement0.Hover)
            {
                Unhover();
                _buttonAchievement1.Hover = true;
            }
            else if (_buttonAchievement1.Hover)
            {
                Unhover();
                _buttonAchievement2.Hover = true;
            }
            else if (_buttonAchievement2.Hover)
            {
                Unhover();
                _buttonAchievement3.Hover = true;
            }
            else if (_buttonAchievement3.Hover)
            {
                Unhover();
                _buttonAchievement4.Hover = true;
            }
            else if (_buttonAchievement4.Hover)
            {
                Unhover();
                _buttonAchievement5.Hover = true;
            }
            else if (_buttonAchievement5.Hover)
            {
                Unhover();
                _buttonAchievement6.Hover = true;
            }
            else if (_buttonAchievement6.Hover)
            {
                Unhover();
                _buttonAchievement7.Hover = true;
            }
            else if (_buttonAchievement7.Hover)
            {
                Unhover();
                _buttonAchievement8.Hover = true;
            }
            else if (_buttonAchievement8.Hover)
            {
                Unhover();
                _buttonAchievement9.Hover = true;
            }
            else if (_buttonAchievement9.Hover)
            {
                Unhover();
                _buttonAchievement10.Hover = true;
            }
            else if (_buttonAchievement10.Hover)
            {
                Unhover();
                _buttonAchievement11.Hover = true;
            }
            else if (_buttonAchievement11.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
        }

        public void Left()
        {
            if (_buttonExit.Hover)
            {
                Unhover();
                _buttonAchievement11.Hover = true;
            }
            else if (_buttonAchievement11.Hover)
            {
                Unhover();
                _buttonAchievement10.Hover = true;
            }
            else if (_buttonAchievement10.Hover)
            {
                Unhover();
                _buttonAchievement9.Hover = true;
            }
            else if (_buttonAchievement9.Hover)
            {
                Unhover();
                _buttonAchievement8.Hover = true;
            }
            else if (_buttonAchievement8.Hover)
            {
                Unhover();
                _buttonAchievement7.Hover = true;
            }
            else if (_buttonAchievement7.Hover)
            {
                Unhover();
                _buttonAchievement6.Hover = true;
            }
            else if (_buttonAchievement6.Hover)
            {
                Unhover();
                _buttonAchievement5.Hover = true;
            }
            else if (_buttonAchievement5.Hover)
            {
                Unhover();
                _buttonAchievement4.Hover = true;
            }
            else if (_buttonAchievement4.Hover)
            {
                Unhover();
                _buttonAchievement3.Hover = true;
            }
            else if (_buttonAchievement3.Hover)
            {
                Unhover();
                _buttonAchievement2.Hover = true;
            }
            else if (_buttonAchievement2.Hover)
            {
                Unhover();
                _buttonAchievement1.Hover = true;
            }
            else if (_buttonAchievement1.Hover)
            {
                Unhover();
                _buttonAchievement0.Hover = true;
            }
            else if (_buttonAchievement0.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
        }

        public void Up()
        {
            if (_buttonAchievement0.Hover || _buttonAchievement1.Hover || _buttonAchievement2.Hover || _buttonAchievement3.Hover || _buttonAchievement4.Hover || _buttonAchievement5.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
            else if (_buttonExit.Hover)
            {
                Unhover();
                _buttonAchievement8.Hover = true;
            }
            else if (_buttonAchievement6.Hover)
            {
                Unhover();
                _buttonAchievement0.Hover = true;
            }
            else if (_buttonAchievement7.Hover)
            {
                Unhover();
                _buttonAchievement1.Hover = true;
            }
            else if (_buttonAchievement8.Hover)
            {
                Unhover();
                _buttonAchievement2.Hover = true;
            }
            else if (_buttonAchievement9.Hover)
            {
                Unhover();
                _buttonAchievement3.Hover = true;
            }
            else if (_buttonAchievement10.Hover)
            {
                Unhover();
                _buttonAchievement4.Hover = true;
            }
            else if (_buttonAchievement11.Hover)
            {
                Unhover();
                _buttonAchievement5.Hover = true;
            }
        }

        public void Down()
        {
            if (_buttonAchievement6.Hover || _buttonAchievement7.Hover || _buttonAchievement8.Hover || _buttonAchievement9.Hover || _buttonAchievement10.Hover || _buttonAchievement11.Hover)
            {
                Unhover();
                _buttonExit.Hover = true;
            }
            else if (_buttonExit.Hover)
            {
                Unhover();
                _buttonAchievement2.Hover = true;
            }
            else if (_buttonAchievement0.Hover)
            {
                Unhover();
                _buttonAchievement6.Hover = true;
            }
            else if (_buttonAchievement1.Hover)
            {
                Unhover();
                _buttonAchievement7.Hover = true;
            }
            else if (_buttonAchievement2.Hover)
            {
                Unhover();
                _buttonAchievement8.Hover = true;
            }
            else if (_buttonAchievement3.Hover)
            {
                Unhover();
                _buttonAchievement9.Hover = true;
            }
            else if (_buttonAchievement4.Hover)
            {
                Unhover();
                _buttonAchievement10.Hover = true;
            }
            else if (_buttonAchievement5.Hover)
            {
                Unhover();
                _buttonAchievement11.Hover = true;
            }
        }

        public void Unhover()
        {
            _buttonAchievement0.Hover = false;
            _buttonAchievement1.Hover = false; 
            _buttonAchievement2.Hover = false;
            _buttonAchievement3.Hover = false; 
            _buttonAchievement4.Hover = false; 
            _buttonAchievement5.Hover = false; 
            _buttonAchievement6.Hover = false; 
            _buttonAchievement7.Hover = false; 
            _buttonAchievement8.Hover = false; 
            _buttonAchievement9.Hover = false; 
            _buttonAchievement10.Hover = false;
            _buttonAchievement11.Hover = false;
            _buttonExit.Hover = false;
        }
    }
}
