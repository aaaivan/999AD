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
    static class MidBoss
    {
        //Enumerator for the several states of the boss
        public enum BossState
        {
            idle, move, attack, death, total
        }

        #region DECLARATIONS
        static Texture2D bossSheet;
        static Animation[] bossAnimations;
        public static readonly float timeUntilShot = 2f;
        static float elapsedShotTime;

        static readonly float timeUntilChange = 0.5f;
        static float elapsedChangeTime;

        //Size Variables
        public static readonly int bossWidth = 20;
        public static readonly int bossHeight = 26;

        //Position Variables
        static Vector2 point1;
        static Vector2 point2;
        static Vector2 bossPoint;
        static bool isFacingRight = false;

        //Movement Variables
        static float movementSpeed;
        static bool moveToP1, moveToP2;

        //Vector for projectile velocity
        //First value is horizontal distance, second value is vertical distance
        public static readonly Vector2 projectileInitialVelocity = new Vector2(500, -150);

        public static BossState bossState = BossState.idle;
        static readonly int maxHP = 10;
        static int bossHP;
        static Random random = new Random();
        static bool dead = false;
        static Color bossColor = Color.White;
        #endregion

        #region CONSTRUCTOR
        //Constructor for midboss
        //Takes a spritesheet as a parameter
        public static void Initialise(Texture2D BossSheet)
        {
            point1 = new Vector2(200,175);
            point2 = new Vector2(100, 175);
            bossSheet = BossSheet;
            bossColor = Color.White;

            bossHP = maxHP;
            bossPoint = point1;

            movementSpeed = 0f;
            moveToP2 = false;
            moveToP1 = false;

            bossAnimations = new Animation[(int)BossState.total]
            {
                new Animation(new Rectangle(0,0,104,20),20,26,4,1f,true),
                new Animation(new Rectangle(0,26,52,20),20,26,2,1f,true),
                new Animation(new Rectangle(0,52,52,20),20,26,2,1f,true),
                new Animation(new Rectangle(0,78,52,20),20,26,2,1f,false, true),
            };
        }
        #endregion

        #region PROPERTIES
        //To return the bounding rectangle for the boss
        public static Rectangle BossCollisionRect
        {
            get { return new Rectangle((int)(bossPoint.X - bossWidth / 2), (int)(bossPoint.Y - bossHeight / 2), bossWidth, bossHeight); }
        }

        //To return a rectangle for the coordinates where the boss is to be drawn
        public static Rectangle BossDrawRect
        {
            get
            {
                return new Rectangle((int)(bossPoint.X - bossAnimations[(int)bossState].Frame.Width / 2),
                    (int)(bossPoint.Y - bossAnimations[(int)bossState].Frame.Height / 2),
                    bossAnimations[(int)bossState].Frame.Width,
                    bossAnimations[(int)bossState].Frame.Height);
            }
        }

        //Returns a boolean whether the boss is dead or not
        public static bool Dead
        {
            get { return dead; }
        }

        #endregion

        #region METHODS
        //Update Function
        public static void Update(float elapsedTime)
        {
            //Updating the animation of the boss sprite
            bossAnimations[(int)bossState].Update(elapsedTime);

            //If the boss HP goes below 0, its state will be set to death
            //This will play the death animation
            if(bossHP<=0)
            {
                bossState = BossState.death;
            }

            //Updating direction the midboss faces
            if(BossCollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            //Switch case statement for the bossState
            switch(bossState)
            {
                case BossState.idle:
                    ChangeFromIdle(elapsedTime);
                    break;
                case BossState.move:
                    Move(elapsedTime);
                    break;
                case BossState.attack:
                    Attack(elapsedTime);
                    break;
                case BossState.death:
                    Death();
                    break;
            }
        }

        //Draw Function
        public static void Draw(SpriteBatch spriteBatch)
        {
            if(dead)
            {
                return;
            }

            //If the boss is moving to the second point, it will be drawn without being flipped
            if(moveToP2)
            {
                spriteBatch.Draw(bossSheet, Camera.RelativeRectangle(BossDrawRect), bossAnimations[(int)bossState].Frame, bossColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            //If the boss is moving back to the first point, it will be drawn without being flipped
            else if(moveToP1)
            {
                spriteBatch.Draw(bossSheet, Camera.RelativeRectangle(BossDrawRect), bossAnimations[(int)bossState].Frame, bossColor, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            //If the boss is not moving, it will be flipped around depending upon the position of the player
            else
            {
                spriteBatch.Draw(bossSheet, Camera.RelativeRectangle(BossDrawRect), bossAnimations[(int)bossState].Frame, bossColor, 0f, Vector2.Zero, isFacingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }

        //Function to handle Attack
        public static void Attack(float elapsedTime)
        {
            if(elapsedShotTime > timeUntilShot)
            {
                ProjectilesManager.ShootBossProjectile(isFacingRight ? bossPoint : (bossPoint - new Vector2(bossWidth-5, 0)), projectileInitialVelocity * (isFacingRight ? new Vector2(1, 1) : new Vector2(-1, 1)));
                elapsedShotTime = 0;
                bossState = BossState.idle;
            }
            else
            {
                elapsedShotTime += elapsedTime;
            }
        }

        //Function to handle midboss movement
        public static void Move(float elapsedTime)
        {
            movementSpeed = 100;
            if(bossPoint==point1)
            {
                moveToP2 = true;
            }
            else if(bossPoint==point2)
            {
                moveToP1 = true;
            }

            if(moveToP2)
            {
                bossPoint.X -= movementSpeed * elapsedTime;
                if(bossPoint.X<=point2.X)
                {
                    bossPoint = point2;
                    moveToP2 = false;
                    movementSpeed = 0;
                    bossState = BossState.idle;
                }
            }

            if(moveToP1)
            {
                bossPoint.X += movementSpeed * elapsedTime;
                if(bossPoint.X>=point1.X)
                {
                    bossPoint = point1;
                    moveToP1 = false;
                    movementSpeed = 0;
                    bossState = BossState.idle;
                }
            }
            
        }

        //Function that returns boolean if the boss is hit by projectile
        public static bool BossHitByRect(Rectangle collisionRect)
        {
            if (BossCollisionRect.Intersects(collisionRect))
            {
                bossHP -= 1;
                bossColor = Color.Red * 0.8f;
                return true;
            }
            return false;
        }

        //Function that returns boolean if the player is hit by projectile
        public static bool PlayerHitByRect(Rectangle collisionRect)
        {
            if(Player.CollisionRectangle.Intersects(collisionRect))
            {
                Player.takeDamage();
                return true;
            }
            return false;
        }

        //Function to change states from idle
        //Takes elapsed time as a parameter to produce a delay when states are changed
        public static void ChangeFromIdle(float elapsedTime)
        {
            if (elapsedChangeTime > timeUntilChange)
                {
                    ChangeState();
                    elapsedChangeTime = 0;
                }
                else
                {
                    elapsedChangeTime += elapsedTime;
                }
        }

        //Function to change states
        public static void ChangeState()
        {
            if (Math.Abs(bossPoint.X - Player.CollisionRectangle.X) < 75)
            {
                int randomNum = random.Next(0, 2);
                if (randomNum == 0)
                {
                    bossState = BossState.move;
                }
                else if (randomNum == 1)
                {
                    bossState = BossState.attack;
                }
            }
        }

        //Function to handle the boss death
        public static void Death()
        {
            if (bossAnimations[(int)bossState] != bossAnimations[(int)BossState.death])
            {
                bossAnimations[(int)bossState] = bossAnimations[(int)BossState.death];
            }
            else if (!bossAnimations[(int)bossState].Active)
            {
                dead = true;
            }
        }

        #endregion
    }
}
