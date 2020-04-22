﻿//#define LEVEL_EDITOR

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int screenWidth = 960; //resolution
        public static int screenHeight = 540; //resolution
#if LEVEL_EDITOR
        public readonly static bool levelEditorMode = true;
        LevelEditor levelEditor;
        public static MouseState mouseState;
        public static MouseState previousMouseState;
        public static int tilesPerRow=5;
        public static int infoBoxHeightPx = 40;
#else
        public readonly static bool levelEditorMode = true;
#endif
        public static KeyboardState previousKeyboard;
        public static KeyboardState currentKeyboard;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            this.IsMouseVisible = true;
#if LEVEL_EDITOR
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            graphics.PreferredBackBufferWidth = screenWidth+ Tile.tileSize * tilesPerRow;
            graphics.PreferredBackBufferHeight = screenHeight+ infoBoxHeightPx;
#else
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
#endif     
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Gravity.Inizialize(2000);
            previousKeyboard = Keyboard.GetState();

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

            // TODO: use this.Content to load your game content here
            MapsManager.Inizialize(Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize
            (
                new Texture2D[(int)RoomsManager.Rooms.total]
                {
                    Content.Load<Texture2D>(@"backgrounds\room1"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\finalBoss")
                }
            );
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            ProjectilesManager.Inizialize(Content.Load<Texture2D>("projectile"));
            Player.Inizialize(Content.Load <Texture2D>("player"), new Vector2(20,0));
            RoomsManager.Inizialize();
            GameEvents.Inizialize();
#if LEVEL_EDITOR
            levelEditor = new LevelEditor(Content.Load<SpriteFont>(@"fonts\arial32"),
                                          Content.Load<SpriteFont>(@"fonts\arial14"),
                                          Content.Load<Texture2D>("whiteTile"));

#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKeyboard = Keyboard.GetState();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here
#if LEVEL_EDITOR
            mouseState = Mouse.GetState();
            levelEditor.Update(mouseState, previousMouseState, tilesPerRow, infoBoxHeightPx);
            CameraManager.Update(elapsedTime);
            previousMouseState = mouseState;

#else
            RoomsManager.Update(elapsedTime);
            CameraManager.Update(elapsedTime);
            MapsManager.Update(elapsedTime);
            Player.Update(elapsedTime);
            ProjectilesManager.Update(elapsedTime);
#endif
            previousKeyboard = currentKeyboard;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
#if LEVEL_EDITOR
            levelEditor.Draw(spriteBatch, tilesPerRow, infoBoxHeightPx);

#else
            Camera.Draw(spriteBatch);
            RoomsManager.Draw(spriteBatch);
            ProjectilesManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
