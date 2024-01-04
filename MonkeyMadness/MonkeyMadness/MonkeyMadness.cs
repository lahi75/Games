using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace MonkeyMadness
{
    public class MonkeyMadness : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;        

        MMGameMain _game;
        MMMouse _mouse;

        public MonkeyMadness()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();            
        }

        protected override void Initialize()
        {
            base.Window.Title = "Monkey Madness";

            base.Initialize();

            string s = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //string v;
            //var assembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            //v = assembly.Split('=')[1].Split(',')[0];
            //s = s.Remove(s.LastIndexOf('.'));
            _game.SetVersion(s);

        }

        protected override void LoadContent()
        {            
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Rectangle r = new Rectangle(_graphics.GraphicsDevice.Viewport.X, _graphics.GraphicsDevice.Viewport.Y, _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
            _game = new MMGameMain(this, r, GraphicsDevice.Viewport.TitleSafeArea);
            this.IsMouseVisible = false;

            _mouse = new MMMouse(new Vector2(0, 0), this.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("cursor"));
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                // HandleGamePadInput(gameTime);
            }
            else
            {
                HandleKeyboardInput(gameTime);
            }

            Point p = new Point(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y);
            Boolean click = Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Pressed;

            if (_game.Update(gameTime, p, click) == MMGameMain.GameResult.exit)
                this.Exit();

            //update mouse cursor
            _mouse.Update();

            base.Update(gameTime);
        }

        private void HandleKeyboardInput(GameTime time)
        {
            KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
                _game.Right(time);

            if (keyState.IsKeyDown(Keys.Left))
                _game.Left(time);

            if (keyState.IsKeyDown(Keys.Up))
                _game.Jump(time);

            if (keyState.IsKeyDown(Keys.Enter))
                _game.Start();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                if( _game.Back(time) == true)
                    Exit(); // exit game when on welcome page
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

           // Resolution.BeginDraw();
            
            _game.Draw();

            if (_game.ShowMouseCursor)
            {
                SpriteBatch batch = new SpriteBatch(this.GraphicsDevice);                
                batch.Begin();
                _mouse.Draw(batch);
                batch.End();
            }
            base.Draw(gameTime);
        }
    }
}
