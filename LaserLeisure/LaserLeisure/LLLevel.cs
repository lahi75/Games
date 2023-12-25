using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;


namespace LLGameLibrary
{
    public class LLLevel
    {
        bool _isTouched = false;

        LLDialogRepeat _repeatDlg = null;
        LLDialogBomb _bombDlg = null;
        LLDialogDone _doneDlg = null;
        LLDialogPause _pauseDlg = null;
        LLDialogStage _stageDlg = null;

        LLTargetButton _targetButton;
        LLTargetCross _targetCross;

        LLAnimatedTexture _coinsTexture;
        Texture2D _clock;        

        SpriteFont _font25;
        Texture2D _background;
                
        Rectangle _screenRect;                
        Game _gameMain;
        ContentManager _content;

        double _startTime = 0;

        LLTools _tools;
        LLObjects _objects;
        LLCounters _counters;

        int _penalty = 0;
        int _initialPoints = 0;        
        int _bonusPoints = 0;
        int _timePoints = 0;
        int _totalTools = 0;
        int _currentTime = 0;
        int _levelTime = 0;

        LLLevelDefinition _levelDefinition;

        LevelState _state = LevelState.edit;
        LevelState _afterWait;
        LevelState _prePause;

        IServiceProvider Services;

        double _pauseStart = 0;
        double _waitTime = 0;
        Boolean _back = false;
        
        public enum LevelState
        {
            edit,
            fire,
            repeat,
            reset,
            done,
            wait,
            stage,
            pause
        }

        public enum GameResult
        {
            next,
            exit_next,
            exit,
            finish,
            noresult
        }        

