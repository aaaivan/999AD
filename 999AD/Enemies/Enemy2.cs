﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

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
        static Animation[] enemyAnimations;

        //Size Variables
        public readonly int width = 32;
        public readonly int height = 26;

        //Position Variables
        Vector2 currentPoint;
        Vector2 enemyPoint;
        Vector2 enemyPoint2;
        bool isFacingRight = false;
        bool moveToP1, moveToP2;
        bool moving;

        //Movement Variables
        float movementSpeed;

        //Distance between which enemy notices the player
        readonly int meleeDistance = 30;
        readonly int shootDistance = 60;

        public EnemyState enemyState = EnemyState.idle;
        readonly int maxHP = 3;
        int enemyHP;
        bool dead = false;
        Color enemyColor = Color.White;

        public readonly float timeUntilShot = 3f;
        float elapsedShotTime;

        public readonly float timeUntilMelee = 3f;
        float elapsedMeleeTime;

        //Vector for projectile velocity
        //First value is horizontal distance, second value is vertical distance
        public readonly Vector2 projectileInitialVelocity = new Vector2(500, -150);

        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy 2
        //Takes a spritesheet and a vector as parameters
        public Enemy2(Texture2D spritesheet, Vector2 EnemyPoint)
        {
            enemyPoint = EnemyPoint;
            enemySheet = spritesheet;
            enemyColor = Color.White;

            enemyHP = maxHP;
            currentPoint = enemyPoint;

            enemyPoint2 = enemyPoint;
            enemyPoint2.X += 50;

            movementSpeed = 0f;

            enemyAnimations = new Animation[(int)EnemyState.total]
            {
                new Animation(new Rectangle(0,0,128,32),32,26,3,1f,true),
                new Animation(new Rectangle(0,26,64,32),32,26,3,1f,true),
                new Animation(new Rectangle(0,52,64,32),32,26,3,1f,true),
                new Animation(new Rectangle(0,78,64,32),32,26,3,1f,false, true),
            };
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
            //updating the animation of the enemy sprite
            enemyAnimations[(int)enemyState].Update(elapsedTime);

            //If the enemy HP goes below 0, its state will be set to death
            //This will play the death animation
            if(enemyHP<0)
            {
                enemyState = EnemyState.death;
            }

            //Updating direction the enemy faces
            if(Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
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
            if(dead)
            {
                return;
            }

            //If the enemy is moving to the second point, it will be drawn without being flipped
            if(moveToP2 && moving)
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            //If the enemy is moving back to the first point, it will be drawn flipped
            else if(moveToP1 && moving)
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            //If the boss is not moving, it will be flipped around depending upon the position of the player
            else
            {
                spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy2DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, isFacingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
        }

        //Function to handle Attack
        public void Attack(float elapsedTime)
        {
            moving = false;
            if (Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            //If the player goes outside the given range,
            //The state will be changed acccordingly
            //Else, the Projectile attack will be delivered
            if (Math.Abs(currentPoint.X - Player.CollisionRectangle.X) < meleeDistance)
            {
                enemyState = EnemyState.melee;
            }
            else if(Math.Abs(currentPoint.X-Player.CollisionRectangle.X)>=shootDistance)
            {
                enemyState = EnemyState.idle;
            }
            else
            {
                if(elapsedShotTime>timeUntilShot)
                {
                    ProjectilesManager.ShootEnemyProjectile(currentPoint, projectileInitialVelocity * (isFacingRight ? new Vector2(1, 1) : new Vector2(-1, 1)));
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
            moving = false;
            //If the player goes outside the given range,
            //The state will be changed accordingly
            //Else, the Melee attack will be delivered
            if(Math.Abs(currentPoint.X - Player.CollisionRectangle.X)>=meleeDistance)
            {
                enemyState = EnemyState.attack;
            }
            else if(Math.Abs(currentPoint.X - Player.CollisionRectangle.X)>=shootDistance)
            {
                enemyState = EnemyState.idle;
            }
            else
            {
                if(elapsedMeleeTime>timeUntilMelee)
                {
                    Player.takeDamage();
                    elapsedMeleeTime = 0;
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
            if(Enemy2CollisionRect.Intersects(collisionRect))
            {
                enemyHP--;
                enemyColor = Color.Red * 0.5f;
                return true;
            }
            return false;
        }

        //Function to handle if Player is hit by Enemy 2
        public void PlayerHitByEnemy2()
        {
            //work required
        }

        //Function to handle the enemy death
        public void Death()
        {
            if(enemyAnimations[(int)enemyState]!=enemyAnimations[(int)EnemyState.death])
            {
                enemyAnimations[(int)enemyState] = enemyAnimations[(int)EnemyState.death];
            }
            else if(!enemyAnimations[(int)enemyState].Active)
            {
                dead = true;
            }
        }

        //Function to handle the enemy behaviour when idle
        public void Idle(float elapsedTime)
        {
            moving = true;

            if(Math.Abs(currentPoint.X - Player.CollisionRectangle.X)<shootDistance)
            {
                movementSpeed = 0f;
                if (Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
                {
                    isFacingRight = false;
                }
                enemyState = EnemyState.attack;
            }
            else
            {
                movementSpeed = 25f;
                if(currentPoint==enemyPoint)
                {
                    moveToP2 = true;
                }
                else if(currentPoint==enemyPoint2)
                {
                    moveToP1 = true;
                }

                if(moveToP2)
                {
                    currentPoint.X += movementSpeed * elapsedTime;
                    if(currentPoint.X>=enemyPoint2.X)
                    {
                        moveToP2 = false;
                        moveToP1 = true;
                    }
                }

                if(moveToP1)
                {
                    currentPoint.X -= movementSpeed * elapsedTime;
                    if(currentPoint.X<=enemyPoint.X)
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
            if(Player.CollisionRectangle.Intersects(collisionRect))
            {
                Player.takeDamage();
                return true;
            }
            return false;
        }

        #endregion
    }
}