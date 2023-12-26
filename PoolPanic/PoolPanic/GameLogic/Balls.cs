using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace PoolPanic
{
    /// <summary>
    /// collection of all _balls
    /// </summary>
    class Balls
    {
        List<Ball> _lastSunkenBalls = new List<Ball>();
        Ball _firstHit = null;

        List<Ball> _balls = new List<Ball>();
        private float _damping = 80.0f; // 80% speed _damping per second


        public Balls()
        {
        }

        /// <summary>
        /// create _balls and reset position for a 8 ball game
        /// </summary>        
        public void Create8Balls(ContentManager content, Rectangle table)
        {
            _balls.Clear();

            int x = 200 + 10;
            int y = 300 + 10;
            int ballwidth;

            for (int i = 0; i < 16; i++)
            {
                Vector2 pos = new Vector2(x, y);

                Ball b = new Ball(content, pos, i);
                
                _balls.Add(b);
                ballwidth = (int)b.Radius * 2;

                x += ballwidth + 10;
                if (x > table.X + table.Width - ballwidth - 20)
                {
                    y += ballwidth + 10;
                    x = ballwidth + 10;
                }              
            }

            Reset8Ball(table);

            ResetShot();
        }

        /// <summary>
        /// create _balls and reset position for a 9 ball game
        /// </summary>        
        public void Create9Balls(ContentManager content, Rectangle table)
        {
            _balls.Clear();

            int x = 200 + 10;
            int y = 300 + 10;
            int ballwidth;

            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = new Vector2(x, y);

                Ball b = new Ball(content, pos, i);

                _balls.Add(b);
                ballwidth = (int)b.Radius * 2;

                x += ballwidth + 10;
                if (x > table.X + table.Width - ballwidth - 20)
                {
                    y += ballwidth + 10;
                    x = ballwidth + 10;
                }
            }

            Reset9Ball(table);

            ResetShot();
        }

        /// <summary>
        /// position the balls on the texture
        /// </summary>        
        public void Reset8Ball(Rectangle table)
        {                      
            // white ball
            Vector2 pos = new Vector2(table.X + table.Width / 5 * 4, table.Y + table.Height / 2);            
            _balls[0].Position = pos;
                                    
            pos = new Vector2(table.X + table.Width / 3, table.Y + table.Height / 2);
            _balls[2].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 50, table.Y + table.Height / 2 - 29);
            _balls[11].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 50, table.Y + table.Height / 2 + 29);
            _balls[6].Position = pos;
            
            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2);
            _balls[8].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2 + 58);
            _balls[10].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2 - 58);
            _balls[12].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 + 29);
            _balls[7].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 - 29);
            _balls[14].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 - 88);
            _balls[5].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 + 88);
            _balls[9].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2);
            _balls[15].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2 + 58);
            _balls[3].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2 - 58);
            _balls[13].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2 - 116);
            _balls[1].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2 + 116);
            _balls[4].Position = pos;

            foreach (Ball b in _balls)
                b.Active = true;
             
        }

        /// <summary>
        /// position the _balls for a 9 ball game on the texture
        /// </summary>        
        public void Reset9Ball(Rectangle table)
        {
            // white ball
            Vector2 pos = new Vector2(table.X + table.Width / 5 * 4, table.Y + table.Height / 2);
            _balls[0].Position = pos;

            pos = new Vector2(table.X + table.Width / 3, table.Y + table.Height / 2);
            _balls[1].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 50, table.Y + table.Height / 2 - 29);
            _balls[2].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 50, table.Y + table.Height / 2 + 29);
            _balls[6].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2);
            _balls[8].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2 + 58);
            _balls[9].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 100, table.Y + table.Height / 2 - 58);
            _balls[4].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 + 29);
            _balls[7].Position = pos;

            pos = new Vector2(table.X + table.Width / 3 - 150, table.Y + table.Height / 2 - 29);
            _balls[5].Position = pos;
            
            pos = new Vector2(table.X + table.Width / 3 - 200, table.Y + table.Height / 2);
            _balls[3].Position = pos;

            foreach (Ball b in _balls)
                b.Active = true;
        }

        /// <summary>
        /// returns true as long as a ball is moving
        /// </summary>
        /// <returns></returns>
        public Boolean IsMoving()
        {
            foreach( Ball b in _balls)
            {
                if (b.Speed.Length() > 0 || b.InHole == true)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// draw all balls on the table
        /// </summary>       
        public void DrawActiveBalls(SpriteBatch spriteBatch, GameTime DrawTime)
        {            
            foreach (Ball b in _balls)
            {             
                if( b.Active )
                    b.Draw(spriteBatch, DrawTime);                
            }
        }

        /// <summary>
        /// draw inactive balls below the table
        /// </summary>        
        public void DrawInactiveBalls(SpriteBatch spriteBatch, GameTime DrawTime, Vector2 Position)
        {
            int x = (int)Position.X;
            int y = (int)Position.Y;

            foreach (Ball b in _balls)
            {                
                if (b.Active == false)
                {
                    if (b.InHole == false)
                    {
                        // force the ball from table if the in hole animation stoped
                        b.Position = new Vector2(x, y);
                        y += (int)b.Radius * 2 + 7;    
                    }
                    
                    b.Draw(spriteBatch, DrawTime);
                }
            }
        }

        /// <summary>
        /// move the _balls on the _texture
        /// compute and resolve _collisions
        /// check if a ball is in the hole
        /// compute border boucning
        /// </summary>        
        public void MoveBalls(GameTime UpdateTime, Table table)
        {         
            foreach (Ball b1 in _balls)
            {
                // skip inactive balls
                if (b1.Active == false)
                    continue;

                Vector2 oldPosition = b1.Position;
               
                b1.Position += b1.Speed * (float)UpdateTime.ElapsedGameTime.TotalSeconds;

                Hole hole = table.BallInHole(b1);

                if( hole != null )
                {
                    b1.Speed = new Vector2(0, 0);
                    b1.Active = false;                    

                    
                    b1.PutInHole(UpdateTime.TotalGameTime, hole.Position + hole.Offset);
                    
                    // add this ball to the list of sunken balls
                    _lastSunkenBalls.Add(b1);

                    FxManager.Fx.PlayInHole();
                }   
             
                if(b1.Active)                
                    table.BounceBorder(b1);

                // check collision with each of the other balls
                foreach (Ball b2 in _balls)
                {
                    // don't check collision with myself or skip inactive balls
                    if (b1.Equals(b2) || b2.Active == false)
                        continue;

                    if (b1.IsCollision(b2))
                    {
                        if (b1.IsCollided(b2) == false)
                        {
                            // these two balls never collided
                            b1.EllasticCollisionPhysics(b2);
                            b1.AddCollision(b2);

                            if (_firstHit == null)
                                _firstHit = b2;
                            
                            FxManager.Fx.PlayClash(b1.NormalizedSpeed());
                        }

                        // resolve collision by bringing the ball back to old position
                        // Todo: do this better by moving back just to the position where the balls don't intersect
                        b1.Position = oldPosition;
                    }
                }
            }
            
            foreach (Ball b in _balls)
            {
                b.ClearCollision();              
            }            
        }

        public void AnimateBallInHoles(GameTime UpdateTime)
        {
            foreach (Ball b in _balls)
            {
                if(b.InHole)
                {
                    b.RemoveFromTable(UpdateTime);
                }
            }
        }

        /// <summary>
        /// damp speed of all _balls
        /// </summary>        
        public void DampBalls(GameTime UpdateTime)
        {
            foreach (Ball b in _balls)
            {
                b.Damp(_damping * (float)(UpdateTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
            }
        }

        /// <summary>
        /// reset the results of the last shot
        /// </summary>
        public void ResetShot()
        {
            // reset last shot results
            _firstHit = null;
            _lastSunkenBalls.Clear();
        }

        /// <summary>
        /// returns a list all sunken balls of the previous shoot
        /// </summary>        
        public List<Ball> GetLastSunkenBalls()
        {
            return _lastSunkenBalls;
        }

        /// <summary>
        /// returns all remaining balls on table
        /// </summary>
        /// <returns></returns>
        public List<Ball> GetTableBalls()
        {
            List<Ball> balls = new List<Ball>();

            foreach (Ball b in _balls)
            {
                if (b.Active)
                    balls.Add(b);
            }

            return balls;
        }

        /// <summary>
        /// gets the last first hit of the previous shoot
        /// </summary>        
        public Ball GetLastFirstHit()
        {
            return _firstHit;
        }
          
        /// <summary>
        /// access the game ball
        /// </summary>        
        public Ball BallZero()
        {
            return _balls[0];
        }

        /// <summary>
        /// find a spot for the game ball which doesn't collide with a colour ball
        /// </summary>
        public void ResolveBallInHand()
        {
            Ball b1 = BallZero();

            bool resolved = true;

            do
            {
                resolved = true;

                // check collision with each of the other balls
                foreach (Ball b2 in _balls)
                {
                    // don't check collision with myself or skip inactive balls
                    if (b1.Equals(b2) || b2.Active == false)
                        continue;

                    if (b1.IsCollision(b2))
                    {
                        resolved = false;
                        b1.Position = new Vector2(b1.Position.X, b1.Position.Y + 1);
                    }
                }
            } while (resolved == false);
        }

        /// <summary>
        /// return the speed of the game ball
        /// </summary>        
        public void SetBallZeroSpeed(Vector2 speed)
        {
            _balls[0].Speed = speed;
        }
    }
}
