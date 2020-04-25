//#define LEVEL_EDITOR

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
            Player.Inizialize(Content.Load <Texture2D>(@"characters\player"), new Vector2(20,0));
            RoomsManager.Inizialize();
            GameEvents.Inizialize();
            FireBallsManager.Inizialize(Content.Load<Texture2D>("fireball"), Content.Load<Texture2D>("laser"));
            LavaGeyserManager.Inizialize(Content.Load<Texture2D>("lavaGeyser"));
            FinalBoss.Inizialize(Content.Load<Texture2D>(@"characters\finalBoss"),
                                 new Texture2D[] { Content.Load<Texture2D>(@"characters\stoneWing"),
                                                   Content.Load<Texture2D>(@"characters\healthyWing"),
                                                   Content.Load<Texture2D>(@"characters\damagedWing"),
                                                   Content.Load<Texture2D>(@"characters\deadWing")});
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
            Player.Update(elapsedTime);
            ProjectilesManager.Update(elapsedTime);
            GameEvents.Update(elapsedTime);
            if (currentKeyboard.IsKeyDown(Keys.Q))
                FireBallsManager.ThrowAtPlayer(4, 4, 0.5f);
            if (currentKeyboard.IsKeyDown(Keys.I))
                FireBallsManager.ThrowInAllDirections(6, 500, 4);
            if (currentKeyboard.IsKeyDown(Keys.E))
                FireBallsManager.TrowWithinCircularSector(30,300,3, 120);
            if (currentKeyboard.IsKeyDown(Keys.R))
                FireBallsManager.TargetPlatform(new int[] { 0,1,3}, 8, 5);
            if (currentKeyboard.IsKeyDown(Keys.T))
                FireBallsManager.Sweep(1.5f, 6);
            if (currentKeyboard.IsKeyDown(Keys.Y))
                FireBallsManager.Spiral(30, 15, 0.2f, 300);
            if (currentKeyboard.IsKeyDown(Keys.U))
                FireBallsManager.RandomSweep(1, 3, 5);
            if (currentKeyboard.IsKeyDown(Keys.L))
                LavaGeyserManager.ShootGeyser(new float[] { 300, 800 }, 2);
            if (currentKeyboard.IsKeyDown(Keys.J) && !previousKeyboard.IsKeyDown(Keys.J))
                LavaGeyserManager.EquallySpaced(LavaGeyser.size*2.5f, 2, 0);

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
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
