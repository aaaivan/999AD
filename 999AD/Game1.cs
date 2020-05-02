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
        public static readonly  int gameWidth = 384; //pixels on the x axis
        public static readonly int gameHeight = 216; //pixels on the y axis
        public static Rectangle viewportRectangle;
        RenderTarget2D nativeRenderTarget;
        public static int scale;
        //debug
        SpriteFont spriteFont;

#if LEVEL_EDITOR
        LevelEditor levelEditor;
        public static int editorWidth;
        public static int editorHeight;
        public static MouseState mouseState;
        public static MouseState previousMouseState;
        public static int tilesPerRow=10;
        public static int infoBoxHeightPx = 20;
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
            editorWidth= gameWidth + Tile.tileSize * tilesPerRow;
            editorHeight= gameHeight + infoBoxHeightPx;
            scale = MathHelper.Min(GraphicsDevice.DisplayMode.Width / editorWidth, GraphicsDevice.DisplayMode.Height / editorHeight);
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, editorWidth, editorHeight);
            viewportRectangle = new Rectangle(0,0,editorWidth * scale,editorHeight * scale);
            graphics.PreferredBackBufferWidth = viewportRectangle.Width;
            graphics.PreferredBackBufferHeight = viewportRectangle.Height;
#else
            scale = MathHelper.Min(GraphicsDevice.DisplayMode.Width / gameWidth, GraphicsDevice.DisplayMode.Height / gameHeight);
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, gameWidth, gameHeight);
            viewportRectangle = new Rectangle(
                0,
                0,
                gameWidth * scale,
                gameHeight * scale);
            graphics.PreferredBackBufferWidth = viewportRectangle.Width;
            graphics.PreferredBackBufferHeight = viewportRectangle.Height;
#endif     
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
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
            spriteFont = Content.Load<SpriteFont>(@"fonts\monologue");

            MapsManager.Inizialize(Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize
            (
                new Texture2D[(int)RoomsManager.Rooms.total]
                {
                    Content.Load<Texture2D>(@"backgrounds\room1"),
                    Content.Load<Texture2D>(@"backgrounds\room1"),
                    Content.Load<Texture2D>(@"backgrounds\room1"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\finalBoss"),
                    Content.Load<Texture2D>(@"backgrounds\escape"),
                    Content.Load<Texture2D>(@"backgrounds\escape"),
                }
            );
#if LEVEL_EDITOR
            levelEditor = new LevelEditor(Content.Load<SpriteFont>(@"fonts\arial32"),
                                          Content.Load<SpriteFont>(@"fonts\arial14"),
                                          Content.Load<Texture2D>("whiteTile"));
#else
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            ProjectilesManager.Inizialize(Content.Load<Texture2D>("projectile"));
            Player.Inizialize(Content.Load <Texture2D>(@"characters\player"), new Vector2(1290,40));
            RoomsManager.Inizialize();
            GameEvents.Inizialize();
            FireBallsManager.Inizialize(Content.Load<Texture2D>("fireball"), Content.Load<Texture2D>("laser"));
            LavaGeyserManager.Inizialize(Content.Load<Texture2D>("lavaGeyser"),
                                         Content.Load<Texture2D>("whiteTile"));
            FinalBoss.Inizialize(Content.Load<Texture2D>(@"characters\finalBoss"),
                                 new Texture2D[] { Content.Load<Texture2D>(@"characters\stoneWing"),
                                                   Content.Load<Texture2D>(@"characters\healthyWing"),
                                                   Content.Load<Texture2D>(@"characters\damagedWing"),
                                                   Content.Load<Texture2D>(@"characters\deadWing")});
            CollectablesManager.Inizialize(Content.Load<Texture2D>("collectables"));
            MonologuesManager.Inizialize(Content.Load<Texture2D>("dialogueBox"),
                                         Content.Load<Texture2D>("arrowDialogue"),
                                         Content.Load<Texture2D>("interact"),
                                         Content.Load<SpriteFont>(@"fonts\monologue"));
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
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
#if LEVEL_EDITOR
            levelEditor.Draw(spriteBatch, tilesPerRow, infoBoxHeightPx, editorWidth, editorHeight);
#else
            Camera.Draw(spriteBatch);
            RoomsManager.Draw(spriteBatch);
            //debug
            MouseState mouseState = Mouse.GetState();
            spriteBatch.DrawString(spriteFont, (mouseState.X/5 + (int)Camera.position.X) + "," + (mouseState.Y/5 + (int)Camera.position.Y), new Vector2(10, 10), Color.Blue);
#endif
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, viewportRectangle, Color.White);
#if LEVEL_EDITOR
            levelEditor.DrawText(spriteBatch, infoBoxHeightPx);
#endif
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
