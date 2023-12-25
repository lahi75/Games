using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TrainTrouble.Properties;

namespace TGGameLibrary
{
    class TGLevelSelection
    {
        TGButton _buttonExit;

        TGLevelButton _level0;
        TGLevelButton _level1;
        TGLevelButton _level2;
        TGLevelButton _level3;
        TGLevelButton _level4;
        
        Rectangle _screenRect;
        Texture2D _background;
        SpriteFont _font;

        SpriteFont _tinyFont;
        SpriteFont _largeFont;
        SpriteFont _mediumFont;
        SpriteFont _smallFont;        

        public enum Result
        {
            Level_0,
            Level_1,
            Level_2,
            Level_3,
            Level_4,
            noresult,
            back
        }

        public TGLevelSelection(IServiceProvider serviceProvider, Rectangle screenRect)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");

            _screenRect = screenRect;

            _level0 = new TGLevelButton(content, LevelManager.Levels.GetLevel(0));
            _level1 = new TGLevelButton(content, LevelManager.Levels.GetLevel(1));
            _level2 = new TGLevelButton(content, LevelManager.Levels.GetLevel(2));
            _level3 = new TGLevelButton(content, LevelManager.Levels.GetLevel(3));
            _level4 = new TGLevelButton(content, LevelManager.Levels.GetLevel(4));

            _level0.CenterPosition(new Vector2(80, 220));
            _level1.CenterPosition(new Vector2(240, 220));
            _level2.CenterPosition(new Vector2(400, 220));
            _level3.CenterPosition(new Vector2(560, 220));
            _level4.CenterPosition(new Vector2(720, 220));
            
            _font = content.Load<SpriteFont>("fonts/ButtonFont");
            _largeFont = content.Load<SpriteFont>("fonts/largeFont");
            _smallFont = content.Load<SpriteFont>("fonts/smallFont");
            _tinyFont = content.Load<SpriteFont>("fonts/tinyFont");
            _mediumFont = content.Load<SpriteFont>("fonts/mediumFont");
            
            _background = content.Load<Texture2D>("background/small");

            _buttonExit = new TGButton(content, Resources.back, false, false);
            int y = 430;
            _buttonExit.CenterPosition(new Vector2(screenRect.Width / 2, y));
            _buttonExit.Hover = true;

            //_currentState = State.Overview;
        }

        public Result Update(GameTime game, Point mousePosition, bool mouseDown)
        {
            if (_level0.Update(mousePosition, mouseDown))
                return Result.Level_0;

            if (_level1.Update(mousePosition, mouseDown))
                return Result.Level_1;

            if (_level2.Update(mousePosition, mouseDown))
                return Result.Level_2;

            if (_level3.Update(mousePosition, mouseDown))
                return Result.Level_3;

            if (_level4.Update(mousePosition, mouseDown))
                return Result.Level_4;

            if (_buttonExit.Update(mousePosition, mouseDown))            
                return Result.back;            

            return Result.noresult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, _screenRect,  Color.White);

            Vector2 position = new Vector2(50, 20);

            // draw page caption
            DrawShadowedString(spriteBatch, _largeFont, Resources.levelSelection, position, new Color(25,87,168,255));


            _level0.Draw(spriteBatch, _font);

            String s = LevelManager.Levels.Level0.LevelName;
            int sOffset = (int)_tinyFont.MeasureString(s).X / 2;
            Vector2 p = new Vector2(_level0.GetRect().Center.X - sOffset, _level0.GetRect().Top - _tinyFont.MeasureString(s).Y - 20);
            DrawShadowedString(spriteBatch, _tinyFont, s, p, Color.Crimson);

            p = new Vector2(_level0.GetRect().X, _level0.GetRect().Bottom + 20);
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription1TC, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription2TC, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level0.Stops + " " + Resources.levelDescription3stop, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level0.Boosts + " " + Resources.levelDescription3boost, p, new Color(25,87,168,255));
            
            _level1.Draw(spriteBatch, _font);

            s = LevelManager.Levels.Level1.LevelName;
            sOffset = (int)_tinyFont.MeasureString(s).X / 2;
            p = new Vector2(_level1.GetRect().Center.X - sOffset, _level1.GetRect().Top - _tinyFont.MeasureString(s).Y - 20);
            DrawShadowedString(spriteBatch, _tinyFont, s, p, Color.Crimson);

            p = new Vector2(_level1.GetRect().X, _level1.GetRect().Bottom + 20);
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription1DR, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription2DR, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level1.Stops + " " + Resources.levelDescription3stop, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level1.Boosts + " " + Resources.levelDescription3boost, p, new Color(25,87,168,255));

            _level2.Draw(spriteBatch, _font);

            s = LevelManager.Levels.Level2.LevelName;
            sOffset = (int)_tinyFont.MeasureString(s).X / 2;
            p = new Vector2(_level2.GetRect().Center.X - sOffset, _level2.GetRect().Top - _tinyFont.MeasureString(s).Y - 20);
            DrawShadowedString(spriteBatch, _tinyFont, s, p, Color.Crimson);

            p = new Vector2(_level2.GetRect().X, _level2.GetRect().Bottom + 20);
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription1WW, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription2WW, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level2.Stops + " " + Resources.levelDescription3stop, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level2.Boosts + " " + Resources.levelDescription3boost, p, new Color(25,87,168,255));
                                    
            _level3.Draw(spriteBatch, _font);

            s = LevelManager.Levels.Level3.LevelName;
            sOffset = (int)_tinyFont.MeasureString(s).X / 2;
            p = new Vector2(_level3.GetRect().Center.X - sOffset, _level3.GetRect().Top - _tinyFont.MeasureString(s).Y - 20);
            DrawShadowedString(spriteBatch, _tinyFont, s, p, Color.Crimson);

            p = new Vector2(_level3.GetRect().X, _level3.GetRect().Bottom + 20);
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription1VA, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription2VA, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level3.Stops + " " + Resources.levelDescription3stop, p,  new Color(25,87,168,255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level3.Boosts + " " + Resources.levelDescription3boost, p, new Color(25,87,168,255));


            _level4.Draw(spriteBatch, _font);

            s = LevelManager.Levels.Level4.LevelName;
            sOffset = (int)_tinyFont.MeasureString(s).X / 2;
            p = new Vector2(_level4.GetRect().Center.X - sOffset, _level4.GetRect().Top - _tinyFont.MeasureString(s).Y - 20);
            DrawShadowedString(spriteBatch, _tinyFont, s, p, Color.Crimson);

            p = new Vector2(_level4.GetRect().X, _level4.GetRect().Bottom + 20);
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription1IS, p, new Color(25, 87, 168, 255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, Resources.levelDescription2IS, p, new Color(25, 87, 168, 255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level4.Stops + " " + Resources.levelDescription3stop, p, new Color(25, 87, 168, 255));
            p.Y += 20;
            spriteBatch.DrawString(_tinyFont, "- " + LevelManager.Levels.Level4.Boosts + " " + Resources.levelDescription3boost, p, new Color(25, 87, 168, 255));

            _buttonExit.Draw(spriteBatch, _font);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
