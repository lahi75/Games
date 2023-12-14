using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MonkeyMadness
{
          
    class MMGameLogic
    {
        public enum GameResult
        {
            exit,
            noresult
        }

        public bool ShowMouseCursor
        {
            get { return _jack1.State == JJJack.States.won || _jack1.State == JJJack.States.dead; }
        }
        public bool DrawAd()
        {
            //if (_jack1.State == JJJack.States.won || _jack1.State == JJJack.States.dead)
                return true;

            //return false;
        }

        TimeSpan _levelStartTime;
        TimeSpan _levelTotalTime;
        Texture2D _numberLives;
        bool _hint = false;

        bool _howToPlay = false;
        
        SpriteFont _font;
        Texture2D _background;
        Texture2D _howToImage;        

        JJMonsters _monster;
        JJHoles _holes;
        JJJack _jack1;
        JJLines _lines;

        Game _gameMain;

        MMBonuses _bonuses;        

        // difficulty options
        Int32 _maxLines = 6;
        Int32 _holeWidth = 55;
        Int32 _gameSpeed = 5;
        Int32 _maxHoles = 12;
        Int32 _bonusChance = 5;
        Int32 _fallStunTime = 2500;
        Int32 _monsterStunTime = 3300;
        MMSettings.DifficultyType _difficulty;
        
        Boolean _trial = false;
        Int32 _level = 1;

        Int32 _cumulatedPoints = 0;

        JJPoints _points = new JJPoints();

        Rectangle _titleSafe;
        Rectangle _screenRect;
       // JJLevelPage _levelPage;
       // JJOverPage _gameOverPage;

        public void Activate(IDictionary<string, object> dict)
        {
            // TODO:
            // Preserve states of Monsters, Items, Jack, Points, Gamestate
        }

        public void Deactivate(IDictionary<string, object> dict)
        {
            // TODO:
            // Restore states of Monsters, Items, Jack, Points, Gamestate
        }        

        public MMGameLogic(Game gameMain, Rectangle screenRect, Rectangle titleSafe)
        {
            _gameMain = gameMain;

            _screenRect = screenRect;
            _titleSafe = titleSafe;            
            
            _font = gameMain.Content.Load<SpriteFont>("fonts/InGameFont");

            _howToImage = gameMain.Content.Load<Texture2D> ("misc/howto");            

            _lines = new JJLines(gameMain);
            _holes = new JJHoles(gameMain);
            _monster = new JJMonsters(gameMain);
            _jack1 = new JJJack(gameMain);
            _bonuses = new MMBonuses(gameMain);

            _numberLives = gameMain.Content.Load<Texture2D>("bonus/bonus_life");
            _background = gameMain.Content.Load<Texture2D>("background/form_aux");            

         //   _levelPage = new JJLevelPage(gameMain, screenRect, _titleSafe);
         //   _gameOverPage = new JJOverPage(gameMain, screenRect, titleSafe);            
        }

        public void Init(MMSettings.DifficultyType difficulty, Boolean trial)
        {            
            _trial = trial;            

#if XBOX
            _maxLines = 6;
#endif

            switch (difficulty)
            {
                default:
                case MMSettings.DifficultyType.easy:
                    _gameSpeed = 8;
                    _maxHoles = 10;
                    _bonusChance = 15;
                    _fallStunTime = 1300;
                    _monsterStunTime = 1600;
                    break;
                case MMSettings.DifficultyType.medium:
                    _gameSpeed = 7;
                    _maxHoles = 11;
                    _bonusChance = 13;
                    _fallStunTime = 1700;
                    _monsterStunTime = 2000;
                    break;
                case MMSettings.DifficultyType.hard:
                    _gameSpeed = 6;
                    _maxHoles = 13;
                    _bonusChance = 11;
                    _fallStunTime = 1900;
                    _monsterStunTime = 2200;
                    break;
            }

            _difficulty = difficulty;

            _lines.Init(_screenRect, _titleSafe,  _maxLines);
            _holes.Init(_screenRect, _titleSafe, _gameSpeed, _maxLines, _holeWidth, _lines.LinesOffset, _lines.LinesDelta, _background);
            _monster.Init(_screenRect, _titleSafe, _gameSpeed, _maxLines, _holeWidth, _lines.LinesOffset, _lines.LinesDelta);
            _jack1.Init(_screenRect, _titleSafe,  _maxLines, _gameSpeed, _lines.LinesOffset, _lines.LinesDelta, _fallStunTime, _monsterStunTime);
            
            _bonuses.Init(_screenRect,_gameSpeed, _bonusChance, _maxLines, _lines.LinesOffset, _lines.LinesDelta);

            _levelStartTime = TimeSpan.Zero;
            _cumulatedPoints = 0;
            _level = 1;
            _points.Init(_level);
            
            //_levelPage.Reset(_level);

            MonitorManager.Monitor.ResetGame();

            UpdateBackground();
       
            MMFxManager.Fx.PlayMonkeyScream();
        }
     
        public GameResult Update(GameTime gameTime,Point mousePosition, bool mouseDown)
        {
            if (_howToPlay)
                return GameResult.noresult;

            if (_levelStartTime == TimeSpan.Zero)
                _levelStartTime = gameTime.TotalGameTime;

            _levelTotalTime = gameTime.TotalGameTime.Subtract(_levelStartTime);

            UpdateVolume();

            // move holes and monster
            _holes.Move();
            _monster.Move(gameTime);

            // check if our hero can fall into a hole
            if (_jack1.CanFall())
            {
                bool leftHole = false;
                // check if jack falls into a hole
                if (_holes.TestHole(_jack1.JacksLine + 1, _jack1.JacksPosition, true, out leftHole))
                {
                    _jack1.SetFalling(true);
                    _points.Fall();
                }
            }

            // check if our hero can be hit by a monster
            if (_jack1.CanStun())
            {
                // check if the monster stunns jack
                if (_monster.TestMonster(_jack1.JacksLine + 1, _jack1.JacksPosition))
                {
                    _jack1.SetStunned(gameTime);
                    _points.MonsterCrash();                    
                }
            }
            
            // now move jack
            _jack1.Update(gameTime);

            // update the bonuses ( create/remove them)
            _bonuses.Update(gameTime,_level);

            // check if our hero catches a bonus
            if( _bonuses.TestBonuses(_jack1.JacksLine + 1, _jack1.JacksPosition))
            {
                MMFxManager.Fx.PlayBonusCollect();

                _points.BonusCatched();

                MonitorManager.Monitor.BonusCollect(_bonuses.LastBonus);

                if (_bonuses.LastBonus == MMBonus.BonusType.Life)
                {
                    _jack1.Lives++;
                }
                else if (_bonuses.LastBonus == MMBonus.BonusType.Poison)
                {
                    _monster.Remove();

                    MonitorManager.Monitor.MonsterRemoved();
                }
                else if(_bonuses.LastBonus == MMBonus.BonusType.Teleport)
                {
                    Random r = new Random((int)DateTime.Now.Ticks);
                    // Teleport avatar

                    int i = 5;
                    bool leftHole = false;
                    do
                    {
                        i--;
                        _jack1.Teleport(r);
                    } while (_holes.TestHole(_jack1.JacksLine + 1, _jack1.JacksPosition, true, out leftHole) && i > 0);

                    MMFxManager.Fx.PlayTeleport();
                }
                else
                {
                    _jack1.Bonus.AddBonus(_bonuses.LastBonus);
                }
            }

            // play noises according to jacks state
            if (_jack1.State == JJJack.States.stunned)
            {
                MMFxManager.Fx.PlayBirdScream(true);                
            }
            else
            {
                MMFxManager.Fx.PlayBirdScream(false);
            }

            // exit the game if jack lost all his lifes
            if (_jack1.State == JJJack.States.dead)
            {
                MonitorManager.Monitor.Score(_cumulatedPoints + _points.Points);

              //  if (_gameOverPage.Update(gameTime, mousePosition, mouseDown, _points, _cumulatedPoints) == JJOverPage.Result.exit)
                {
                //    _cumulatedPoints += _points.Points;                    
                  //  return GameResult.exit;
                }
            }

            if (_jack1.State == JJJack.States.won)
            {
                MonitorManager.Monitor.LevelTime((int)_levelTotalTime.TotalSeconds);
                MonitorManager.Monitor.CheckPleasant();
                MonitorManager.Monitor.CheckLefty();                
                MonitorManager.Monitor.SuperMonkey(_difficulty, _level);

               // if (_levelPage.Update(gameTime, mousePosition, mouseDown, _points, _cumulatedPoints, _levelTotalTime) == JJLevelPage.Result.exit)
                {
               //     if (NextLevel() == false)
                 //   {
                   //     _cumulatedPoints += _points.Points;                        
                   //     return GameResult.exit;
                   // }
                }
            }

            bool unused = false; // checks if the hole moves from left to right
            // create hint for jack to jump in the first level            

            if (_level == 1 && _difficulty == MMSettings.DifficultyType.easy && _jack1.CanJump())
                _hint = _holes.TestHole(_jack1.JacksLine, _jack1.JacksPosition, false, out unused);
            else
                _hint = false;

            //if( _jack1.State != JJJack.States.dead && _jack1.State != JJJack.States.won )
            //    _points.Update(gameTime); // decrease the points with each second

            AchievementsManager.Achievements.Update(gameTime);

            return GameResult.noresult;
        }
        
        void UpdateVolume()
        {
            if (MusicPlayer.IsExternalPlay)
                return;

            if (_jack1.State == JJJack.States.won || _jack1.State == JJJack.States.dead)
            {
                if (MediaPlayer.Volume != 0.3f)
                    MediaPlayer.Volume = 0.3f;
            }
            else
            {
                if (MediaPlayer.Volume != 0.8f)
                    MediaPlayer.Volume = 0.8f;
            }
        }        

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_background, _screenRect, Color.White);

            if (_howToPlay)
            {                
                Vector2 position = new Vector2( _titleSafe.X + 20, _titleSafe.Y + 20);

                DrawShadowedString(spriteBatch, _font, "void", new Vector2(_titleSafe.X + 10, 15), Color.Red);

                position.X = _screenRect.Width / 2 - _howToImage.Width / 2;

                position.Y += 50;

                spriteBatch.Draw(_howToImage, position, Color.White);
                return;
            }           

            if (_jack1.State == JJJack.States.won)
            {
                bool showTrial = false;

                if (_level == 2 && _trial)
                    showTrial = true;

             //   _levelPage.Draw(spriteBatch, showTrial);
            }
            else if (_jack1.State == JJJack.States.dead)
            {                
               // _gameOverPage.Draw(spriteBatch);
            }
            else
            {
                String s = _levelTotalTime.Minutes.ToString("00") + ":" + _levelTotalTime.Seconds.ToString("00");

                int y = 15;

#if XBOX
                y += 50;
#endif

                DrawShadowedString(spriteBatch, _font, s, new Vector2(_titleSafe.X + 10, y), Color.Red);


                // draw points
                Vector2 position = new Vector2(_titleSafe.Width + _titleSafe.X - _font.MeasureString("Points" + ": ?????").X - 15, _titleSafe.Height + _titleSafe.Y - _font.MeasureString("Points").Y - 5);

#if XBOX
                position.Y -= 50;
#endif
#if WINDOWS_PHONE
                position.X = 150;
                position.Y = 15;
#endif

                DrawShadowedString(spriteBatch, _font, "Points" + ": " + (_cumulatedPoints + _points.Points), position, Color.Red);                                



                _lines.Draw(spriteBatch);
              
                // draw the number of lifes
                Vector2 pos = new Vector2(_titleSafe.X + 10, _titleSafe.Height + _titleSafe.Y - _numberLives.Height - 5);     
           
#if XBOX
                pos.Y -= 40;
#endif

                for (int i = 0; i < _jack1.Lives; i++)
                {                    
                    spriteBatch.Draw(_numberLives, pos, Color.White);
                    pos.X += _numberLives.Width;
                }               

                _holes.Draw(spriteBatch);

                _bonuses.Draw(spriteBatch);

                _jack1.Draw(spriteBatch, _hint);

                _monster.Draw(spriteBatch);             
            }            

            // draw the achievement, if there are any
            AchievementsManager.Achievements.Draw(spriteBatch, new Vector2(_screenRect.Width / 2 - AchievementsManager.Achievements.AchievementDrillerKiller.Image.Width / 2, 419));
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        public void Left()
        {
            _jack1.Left();
        }

        public void Right()
        {
            _jack1.Right();
        }

        public void Up()
        {
            if (_howToPlay)
            {
                _howToPlay = false;
                UpdateBackground();
                return;
            }

            if (!_jack1.CanJump())
                return;

            bool leftHole = false; // checks if the hole moves from left to right

            // check if jack doesn't hit a line
            if (_holes.TestHole(_jack1.JacksLine, _jack1.JacksPosition, false, out leftHole) == false )
            {
                if (!_jack1.Bonus.CanDrill) // if he can't drill he crashes with the head
                {
                    _jack1.SetHeadCrash();
                    _points.HeadCrash();

                    MMFxManager.Fx.PlayCrashScream();

                    return;
                }
                else
                {
                    _jack1.Bonus.ResetDrill(); // use the drill joker to jump
                    
                    if (_jack1.JacksLine == 1)
                    {
                        MonitorManager.Monitor.UpperLineDrilled();
                    }
                }
            }             
            
            // and jump now
            if (_jack1.Up())
            {
                // create new hole after successful jump
                if (_holes.Count < _maxHoles)
                {
                    Random r = new Random((int)DateTime.Now.Ticks);
                    _holes.AddHole(r,false);
                }

                // if the hole is not a left one, reset the lefty achievement
                if (leftHole == false)
                    MonitorManager.Monitor.JumpRight();

                _points.Up();
             
                MMFxManager.Fx.PlayMonkeyScream();
            }            
        }

        public void Start()
        {
#if XBOX
            if (_difficulty == JJSettings.DifficultyType.easy && _level == 1)
                _howToPlay = true;
            else
            {
                _howToPlay = false;                
            }
#endif

            UpdateBackground();

            if (_jack1.State == JJJack.States.won)
            {
              //  if (_levelPage.Counting())
                //    _levelPage.Finish();

                NextLevel();
                AchievementsManager.SaveSettings();
            }
            else if (_jack1.State == JJJack.States.dead)
            {
                AchievementsManager.SaveSettings();                
            }
        }

        public int CumulatedPoints
        {
            get
            {
                return _cumulatedPoints;
            }
        }

        private Boolean NextLevel()
        {
            _level++;                       

            if (_trial && _level == 3) // no access to level 3 in trial mode
                return false;

            MonitorManager.Monitor.ResetLevel();

            _jack1.Reset();
            _holes.Reset();
            _bonuses.Clear();
            //_levelPage.Reset(_level);
            
            _cumulatedPoints += _points.Points;

            _points.Init(_level);

            // every second level comes a monster
            //if (_level % 2 == 0)
            {
                Random r = new Random((int)DateTime.Now.Ticks);
                _monster.AddMonster(r);
            }

            UpdateBackground();

            _levelStartTime = TimeSpan.Zero;

            return true;
        }

        private void UpdateBackground()
        {
            if (_howToPlay)
            {
                _background = _gameMain.Content.Load<Texture2D>("background/form_aux");
                return;
            }


            int postfix = _level;

            if (postfix > 7)
                postfix = _level % 7 + 1;

            String s = "background/level_" + postfix;

            _background = _gameMain.Content.Load<Texture2D>(s);

            _holes.SetBackground(_background);
        }

        public Int32 CurrentLevel
        {
            get
            {
                return _level;
            }
        }
    }
}
