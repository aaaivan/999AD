//#define LEVEL_EDITOR
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
            titleScreen, controls, credits, intro, playing, pause,  dead, ending, quit, confirmQuit, doubleJump, wallJump,total
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static readonly int min_gameWidth = 1920/5; //pixels on the x axis
        public static readonly int min_gameHeight = 1080 / 5; //pixels on the y axis
        public static  int gameWidth; //pixels on the x axis
        public static int gameHeight; //pixels on the y axis
        public static Rectangle viewportRectangle;
        static RenderTarget2D nativeRenderTarget;
        static RenderTarget2D renderTarget_zoom1;
        static RenderTarget2D renderTarger_zoom0dot5;
        public static int scale;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static KeyboardState previousKeyboard;
        public static KeyboardState currentKeyboard;
        public static GamePadState previousGamePad;
        public static GamePadState currentGamePad;
        public static GameStates currentGameState;

        bool gameInitialized;
        //<debug>
        SpriteFont spriteFont;
        Texture2D white;
        //</debug>
#if LEVEL_EDITOR
        LevelEditor levelEditor;
        public static int editorWidth;
        public static int editorHeight;
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
            currentMouseState = Mouse.GetState();
            previousMouseState = currentMouseState;
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
            previousMouseState = Mouse.GetState();
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
                    Content.Load<Texture2D>(@"backgrounds\tutorial0"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial1"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial2"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial3"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial4"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower0"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower1"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower2"),
                    Content.Load<Texture2D>(@"backgrounds\midBoss"),
                    Content.Load<Texture2D>(@"backgrounds\groundFloor"),
                    Content.Load<Texture2D>(@"backgrounds\altarRoom"),
                    Content.Load<Texture2D>(@"backgrounds\firstFloor"),
                    Content.Load<Texture2D>(@"backgrounds\secondFloor"),
                    Content.Load<Texture2D>(@"backgrounds\descent"),
                    Content.Load<Texture2D>(@"backgrounds\finalBoss"),
                    Content.Load<Texture2D>(@"backgrounds\escape0"),
                    Content.Load<Texture2D>(@"backgrounds\escape1"),
                    Content.Load<Texture2D>(@"backgrounds\escape2"),
                }
            );
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            levelEditor = new LevelEditor(Content.Load<SpriteFont>(@"fonts\arial32"),
                                          Content.Load<SpriteFont>(@"fonts\arial14"),
                                          Content.Load<Texture2D>("whiteTile"));
