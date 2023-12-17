using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MonkeyMadness
{
    /// <summary>
    /// class for the game avatar
    /// </summary>
    class MMJack
    {        
        /// <summary>
        /// constructor
        /// </summary>        
        public MMJack(Game gameMain)
        {
            _gameMain = gameMain;

            // create the animation
            _run = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f,true);                        
            _run.Load(gameMain.Content, "monkey/run", 16, 16);

            _jump = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f,false);
            _jump.Load(gameMain.Content, "monkey/jump", 11, 16);

            _fall = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _fall.Load(gameMain.Content, "monkey/fall", 16, 24);

            _idle = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _idle.Load(gameMain.Content, "monkey/idle", 16, 16);

            _crash = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            _crash.Load(gameMain.Content, "monkey/crash", 6, 16);

            _stunned = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _stunned.Load(gameMain.Content, "monkey/stunned", 16, 16);

            _hint = gameMain.Content.Load<Texture2D>("misc/hole");

            _bonus = new JJJackBonus(gameMain);                                   
        }        

        public void Init(Rectangle screenRect, Rectangle titleSafe, Int32 maxLine, Int32 gameSpeed, Int32 offset, float delta, int fallStunTime, int monsterStunTime)
        {
            _titleSafe = titleSafe;
            _screenRect = screenRect;

            _increment = ((_titleSafe.Width / gameSpeed) * (float)_gameMain.TargetElapsedTime.Milliseconds / 1000);

            _jackHeight = (int)_idle.Size.Y;
            _jackWidth = (int)_idle.Size.X;

            _maxLines = _currentLine = maxLine;

            //_position = new Vector2(0, maxLine * delta + _jackHeight);
            

#if XBOX
           // _position.Y -= 8;
#endif

            _lives = 4;

            _delta = delta;
            _offset = offset;

            _fallStunnedTime = fallStunTime;
            _monsterStunnedTime = monsterStunTime;

            Reset();
        }

        public void Reset()
        {            
            //_position = new Vector2(_screenRect.Width/ 2, _screenRect.Height - _jackHeight);
            _position = new Vector2(_screenRect.Width / 2, _maxLines * _delta - _offset - _jackHeight + _delta);//_maxLines * _delta - _offset + _jackHeight + 5);


            


#if XBOX
           // _position.Y -= 8;
#endif

            _currentLine = _maxLines;
            _currentState = States.idle;
            _newState = States.idle;

            _bonus.Reset();
        }

        /// <summary>
        /// draw the avatar at the current location
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, bool hint)
        {
            if (hint)
                spriteBatch.Draw(_hint, new Rectangle((int)_position.X, (int)_position.Y + _jackHeight, (int)_jackWidth, (int)_jackHeight / 8), Color.Red);

            switch (_currentState)
            {
                case States.idle:
                    _idle.DrawFrame(spriteBatch, _position, _direction);
                    break;
                case States.right:
                    _run.DrawFrame(spriteBatch, _position, SpriteEffects.None);                    
                    break;
                case States.left:
                    _run.DrawFrame(spriteBatch, _position, SpriteEffects.FlipHorizontally);                                        
                    break;
                case States.jumping:                    
                    _jump.DrawFrame(spriteBatch, _position, _direction);
                    break;
                case States.falling:
                    _fall.DrawFrame(spriteBatch, _position, _direction);
                    break;
                case States.stunned:
                    _stunned.DrawFrame(spriteBatch, _position, _direction);                    
                    break;            
                case States.headcrash:                    
                    _crash.DrawFrame(spriteBatch, _position, _direction);
                    break;
            }

            _bonus.Draw(spriteBatch, _position);

            

            //_bonus.Debug();
            
        }

        public Int32 Lives
        {
            get
            {
                return _lives;
            }
            set
            {
                _lives = value;
            }
        }

        public Int32 JacksLine
        {
            get
            {
                return _currentLine;
            }
        }

        public void Teleport(Random r)
        {            
            _currentLine = r.Next(2, _maxLines);
         
            _position.X = r.Next(_titleSafe.X, _titleSafe.X + _titleSafe.Width);


            float linePos = _currentLine * _delta - _offset;                        
            // calculate the y position of the avatar            
            _position.Y = linePos - _jackHeight + _delta;
        }

        public Int32 JacksPosition
        {
            get
            {
                return (int)(_position.X + _jackWidth / (float)2);
            }
        }

        /// <summary>
        /// jump up with the avatar
        /// </summary>
        public Boolean Up()
        {            
            if (_currentState == States.idle || _currentState == States.left || _currentState == States.right)
            {                
                _newState = States.jumping;
                _jump.Stop();
                _jump.Play();

                return true;
            }
            return false;
        }

        /// <summary>
        /// run left
        /// </summary>
        public void Left()
        {
            if (_currentState == States.stunned)
                return;

            if (_currentState == States.right || _currentState == States.idle || _currentState == States.left)
                _newState = States.left;

            _direction = SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// move right with the avatar
        /// </summary>
        public void Right()
        {
            if (_currentState == States.stunned)
                return;

            if (_currentState == States.left || _currentState == States.idle || _currentState == States.right)
                _newState = States.right;

            _direction = SpriteEffects.None;
        }

        public Boolean CanJump()
        {
            if (_currentState == States.idle || _currentState == States.left || _currentState == States.right)
            {
                return true;
            }
            return false;
        }

        public Boolean CanFall()
        {
            // with wings we can't fall
            if (_bonus.CanWings)
                return false;

            // we just started jumping, don't fall
            if (_newState == States.jumping)
                return false;

            if (_currentState == States.idle || _currentState == States.left || _currentState == States.right || _currentState == States.stunned )
                return true;
            return false;
        }

        public Boolean CanStun()
        {
            // we can't get stunned by monsters with amor
            if (_bonus.CanAmor)
                return false;

            // can't get stunned if we start to fall
            if (_newState == States.falling)
                return false;

            if (_currentState == States.idle || _currentState == States.left || _currentState == States.right || _currentState == States.stunned)
                return true;
            return false;
        }

        public void SetFalling(Boolean fallLine)
        {
            if (fallLine)
            {
                _currentLine += 1;

                // we are falling, update the achievement
                MonitorManager.Monitor.LuckyDevilFall();

                // prepare the icarus achievement, but only if we don't have a parachute
                if (!_bonus.CanParachute)
                    MonitorManager.Monitor.FallLine();

                if (_currentLine == _maxLines)
                {
                    _lives--;

                    MMFxManager.Fx.PlayLostLive();

                    MonitorManager.Monitor.ResetMonsterCrash();
                }
            }
           
            _newState = States.falling;

            MonitorManager.Monitor.UnPleasant();

            _fall.Stop();
            _fall.Play();
        }

        public void SetHeadCrash()
        {
            _bonus.ResetParachute();
            _newState = States.headcrash;

            _crash.Stop();
            _crash.Play();
        }

        public void SetStunned(GameTime gameTime)
        {
            if (_currentState != States.stunned)
                MonitorManager.Monitor.MonsterCrash();

            MonitorManager.Monitor.UnPleasant();

            MonitorManager.Monitor.StalkerCrash();

            _newState = States.stunned;
            _stunnedBegin = gameTime.TotalGameTime;
            _stunnedTime = _monsterStunnedTime;
        }

        public States State
        {
            get
            {
                return _currentState;
            }
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _bonus.Update(gameTime);

            _currentState = _newState;
            
            switch (_newState)
            {
                case States.idle:                    
                    _idle.UpdateFrame(elapsed);
                    break;

                case States.jumping:
                    {                                                   
                        _position.Y -= _increment;
                     
                        float linePos = _currentLine * _delta-_offset;                        


                        if (_position.Y + _jackHeight < linePos)
                        {
                            _position.Y = linePos - _jackHeight;
                            _currentLine -= 1;

                            if (_currentLine == 0)
                            {
                                _newState = States.won;
                                MMFxManager.Fx.PlayLevelSuccess();
                            }
                            else
                                _newState = States.idle;                            
                        }

                        _jump.UpdateFrame(elapsed);
                    }
                    
                    break;
                case States.headcrash:
                    {
                        _position.Y -= _increment /2;
                        
                        float linePos = _currentLine * _delta-_offset;

                        if (_position.Y + 5 < linePos)
                            SetFalling(false);

                        _crash.UpdateFrame(elapsed);
                    }
                    break;
                case States.falling:
                    {                        
                        _position.Y += _increment*2/3;
                        
                        float linePos = _currentLine * _delta-_offset;

                        if (_position.Y + _jackHeight - _delta - 5> linePos)
                        {
                            _position.Y = linePos - _jackHeight + _delta;

#if XBOX
                      //  if( _currentLine == _maxLines )
                      //      _position.Y -= 8;
#endif



                            if (_bonus.CanParachute)
                                _newState = States.idle;
                            else
                            {
                                _newState = States.stunned;
                                _stunnedBegin = gameTime.TotalGameTime;
                                _stunnedTime = _fallStunnedTime;
                            }
                        }

                        _fall.UpdateFrame(elapsed);
                    }
                    break;
                case States.left:                    

                    _position.X -= _increment;

                    // overflow at the left side, re-enter right
                    if (_position.X < _titleSafe.X - _jackWidth / 2)
                        _position.X = _titleSafe.X + _titleSafe.Width - _jackWidth / 2;
                    
                    _newState = States.idle;

                    
                    _run.UpdateFrame(elapsed);

                    break;
                case States.right:                    

                    _position.X += _increment;

                    // overflow at the right side, re-enter left
                    if (_position.X + _jackWidth / 2 > _titleSafe.X + _titleSafe.Width)
                        _position.X = _titleSafe.X - _jackWidth / 2;
                     
                    _newState = States.idle;
                    
                    _run.UpdateFrame(elapsed);

                    break;
                case States.stunned:
                    if (gameTime.TotalGameTime.Subtract(_stunnedBegin).TotalMilliseconds > _stunnedTime)
                    {
                        if (_lives < 1)
                            _newState = States.dead;
                        else
                        {
                            _newState = States.idle;

                            // recover, check if we did this on the last line
                            MonitorManager.Monitor.LuckyDevilRecover(_maxLines, _currentLine);
                            MonitorManager.Monitor.IcarusRecover();
                            MonitorManager.Monitor.StalkerCrashRecover();
                        }
                    }

                    _stunned.UpdateFrame(elapsed);
                    break;
                default:                
                    break;                
            }            
        }

        public enum States
        {
            idle,
            jumping,
            falling,
            left,
            right,
            stunned,
            dead,
            won,
            headcrash
        }

        public JJJackBonus Bonus
        {
            get
            {
                return _bonus;
            }
        }
       

        float _increment;
        Vector2 _position;
        Rectangle _screenRect;
        Rectangle _titleSafe;

        MMAnimatedTexture _run;
        MMAnimatedTexture _jump;
        MMAnimatedTexture _fall;
        MMAnimatedTexture _idle;
        MMAnimatedTexture _crash;
        MMAnimatedTexture _stunned;

        Texture2D _hint;

        States _currentState = States.idle;
        States _newState = States.idle;
        Int32 _currentLine;
        Int32 _maxLines;
        Game _gameMain;
        Int32 _jackHeight;
        Int32 _jackWidth;

        Int32 _stunnedTime = 3000;

        Int32 _fallStunnedTime = 2500;
        Int32 _monsterStunnedTime = 3300;

        float _delta;               // distance between two lines
        Int32 _offset;              // distance of the first line from the top of the screen

        TimeSpan _stunnedBegin;
        Int32 _lives;       

        JJJackBonus _bonus;
        SpriteEffects _direction = SpriteEffects.None;
    }
}
