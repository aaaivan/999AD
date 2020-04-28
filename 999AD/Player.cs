﻿using System;
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
            idle, walk, jump, fall, attack, die, total
        }
        #region DECLARATIONS
        static Texture2D spritesheet;
        static List<Animation> animations= new List<Animation>();
        static AnimationTypes currentAnimation;
        public static Vector2 position;
        public static readonly int height= 24;
        public static readonly int width= 16;
        static Vector2 velocity= Vector2.Zero;
        static bool isFacingRight=true;
        public static readonly float walkingSpeed= 100; //movement speed
        public static readonly float jumpingSpeed= -500; //jumping initial vertical speed
        public static readonly float maxWallJumpXSpeed = 300; //wall jump initial horizontal speed
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
            animations.Add(new Animation(new Rectangle(0, 0, 16*3, 24), 16, 24, 3, 0.3f, true));
            animations.Add( new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false, true));
            animations.Add(new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, true));
            animations.Add(new Animation( new Rectangle(0, 0, 0, 0), 0, 0, 0, 0f, false));
            currentAnimation = AnimationTypes.idle;
        }
        #endregion
        #region PROPERTIES
        //return the player's collision rectangle
        public static Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
        //return the player's draw rectangle
        public static Rectangle DrawRectangle
        {
            get { return new Rectangle((int)(Center.X-animations[(int)currentAnimation].Frame.Width/2),
                                       (int)position.Y - animations[(int)currentAnimation].Frame.Height+height,
                                       animations[(int)currentAnimation].Frame.Width,
                                       animations[(int)currentAnimation].Frame.Height); }
        }
        //return the center of the player's rectangle
        public static Vector2 Center
        {
            get { return new Vector2(position.X+width/2f, position.Y + height / 2f); }
        }
        //return the initial velocity of the projectile based on the direction the player is facing
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
        public static void Update(float elapsedTime)
        {
            getPlayerInput();
            Move(elapsedTime);
            animations[(int)currentAnimation].Update(elapsedTime);
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
                    velocity.Y = jumpingSpeed;
                    elapsedTimeStuckOnWall = 0;
                    canDoubleJump = true;
                    if (!isFacingRight)
                        wallJumpXSpeed= maxWallJumpXSpeed;
                    else
                        wallJumpXSpeed= -maxWallJumpXSpeed;
                    isFacingRight = !isFacingRight;
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
        //move player based on his current velocity and check for collisions
        static void Move(float elapsedTime)
        {
            #region CHECK IF THE PLAYER IS ON THE WALL
            int topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = (int)MathHelper.Clamp((position.Y + height - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = (int)MathHelper.Clamp((position.X + width - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
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
            if (isOnMovingPlatform)
            {
                velocity.X += PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex].Shift.X / elapsedTime;
                velocity.Y= PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex].Shift.Y / elapsedTime;
            }
            else
            {
                velocity.X += wallJumpXSpeed;
                if (Math.Abs(wallJumpXSpeed) > walkingSpeed)
                    velocity.X = MathHelper.Clamp(velocity.X, -maxWallJumpXSpeed, maxWallJumpXSpeed);
                else
                    velocity.X = MathHelper.Clamp(velocity.X, -walkingSpeed, walkingSpeed);
                wallJumpXSpeed -= 2*wallJumpXSpeed*Gravity.airFrictionCoeff*elapsedTime; //2 magic number
                if (isOnTheWall)
                {
                    if (elapsedTimeStuckOnWall < maxTimeStuckOnwal)
                    {
                        elapsedTimeStuckOnWall += elapsedTime;
                        velocity.Y = 0;
                    }
                    else if (Gravity.gravityAcceleration - velocity.Y * Gravity.wallFrictionCoeff > 0)
                    {
                        velocity.Y += (Gravity.gravityAcceleration - velocity.Y * Gravity.wallFrictionCoeff)*elapsedTime;
                    }
                }
                else
                {
                    if (Gravity.gravityAcceleration - velocity.Y * Gravity.airFrictionCoeff>0)
                    velocity.Y += (Gravity.gravityAcceleration-velocity.Y*Gravity.airFrictionCoeff) * elapsedTime;
                }
                if (velocity.Y > Gravity.gravityAcceleration * elapsedTime * 15)
                    isTouchingTheGround = false;
            }
            #endregion
            position.X += velocity.X * elapsedTime;
            #region CHECK COLLISION WITH SOLID TILES
            leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X + width - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = (int)MathHelper.Clamp((position.Y + height - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);

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
            if (isTouchingTheGround)
                velocity.X = 0;
            else
                velocity.X *= 0.3f;
            #endregion
            position.Y += velocity.Y * elapsedTime;
            #region CHECK COLLISIONS WITH TILES
            topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = (int)MathHelper.Clamp((position.Y + height - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X + width - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);

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
                        isOnMovingPlatform = false;
                        canDoubleJump = true;
                        isOnTheWall = false;
                        elapsedTimeStuckOnWall = 0;
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
                        position.Y = (topRow + 1) * Tile.tileSize;
                        velocity.Y = 0;
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
                        position.Y + height - p.Position.Y < velocity.Y*elapsedTime - p.Shift.Y)
                    {
                        isOnMovingPlatform = true;
                        position.Y = p.Position.Y - height + 1;
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
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(DrawRectangle) ,animations[(int)currentAnimation].Frame, Color.White, 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        #endregion
    }
}