#else
            currentGameState = GameStates.titleScreen;
            MapsManager.Inizialize(Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize
            (
                new Texture2D[(int)RoomsManager.Rooms.total]
                {
                    Content.Load<Texture2D>(@"backgrounds\tutorial0"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial1"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial2"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial3"),
                    Content.Load<Texture2D>(@"backgrounds\tutorial4"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower0"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower1"),
                    Content.Load<Texture2D>(@"backgrounds\bellTower2"),
                    Content.Load<Texture2D>(@"backgrounds\midBoss"),
                    Content.Load<Texture2D>(@"backgrounds\groundFloor"),
                    Content.Load<Texture2D>(@"backgrounds\altarRoom"),
                    Content.Load<Texture2D>(@"backgrounds\firstFloor"),
                    Content.Load<Texture2D>(@"backgrounds\secondFloor"),
                    Content.Load<Texture2D>(@"backgrounds\descent"),
                    Content.Load<Texture2D>(@"backgrounds\finalBoss"),
                    Content.Load<Texture2D>(@"backgrounds\escape0"),
                    Content.Load<Texture2D>(@"backgrounds\escape1"),
                    Content.Load<Texture2D>(@"backgrounds\escape2"),
                }
            );
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platforms"));
            ProjectilesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            Player.Inizialize(Content.Load <Texture2D>(@"characters\player"), new Vector2(16,185));
            RoomsManager.Inizialize();
            GameEvents.Inizialize();
            FireBallsManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            LavaGeyserManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            EnemyManager.Initialise(Content.Load<Texture2D>(@"characters\enemy1"), Content.Load<Texture2D>(@"characters\enemy2"));
            MidBoss.Initialise(Content.Load<Texture2D>(@"characters\midboss"));
            FinalBoss.Inizialize(Content.Load<Texture2D>(@"characters\finalBoss"),
                                 new Texture2D[] { Content.Load<Texture2D>(@"characters\stoneWing"),
                                                   Content.Load<Texture2D>(@"characters\healthyWing"),
                                                   Content.Load<Texture2D>(@"characters\damagedWing"),
                                                   Content.Load<Texture2D>(@"characters\deadWing")});
            CollectablesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            MonologuesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"),
                                         Content.Load<SpriteFont>(@"fonts\monologue"));
            DoorsManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            AnimatedSpritesManager.Inizialize(Content.Load<Texture2D>("animatedSprites"));
            PlayerDeathManager.Initialize(Content.Load<Texture2D>(@"menus\deathScreen"),
                                          Content.Load<Texture2D>(@"menus\menuOptions"));
            MenusManager.Initialize(Content.Load<Texture2D>(@"menus\menuOptions"),
                new Texture2D[]
                {
                    Content.Load<Texture2D>(@"menus\titleScreen"),
                    Content.Load<Texture2D>(@"menus\controls"),
                    Content.Load<Texture2D>(@"menus\credits"),
                    Content.Load<Texture2D>(@"menus\pause"),
                    Content.Load<Texture2D>(@"menus\quit"),
                    Content.Load<Texture2D>(@"menus\doubleJump"),
                    Content.Load<Texture2D>(@"menus\wallJump"),

                });
            CutscenesManager.Initialize(Content.Load<Texture2D>(@"characters\enemy1"),
                                        Content.Load<Texture2D>(@"characters\player"),
                                        Content.Load<SpriteFont>(@"fonts\monologue"));
            SoundEffects.Initialise
                (
                //Player Sound Effects
                Content.Load<SoundEffect>(@"sounds\pJump"),
                Content.Load<SoundEffect>(@"sounds\pShoot"),
                Content.Load<SoundEffect>(@"sounds\pHurt"),
                Content.Load<SoundEffect>(@"sounds\pickup"),

                //Enemy Sound Effects
                Content.Load<SoundEffect>(@"sounds\enemyAttack"),
                Content.Load<SoundEffect>(@"sounds\enemyHurt"),
                Content.Load<SoundEffect>(@"sounds\e2Attack"),

                //Midboss Sound Effects
                Content.Load<SoundEffect>(@"sounds\midMove"),
                Content.Load<SoundEffect>(@"sounds\midAttack"),
                Content.Load<SoundEffect>(@"sounds\midHurt"),

                Content.Load<SoundEffect>(@"sounds\finAttack"),
                Content.Load<SoundEffect>(@"sounds\finHurt"),
                Content.Load<SoundEffect>(@"sounds\finAwaken"),
                Content.Load<SoundEffect>(@"sounds\finRecover"),
                Content.Load<Song>(@"sounds\finalBossMusic")
                );
            gameInitialized=true;
            Zoom1();
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
            currentMouseState = Mouse.GetState();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here
#if LEVEL_EDITOR
            levelEditor.Update(currentMouseState, previousMouseState, tilesPerRow, infoBoxHeightPx);
            CameraManager.Update(elapsedTime);
            PlatformsManager.platformsRoomManagers[levelEditor.currentRoomNumber].Update(elapsedTime);
#else
            switch(currentGameState)
            {
                case GameStates.titleScreen:
                    if (!gameInitialized)
                        LoadContent();
                    MenusManager.menus[(int)MenusManager.MenuType.titleScreen].Update();
                    break;
                case GameStates.controls:
                    MenusManager.menus[(int)MenusManager.MenuType.controls].Update();
                    break;
                case GameStates.credits:
                    MenusManager.menus[(int)MenusManager.MenuType.credits].Update();
                    break;
                case GameStates.intro:
                    if (CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.intro].active)
                        CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.intro].Update(elapsedTime);
                    else
                        currentGameState = GameStates.playing;
                    break;
                case GameStates.playing:
                    if ((currentKeyboard.IsKeyDown(Keys.P) && !previousKeyboard.IsKeyDown(Keys.P)) ||
                        (currentKeyboard.IsKeyDown(Keys.M) && !previousKeyboard.IsKeyDown(Keys.M)) ||
                        (currentGamePad.Buttons.Start== ButtonState.Pressed && previousGamePad.Buttons.Start == ButtonState.Released))
                    {
                        currentGameState = GameStates.pause;
                        break;
                    }
                    RoomsManager.Update(elapsedTime);
                    Player.Update(elapsedTime);
                    ProjectilesManager.Update(elapsedTime);
                    GameEvents.Update(elapsedTime);
                    Collisions.Update(elapsedTime);
                    CollectablesManager.Update(elapsedTime);
                    break;
                case GameStates.pause:
                    if ((currentKeyboard.IsKeyDown(Keys.P) && !previousKeyboard.IsKeyDown(Keys.P)) ||
                        (currentKeyboard.IsKeyDown(Keys.M) && !previousKeyboard.IsKeyDown(Keys.M)) ||
                        (currentGamePad.Buttons.Start == ButtonState.Pressed && previousGamePad.Buttons.Start == ButtonState.Released))
                    {
                        MenusManager.menus[(int)MenusManager.MenuType.pause].Reset();
                        currentGameState = GameStates.playing;
                        break;
                    }
                    MenusManager.menus[(int)MenusManager.MenuType.pause].Update();
                    break;
                case GameStates.confirmQuit:
                    MenusManager.menus[(int)MenusManager.MenuType.confirmQuit].Update();
                    if (currentGameState == GameStates.titleScreen)
                        gameInitialized = false;
                    break;
                case GameStates.dead:
                    PlayerDeathManager.Update(elapsedTime);
                    break;
                case GameStates.ending:
                    if (!CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.ending].active)
                    {
                        currentGameState = GameStates.credits;
                        gameInitialized = false;
                    }
                    CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.ending].Update(elapsedTime);
                    break;
                case GameStates.quit:
                    Exit();
                    break;
                case GameStates.doubleJump:
                    MenusManager.menus[(int)MenusManager.MenuType.doubleJump].Update();
                    break;
                case GameStates.wallJump:
                    MenusManager.menus[(int)MenusManager.MenuType.wallJump].Update();
                    break;
            }

