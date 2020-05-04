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
            idle, walk, jump, attck, fall, hurt, total
        }
        #region DECLARATIONS
        static Texture2D spritesheet;
        static List<Animation> animations= new List<Animation>();
        static AnimationTypes currentAnimation;
        public static Vector2 position;
        public static readonly int height= 22; //refer to player rectangle not to the sprite size
        public static readonly int width= 12; //refer to player rectangle not to the sprite size
        static Vector2 jumpSpeed= Vector2.Zero;
        static bool isFacingRight=true;
        public static readonly float maxWalkingSpeed= 100; //movement speed
        public static readonly Vector2 initialJumpSpeed= new Vector2(190, -400); //jumping initial speed
        public static float walkingSpeed= 0;
        static bool isTouchingTheGround=false;
        static bool isOnTheWall= false;
        static bool isOnMovingPlatform = false;
        static bool canDoubleJump = true;
        static bool isWallJumping = false;
        public static readonly float maxTimeStuckOnwal = 0.2f;
        static float elapsedTimeStuckOnWall=0f;
        public static readonly Vector2 projectileInitialVelocity = new Vector2(500, -100);
        public static readonly float timeBetweenShots = 0.4f; //minimum time between shots
        static float elapsedShotTime = 0f;
        public static readonly int maxHealthPoints = 3;
        public static int healthPoints = 3;
        static int deaths = 0;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet, Vector2 _position)
        {
            spritesheet = _spritesheet;
            position = _position;
            //fill following statements with sprite info
            animations.Add(new Animation(new Rectangle(0, 0, 128, 24), 16, 24, 8, 0.3f, true));
            animations.Add( new Animation( new Rectangle(0, 24, 160,24), 16, 24, 10, 0.06f, true));
            animations.Add(new Animation( new Rectangle(0, 48, 80, 24), 16, 24, 5, 0f, false, true));
            animations.Add(new Animation( new Rectangle(0, 72, 112, 24), 16, 24, 7, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 96, 64, 24), 16, 24, 4, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 120, 96, 24), 16, 24, 6, 0f, true));
            currentAnimation = AnimationTypes.walk;
        }
        #endregion
        #region PROPERTIES
        //return the player's collision rectangle
        public static Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height-2); }//-2 magic number: improve?
        }
        //return the center of the player's rectangle
        public static Vector2 Center
        {
            get { return new Vector2(position.X+width/2f, position.Y + height / 2f); }
        }
        public static Vector2 JumpSpeed
        {
            get { return jumpSpeed; }
        }
        //return the initial velocity of the projectile based on the direction the player is facing
        static Vector2 ProjectileInitialVelocity
        {
            get
            {
                return ( new Vector2((isFacingRight ? projectileInitialVelocity.X : -projectileInitialVelocity.X)+jumpSpeed.X,
                                    projectileInitialVelocity.Y+ 0.5f*jumpSpeed.Y)
                        );
            }
        }
        public static bool IsOnMovingPlatform
        {
            get { return isOnMovingPlatform; }
        }
        #endregion
        #region METHODS
        public static void Update(float elapsedTime)
        {
            getPlayerInput(elapsedTime);
            Move(elapsedTime);
            animations[(int)currentAnimation].Update(elapsedTime);
        }
        //check input for movement
        static void getPlayerInput(float elapsedTime)
        {
            currentAnimation = AnimationTypes.idle;
            if (Game1.currentKeyboard.IsKeyDown(Keys.W) && !Game1.previousKeyboard.IsKeyDown(Keys.W))
            {
                if (isTouchingTheGround)
                {
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    if (Game1.currentKeyboard.IsKeyDown(Keys.D))
                        jumpSpeed.X = initialJumpSpeed.X;
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                        jumpSpeed.X = -initialJumpSpeed.X;
                    isTouchingTheGround = false;
                    isOnMovingPlatform = false;
                    return;
                }
                else if (isOnTheWall)
                {
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    elapsedTimeStuckOnWall = 0;
                    canDoubleJump = false;
                    isOnTheWall = false;
                    isWallJumping = true;
                    if (!isFacingRight)
                        jumpSpeed.X = initialJumpSpeed.X;
                    else
                        jumpSpeed.X = -initialJumpSpeed.X;
                    isFacingRight = !isFacingRight;
                }
                else if (canDoubleJump)
                {
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    canDoubleJump = false;
                    return;
                }
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.D))
            {
                walkingSpeed = maxWalkingSpeed;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.A))
            {
                walkingSpeed =- maxWalkingSpeed;
            }
            if (elapsedShotTime > timeBetweenShots)
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Space) && !Game1.previousKeyboard.IsKeyDown(Keys.Space))
                {
                    ProjectilesManager.ShootPlayerProjectile(isFacingRight ? (position + new Vector2(width, 0)) : position, ProjectileInitialVelocity);
                    elapsedShotTime = 0;
                }
            }
            else
                elapsedShotTime += elapsedTime;
        }
        //move player based on his current velocity and check for collisions
        static void Move(float elapsedTime)
        {
            Vector2 totalSpeed = new Vector2();
            #region CHECK IF IS STILL ON THE WALL
            int topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = (int)MathHelper.Clamp((position.Y + height - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = (int)MathHelper.Clamp((position.X + width - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            if (isOnTheWall)
            {
                isOnTheWall = false;
                if (isFacingRight)
                {
                    for (int row = topRow; row <= btmRow; row++)
                    {
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol + 1].isSolid())
                            isOnTheWall = true;
                    }
                }
                else
                {
                    for (int row = topRow; row <= btmRow; row++)
                    {
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol - 1].isSolid())
                            isOnTheWall = true;
                    }
                }
            }
            #endregion
            #region CALCULATE VELOCITIES
            if (isWallJumping)
            {
                if (-2*Math.Abs(jumpSpeed.X)< maxWalkingSpeed* Math.Sign(jumpSpeed.X) &&
                    maxWalkingSpeed * Math.Sign(jumpSpeed.X)< Math.Abs(jumpSpeed.X))
                    totalSpeed.X = jumpSpeed.X;
                else
                {
                    if (Math.Sign(jumpSpeed.X) == -Math.Sign(walkingSpeed))
                    {
                        isWallJumping = false;
                        jumpSpeed.X = 0;
                        totalSpeed.X = walkingSpeed;
                    }
                    else
                    {
                        totalSpeed.X = jumpSpeed.X+walkingSpeed;
                        totalSpeed.X = MathHelper.Clamp(totalSpeed.X, -maxWalkingSpeed, maxWalkingSpeed);
                    }
                }
                jumpSpeed.X -= 2f * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//2 is a magic number
            }
            else if (!isTouchingTheGround)
            {
                if (Math.Sign(jumpSpeed.X) == -Math.Sign(walkingSpeed))
                    totalSpeed.X = walkingSpeed;
                else if (Math.Abs(jumpSpeed.X) > maxWalkingSpeed)
                {
                    totalSpeed.X = jumpSpeed.X;
                    jumpSpeed.X -= 2 * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//2 is a magic number
                }
                else
                {
                    totalSpeed.X = jumpSpeed.X+walkingSpeed;
                    totalSpeed.X = MathHelper.Clamp(totalSpeed.X, -maxWalkingSpeed, maxWalkingSpeed);
                    jumpSpeed.X -= 5 * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//5 is a magic number
                }
            }
            else
            {
                totalSpeed.X = walkingSpeed;
            }
            walkingSpeed = 0;
            if (totalSpeed.X > 0)
                isFacingRight = true;
            else if (totalSpeed.X < 0)
                isFacingRight = false;
            if (!isOnMovingPlatform)
            {
                if (isOnTheWall)
                {
                    if (elapsedTimeStuckOnWall < maxTimeStuckOnwal)
                    {
                        elapsedTimeStuckOnWall += elapsedTime;
                        jumpSpeed.Y = 0;
                    }
                    else if (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.wallFrictionCoeff > 0)
                    {
                        jumpSpeed.Y += (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.wallFrictionCoeff) * elapsedTime;
                    }
                }
                else
                {
                    if (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.airFrictionCoeff > 0)
                        jumpSpeed.Y += (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.airFrictionCoeff) * elapsedTime;
                }
                if (jumpSpeed.Y > Gravity.gravityAcceleration * elapsedTime * 20)
                {
                    isTouchingTheGround = false;
                    isOnMovingPlatform = false;
                }
                totalSpeed.Y = jumpSpeed.Y;
            }
            else
            {
                totalSpeed += PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex].Shift / elapsedTime;
            }
            #endregion
            position.X += totalSpeed.X * elapsedTime;
            #region CHECK COLLISION WITH SOLID TILES
            topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = (int)MathHelper.Clamp((position.Y + height - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X + width - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check right-hand side
            if (totalSpeed.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.tileSize - width;
                        if (!isTouchingTheGround && totalSpeed.Y > -10) //improve
                        {
                            isOnTheWall = true;
                            isWallJumping = false;
                            jumpSpeed.X = 0;
                        }
                        break;
                    }
                }
            }
            //check left-hand side
            else if (totalSpeed.X < 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                    {
                        position.X = (leftCol + 1) * Tile.tileSize;
                        if (!isTouchingTheGround && totalSpeed.Y > -10) //improve
                        {
                            isOnTheWall = true;
                            isWallJumping = false;
                            jumpSpeed.X = 0;
                        }
                        break;
                    }
                }
            }
            #endregion
            position.Y += totalSpeed.Y * elapsedTime;
            #region CHECK COLLISIONS WITH TILES
            topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = (int)MathHelper.Clamp((position.Y + height - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X + width - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check bottom
            if (totalSpeed.Y > 0)
            {
                int span= ((int)(totalSpeed.Y*elapsedTime+Tile.tileSize)/ Tile.tileSize);
                for (int i = span - 1; i >= 0; i--)
                {
                    for (int col = leftCol; col <= rightCol; col++)
                    {
                        if (btmRow - i < 0)
                            break;
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[btmRow-i, col].isSolid())
                        {
                            position.Y = (btmRow-i) * Tile.tileSize - height;
                            jumpSpeed.Y = 0;
                            jumpSpeed.X = 0;
                            isTouchingTheGround = true;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            isWallJumping = false;
                            elapsedTimeStuckOnWall = 0;
                            i = 0;
                            break;
                        }
                    }
                }
            }
            //check top
            else if (totalSpeed.Y < 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[topRow, col].isSolid())
                    {
                        position.Y = (topRow + 1) * Tile.tileSize;
                        jumpSpeed.Y = 0;
                        break;
                    }
                }
            }
            #endregion
            #region CHECK COLLISION WITH MOVING PLATFORM
            if (!isOnMovingPlatform)
            {
                for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length; i++)
                {
                    MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i];
                    if (position.X+width>p.Position.X &&
                        position.X<p.Position.X+p.width &&
                        position.Y+height- p.Position.Y>0 &&
                        position.Y + height - p.Position.Y < totalSpeed.Y*elapsedTime - p.Shift.Y)
                    {
                        isOnMovingPlatform = true;
                        position.Y = p.Position.Y - height + 1;
                        jumpSpeed.Y = 0;
                        jumpSpeed.X = 0;
                        PlatformsManager.platformIndex = i;
                        isTouchingTheGround = true;
                        canDoubleJump = true;
                        isOnTheWall = false;
                        isWallJumping = false;
                        elapsedTimeStuckOnWall = 0;
                        return;
                    }
                }
            }
            else
            {
                #region MOVE WITH PLATFORM
                MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex];
                if (position.X - p.Position.X <= -width || position.X - p.Position.X >= p.width)
                    isOnMovingPlatform = false;
                #endregion
            }
            #endregion
        }
        public static void Rebound(float ratioReboundToNormalJump,bool right)
        {
            jumpSpeed.Y = initialJumpSpeed.Y * ratioReboundToNormalJump;
            jumpSpeed.X = right ? initialJumpSpeed.X : -initialJumpSpeed.X;
            canDoubleJump = true;
        }
        public static void takeDamage(int damage=1)
        {
            healthPoints -= damage;
            if (healthPoints<=0)
            {
                //do something
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(new Vector2(Center.X - animations[(int)currentAnimation].Frame.Width / 2,
                                                     position.Y - animations[(int)currentAnimation].Frame.Height + height),
                                         animations[(int)currentAnimation].Frame.Width,
                                         animations[(int)currentAnimation].Frame.Height),
                animations[(int)currentAnimation].Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
