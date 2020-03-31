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
        public static Vector2 position;
        public static int height;
        public static int width; 
        static Vector2 velocity= new Vector2(0,0);
        static bool isFacingRight=true;
        static float walkingSpeed; //movement speed
        static float jumpingSpeed= -600; //improve this later... move to inizialize
        static bool isTouchingTheGround=false;
        static bool isOnTheWall= false;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet, Vector2 _position, int _width, int _height, float _walkingSpeed)
        {
            spritesheet = _spritesheet;
            position = _position;
            width = _width;
            height = _height;
            //fill following assignments with sprite info
            idle = new Animation(spritesheet, new Rectangle(0, 0, 96, 128), 96, 128, 1, 1f, true);
            walk = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            jump = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false, true);
            fall = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            attack = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            push = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true);
            interact = new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false);
            currentAnimation = idle;
            walkingSpeed = _walkingSpeed;
        }
        #endregion
        #region PROPERTIES
        //return the senter of the player's rectangle
        public static Rectangle Rectangle
        {
            get { return new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), width, height); }
        }
        public static Point Center
        {
            get { return Rectangle.Center; }
        }
        #endregion
        #region METHODS
        public static void Update(GameTime gameTime)
        {
            getPlayerInput();
            Move((float)gameTime.ElapsedGameTime.TotalSeconds);
            velocity.X = 0;
        }
        //check input for movement
        static void getPlayerInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.W))
                velocity.Y=jumpingSpeed;
            if (Game1.currentKeyboard.IsKeyDown(Keys.D))
            {
                velocity.X = walkingSpeed;
                isFacingRight = true;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.A))
            {
                velocity.X = -walkingSpeed;
                isFacingRight = false;
            }
        }
        static void Move(float elapsedTime)
        {
            position.X += velocity.X * elapsedTime;
            isOnTheWall = false;
            int topRow = MathHelper.Clamp(Rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = MathHelper.Clamp((Rectangle.Bottom-1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = MathHelper.Clamp(Rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = MathHelper.Clamp((Rectangle.Right-1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check right-hand side
            if (velocity.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.TileSize - width;
                        if (!isTouchingTheGround)
                        {
                            velocity.Y = 100;
                            isOnTheWall = true;
                        }
                        break;
                    }
                }
            }
            //check left-hand side
            else if(velocity.X < 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                    {
                        position.X = (leftCol + 1) * Tile.TileSize;
                        if (!isTouchingTheGround)
                        {
                            velocity.Y = 100;
                            isOnTheWall = true;
                        }
                        break;
                    }
                }
            }
            velocity.Y += Gravity.gravityAcceleration * elapsedTime;
            if (velocity.Y > 250f)
                velocity.Y = 250f;
            position.Y += velocity.Y*elapsedTime;
            topRow = MathHelper.Clamp(Rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = MathHelper.Clamp((Rectangle.Bottom - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = MathHelper.Clamp(Rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = MathHelper.Clamp((Rectangle.Right - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check bottom
            if (velocity.Y > 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[btmRow, col].isSolid())
                    {
                        position.Y = btmRow * Tile.TileSize - height;
                        velocity.Y = 0;
                        isOnTheWall = false;
                        break;
                    }
                }
            }
            //check top
            else if (velocity.Y < 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[topRow, col].isSolid())
                    {
                        position.Y = (topRow+1) * Tile.TileSize;
                        velocity.Y = 0;
                        break;
                    }
                }
            }
            velocity.Y += Gravity.gravityAcceleration * elapsedTime;
            if (velocity.Y > 250f)
                velocity.Y = 250f;
            position.Y += velocity.Y * elapsedTime;
            topRow = MathHelper.Clamp(Rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = MathHelper.Clamp((Rectangle.Bottom - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = MathHelper.Clamp(Rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = MathHelper.Clamp((Rectangle.Right - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.DrawRectangle(Rectangle), currentAnimation.Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
