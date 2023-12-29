using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PoolPanic
{
    /// <summary>
    /// class for drawing the billiard cue
    /// </summary>
    class Cue
    {
        Vector2 _p1;
        Vector2 _p2;
     
        const int _maxSpeed = 3000;

        const int _animatedCueHeight = 16;
        const int _animatedCueCount = 10;
        
        private Texture2D _animatedCue;
        private Texture2D _line;

        float _startTime = 0;

        enum CueState
        {
            Aim,
            Animate,
            Fire            
        }

        CueState _state = CueState.Aim;
         
        /// <summary>
        /// ctor
        /// </summary>
        public Cue()
        {
            _p1 = new Vector2(0, 0);
            _p2 = new Vector2(1, 1);            
        }

        /// <summary>
        /// load assets
        /// </summary>        
        public void CreateCue(ContentManager content)
        {
            _line = content.Load<Texture2D>("images/line");            
            _animatedCue = content.Load<Texture2D>("images/cue-animated");                                 
        }

        /// <summary>
        /// draw the cue
        /// </summary>        
        public void DrawCue(SpriteBatch spriteBatch, GameTime DrawTime, Table table)
        {
            // dont draw it when firing
            if (_state == CueState.Fire)
                return;
          
            Line l = new Line(GetOrigin(), GetCurrent());
            table.Clip(ref l);

            // length of the red line
            float length = (float)l.GetLength();
            // direction of the red line
            float rot = (float)l.GetRotation(); 

            spriteBatch.Draw(_line, new Rectangle((int)l.P1.X, (int)l.P1.Y, (int)length, _line.Width), new Rectangle(0, 0, _line.Width, _line.Height), Color.White, rot, new Vector2(_line.Width, _line.Height), SpriteEffects.None, 0.0f);
            
            // make sure the red line aligns with the middle of the ball
            Vector2 p = new Vector2(_p1.X + (float)((_animatedCueHeight / 2) * Math.Sin(rot)), _p1.Y - (float)((_animatedCueHeight / 2) * Math.Cos(rot)));                        

            float time;
            
            if(_state == CueState.Aim)
                time = 0;                 // always draw the first cue of the animation when aiming
            else            
                time = (float)(DrawTime.TotalGameTime.TotalSeconds) - _startTime;            // animate the cue

            // get the right cue from the asset
            var sourceRectangle = GetCueAnimation(2.00f * time);

            spriteBatch.Draw(_animatedCue, p, sourceRectangle, Color.White, rot, new Vector2(0,0), 1.0f, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// retrieve the right image from the asset rectangle, time dependend
        /// </summary>        
        private Rectangle GetCueAnimation(float time)
        {
            var i = SharpDX.MathUtil.Clamp((int)((time % 1.0f) * _animatedCueCount), 0, _animatedCueCount);

            // switch cue states when the animation played once
            if (i == _animatedCueCount-1)
                _state = CueState.Fire;

            return new Rectangle(0, _animatedCueHeight * i, _animatedCue.Width, _animatedCueHeight);
        }

        /// <summary>
        /// update the position from mouse pos and click events
        /// </summary>        
        public void Update(Vector2 position)
        {                  
            _p2 = position;                 
        }

        /// <summary>
        /// switch the cue to the animation state
        /// </summary>        
        public void StartAnimation(GameTime time)
        {
            _state = CueState.Animate;
            _startTime = (float)(time.TotalGameTime.TotalSeconds);
        }

        public Boolean IsAnimating()
        {
            return _state == CueState.Animate;
        }

        /// <summary>
        /// check if we need to shoot
        /// </summary>        
        public Boolean IsShoot()
        {
            return _state == CueState.Fire;
        }

        /// <summary>
        /// returns the normalized speed vector
        /// </summary>        
        public Vector2 NormalizedSpeed(float power)
        {
            float speed = _maxSpeed * power;
                
            Line l = new Line(GetOrigin(), GetCurrent());
            float rot = (float)l.GetRotation();
            float x = (float)Math.Cos(rot) * speed;
            float y = (float)Math.Sin(rot) * speed;

            return new Vector2(x,y);
        }
       
        /// <summary>
        /// updates the origin of the _line
        /// </summary>        
        public void SetOrigin(Vector2 p)
        {
            _p1 = p;
            _state = CueState.Aim; // always start aiming when we set the origin
        }

        /// <summary>
        /// get the origin of the cue
        /// </summary>        
        public Vector2 GetOrigin()
        {
            return _p1;
        }

        /// <summary>
        /// get the second point of the cue
        /// </summary>        
        public Vector2 GetCurrent()
        {
            return _p2;
        }             
    }   
}
