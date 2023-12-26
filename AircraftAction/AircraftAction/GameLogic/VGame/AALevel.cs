using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Media;



namespace AAGameLibrary
{
    public class AALevel
    {
        Texture2D _background;

        float _bgX = 0.0f;

        Rectangle _screenRect;        
        AAPlanes _planes;
        AAPlayground _playGround;
        Game _gameMain;

        public enum Destination
        {
            red,
            yellow,
            green,
            blue,
            purple,
            orange
        } 

        public enum GameResult
        {
            exit,
            noresult
        }




        IServiceProvider Services;

        public AALevel(Game gameMain, IServiceProvider serviceProvider, Rectangle screenRect, Rectangle titleSafe)
        {

            _gameMain = gameMain;

            Services = serviceProvider;
            ContentManager content = new ContentManager(serviceProvider, "Content");

            _background = content.Load<Texture2D>("background/ingame");

            _playGround = new AAPlayground(serviceProvider);

            _planes = new AAPlanes(Services);



            _screenRect = screenRect;
            
            Init();
        }

        void Init()
        {
            _planes.CreatePlane();




        }

    


        public GameResult Update(GameTime gameTime, Point position, Boolean clickDown)
        {
            _planes.Update(gameTime, position, clickDown);

            return GameResult.noresult;
        }

     
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Vector2(_bgX, 0), new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(_background, new Vector2(_bgX-800, 0), new Rectangle(0, 0, 800, 480), Color.White);

            _bgX += 0.5f;

            if (_bgX > 800)
                _bgX = 0;


            _playGround.Draw(spriteBatch);


            _planes.Draw(spriteBatch);                                   
        }             
    }
}
