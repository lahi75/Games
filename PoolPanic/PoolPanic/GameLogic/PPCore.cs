using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace PoolPanic
{
    public class PPCore
    {
        bool _isTouched = false;

        private Balls _balls;
        private Table _table;
        private Cue _cue = new Cue();






        //LLDialogRepeat _repeatDlg = null;

        Texture2D _clock;        

        SpriteFont _font25;
        Texture2D _background;
                
        Rectangle _screenRect;                
        Game _gameMain;
        ContentManager _content;

        double _startTime = 0;

   

        int _penalty = 0;
        int _initialPoints = 0;        
        int _bonusPoints = 0;
        int _timePoints = 0;
        int _totalTools = 0;
        int _currentTime = 0;
        int _levelTime = 0;

        //LLLevelDefinition _levelDefinition;

        private GameState _state = GameState.Idle;

        IServiceProvider Services;

        double _pauseStart = 0;
        double _waitTime = 0;
        Boolean _back = false;
        
        public enum GameState
        {
            Idle,
            BallInHand,
            Aiming,
            Power,
            Processing
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
            _gameMain = gameMain;

            Services = serviceProvider;
            _content = new ContentManager(serviceProvider, "Content");


            _table = new Table(_content);

            _balls = new Balls();

            _cue.CreateCue(_content);

            Rectangle t = _table.GetBounds();
            _balls.Create8Balls(_content, t);


            //_tools = new LLTools(_content, new Rectangle(50,80,750,400));
            //_objects = new LLObjects(_content);
            //_counters = new LLCounters();            

            //_levelDefinition = new LLLevelDefinition(_content, _objects, _tools,_counters);

            //_targetButton = new LLTargetButton(_content, new Point( 40, 40));
            //_targetCross = new LLTargetCross(_content,_levelDefinition.Area());

            //_coinsTexture = new LLAnimatedTexture(Vector2.Zero, 0, 1.0f, 0.5f, false);
            //_coinsTexture.Load(_content, "objects/coins", 6, 12);
            //_clock = _content.Load<Texture2D>("clock");

            //_screenRect = screenRect;            
            //_font25 = _content.Load<SpriteFont>("fonts/tycho_25");
        }

        public GameState State
        {
            get { return _state; }
        }

        public void Init()
        {            
            //LLLevelDefinition.LevelStage l;

            //switch (stage)
            //{
            //    default:
            //    case LLSettings.Difficulty.novice:
            //        l = LLLevelDefinition.LevelStage.novice;
            //        break;
            //    case LLSettings.Difficulty.advanced:
            //        l = LLLevelDefinition.LevelStage.advanced;
            //        break;
            //    case LLSettings.Difficulty.expert:
            //        l = LLLevelDefinition.LevelStage.expert;
            //        break;
            //    case LLSettings.Difficulty.master:
            //        l = LLLevelDefinition.LevelStage.master;
            //        break;
            //}
            //_state = LevelState.edit;
            //_background = _levelDefinition.CreateLevel(l, levelNumber, out _levelTime);
            //_currentTime = _levelTime;
            //_counters.Update(_tools);

            //_initialPoints = 100;
            //CurrentPoints = _initialPoints;
            //_bonusPoints = 0;
            //_penalty = 0;
            //_timePoints = 0;
            //_totalTools = _counters.CountItems(); // count how many items are available in this level

            //_pauseDlg = null;
            //_stageDlg = null;
            //_repeatDlg = null;
            //_doneDlg = null;
            //_bombDlg = null;

            //CurrentLevel = levelNumber;
            //Stage = stage;
            //_startTime = 0;
            //_pauseStart = 0;
        }

        public Int32 CurrentLevel { get; set; }

        public Int32 CurrentPoints { get; set; }

        

        public void Back(GameTime gameTime)
        {
          
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
                    }
                    break;
                case GameState.BallInHand:
                    break;
                case GameState.Aiming:
                    {
                    }
                    break;
                case GameState.Power:
                    {

                    }
                    break;
                case GameState.Processing:
                    {
                    }
                    break;
            }        

            return GameResult.noresult;


        }
        
        private void UpdatePoints()
        {
            //CurrentPoints = _initialPoints + _timePoints + _bonusPoints - (_totalTools - _counters.CountItems()) - _penalty;

            //if (CurrentPoints < 0)
            //    CurrentPoints = 0;
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
            Vector2 ballPos = new Vector2(r.X - 100, r.Y - 50);
            _balls.DrawInactiveBalls(spriteBatch, gameTime, ballPos);

        }             
    }
}
