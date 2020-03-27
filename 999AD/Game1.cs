using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //SpriteFont sprite;

        public static KeyboardState previusKeyboard;
        public static KeyboardState currentKeyboard;
        public static int screenWidth=960; //resolution
        public static int screenHeight = 540; //resolution


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
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            previusKeyboard = Keyboard.GetState();
            
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
            MapsManager.Inizialize(32, Content.Load<Texture2D>("tiles"));
            CameraManager.Inizialize(new Texture2D[] {Content.Load<Texture2D>("room1"),
                                                    Content.Load<Texture2D>("room2")});
            Player.Inizialize(Content.Load <Texture2D>("player"), new Rectangle(0, 0, 48, 64), 500f);
            //sprite = Content.Load<SpriteFont>("file");
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
            // TODO: Add your update logic here
            RoomsManager.Update(gameTime);
            CameraManager.Update(gameTime);
            Player.Update(gameTime);

            previusKeyboard = currentKeyboard;
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
            //spriteBatch.DrawString(sprite, screenHeight + " " + screenWidth, new Vector2(100, 100), Color.White);
            Camera.Draw(spriteBatch);
            RoomsManager.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
