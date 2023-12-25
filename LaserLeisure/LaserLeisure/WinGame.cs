using LLGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace LaserLeisure
{
    public class WinGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        LLGameMain _game;

        LLMouse _mouse;

        public WinGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Rectangle r = new Rectangle(graphics.GraphicsDevice.Viewport.X, graphics.GraphicsDevice.Viewport.Y, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Rectangle titleSafe = r;

            _game = new LLGameLibrary.LLGameMain(this, Services, r, titleSafe);

            _mouse = new LLMouse(new Vector2(0, 0), this.Content.Load<Texture2D>("cursor"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (_game.Back(gameTime) == true) // exit the game when back was pressed on the front page
                    this.Exit();

            HandleKeyboardInput(gameTime);

            Point p = new Point(Mouse.GetState().X, Mouse.GetState().Y);

            Boolean click = Mouse.GetState().LeftButton == ButtonState.Pressed;

            //_game.Update(gameTime, p, click);
            _game.Update(gameTime, p, click);

            //update mouse cursor
            _mouse.Update();


            base.Update(gameTime);
        }

        private void HandleKeyboardInput(GameTime time)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
                _game.Back(time);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            _game.Draw(spriteBatch);

            _mouse.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
