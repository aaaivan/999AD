using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace _999AD
{
    static class Player
    {
        enum AnimationTypes
        {
            idle, walk, jump, attack, fall, die, total
        }
        #region DECLARATIONS
        static Texture2D spritesheet;
        static List<Animation> animations;
        static AnimationTypes currentAnimation;
        public static Vector2 position;
        public static readonly int height= 22; //refer to player rectangle not to the sprite size
        public static readonly int width= 12; //refer to player rectangle not to the sprite size
        static Vector2 jumpSpeed;
        static bool isFacingRight;
        public static readonly float maxHorizontalMovementSpeed= 100; //movement speed
        public static float horizontalMovementSpeed;
        public static readonly Vector2 initialJumpSpeed= new Vector2(190, -400); //jumping initial speed
        static bool isTouchingTheGround;
        static bool isOnTheWall;
        static bool isOnMovingPlatform;
        static bool canDoubleJump;
        static bool isWallJumping;
        public static bool doubleJumpUnlocked;
        public static bool wallJumpUnlocked;
        public static readonly Vector2 projectileInitialVelocity = new Vector2(200, -200);
        public static readonly float timeBetweenShots = 0.4f; //minimum time between shots
        static float elapsedShotTime;
        public static readonly int maxHealthPoints = 3;
        public static int healthPoints;
        static readonly float invulnerabilityTime = 3;
        static float elapsedInvulnerabilityTime;
        static bool invulnerable;
        static float alphaValue;
        public static bool haltInput;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet, Vector2 _position)
        {
            Reset(_position);
            spritesheet = _spritesheet;
            doubleJumpUnlocked =false;
            wallJumpUnlocked = false;
            //fill following statements with sprite info
            animations = new List<Animation>();
            animations.Add(new Animation(new Rectangle(0, 0, 128, 24), 16, 24, 8, 0.3f, true));
            animations.Add( new Animation( new Rectangle(0, 24, 160,24), 16, 24, 10, 0.06f, true));
            animations.Add(new Animation( new Rectangle(0, 48, 80, 24), 16, 24, 5, 0.06f, false, true));
            animations.Add(new Animation( new Rectangle(0, 72, 112, 24), 16, 24, 7, 0.06f, false, false));
            animations.Add(new Animation( new Rectangle(0, 96, 64, 24), 16, 24, 4, 0.1f, true));
            animations.Add(new Animation( new Rectangle(0, 120, 160, 24), 16, 24, 10, 0.1f, false, true));
        }
        public static void Reset(Vector2 _position)
        {
            position = _position;
            jumpSpeed = Vector2.Zero;
            isFacingRight = true;
            horizontalMovementSpeed = 0;
            isTouchingTheGround = false;
            isOnTheWall = false;
            isOnMovingPlatform = false;
            canDoubleJump = true;
            isWallJumping = false;
            elapsedShotTime = 0f;
            healthPoints = maxHealthPoints;
            elapsedInvulnerabilityTime = 0;
            invulnerable = false;
            alphaValue = 1;
            haltInput = false;
            currentAnimation = AnimationTypes.idle;
        }
        #endregion
        #region PROPERTIES
        //return the player's collision rectangle
        public static Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y+2, width, height-4); }//-2 magic number: improve?
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
            if (healthPoints > 0 && !haltInput)
            {
                CheckMovementInput();
                CheckAttackInput(elapsedTime);
            }
            Move(elapsedTime);
            AnimationStateMachine(elapsedTime);
            if (invulnerable)
            {
                elapsedInvulnerabilityTime += elapsedTime;
                alphaValue = (alphaValue + 0.1f) - (int)(alphaValue + 0.1f);
                if (elapsedInvulnerabilityTime > invulnerabilityTime)
                {
                    invulnerable = false;
                    alphaValue = 1;
                }
            }
        }
        static void AnimationStateMachine(float elapsedTime)
        {
            switch (currentAnimation)
            {
                case AnimationTypes.idle:
                    animations[(int)AnimationTypes.jump].Reset();
                    break;
                case AnimationTypes.die:
                    if (!animations[(int)currentAnimation].Active)
                    {
                        animations[(int)AnimationTypes.attack].Reset();
                        animations[(int)AnimationTypes.jump].Reset();
                        Game1.currentGameState = Game1.GameStates.dead;
                    }
                    break;
                case AnimationTypes.attack:
                    if (!animations[(int)currentAnimation].Active)
                    {
                        animations[(int)AnimationTypes.attack].Reset();
                        animations[(int)AnimationTypes.jump].Reset();
                        currentAnimation = AnimationTypes.idle;
                    }
                    break;
                case AnimationTypes.jump:
                    break;
                case AnimationTypes.fall:
                    animations[(int)AnimationTypes.jump].Reset();
                    break;
                case AnimationTypes.walk:
                    animations[(int)AnimationTypes.jump].Reset();
                    break;
            }
            animations[(int)currentAnimation].Update(elapsedTime);
        }
        //check input for shooting
        static void CheckAttackInput(float elapsedTime)
        {
            if (elapsedShotTime > timeBetweenShots)
            {
                //Space to Shoot / Controller - B to Shoot
                if ((Game1.currentKeyboard.IsKeyDown(Keys.Space) && !Game1.previousKeyboard.IsKeyDown(Keys.Space))||
                    (Game1.currentGamePad.Buttons.B==ButtonState.Pressed && Game1.previousGamePad.Buttons.B == ButtonState.Released))
                {
                    ProjectilesManager.ShootPlayerProjectile(isFacingRight ? (position + new Vector2(width, 0)) : position, ProjectileInitialVelocity);
                    elapsedShotTime = 0;
                    SoundEffects.PlayerAttack.Play();
                    if (currentAnimation != AnimationTypes.die)
                        currentAnimation = AnimationTypes.attack;
                }
            }
            else
                elapsedShotTime += elapsedTime;
        }
        //check input for movement
        static void CheckMovementInput()
        {
            //Get Thumbsticks controls
            /*position.X += Game1.currentGamePad.ThumbSticks.Left.X * maxHorizontalMovementSpeed;
            if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack)
                currentAnimation = AnimationTypes.walk;*/

            //W to Jump / Controller - A to Jump
            if ((Game1.currentKeyboard.IsKeyDown(Keys.W) && !Game1.previousKeyboard.IsKeyDown(Keys.W)) ||
                (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
            {
                if (isTouchingTheGround)
                {
                    if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack)
                        currentAnimation = AnimationTypes.jump;
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    SoundEffects.PlayerJump.Play();
                    if (Game1.currentKeyboard.IsKeyDown(Keys.D))
                        jumpSpeed.X = initialJumpSpeed.X;
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                        jumpSpeed.X = -initialJumpSpeed.X;
                    isTouchingTheGround = false;
                    isOnMovingPlatform = false;
                    return;
                }
                else if (isOnTheWall && wallJumpUnlocked)
                {
                    if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack)
                        currentAnimation = AnimationTypes.jump;
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    SoundEffects.PlayerJump.Play();
                    canDoubleJump = false;
                    isOnTheWall = false;
                    isWallJumping = true;
                    if (!isFacingRight)
                        jumpSpeed.X = initialJumpSpeed.X;
                    else
                        jumpSpeed.X = -initialJumpSpeed.X;
                    isFacingRight = !isFacingRight;
                }
                else if (canDoubleJump && doubleJumpUnlocked)
                {
                    if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack)
                        currentAnimation = AnimationTypes.jump;
                    jumpSpeed.Y = initialJumpSpeed.Y;
                    SoundEffects.PlayerJump.Play();
                    canDoubleJump = false;
                    return;
                }
            }
            if ((Game1.currentKeyboard.IsKeyDown(Keys.D))||(Game1.currentGamePad.DPad.Right==ButtonState.Pressed))
            {
                horizontalMovementSpeed += maxHorizontalMovementSpeed;
                if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack && isTouchingTheGround)
                    currentAnimation = AnimationTypes.walk;
            }
            if ((Game1.currentKeyboard.IsKeyDown(Keys.A))||(Game1.currentGamePad.DPad.Left==ButtonState.Pressed))
            {
                horizontalMovementSpeed -= maxHorizontalMovementSpeed;
                if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack && isTouchingTheGround)
                    currentAnimation = AnimationTypes.walk;
            }
            if (isTouchingTheGround &&
                horizontalMovementSpeed == 0 &&
                currentAnimation != AnimationTypes.die &&
                currentAnimation != AnimationTypes.attack)
                currentAnimation = AnimationTypes.idle;
        }
        //move player based on his current velocity and check for collisions
        static void Move(float elapsedTime)
        {
            Vector2 totalSpeed = new Vector2();
            #region CHECK IF IS STILL ON THE WALL
            int topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = (int)MathHelper.Clamp((position.Y + height - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = (int)MathHelper.Clamp((position.X-1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = (int)MathHelper.Clamp((position.X+1 + width - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            if (isOnTheWall)
            {
                isOnTheWall = false;
                if (isFacingRight)
                {
                    for (int row = topRow; row <= btmRow; row++)
                    {
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                            isOnTheWall = true;
                    }
                }
                else
                {
                    for (int row = topRow; row <= btmRow; row++)
                    {
                        if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                            isOnTheWall = true;
                    }
                }
            }
            #endregion
            #region CALCULATE VELOCITIES
            if (isWallJumping)
            {
                if (Math.Abs(jumpSpeed.X)> maxHorizontalMovementSpeed)
                    totalSpeed.X = jumpSpeed.X;
                else
                {
                    if (Math.Sign(jumpSpeed.X) == -Math.Sign(horizontalMovementSpeed))
                    {
                        if (Math.Abs(jumpSpeed.X) >= maxHorizontalMovementSpeed / 2f)
                            totalSpeed.X = jumpSpeed.X;
                        else
                        {
                            isWallJumping = false;
                            jumpSpeed.X = 0;
                            totalSpeed.X = horizontalMovementSpeed;
                        }
                    }
                    else
                    {
                        totalSpeed.X = jumpSpeed.X+horizontalMovementSpeed;
                        totalSpeed.X = MathHelper.Clamp(totalSpeed.X, -maxHorizontalMovementSpeed, maxHorizontalMovementSpeed);
                    }
                }
                jumpSpeed.X -= 2f * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//2 is a magic number
            }
            else if (!isTouchingTheGround)
            {
                if (Math.Sign(jumpSpeed.X) == -Math.Sign(horizontalMovementSpeed))
                    totalSpeed.X = horizontalMovementSpeed;
                else if (Math.Abs(jumpSpeed.X) > maxHorizontalMovementSpeed)
                {
                    totalSpeed.X = jumpSpeed.X;
                    jumpSpeed.X -= 2 * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//2 is a magic number
                }
                else
                {
                    totalSpeed.X = jumpSpeed.X+horizontalMovementSpeed;
                    totalSpeed.X = MathHelper.Clamp(totalSpeed.X, -maxHorizontalMovementSpeed, maxHorizontalMovementSpeed);
                    jumpSpeed.X -= 6 * jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;//6 is a magic number
                }
            }
            else
            {
                totalSpeed.X = horizontalMovementSpeed;
            }
            horizontalMovementSpeed = 0;
            if (totalSpeed.X > 0)
                isFacingRight = true;
            else if (totalSpeed.X < 0)
                isFacingRight = false;
            if (!isOnMovingPlatform)
            {
                if (isOnTheWall)
                {
                    if (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.wallFrictionCoeff > 0)
                    {
                        jumpSpeed.Y += (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.wallFrictionCoeff) * elapsedTime;
                    }
                }
                else
                {
                    if (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.airFrictionCoeff > 0)
                        jumpSpeed.Y += (Gravity.gravityAcceleration - jumpSpeed.Y * Gravity.airFrictionCoeff) * elapsedTime;
                }
                if (jumpSpeed.Y > Gravity.gravityAcceleration * elapsedTime * 15)
                {
                    isTouchingTheGround = false;
                    isOnMovingPlatform = false;
                    if (currentAnimation != AnimationTypes.die && currentAnimation != AnimationTypes.attack)
                        currentAnimation = AnimationTypes.fall;
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
            leftCol = (int)MathHelper.Clamp((position.X-1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X +1+ width - 0.001f) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check right-hand side
            if (totalSpeed.X >= 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.tileSize - width;
                        if (!isTouchingTheGround)
                        {
                            isWallJumping = false;
                            if (jumpSpeed.Y > 0 && !isOnTheWall)
                            {
                                jumpSpeed.Y = 0;
                            }
                            isOnTheWall = true;
                            jumpSpeed.X = 0;
                        }
                        break;
                    }
                }
            }
            //check left-hand side
            else if (totalSpeed.X <= 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                    {
                        position.X = (leftCol + 1) * Tile.tileSize;
                        if (!isTouchingTheGround)
                        {
                            isWallJumping = false;
                            if (jumpSpeed.Y > 0 && !isOnTheWall)
                            {
                                jumpSpeed.Y = 0;
                            }
                            isOnTheWall = true;
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
                            isOnMovingPlatform = false;
                            canDoubleJump = true;
                            isOnTheWall = false;
                            isWallJumping = false;
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
                        isOnMovingPlatform = false;
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
                    if (!p.Transparent &&
                        position.X+width>p.Position.X &&
                        position.X<p.Position.X+p.width &&
                        position.Y+height- p.Position.Y>=0 &&
                        position.Y + height - p.Position.Y <= totalSpeed.Y*elapsedTime - p.Shift.Y)
                    {
                        isOnMovingPlatform = true;
                        position.Y = p.Position.Y - height;
                        jumpSpeed.Y = 0;
                        jumpSpeed.X = 0;
                        PlatformsManager.platformIndex = i;
                        isTouchingTheGround = true;
                        canDoubleJump = true;
                        isOnTheWall = false;
                        isWallJumping = false;
                        return;
                    }
                }
            }
            else
            {
                MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex];
                if (position.X - p.Position.X <= -width || position.X - p.Position.X >= p.width||p.Transparent)
                    isOnMovingPlatform = false;
            }
            #endregion
        }
        public static void Rebound(float ratioReboundToNormalJump)
        {
            jumpSpeed.Y = initialJumpSpeed.Y * ratioReboundToNormalJump;
            SoundEffects.PlayerJump.Play();
            canDoubleJump = true;
            isTouchingTheGround = false;
            isOnMovingPlatform = false;
            isWallJumping = false;
        }
        public static void Rebound(float ratioReboundToNormalJump_Y, float ratioReboundToNormalJump_X, bool toTheRight)
        {
            jumpSpeed.X = initialJumpSpeed.X * ratioReboundToNormalJump_X * (toTheRight ? 1 : -1);
            jumpSpeed.Y = initialJumpSpeed.Y * ratioReboundToNormalJump_Y;
            SoundEffects.PlayerJump.Play();
            canDoubleJump = true;
            isTouchingTheGround = false;
            isOnMovingPlatform = false;
            isWallJumping = false;
        }
        public static void takeDamage(int damage=1, bool damageEvenIfInvulnerable=false)
        {
            if (invulnerable && !damageEvenIfInvulnerable)
                return;
            healthPoints -= damage;
            SoundEffects.PlayerHurt.Play();
            if (healthPoints==0)
            {
                currentAnimation = AnimationTypes.die;
            }
            else
            {
                invulnerable = true;
                elapsedInvulnerabilityTime = 0;
            }
        }
        public static void IncreaseHealth()
        {
            if (healthPoints < maxHealthPoints)
                healthPoints++;
        }
        public static void ReplenishHealth()
        {
            healthPoints = maxHealthPoints;
            animations[(int)AnimationTypes.die].Reset();
            currentAnimation = AnimationTypes.idle;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(new Vector2(Center.X - animations[(int)currentAnimation].Frame.Width / 2,
                                                     position.Y - animations[(int)currentAnimation].Frame.Height + height),
                                         animations[(int)currentAnimation].Frame.Width,
                                         animations[(int)currentAnimation].Frame.Height),
                animations[(int)currentAnimation].Frame, Color.White*(invulnerable?alphaValue:1), 0f, Vector2.Zero,
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public static void DrawGUI(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < healthPoints; i++)
            {
                spriteBatch.Draw(Collectable.Sprites, new Vector2(5 + i * 16, 5), new Rectangle(0, 110, 16, 19), Color.White);
            }
        }
        #endregion
    }
}