#endif
            previousKeyboard = currentKeyboard;
            previousGamePad = currentGamePad;
            previousMouseState = currentMouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
#if LEVEL_EDITOR
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(Color.Black);
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
            switch(currentGameState)
            {
                case GameStates.titleScreen:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.titleScreen].Draw(spriteBatch);
                    break;
                case GameStates.controls:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.controls].Draw(spriteBatch);
                    break;
                case GameStates.credits:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.credits].Draw(spriteBatch);
                    break;
                case GameStates.intro:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.intro].Draw(spriteBatch);
                    break;
                case GameStates.playing:
                    GraphicsDevice.SetRenderTarget(nativeRenderTarget);
                    GraphicsDevice.Clear(Color.Black);
                    Camera.Draw(spriteBatch);
                    RoomsManager.Draw(spriteBatch);
                    CollectablesManager.Draw(spriteBatch);
                    Player.DrawGUI(spriteBatch);
                    break;
                case GameStates.pause:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.pause].Draw(spriteBatch);
                    break;
                case GameStates.dead:
                    GraphicsDevice.SetRenderTarget(nativeRenderTarget);
                    GraphicsDevice.Clear(Color.Black);
                    Camera.Draw(spriteBatch);
                    RoomsManager.Draw(spriteBatch);
                    CollectablesManager.Draw(spriteBatch);
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    PlayerDeathManager.Draw(spriteBatch);
                    break;
                case GameStates.ending:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    CutscenesManager.cutscenes[(int)CutscenesManager.CutsceneType.ending].Draw(spriteBatch);
                    break;
                case GameStates.confirmQuit:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.confirmQuit].Draw(spriteBatch);
                    break;
                case GameStates.doubleJump:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.doubleJump].Draw(spriteBatch);
                    break;
                case GameStates.wallJump:
                    GraphicsDevice.SetRenderTarget(renderTarget_zoom1);
                    GraphicsDevice.Clear(Color.Black);
                    MenusManager.menus[(int)MenusManager.MenuType.wallJump].Draw(spriteBatch);
                    break;
            }

            //<debug>
            //spriteBatch.Draw(white, Camera.RelativeRectangle(Player.CollisionRectangle), Color.Green);
            //MouseState mouseState = Mouse.GetState();
            //spriteBatch.DrawString(spriteFont, (currentMouseState.X / 5 + (int)Camera.position.X) + "," + (currentMouseState.Y / 5 + (int)Camera.position.Y), new Vector2(10, 10), Color.Blue);
            //spriteBatch.DrawString(spriteFont, Player.position.X + "," + Player.position.Y, new Vector2(10, 20), Color.Blue);
            //spriteBatch.DrawString(spriteFont, Player.healthPoints+"", new Vector2(10, 10), Color.Blue);
            //</debug>

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (currentGameState == GameStates.playing)
                spriteBatch.Draw(nativeRenderTarget, viewportRectangle, Color.White);
            else if (currentGameState == GameStates.dead)
            {
                spriteBatch.Draw(nativeRenderTarget, viewportRectangle, Color.White);
                spriteBatch.Draw(renderTarget_zoom1, viewportRectangle, Color.White);
            }
            else
                spriteBatch.Draw(renderTarget_zoom1, viewportRectangle, Color.White);

            spriteBatch.End();
#endif
            base.Draw(gameTime);
        }
    }
}
