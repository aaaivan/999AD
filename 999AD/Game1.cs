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
        public enum GameStates
        {
            playing, dead, total
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static readonly int min_gameWidth = 1920 / 5; //pixels on the x axis
        public static readonly int min_gameHeight = 1080 / 5; //pixels on the y axis
        public static  int gameWidth; //pixels on the x axis
        public static int gameHeight; //pixels on the y axis
        public static Rectangle viewportRectangle;
        static RenderTarget2D nativeRenderTarget;
        static RenderTarget2D renderTarget_zoom1;
        static RenderTarget2D renderTarger_zoom0dot5;
        public static int scale;
        public static KeyboardState previousKeyboard;
        public static KeyboardState currentKeyboard;
        public static GamePadState previousGamePad;
        public static GamePadState currentGamePad;
        public static GameStates currentGameState;
        //<debug>
        SpriteFont spriteFont;
        Texture2D white;
        //</debug>

#if LEVEL_EDITOR
        LevelEditor levelEditor;
        public static int editorWidth;
        public static int editorHeight;
        public static MouseState mouseState;
        public static MouseState previousMouseState;
        public static int tilesPerRow=11;
        public static int infoBoxHeightPx = 20;
#endif
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
            gameWidth = min_gameWidth;
            gameHeight = min_gameHeight;
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
            graphics.ApplyChanges();
            previousKeyboard = Keyboard.GetState();

#else
            scale = MathHelper.Min(GraphicsDevice.DisplayMode.Width / gameWidth, GraphicsDevice.DisplayMode.Height / gameHeight);
            //scale = 1;
            renderTarget_zoom1 = new RenderTarget2D(GraphicsDevice, gameWidth, gameHeight);
            renderTarger_zoom0dot5 = new RenderTarget2D(GraphicsDevice, gameWidth*2, gameHeight*2);
            nativeRenderTarget = renderTarget_zoom1;
            viewportRectangle = new Rectangle(
                0,
                0,
                gameWidth * scale,
                gameHeight * scale);
            graphics.PreferredBackBufferWidth = viewportRectangle.Width;
            graphics.PreferredBackBufferHeight = viewportRectangle.Height;
            graphics.ApplyChanges();
            previousKeyboard = Keyboard.GetState();
            previousGamePad = GamePad.GetState(PlayerIndex.One);

#endif
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

            //<debug>
            spriteFont = Content.Load<SpriteFont>(@"fonts\monologue");
            white = Content.Load<Texture2D>("whiteTile");
            //</debug>

#if LEVEL_EDITOR
            MapsManager.Inizialize(Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize
            (
                new Texture2D[(int)RoomsManager.Rooms.total]
                {
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\midboss"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                }
            );
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            levelEditor = new LevelEditor(Content.Load<SpriteFont>(@"fonts\arial32"),
                                          Content.Load<SpriteFont>(@"fonts\arial14"),
                                          Content.Load<Texture2D>("whiteTile"));
#else
            currentGameState = GameStates.playing;
            MapsManager.Inizialize(Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize
            (
                new Texture2D[(int)RoomsManager.Rooms.total]
                {
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                    Content.Load<Texture2D>(@"backgrounds\room2"),
                }
            );
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            ProjectilesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            Player.Inizialize(Content.Load <Texture2D>(@"characters\player"), new Vector2(300,80));
            RoomsManager.Inizialize();
            GameEvents.Inizialize();
            FireBallsManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            LavaGeyserManager.Inizialize(Content.Load<Texture2D>("animatedSprites"),
                                         Content.Load<Texture2D>("whiteTile"));
            EnemyManager.Initialise(Content.Load<Texture2D>(@"characters\enemy1"), Content.Load<Texture2D>(@"characters\enemy2"));
            MidBoss.Initialise(Content.Load<Texture2D>(@"characters\midboss"));
            FinalBoss.Inizialize(Content.Load<Texture2D>(@"characters\finalBoss"),
                                 new Texture2D[] { Content.Load<Texture2D>(@"characters\stoneWing"),
                                                   Content.Load<Texture2D>(@"characters\healthyWing"),
                                                   Content.Load<Texture2D>(@"characters\damagedWing"),
                                                   Content.Load<Texture2D>(@"characters\deadWing")});
            CollectablesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            MonologuesManager.Inizialize(Content.Load<Texture2D>("dialogueBox"),
                                         Content.Load<Texture2D>("arrowDialogue"),
                                         Content.Load<Texture2D>("interact"),
                                         Content.Load<SpriteFont>(@"fonts\monologue"));
            DoorsManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            AnimatedSpritesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
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
        public static void Zoom0Dot5()
        {
            gameWidth = min_gameWidth * 2;
            gameHeight = min_gameHeight * 2;
            nativeRenderTarget = renderTarger_zoom0dot5;
            CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
        }
        public static void Zoom1()
        {
            gameWidth = min_gameWidth;
            gameHeight = min_gameHeight;
            nativeRenderTarget = renderTarget_zoom1;
            CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
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
            currentGamePad = GamePad.GetState(PlayerIndex.One);
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here
#if LEVEL_EDITOR
            mouseState = Mouse.GetState();
            levelEditor.Update(mouseState, previousMouseState, tilesPerRow, infoBoxHeightPx);
            CameraManager.Update(elapsedTime);
            PlatformsManager.platformsRoomManagers[levelEditor.currentRoomNumber].Update(elapsedTime);
            previousMouseState = mouseState;
#else
            switch(currentGameState)
            {
                case GameStates.playing:
                    RoomsManager.Update(elapsedTime);
                    Player.Update(elapsedTime);
                    ProjectilesManager.Update(elapsedTime);
                    GameEvents.Update(elapsedTime);
                    Collisions.Update(elapsedTime);
                    CollectablesManager.Update(elapsedTime);
                    break;
                case GameStates.dead:
                    PlayerDeathManager.Update(elapsedTime);
                    break;
            }

#endif
            previousKeyboard = currentKeyboard;
            previousGamePad = currentGamePad;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
#if LEVEL_EDITOR
            spriteBatch.Begin();
            levelEditor.Draw(spriteBatch, tilesPerRow, infoBoxHeightPx, editorWidth, editorHeight);
            PlatformsManager.platformsRoomManagers[levelEditor.currentRoomNumber].Draw(spriteBatch);
            MouseState mouseState = Mouse.GetState();
            spriteBatch.DrawString(spriteFont, (mouseState.X / 4 + (int)Camera.position.X) + "," + (mouseState.Y / 4 + (int)Camera.position.Y), new Vector2(10, 10), Color.Blue);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, viewportRectangle, Color.White);
            levelEditor.DrawText(spriteBatch, infoBoxHeightPx);

            spriteBatch.End();
#else
            spriteBatch.Begin();
            Camera.Draw(spriteBatch);
            RoomsManager.Draw(spriteBatch);
            CollectablesManager.Draw(spriteBatch);

            //<debug>
            //spriteBatch.Draw(white, Camera.RelativeRectangle(Player.CollisionRectangle), Color.Green);
            //MouseState mouseState = Mouse.GetState();
            //spriteBatch.DrawString(spriteFont, (mouseState.X / 5 + (int)Camera.position.X) + "," + (mouseState.Y / 5 + (int)Camera.position.Y), new Vector2(10, 10), Color.Blue);
            //spriteBatch.DrawString(spriteFont, Player.position.X + "," + Player.position.Y, new Vector2(10, 10), Color.Blue);
            //spriteBatch.DrawString(spriteFont, Player.healthPoints+"", new Vector2(10, 10), Color.Blue);
            //</debug>

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, viewportRectangle, Color.White);
            spriteBatch.End();
#endif
            base.Draw(gameTime);
        }
    }
}
