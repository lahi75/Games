﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PoolPanic
{
    public class WinGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PPGameMain _game;
        PPMouse _mouse;

        public WinGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

             graphics.PreferredBackBufferWidth = 1900;
             graphics.PreferredBackBufferHeight = 1200;

            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();
           
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
            base.Window.Title = "Pool Panic";

            base.Initialize();

            string s = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _game.SetVersion(s);            
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _mouse = new PPMouse(new Vector2(0, 0), this.Content.Load<Texture2D>("images/cursor"));

            Rectangle r = new Rectangle(graphics.GraphicsDevice.Viewport.X, graphics.GraphicsDevice.Viewport.Y, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Rectangle titleSafe = r;

            _game = new PPGameMain(this, Services, r, titleSafe);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

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
                this.Exit();

            HandleKeyboardInput(gameTime);

            Point p = new Point(Mouse.GetState().X, Mouse.GetState().Y);

            Boolean click = Mouse.GetState().LeftButton == ButtonState.Pressed;

            if (_game.Update(gameTime, p, click) == PPGameMain.GameResult.exit)
                this.Exit();

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

            _game.Draw(spriteBatch, gameTime);           
            _mouse.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
