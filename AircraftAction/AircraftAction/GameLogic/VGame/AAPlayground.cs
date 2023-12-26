using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AAGameLibrary
{
    class AAPlayground
    {
        Rectangle _ground;
        int _offset = 80;

        

        AAExit _exitBlue;
        AAExit _exitRed;
        AAExit _exitYellow;
        AAExit _exitGreen;
        AAExit _exitPurple;
        AAExit _exitOrange;

        public AAPlayground(IServiceProvider serviceProvider)
        {
            _ground = new Rectangle(0, 0, 800, 480);            

            ContentManager content = new ContentManager(serviceProvider, "Content");


            int x = _ground.Width / 3 - 150 / 2;
            int y = _ground.Y + _offset;

            _exitBlue = new AAExit(AALevel.Destination.blue, new Rectangle(x,y,150,20), false, content.Load<Texture2D>("sprites/exit_blue"));


            x = _ground.Width * 2  / 3 - 150 / 2;
            y = _ground.Y + _offset;

            _exitRed = new AAExit(AALevel.Destination.red, new Rectangle(x, y, 150, 20), false, content.Load<Texture2D>("sprites/exit_red"));



            x = _ground.Width - 20;
            y = _ground.Height / 2 + _offset + 20/2;


            _exitYellow = new AAExit(AALevel.Destination.yellow, new Rectangle(x, y, 150, 20), true, content.Load<Texture2D>("sprites/exit_yellow"));

            x = _ground.Width * 2 / 3 - 150 / 2;
            y = _ground.Height - 20;

            _exitGreen = new AAExit(AALevel.Destination.green, new Rectangle(x, y, 150, 20), false, content.Load<Texture2D>("sprites/exit_green"));


            x = _ground.Width / 3 - 150 / 2;
            y = _ground.Height - 20;

            _exitOrange = new AAExit(AALevel.Destination.orange, new Rectangle(x, y, 150, 20), false, content.Load<Texture2D>("sprites/exit_orange"));


            x = 0;
            y = _ground.Height / 2 + _offset + 20 / 2;

            _exitPurple = new AAExit(AALevel.Destination.purple, new Rectangle(x, y, 150, 20), true, content.Load<Texture2D>("sprites/exit_purple"));            

        }


        public void Draw(SpriteBatch spriteBatch)
        {
             _exitBlue.Draw(spriteBatch);
             _exitRed.Draw(spriteBatch);
             _exitYellow.Draw(spriteBatch);
             _exitGreen.Draw(spriteBatch);
             _exitOrange.Draw(spriteBatch);
             _exitPurple.Draw(spriteBatch);




        }
    }
}
