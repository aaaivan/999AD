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
        static float jumpingSpeed= -800; //improve this later... move to inizialize
        static float maxWallJumpXSpeed = 1000; //improve this later... move to inizialize
        static float wallJumpXSpeed = 0;
        static bool isTouchingTheGround=false;
        static bool isOnTheWall= false;
        static bool isOnMovingPlatform = false;
        static bool canDoubleJump = true;
        static float maxTimeStuckOnwal = 0.2f; //improve this later... move to inizialize
        static float elapsedTimeStuckOnWall=0f;
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
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
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
            velocity.X =0;
        }
        //check input for movement
        static void getPlayerInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.W) && !Game1.previousKeyboard.IsKeyDown(Keys.W))
            {
                if (isTouchingTheGround)
                {
                    velocity.Y = jumpingSpeed;
                    isTouchingTheGround = false;
                    isOnMovingPlatform = false;
                }
                else if(isOnTheWall)
                {
                    velocity.Y = jumpingSpeed*0.7f;
                    elapsedTimeStuckOnWall = 0;
                    if (Game1.previousKeyboard.IsKeyDown(Keys.A))
                        wallJumpXSpeed= maxWallJumpXSpeed;
                    if (Game1.previousKeyboard.IsKeyDown(Keys.D))
                        wallJumpXSpeed= -maxWallJumpXSpeed;
                }
                else if (canDoubleJump)
                {
                    velocity.Y = jumpingSpeed;
                    canDoubleJump = false;
                }
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.D))
            {
                velocity.X += walkingSpeed;
                isFacingRight = true;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= walkingSpeed;
                isFacingRight = false;
            }
        }
        static void Move(float elapsedTime)
        {
            #region MOVE HORIZONTALLY
            velocity.X += wallJumpXSpeed;
            position.X += velocity.X * elapsedTime;
            wallJumpXSpeed *= 0.92f;
            #endregion
            #region CHECK COLLISION WITH SOLID TILES
            int topRow = MathHelper.Clamp(Rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = MathHelper.Clamp((Rectangle.Bottom - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = MathHelper.Clamp(Rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = MathHelper.Clamp((Rectangle.Right - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            isOnTheWall = false;
            //check right-hand side
            if (velocity.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.TileSize - width;
                        if (!isTouchingTheGround && velocity.Y > -10) //improve
                        {
                            isOnTheWall = true;
                            wallJumpXSpeed = 0;
                        }
                        break;
                    }
                }
            }
            //check left-hand side
            else if (velocity.X < 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                    {
                        position.X = (leftCol + 1) * Tile.TileSize;
                        if (!isTouchingTheGround && velocity.Y > -10) //improve
                        {
                            isOnTheWall = true;
                            wallJumpXSpeed = 0;
                        }
                        break;
                    }
                }
            }
            #endregion
            if (!isOnMovingPlatform)
            {
                #region MOVE VERTICALLY
                if (isOnTheWall)
                {
                    if (elapsedTimeStuckOnWall < maxTimeStuckOnwal)
                    {
                        elapsedTimeStuckOnWall += elapsedTime;
                        velocity.Y = 0;
                    }
                    else
                    {
                        velocity.Y = 50; //improve
                    }
                }
                else
                {
                    velocity.Y += Gravity.gravityAcceleration * elapsedTime;
                    if (velocity.Y > 1000) //IMPROVE
                        velocity.Y = 1000; //IMPROVE
                }
                position.Y += velocity.Y * elapsedTime;
                #endregion
                #region CHECK COLLISIONS WITH TILES AND MOVING PLATFORMS
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
                            wallJumpXSpeed = 0;
                            isTouchingTheGround = true;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            elapsedTimeStuckOnWall = 0;
                            break;
                        }
                    }
                    for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms.Count; i++)
                    {
                        RotatingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms[i];
                        if (p.Rectangle.Intersects(Rectangle) &&
                            p.Rectangle.Bottom >= Rectangle.Bottom)
                        {
                            isOnMovingPlatform = true;
                            position.Y = p.Position.Y-height;
                            velocity.Y = 0;
                            wallJumpXSpeed = 0;
                            PlatformsManager.motionType = PlatformsManager.MotionType.rotating;
                            PlatformsManager.platformIndex = i;
                            isTouchingTheGround = true;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            elapsedTimeStuckOnWall = 0;
                            return;
                        }
                    }
                    for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Count; i++)
                    {
                        MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i];
                        if (p.Rectangle.Intersects(Rectangle) &&
                            p.Rectangle.Bottom >= Rectangle.Bottom)
                        {
                            isOnMovingPlatform = true;
                            position.Y = p.Position.Y - height;
                            velocity.Y = 0;
                            wallJumpXSpeed = 0;
                            PlatformsManager.motionType = PlatformsManager.MotionType.backAndForth;
                            PlatformsManager.platformIndex = i;
                            isTouchingTheGround = true;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            elapsedTimeStuckOnWall = 0;
                            return;
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
                            position.Y = (topRow + 1) * Tile.TileSize;
                            velocity.Y = 0;
                            break;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region MOVE WITH PLATFORM
                if (PlatformsManager.motionType == PlatformsManager.MotionType.rotating)
                {
                    RotatingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms[PlatformsManager.platformIndex];
                    position += p.Shift;
                    if (position.X - p.Position.X <= -width || position.X - p.Position.X >= p.width)
                        isOnMovingPlatform = false;
                }
                else
                {
                    MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex];
                    position += p.Shift;
                    if (position.X - p.Position.X <= -width || position.X - p.Position.X >= p.width)
                        isOnMovingPlatform = false;
                }
                #endregion
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.DrawRectangle(Rectangle), currentAnimation.Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
