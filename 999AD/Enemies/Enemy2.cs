using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace _999AD
{
    class Enemy2
    {
        //Enumerator for several states of the enemy 2
        public enum EnemyState
        {
            idle, melee, attack, death, total
        }

        #region DECLARATIONS
        static Texture2D enemySheet;
        Animation[] enemyAnimations;

        //Size Variables
        public readonly int width = 42;
        public readonly int height = 48;

        //Position Variables
        Vector2 currentPoint;
        Vector2 enemyPoint;
        Vector2 enemyPoint2;
        bool isFacingLeft = true;
        bool moveToP1, moveToP2;
        bool moving;

        //Movement Variables
        float movementSpeed;

        //Knockback Variables
        //bool knockback;

        //Distance between which enemy notices the player
        readonly int meleeDistance = 30;
        readonly int shootDistance = 60;

        public EnemyState enemyState = EnemyState.idle;
        readonly int maxHP = 3;
        int enemyHP;
        bool dead = false;
        Color enemyColor = Color.White;

        public readonly float timeUntilShot = 3.5f;
        float elapsedShotTime;

        public readonly float timeUntilMelee = 3f;
        float elapsedMeleeTime;
        //bool melee;

        //Vector for projectile velocity
        //First value is horizontal distance, second value is vertical distance
        public readonly Vector2 projectileInitialVelocity = new Vector2(500, -60);

        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy 2
        //Takes a spritesheet and a vector as parameters
        public Enemy2(Vector2 EnemyPoint, Vector2 EnemyPoint2)
        {
            enemyPoint = EnemyPoint;
            enemyPoint2 = EnemyPoint2;

            if (enemyPoint.X > enemyPoint2.X)
            {
                Vector2 tempVector = enemyPoint2;
                enemyPoint2 = enemyPoint;
                enemyPoint = enemyPoint2;
            }

            enemyColor = Color.White;

            enemyHP = maxHP;
            currentPoint = enemyPoint;


            movementSpeed = 0f;
            //knockback = false;
            //melee = false;

            enemyAnimations = new Animation[(int)EnemyState.total]
            {
                new Animation(new Rectangle(0,0,440,48),44,48,10,0.4f,true), // Animation for Idle - Patrolling
                new Animation(new Rectangle(0,48,616,48),56,48,11,0.4f,true), // Animation for Melee
                new Animation(new Rectangle(0,96,704,48),44,48,16,0.4f,true), // Animation for Attack
                new Animation(new Rectangle(0,144,264,48),44,48,5,0.4f,false, true), // Animation for Death
            };

        }
        public static void Inizialize(Texture2D spritesheet)
        {
            enemySheet = spritesheet;
        }
        #endregion

        #region PROPERTIES
        //To return the bounding rectangle for the enemy
        public Rectangle Enemy2CollisionRect
        {
            get { return new Rectangle((int)(currentPoint.X - width / 2), (int)(currentPoint.Y - height / 2), width, height); }
        }

        //To return a rectangle for the coordinates where the boss is to be drawn
        public Rectangle Enemy2DrawRect
        {
            get
            {
                return new Rectangle((int)(currentPoint.X - enemyAnimations[(int)enemyState].Frame.Width / 2),
                (int)(currentPoint.Y - enemyAnimations[(int)enemyState].Frame.Height / 2),
                enemyAnimations[(int)enemyState].Frame.Width,
                enemyAnimations[(int)enemyState].Frame.Height);
            }
        }

        //Returns a boolean whether the boss is dead or not
        public bool Dead
        {
            get { return dead; }
        }
        #endregion

        #region METHODS
        //Update Function
        public void Update(float elapsedTime)
        {
            if (dead)
            {
                return;
            }

            //updating the animation of the enemy sprite
            enemyAnimations[(int)enemyState].Update(elapsedTime);

            //If the enemy HP goes below 0, its state will be set to death
            //This will play the death animation
            if (enemyHP <= 0)
            {
                enemyState = EnemyState.death;
            }

            //Updating direction the enemy faces
            if (Enemy2CollisionRect.X + 10 < Player.CollisionRectangle.X)
            {
                isFacingLeft = true;
            }
            else
            {
                isFacingLeft = false;
            }

            //Switch case statement for the enemyState
            switch (enemyState)
            {
                case EnemyState.idle:
                    Idle(elapsedTime);
                    break;
                case EnemyState.melee:
                    Melee(elapsedTime);
                    break;
                case EnemyState.attack:
                    Attack(elapsedTime);
                    break;
                case EnemyState.death:
                    Death();
                    break;
            }
        }

        //Draw Function
        public void Draw(SpriteBatch spriteBatch)
        {
            if (dead)
            {
                return;
            }

            //If the enemy is moving to the second point, it will be drawn without being flipped
            if (moveToP2 && moving)
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            //If the enemy is moving back to the first point, it will be drawn flipped
            else if (moveToP1 && moving)
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            //If the boss is not moving, it will be flipped around depending upon the position of the player
            else
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }

        //Function to handle Attack
        public void Attack(float elapsedTime)
        {
            CheckCollisions();
            moving = false;
            if (Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingLeft = false;
            }
            else
            {
                isFacingLeft = true;
            }

            //If the player goes outside the given range,
            //The state will be changed acccordingly
            //Else, the Projectile attack will be delivered
            if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) < meleeDistance && Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 5)
            {
                enemyState = EnemyState.melee;
            }
            else if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) >= shootDistance)
            {
                enemyState = EnemyState.idle;
            }
            else
            {
                if (elapsedShotTime > timeUntilShot)
                {
                    SoundEffects.Enemy2Attack.Play();
                    ProjectilesManager.ShootEnemyProjectile(currentPoint, projectileInitialVelocity * (isFacingLeft ? new Vector2(-1, 1) : new Vector2(1, 1)));
                    elapsedShotTime = 0;
                }
                else
                {
                    elapsedShotTime += elapsedTime;
                }
            }
        }

        //Function to handle Melee Attack
        public void Melee(float elapsedTime)
        {
            CheckCollisions();
            moving = false;

            if (Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingLeft = false;
            }
            else
            {
                isFacingLeft = true;
            }

            //If the player goes outside the given range,
            //The state will be changed accordingly
            //Else, the Melee attack will be delivered
            if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) >= meleeDistance && Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 5)
            {
                enemyState = EnemyState.attack;
            }
            else if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) >= shootDistance)
            {
                enemyState = EnemyState.idle;
            }

            else
            {
                if (elapsedMeleeTime > timeUntilMelee)
                {
                    SoundEffects.Enemy2Melee.Play();
                    Player.takeDamage();
                    elapsedMeleeTime = 0;
                    //knockback = true;
                    //melee = true;
                    Player.Rebound(0.2f, 1.4f, Player.Center.X > currentPoint.X);
                }
                else
                {
                    elapsedMeleeTime += elapsedTime;
                }
            }
        }

        //Function that returns boolean if the enemy is hit by projectile
        public bool Enemy2HitByRect(Rectangle collisionRect)
        {
            if (Enemy2CollisionRect.Intersects(collisionRect))
            {
                enemyHP--;
                SoundEffects.EnemyHurt.Play();
                enemyColor = Color.Red * 0.6f;
                return true;
            }
            return false;
        }

        //Function to handle the enemy death
        public void Death()
        {
            enemyColor = Color.White;
            if (enemyAnimations[(int)enemyState] != enemyAnimations[(int)EnemyState.death])
            {
                SoundEffects.EnemyHurt.Play();
                enemyAnimations[(int)enemyState] = enemyAnimations[(int)EnemyState.death];
            }
            else if (!enemyAnimations[(int)enemyState].Active)
            {
                dead = true;
            }

        }

        //Function to handle the enemy behaviour when idle
        public void Idle(float elapsedTime)
        {
            CheckCollisions();
            moving = true;

            if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) < shootDistance && Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 5)
            {
                movementSpeed = 0f;
                if (Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
                {
                    isFacingLeft = false;
                }
                enemyState = EnemyState.attack;
            }
            else
            {
                movementSpeed = 25f;
                if (currentPoint == enemyPoint)
                {
                    moveToP2 = true;
                }
                else if (currentPoint == enemyPoint2)
                {
                    moveToP1 = true;
                }

                if (moveToP2)
                {
                    currentPoint.X += movementSpeed * elapsedTime;
                    if (currentPoint.X >= enemyPoint2.X)
                    {
                        moveToP2 = false;
                        moveToP1 = true;
                    }
                }

                if (moveToP1)
                {
                    currentPoint.X -= movementSpeed * elapsedTime;
                    if (currentPoint.X <= enemyPoint.X)
                    {
                        moveToP1 = false;
                        moveToP2 = true;
                    }
                }
            }

        }

        //Function that returns boolean if the player is hit by projectile
        public bool PlayerHitByRect(Rectangle collisionRect)
        {
            if (Player.CollisionRectangle.Intersects(collisionRect))
            {
                Player.takeDamage();
                return true;
            }
            return false;
        }

        //Knockback Function
        /*public void KnockBack()
        {
            if (!isFacingLeft)
            {
                if(knockback&&melee)
                {
                    Player.position.X += 15;
                }
                else if(!Player.movingLeft)
                {
                   Player.position.X = Enemy2CollisionRect.X;
                }
            }
            else
            {
                if(knockback&&melee)
                {
                    Player.position.X -= 15;
                }
                else
               {
                   Player.position.X = Enemy2CollisionRect.Right;
               }
            }
            knockback = false;
            melee = false;
        }*/

        //Function to check for collisions between enemy 2 and player
        public void CheckCollisions()
        {
            if (!dead && Player.CollisionRectangle.Intersects(Enemy2CollisionRect))
            {
                if (Math.Abs(Player.CollisionRectangle.Bottom - Enemy2CollisionRect.Top) <= 5)
                {
                    Player.Rebound(0.75f);
                    SoundEffects.EnemyHurt.Play();
                    enemyHP--;
                    enemyColor = Color.Red * 0.6f;
                }
                else
                {
                    Player.takeDamage();
                    //knockback = true;
                    if (Player.Center.X > currentPoint.X)
                        Player.position.X = currentPoint.X + width / 2;
                    else
                        Player.position.X = currentPoint.X - width / 2 - Player.width;
                }
            }
            //KnockBack();
        }
        #endregion
    }
}