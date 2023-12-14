using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyMadness
{
    class JJMonster
    {        
        /// <summary>
        /// constructor
        /// </summary>
        public JJMonster(Game gameMain, int number, bool ghost, bool vulture)
        {
            // create the animation
            _monster = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, true);
            _ghostShadow = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            _ghostShadow1 = new MMAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);

            // the monster animation consists of 10 fields
          //  string s = "monster/monster" + number;
         //   _monster.Load(gameMain.Content, s, 10, 10);


            if (ghost)
            {
                _monster.Load(gameMain.Content, "monster/ghost", 8, 10);
                _ghostShadow.Load(gameMain.Content,"monster/ghost_fade", 9,10);
                _ghostShadow.Pause();
                _ghostShadow1.Load(gameMain.Content, "monster/ghost_fade_in", 9, 10);
                _ghostShadow1.Pause();
            }
            else if (vulture)
            {
                _monster.Load(gameMain.Content, "monster/geier", 9, 10);
            }
            else
                _monster.Load(gameMain.Content, "monster/snake", 10, 10);

            _ghost = ghost;
            _vulture = vulture;

            _number = number;

            _targetTime = (float)gameMain.TargetElapsedTime.Milliseconds / 1000;           
        }

        /// <summary>
        /// initializes the monster with all game parameters
        /// </summary>        
        public void Init(Random r, Rectangle screenRect, Rectangle titleSafe, Int32 gameSpeed, Int32 maxLines, Int32 offset, float delta)
        {
            _width = (int)_monster.Size.X;
            _screenRect = screenRect;
            _titleSafe = titleSafe;

            // increments depends on the game target speed step
            _increment = ((_titleSafe.Width / gameSpeed) * _targetTime);

            _maxLines = maxLines;
            _delta = delta;
            _offset = offset;

            // create random initial values
            ShuffleStart(r);
        }

        /// <summary>
        /// draw the texture on the sprite at the current position
        /// </summary>        
        public void Draw(SpriteBatch spriteBatch)
        {           
            // draw the monster on the current line
            Vector2 p = _position;
            p.Y -= _monster.Size.Y;

            SpriteEffects effect = _increment < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;            

            if( !_fading )
                _monster.DrawFrame(spriteBatch, p, effect);

            if (_ghost)
            {
                if (!_ghostShadow.Is_paused)
                {
                    p = _shadowPosition;
                    p.Y -= _ghostShadow.Size.Y;

                    effect = _shadowIncrement > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    _ghostShadow.DrawFrame(spriteBatch, p, effect);

                    p = _position;
                    p.Y -= _ghostShadow1.Size.Y;

                    effect = _increment < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    _ghostShadow1.DrawFrame(spriteBatch, p, effect);
                }
                else
                    _fading = false;
            }
        }          

        /// <summary>
        /// moves the hole
        /// </summary>
        public void Move(GameTime gameTime)
        {
            if (_inactive && _inactiveStart == 0)
                _inactiveStart = gameTime.TotalGameTime.TotalMilliseconds;

            if( _inactive )
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - _inactiveStart > 1000)
                    _inactive = false;
            }

            if (!_fading)
            {
                if (_ghost)
                {                    
                    if (_currendSecond != (Int32)gameTime.TotalGameTime.TotalSeconds)
                    {
                        Random r = new Random((int)DateTime.Now.Ticks + _number);

                        Int32 percent = r.Next(1, 100);

                        // create the bonus with a likelyhood of N% per second
                        if (percent < _percentDisappear)
                        {
                            _shadowPosition = _position;
                            _shadowIncrement = _increment;
                            _ghostShadow.Play();
                            _ghostShadow1.Play();
                            _fading = true;

                            r = new Random((int)DateTime.Now.Ticks);

                            _currentLine = r.Next(2, _maxLines);

                            _position.X = r.Next(_titleSafe.X, _titleSafe.X + _titleSafe.Width);
                            // calculate the y position of the monster            
                            _position.Y = _currentLine * _delta - _offset;
                        }
                        else if (percent < _percentDirectionChange)
                        {
                            _increment *= -1;
                        }

                        _currendSecond = (Int32)gameTime.TotalGameTime.TotalSeconds;
                    }
                }
                else if (_vulture)
                {
                    if (_currendSecond != (Int32)gameTime.TotalGameTime.TotalSeconds)
                    {
                        Random r = new Random((int)DateTime.Now.Ticks + _number);

                        Int32 percent = r.Next(1, 100);

                        if (percent < _percentDirectionChange)
                        {
                            _increment *= -1;
                        }

                        _currendSecond = (Int32)gameTime.TotalGameTime.TotalSeconds;
                    }
                }

                // shift it on x
                _position.X += _increment;

                // check for horizontal wrap around
                if (_position.X < _titleSafe.X - _width / 2)
                {
                    _position.X = _titleSafe.Width + _titleSafe.X - _width / 2;
                    _currentLine -= 1; // move one line up

                    // vertical wrap around at line 2, monsters don't use line 1
                    if (_currentLine < 2)
                        _currentLine = _maxLines;
                }
                else if (_position.X + _width / 2 > _titleSafe.Width + _titleSafe.X)
                {
                    _position.X = _titleSafe.X - _width / 2;
                    _currentLine += 1;

                    // vertical wrap around
                    if (_currentLine > _maxLines)
                        _currentLine = 2;
                }
                // calculate the position of a new line            
                _position.Y = _currentLine * _delta - _offset;
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            if (_ghost && _fading)
            {
                _ghostShadow.UpdateFrame(elapsed);
                _ghostShadow1.UpdateFrame(elapsed);
            }
            
            _monster.UpdateFrame(elapsed);
        }

        /// <summary>
        /// check if the current position rans into the monster
        /// </summary>        
        public Boolean TestMonster(Int32 line, Int32 position)
        {
            if (_fading)
                return false;

            if (_inactive)
                return false;

            if (line == _currentLine)
            {
                if ((position > _position.X) && (position < _position.X + _width))
                {
                    if (_ghost)
                    {
                        MMFxManager.Fx.PlayGhostCrash();
                    }
                    else if (_vulture)
                    {
                        MMFxManager.Fx.PlayVultureCrash();
                    }
                    else
                    {
                        MMFxManager.Fx.PlaySnakeCrash();
                    }

                    _inactive = true;
                    _inactiveStart = 0;

                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// check if the newly created monster interferes with an existing one
        /// </summary>        
        public Boolean Intersects(JJMonster monster)
        {
            if (_currentLine == monster._currentLine)
            {
                if ((_position.X - _width - 100 < monster._position.X) && (_position.X + _width + 100 > monster._position.X))
                {                  
                    return true;
                }
            }
            return false;
        }

        public int Number
        {
            get
            {
                return _number;
            }
        }

        /// <summary>
        /// initialize the monster with random values
        /// </summary>
        private void ShuffleStart(Random r)
        {                        
            Int32 up = r.Next(0, 2) == 0 ? 1 : -1;
            _increment *= up;

            _currentLine = r.Next(2, _maxLines);

            // each monster has a different speed
            double speedModifier = 0.3 + r.NextDouble();
            _increment *= (float)speedModifier;

            _position.X = r.Next(_titleSafe.X, _titleSafe.X + _titleSafe.Width);

            // calculate the y position of the monster            
            _position.Y = _currentLine * _delta - _offset;            
        }


        float _shadowIncrement;
        Vector2 _shadowPosition;

        double _inactiveStart = 0;
        bool _inactive = false;
        bool _fading = false;
        MMAnimatedTexture _ghostShadow;
        MMAnimatedTexture _ghostShadow1;
        MMAnimatedTexture _monster; // texture to draw the hole        
        Rectangle _screenRect;      // the size of the game area
        Rectangle _titleSafe;
        Int32 _currentLine;         // line in the the hole currently is
        Int32 _maxLines;            // maximum number of lines on the screen
        Vector2 _position;          // current screen position of the hole                
        float _increment;           // number of pixels the hole moves with each gamestep
        Int32 _width;
        float _delta;               // distance between two lines
        Int32 _offset;              // distance of the first line from the top of the screen
        float _targetTime;
        int _number;
        bool _ghost = false;
        bool _vulture = false;
        Int32 _currendSecond;
        Int32 _percentDisappear = 5;
        Int32 _percentDirectionChange = 10;
    }
}