        public LLLevel(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {            
            _gameMain = gameMain;

            Services = serviceProvider;
            _content = new ContentManager(serviceProvider, "Content");
                        
            _tools = new LLTools(_content, new Rectangle(50,80,750,400));
            _objects = new LLObjects(_content);
            _counters = new LLCounters();            

            _levelDefinition = new LLLevelDefinition(_content, _objects, _tools,_counters);

            _targetButton = new LLTargetButton(_content, new Point( 40, 40));
            _targetCross = new LLTargetCross(_content,_levelDefinition.Area());

            _coinsTexture = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            _coinsTexture.Load(_content, "objects/coins", 6, 12);
            _clock = _content.Load<Texture2D>("clock");

            _screenRect = screenRect;            
            _font25 = _content.Load<SpriteFont>("fonts/tycho_25");
        }

        public LevelState State
        {
            get { return _state; }
        }

        public void Init(LLSettings.Difficulty stage, int levelNumber)
        {            
            LLLevelDefinition.LevelStage l;

            switch (stage)
            {
                default:
                case LLSettings.Difficulty.novice:
                    l = LLLevelDefinition.LevelStage.novice;
                    break;
                case LLSettings.Difficulty.advanced:
                    l = LLLevelDefinition.LevelStage.advanced;
                    break;
                case LLSettings.Difficulty.expert:
                    l = LLLevelDefinition.LevelStage.expert;
                    break;
                case LLSettings.Difficulty.master:
                    l = LLLevelDefinition.LevelStage.master;
                    break;
            }
            _state = LevelState.edit;
            _background = _levelDefinition.CreateLevel(l, levelNumber, out _levelTime);
            _currentTime = _levelTime;
            _counters.Update(_tools);

            _initialPoints = 100;
            CurrentPoints = _initialPoints;
            _bonusPoints = 0;
            _penalty = 0;
            _timePoints = 0;
            _totalTools = _counters.CountItems(); // count how many items are available in this level

            _pauseDlg = null;
            _stageDlg = null;
            _repeatDlg = null;
            _doneDlg = null;
            _bombDlg = null;

            CurrentLevel = levelNumber;
            Stage = stage;
            _startTime = 0;
            _pauseStart = 0;
        }

        public Int32 CurrentLevel { get; set; }

        public Int32 CurrentPoints { get; set; }

        public LLSettings.Difficulty Stage { get; set; }

        public void Back(GameTime gameTime)
        {
            if (_state == LevelState.pause)
            {
                _back = true;
                _pauseDlg = null;
                _stageDlg = null;
                _repeatDlg = null;
                _doneDlg = null;
                _bombDlg = null;
            }
            else
            {
                _pauseStart = gameTime.TotalGameTime.TotalSeconds;
                _prePause = _state;
                _state = LevelState.pause;
                LLFxManager.Fx.PlayLaserNoise(false);
            }
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
                case LevelState.edit:
                    {
                        if (_startTime == 0)
                            _startTime = gameTime.TotalGameTime.TotalSeconds;

                        _currentTime = (int)(_levelTime - (gameTime.TotalGameTime.TotalSeconds - _startTime));

                        if (_currentTime == 0)
                        {
                            _state = LevelState.fire;
                            break;
                        }

                        if (_targetButton.Update(position, clickDown))
                        {
                            // toggle target cross
                            _targetCross.Enable = !_targetCross.Enable;
                        }
                        else if (_targetCross.Enable)
                        {
                            if( clickDown && !_targetButton.IsHovered())
                                _targetCross.Update(position);
                        }
                        else
                        {
                            //_targetButton.Pressed = false;
                            //_targetCross.Enable = false;

                            _tools.Update(gameTime, position, clickDown, _objects, _counters);

                            if (clickDown && _isTouched == false)
                                if (_objects.LaserContain(position))
                                    _state = LevelState.fire;

                            if (clickDown == false && _isTouched)
                                UpdatePoints();
                        }
                    }
                    break;
                case LevelState.fire:
                    {
                        LLFxManager.Fx.PlayLaserNoise(true);

                        _tools.Update(gameTime, position, false, _objects, _counters);
                        _objects.Update(gameTime, _tools);

                        if (_objects.CoinsScored())
                        {
                            _bonusPoints = 30;
                            UpdatePoints();
                        }

                        // check if any beam has hit a bomb
                        if (_objects.Bombhit())
                        {
                            LLFxManager.Fx.PlayLaserNoise(false);

                            _objects.Kill();
                            _state = LevelState.wait;                            
                            _afterWait = LevelState.reset;
                            _waitTime = gameTime.TotalGameTime.TotalMilliseconds + 4000; // wait 4 seconds
                        }
                        else if (_objects.TargetFinish())
                        {
                            LLFxManager.Fx.PlayLaserNoise(false);

                            _state = LevelState.wait;
                            _objects.KillCoins();

                            _timePoints = (int)(_currentTime / (double)_levelTime * 10);
                            UpdatePoints();

                            // check if we played all levels of this stage in ladder mode
                            if (CurrentLevel == LevelManager.Level.MaxLevel && SettingsManager.Settings.GameMode == LLSettings.Mode.ladder)
                                _afterWait = LevelState.stage;
                            else
                                _afterWait = LevelState.done;

                            _waitTime = gameTime.TotalGameTime.TotalMilliseconds + 4000; // wait 4 seconds
                        }
                        else if (_objects.LaserDead())
                        {
                            LLFxManager.Fx.PlayLaserNoise(false);

                            _state = LevelState.wait;
                            _objects.KillCoins();
                            _afterWait = LevelState.repeat;
                            _waitTime = gameTime.TotalGameTime.TotalMilliseconds + 4000; // wait 4 seconds
                        }
                    }
                    break;
                case LevelState.done:
                    _objects.Update(gameTime, _tools);

                    if( _doneDlg == null )
                    {
                        _doneDlg = new LLDialogDone(_content, new Rectangle(120, 120, 600, 280), _bonusPoints, _totalTools - _counters.CountItems(), CurrentPoints, _timePoints);
                    }

                    switch (_doneDlg.Update(position, clickDown))
                    {
                        case LLDialogDone.Result.next:
                            _doneDlg = null;
                            _state = LevelState.done;                            
                            return GameResult.next;                            
                        case LLDialogDone.Result.quit:
                            _doneDlg = null;
                            _state = LevelState.done;                            
                            return GameResult.exit_next;                                                        
                    }

                    break;
                case LevelState.stage:
                    _objects.Update(gameTime, _tools);

                    if( _stageDlg == null )
                    {
                        _stageDlg = new LLDialogStage(_content, new Rectangle(120, 120, 600, 280), _bonusPoints, _totalTools - _counters.CountItems(), CurrentPoints, _timePoints);
                    }

                    switch (_stageDlg.Update(position, clickDown))
                    {                        
                        case LLDialogStage.Result.quit:
                            _doneDlg = null;
                            _state = LevelState.done;
                            _stageDlg = null;
                            return GameResult.finish;         
                    }
                    break;
                case LevelState.reset:
                    _objects.Update(gameTime, _tools);

                    if (_bombDlg == null)
                    {
                        _bombDlg = new LLDialogBomb(_content, new Rectangle(100, 150, 640, 200)); 
                    }
                    
                    switch (_bombDlg.Update(position, clickDown))
                    {
                        case LLDialogBomb.Result.reset:
                            _bombDlg = null;
                            _state = LevelState.edit;
                            // after bomb hit, restart the level
                            Init(Stage,CurrentLevel);
                            break;
                        case LLDialogBomb.Result.quit:
                            _bombDlg = null;                            
                            return GameResult.exit;
                    }
                    
                    break;
                case LevelState.repeat:
                    _objects.Update(gameTime, _tools);

                    if (_repeatDlg == null)
                    {
                        _repeatDlg = new LLDialogRepeat(_content, new Rectangle(100, 150, 600, 250)); 
                    }

                    switch (_repeatDlg.Update(position, clickDown))
                    {
                        case LLDialogRepeat.Result.repeat:
                            _repeatDlg = null;
                            _penalty += 10;
                            UpdatePoints();
                            _currentTime = _levelTime;
                            _startTime = 0;
                            _state = LevelState.edit;
                            _objects.Reset();
                            break;
                        case LLDialogRepeat.Result.quit:
                            _repeatDlg = null;                            
                            return GameResult.exit;
                    }

                    break;
                case LevelState.wait:

                    _objects.Update(gameTime, _tools);

                    if (_objects.CoinsScored())
                    {
                        _bonusPoints = 30;
                        UpdatePoints();
                    }

                    if (gameTime.TotalGameTime.TotalMilliseconds > _waitTime)
                        _state = _afterWait;                    

                    break;
                case LevelState.pause:

                    if (_pauseDlg == null)
                    {
                        _pauseDlg = new LLDialogPause(_content, new Rectangle(170, 170, 500, 180));
                    }

                    switch (_pauseDlg.Update(position, clickDown))
                    {
                        case LLDialogPause.Result.resume:
                            // resume from pause here
                            _state = _prePause;
                            _pauseDlg = null;

                            // add the time we paused to the starttime
                            _startTime += gameTime.TotalGameTime.TotalSeconds - _pauseStart;
                            _pauseStart = 0.0;
                            break;
                        case LLDialogPause.Result.quit:
                            _pauseDlg = null;                              
                            _stageDlg = null;
                            _repeatDlg = null;
                            _doneDlg = null;
                            _bombDlg = null;
                            return GameResult.exit;
                    }
                    break;
            }

            _isTouched = clickDown;

            return GameResult.noresult;
        }
        
