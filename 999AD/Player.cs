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
        enum AnimationTypes
        {
            idle, walk, jump, fall, attack, push, interact, total
        }
        #region DECLARATIONS
        static Texture2D spritesheet;
        static List<Animation> animations= new List<Animation>();
        static AnimationTypes currentAnimation;
        public static Vector2 position;
        public static readonly int height= 64;
        public static readonly int width= 48;
        static Vector2 velocity= Vector2.Zero;
        static bool isFacingRight=true;
        public static readonly float walkingSpeed= 300; //movement speed
        public static readonly float jumpingSpeed= -800; //jumping initial vertical spped
        public static readonly float maxWallJumpXSpeed = 1000; //wall jump initial horizontal speed
        static float wallJumpXSpeed = 0;
        static bool isTouchingTheGround=false;
        static bool isOnTheWall= false;
        static bool isOnMovingPlatform = false;
        static bool canDoubleJump = true;
        public static readonly float maxTimeStuckOnwal = 0.2f;
        static float elapsedTimeStuckOnWall=0f;
        public static readonly Vector2 projectileInitialVelocity = new Vector2(500, -300);
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet, Vector2 _position)
        {
            spritesheet = _spritesheet;
            position = _position;
            //fill following assignments with sprite info
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 96*3, 128), 96, 128, 3, 0.3f, true));
            animations.Add( new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false, true));
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation(spritesheet, new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false));
            currentAnimation = AnimationTypes.idle;
        }
        #endregion
        #region PROPERTIES
        //return the senter of the player's rectangle
        public static Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
        public static Vector2 Center
        {
            get { return new Vector2(position.X+width/2f, position.Y + height / 2f); }
        }
        static Vector2 ProjectileInitialVelocity
        {
            get
            {
                return ( new Vector2((isFacingRight ? projectileInitialVelocity.X : -projectileInitialVelocity.X)+velocity.X,
                                    projectileInitialVelocity.Y+ 0.5f*velocity.Y)
                        );
            }
        }
        #endregion
        #region METHODS
        public static void Update(GameTime gameTime)
        {
            getPlayerInput();
            Move((float)gameTime.ElapsedGameTime.TotalSeconds);
            animations[(int)currentAnimation].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
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
            if (Game1.currentKeyboard.IsKeyDown(Keys.Space) && !Game1.previousKeyboard.IsKeyDown(Keys.Space))
            {
                ProjectilesManager.Shoot(isFacingRight ? (position + new Vector2(width, 0)) : position, ProjectileInitialVelocity);
            }
        }
        static void Move(float elapsedTime)
        {
            Rectangle rectangle = Rectangle;
            #region MOVE HORIZONTALLY
            velocity.X += wallJumpXSpeed;
            position.X += velocity.X * elapsedTime;
            wallJumpXSpeed *= 0.92f;
            #endregion
            #region CHECK COLLISION WITH SOLID TILES
            int topRow = MathHelper.Clamp(rectangle.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = MathHelper.Clamp((rectangle.Bottom - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = MathHelper.Clamp(rectangle.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = MathHelper.Clamp((rectangle.Right - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            isOnTheWall = false;
            //check right-hand side
            if (velocity.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.tileSize - width;
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
                        position.X = (leftCol + 1) * Tile.tileSize;
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
                topRow = MathHelper.Clamp(rectangle.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
                btmRow = MathHelper.Clamp((rectangle.Bottom - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
                leftCol = MathHelper.Clamp(rectangle.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
                rightCol = MathHelper.Clamp((rectangle.Right - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
                //check bottom
                if (velocity.Y > 0)
                {
                    for (int col = leftCol; col <= rightCol; col++)
                    {
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[btmRow, col].isSolid())
                        {
                            position.Y = btmRow * Tile.tileSize - height;
                            velocity.Y = 0;
                            wallJumpXSpeed = 0;
                            isTouchingTheGround = true;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            elapsedTimeStuckOnWall = 0;
                            break;
                        }
                    }
                    for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms.Length; i++)
                    {
                        MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms[i];
                        if (p.Rectangle.Intersects(rectangle) &&
                            p.Rectangle.Bottom >= rectangle.Bottom)
                        {
                            isOnMovingPlatform = true;
                            position.Y = p.Position.Y-height;
                            velocity.Y = 0;
                            wallJumpXSpeed = 0;
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
                            position.Y = (topRow + 1) * Tile.tileSize;
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
                MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms[PlatformsManager.platformIndex];
                position += p.Shift;
                if (position.X - p.Position.X <= -width || position.X - p.Position.X >= p.width)
                    isOnMovingPlatform = false;
                #endregion
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(Rectangle) ,animations[(int)currentAnimation].Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
