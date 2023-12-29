using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static PoolPanic.Rules;

namespace PoolPanic
{
    public class PPCore
    {        
        private Balls _balls;
        private Table _table;
        private Cue _cue = new Cue();

        private float _power = 0.1f;
        private float _powerIncrement = 0.3f;
        private Vector2 _ballSpeed;

        private TimeSpan _lockTime;
        bool _mouseUp = false;

        private Vector2 _resolution;
        private Rules _rules = new Rules();
                
        PPProgress _progress;
        PPLabel _playerLabel;
        PPLabel _infoLabel;
        PPWinner _winnerLabel;                
        Texture2D _background;
                
        Rectangle _screenRect;                        
        ContentManager _content;
                      
        private GameState _state = GameState.Idle;        
        
        Boolean _back = false;
        
        public enum GameState
        {
            Idle,
            BallInHand,
            Aiming,
            Power,
            Processing,
            GameOver
        }

        public enum GameMode
        {
            Ball8,
            Ball9
        }

        public enum GameResult
        {
            next,
            exit_next,
            exit,
            finish,
            noresult
        }        

        public PPCore(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {                                    
            _content = new ContentManager(serviceProvider, "Content");
            
            var virtualResolution = new Vector3(screenRect.Width, screenRect.Height, 1);            
            _screenRect = screenRect;
            _resolution = new Vector2(virtualResolution.X, virtualResolution.Y);
            
            // create the pool items
            _table = new Table(_content);
            _balls = new Balls();
            _cue.CreateCue(_content);            
         
            _background = _content.Load<Texture2D>("images/ParadoxBackground");

            _progress = new PPProgress(_content);
            _progress.CenterPosition(new Vector2(1500,1100));

            _playerLabel = new PPLabel(_content);
            _playerLabel.CenterPosition(new Vector2(1500, 100));

            _infoLabel = new PPLabel(_content);
            _infoLabel.CenterPosition(new Vector2(1000, 100));

            _winnerLabel = new PPWinner(_content);
            _winnerLabel.CenterPosition(new Vector2(_resolution.X/2, 600));         
        }

        public void Init(GameMode m)
        {
            _rules.PlayerUpdated += _rules_PlayerUpdated;
            _rules.InfoUpdated += _rules_InfoUpdated;
            _rules.WinnerUpdated += _rules_WinnerUpdated;

            _rules.Reset();

            Rectangle t = _table.GetBounds();

            switch(m)
            {
                case GameMode.Ball8:                
                    _balls.Create8Balls(_content, t);
                    break;
                case GameMode.Ball9:
                    _balls.Create9Balls(_content, t);
                    break;
            }

            _back = false;
            _state = GameState.Idle;           
        }

        private void _rules_WinnerUpdated(Player obj)
        {
            _winnerLabel.SetText(obj.ToString());
        }

        private void _rules_InfoUpdated(string obj)
        {
            _infoLabel.SetText(obj);
        }

        private void _rules_PlayerUpdated(Player arg1, TableState arg2)
        {
            string s = "";

            switch (arg2)
            {
                case TableState.Player1Full:
                    if (arg1 == Player.Player1)
                        s = "Player 1 - full";
                    else
                        s = "Player 2 - half";
                    break;
                case TableState.Player2Full:
                    if (arg1 == Player.Player2)
                        s = "Player 2 - full";
                    else
                        s = "Player 1 - half";
                    break;
                default:
                    if (arg1 == Player.Player1)
                        s = "Player 1";
                    else
                        s = "Player 2";
                    break;                        
            }
            _playerLabel.SetText(s);            
        }

        public void Back(GameTime gameTime)
        {
            _back = true;
        }

        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            if (_back)
            {
                _back = false;
                return GameResult.exit;
            }            

            switch (_state)
            {
                case GameState.Idle:
                    {
                        _progress.SetProgress(0.0f);

                        #region Mouse input
                        MouseState _mouseState = Mouse.GetState();
                        if (_mouseState.Position != new Point(0, 0))
                        {
                            if (_mouseState.LeftButton == ButtonState.Pressed)
                            {
                                _lockTime = gameTime.TotalGameTime;                              
                                _cue.SetOrigin(_balls.BallZero().Position);
                                _state = GameState.Aiming;
                            }
                        }
                        #endregion
                    }
                    break;
                case GameState.BallInHand:
                    {
                        // lock the input for 500ms to prevent a too fast clicking
                        if (gameTime.TotalGameTime.Subtract(_lockTime).TotalMilliseconds < 500)
                            break;

                        // bring ball back on table
                        if (_balls.BallZero().Active == false)
                        {
                            _balls.BallZero().Active = true;
                            Vector2 p = new Vector2(_table.GetBounds().X + _table.GetBounds().Width / 5 * 4, _table.GetBounds().Y + _table.GetBounds().Height / 2);
                            _balls.BallZero().Position = p;
                            _table.ClipYPosition(_balls.BallZero());
                            _balls.ResolveBallInHand();
                        }

                        #region Mouse input
                        MouseState _mouseState = Mouse.GetState();
                        if (_mouseState.Position != new Point(0, 0))
                        {                           
                            _balls.BallZero().Position = new Vector2(_table.GetBounds().X + _table.GetBounds().Width / 5 * 4, _mouseState.Position.Y );
                            _table.ClipYPosition(_balls.BallZero());
                            _balls.ResolveBallInHand();

                            if (_mouseState.LeftButton == ButtonState.Pressed)
                            {
                                _cue.SetOrigin(_balls.BallZero().Position);
                                _state = GameState.Aiming;
                                _lockTime = gameTime.TotalGameTime;                                                                
                            }
                        }
                        #endregion                       
                    }
                    break;
                case GameState.Aiming:
                    {                                                
                        // lock the input for 500ms to prevent a too fast clicking
                        if (gameTime.TotalGameTime.Subtract(_lockTime).TotalMilliseconds < 100)
                            break;

                        #region Mouse input
                        MouseState _mouseState = Mouse.GetState();
                        if (_mouseState.Position != new Point(0, 0))
                        {                           
                            // update the billiard cue                         
                            Vector2 p = new Vector2(_mouseState.Position.X, _mouseState.Position.Y);
                            _cue.Update( p );
                            
                            // start the shoot animation on mouse click
                            if (_mouseState.LeftButton == ButtonState.Pressed)
                            {
                                _state = GameState.Power;
                                _power = 0f;                                
                                _mouseUp = false;
                                _lockTime = gameTime.TotalGameTime;
                            }
                        }
                        #endregion                       
                    }
                    break;
                case GameState.Power:
                    {
                        if (_cue.IsAnimating() == false)
                        {
                            UpdatePower(gameTime);
                            _progress.SetProgress(_power);                           

                            #region Mouse input
                            MouseState _mouseState = Mouse.GetState();
                            if (_mouseState.LeftButton == ButtonState.Released)
                            {                                
                                // start the shoot animation on mouse click                             
                                if( _mouseUp == false )
                                {
                                    _ballSpeed = _cue.NormalizedSpeed(_power);                                   
                                    _cue.StartAnimation(gameTime);
                                    _mouseUp = true;
                                }
                            }
                            #endregion                            
                        }

                        // when the animation of the cue finished we can fire the ball
                        if (_cue.IsShoot())
                        {
                            FxManager.Fx.PlayTock();
                            _balls.SetBallZeroSpeed(_ballSpeed);
                            _state = GameState.Processing;                            
                        }
                    }
            
                    break;
                case GameState.Processing:
                    {
                        _balls.MoveBalls(gameTime, _table);
                        _balls.AnimateBallInHoles(gameTime);
                        _balls.DampBalls(gameTime);
                        
                        // switch to idle when no balls are moving anymore
                        if (_balls.IsMoving() == false)
                        {
                            ///// evaluate results
                            Rules.Result r = _rules.Check8Ball(_balls.GetLastSunkenBalls(), _balls.GetTableBalls(), _balls.GetLastFirstHit());

                            switch (r)
                            {
                                case Rules.Result.Foul:
                                    _rules.SwitchPlayer();                                  
                                    _state = GameState.Idle;                                    
                                    break;
                                case Rules.Result.GameBallSunken:                                    
                                    _state = GameState.BallInHand;
                                    _rules.SwitchPlayer();                                    
                                    _lockTime = gameTime.TotalGameTime;
                                    break;
                                case Rules.Result.Gameover:                                    
                                    _state = GameState.GameOver;
                                    break;
                                case Rules.Result.Ok:
                                    _state = GameState.Idle;                                    
                                    break;
                            }
                            _balls.ResetShot();
                        }
                    }
                    break;
                case GameState.GameOver:
                    {
                        #region Mouse input
                        MouseState _mouseState = Mouse.GetState();
                        if (_mouseState.LeftButton == ButtonState.Pressed)
                        {
                            // exit game after mouse click
                            return GameResult.exit;
                        }
                        #endregion
                        break;
                    }
            }
           
            return GameResult.noresult;
        }

        public void UpdatePower(GameTime gameTime)
        {
            _power += (float)(gameTime.ElapsedGameTime.TotalSeconds) * _powerIncrement;

            if (_power > 1f)
            {
                _powerIncrement = -_powerIncrement;
                _power = 1f;
            }
            if (_power < 0f)
            {
                _powerIncrement = -_powerIncrement;
                _power = 0.05f;
            }           
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if( _background != null )
                spriteBatch.Draw(_background, _screenRect, Color.White);

            _table.DrawTable(spriteBatch, gameTime);
            _balls.DrawActiveBalls(spriteBatch, gameTime);

            if (_state == GameState.Aiming || _state == GameState.Power)
                _cue.DrawCue(spriteBatch, gameTime, _table);

            Rectangle r = _table.GetBounds();
            Vector2 ballPos = new Vector2(r.X - 120, r.Y - 50);
            _balls.DrawInactiveBalls(spriteBatch, gameTime, ballPos);
         
           _progress.Draw(spriteBatch);
         
            _playerLabel.Draw(spriteBatch);
            _infoLabel.Draw(spriteBatch);

            if(_state == GameState.GameOver)
                _winnerLabel.Draw(spriteBatch);            
        }             
    }
}