using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    static class Player
    {
        #region DECLARATIONS
        static Texture2D spritesheet;
        static Animation idle, walk, jump, fall, attack, push, interact;
        static Animation currentAnimation;
        public static Rectangle rectangle; //rectangle surrounding the player
        static bool isFacingRight=true;
        static float speed; //movement speed
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet, Rectangle _rectangle, float _speed)
        {
            spritesheet = _spritesheet;
            rectangle = _rectangle;
            //fill following assignments with sprite info
            idle = new Animation(spritesheet, new Rectangle(0, 0, 96, 128), 96, 128, 1, 1f, true);
            walk = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            jump = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false, true);
            fall = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            attack = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            push = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            interact = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false);
            currentAnimation = idle;
            speed = _speed;
        }
        #endregion
        #region PROPERTIES
        public static Point Center
        {
            get { return rectangle.Center; }
        }
        #endregion
        #region METHODS
        public static void Update(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            if (Game1.currentKeyboard.IsKeyDown(Keys.A))
            {
                rectangle.X -= (int)(speed * elapsedTime);
                isFacingRight = false;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.W))
                rectangle.Y -= (int)(speed * elapsedTime);
            if (Game1.currentKeyboard.IsKeyDown(Keys.D))
            {
                rectangle.X += (int)(speed * elapsedTime);
                isFacingRight = true;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.S))
                rectangle.Y += (int)(speed * elapsedTime);
        }
        static void Move(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Seconds;
            if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                rectangle.X -= (int)(speed * elapsedTime);
            if (Game1.currentKeyboard.IsKeyDown(Keys.W))
                rectangle.Y -= (int)(speed * elapsedTime);
            if (Game1.currentKeyboard.IsKeyDown(Keys.D))
                rectangle.X += (int)(speed * elapsedTime);
            if (Game1.currentKeyboard.IsKeyDown(Keys.S))
                rectangle.Y += (int)(speed * elapsedTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.DrawRectangle(rectangle), currentAnimation.Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