        private void UpdatePoints()
        {
            CurrentPoints = _initialPoints + _timePoints + _bonusPoints - (_totalTools - _counters.CountItems()) - _penalty;

            if (CurrentPoints < 0)
                CurrentPoints = 0;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if( _background != null )
                spriteBatch.Draw(_background, _screenRect, Color.White);

            _tools.DrawFirst(spriteBatch);

            // draws fixed objects, including the beams of the laser
            _objects.Draw(spriteBatch);

            // draws the tools (mirror, splitters etc.)
            _tools.Draw(spriteBatch);

            // draw all tool counters
            _counters.Draw(spriteBatch);

            // do topmost drawing (smoke)
            _objects.Draw2(spriteBatch);

            if (_repeatDlg != null)
                _repeatDlg.Draw(spriteBatch);

            if (_doneDlg != null)
                _doneDlg.Draw(spriteBatch);

            if (_bombDlg != null)
                _bombDlg.Draw(spriteBatch);

            if (_pauseDlg != null)
                _pauseDlg.Draw(spriteBatch);

            if (_stageDlg != null)
                _stageDlg.Draw(spriteBatch);

            if (_state == LevelState.edit)
            {
                _targetButton.Draw(spriteBatch);
                _targetCross.Draw(spriteBatch);
            }

            string text = CurrentPoints.ToString();

            _coinsTexture.DrawFrame(spriteBatch, new Vector2(240, 40), SpriteEffects.None);
            spriteBatch.DrawString(_font25, text, new Vector2(260,25), Color.White);

            spriteBatch.Draw(_clock, new Vector2( 100, 20 ), Color.White);
            TimeSpan t = TimeSpan.FromSeconds(_currentTime);            
            text = (int)t.TotalMinutes + ":" + ((int)(t.TotalSeconds - (int)(t.TotalMinutes)*60)).ToString("00");
            spriteBatch.DrawString(_font25, text, new Vector2(145, 25), _currentTime < 10 ? Color.Red : Color.White);
        }             
    }
}
