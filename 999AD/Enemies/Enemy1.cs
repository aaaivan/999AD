﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class Enemy1
    {
        //Enumerator for the several states of the enemy 1
        public enum EnemyState
        {
            idle, attack, death, total
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

        //Movement Variables
        float movementSpeed;

        //Distance between which enemy notices the player
        readonly int collisionDistance = 50;
        bool hit = false;

        //Variables for knocking back the player
        //readonly int knockbackDistance = 40;
        //int distanceKnocked;
        //bool knockback = false;

        public EnemyState enemyState = EnemyState.idle;
        readonly int maxHP = 2;
        int enemyHP;
        bool dead = false;
        Color enemyColor = Color.White;
        readonly float timeRedColor = 0.2f;
        float elapsedRedColorTime;
        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy 1
        //Takes a spritesheet and a vector as parameters
        public Enemy1(Vector2 EnemyPoint, Vector2 EnemyPoint2)
        {
            elapsedRedColorTime = timeRedColor;
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
            //distanceKnocked = 0;
            enemyAnimations = new Animation[(int)EnemyState.total]
            {
                new Animation(new Rectangle(0,0,420,48),42,48,10,0.2f,true), // Animation for Idle
                new Animation(new Rectangle(0,48,420,48),42,48,10,0.2f,true), //Animation for Attack - Dash
                new Animation(new Rectangle(0,96,252,48),42,48,5,0.2f,false, true) //Animation for Death
            };


        }
        public static void Inizialize(Texture2D spritesheet)
        {
            enemySheet = spritesheet;
        }
        #endregion

        #region PROPERTIES
        //To return the bounding rectangle for the enemy
        public Rectangle Enemy1CollisionRect
        {
            get { return new Rectangle((int)(currentPoint.X - width / 2), (int)(currentPoint.Y - height / 2), width, height); }
        }

        //To return a rectangle for the coordinates where the boss is to be drawn
        public Rectangle Enemy1DrawRect
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
            if (elapsedRedColorTime<timeRedColor)
            {
                elapsedRedColorTime += elapsedTime;
                if (elapsedRedColorTime >= timeRedColor)
                {
                    enemyColor = Color.White;
                }
            }
            //Updating the animation of the enemy sprite
            enemyAnimations[(int)enemyState].Update(elapsedTime);

            //If the enemy HP goes below 0, its state will be set to death
            //This will play the death animation
            if (enemyHP <= 0)
            {
                enemyState = EnemyState.death;
            }

            //Updating the direction the enemy faces
            if (Enemy1CollisionRect.X + 5 < Player.CollisionRectangle.X)
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
                    ChangeFromIdle();
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

            spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy1DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, isFacingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

        }

        //Function to handle Attack
        public void Attack(float elapsedTime)
        {
            CheckCollisions();
            if (!hit)
            {
                movementSpeed = 80;
                if (isFacingLeft)
                {
                    currentPoint.X += movementSpeed * elapsedTime;
                    if (currentPoint.X > enemyPoint2.X)
                    {
                        currentPoint.X = enemyPoint2.X;
                        enemyState = EnemyState.idle;
                    }
                }
                else
                {
                    currentPoint.X -= movementSpeed * elapsedTime;
                    if (currentPoint.X < enemyPoint.X)
                    {
                        currentPoint.X = enemyPoint.X;
                        enemyState = EnemyState.idle;
                    }
                }
                PlayerHitByEnemy1();
            }

            else
            {
                Player.Rebound(0.2f, 1.4f, Player.Center.X > currentPoint.X);
                hit = false;
                //KnockBack();
            }

        }

        ////Function that handles the player knock back
        //public void KnockBack()
        //{
        //    if(!isFacingLeft)
        //    {
        //        if(knockback)
        //        {
        //            Player.position.X -= 2;
        //            distanceKnocked += 2;
        //        }
        //    }
        //    else
        //    {
        //        if(knockback)
        //        {
        //            Player.position.X += 2;
        //            distanceKnocked += 2;
        //        }
        //    }

        //    if(distanceKnocked>=knockbackDistance)
        //    {
        //        knockback = false;
        //        hit = false;
        //        distanceKnocked = 0;
        //        enemyState = EnemyState.idle;
        //    }
        //}

        //Function that returns boolean if the enemy is hit by projectile
        public bool Enemy1HitByRect(Rectangle collisionRect)
        {
            if (Enemy1CollisionRect.Intersects(collisionRect))
            {
                enemyHP--;
                SoundEffects.EnemyHurt.Play();
                enemyColor = Color.Red * 0.6f;
                elapsedRedColorTime = 0;
                return true;
            }
            return false;
        }

        //Function to handle if Player is hit by Enemy 1
        public void PlayerHitByEnemy1()
        {
            if (Enemy1CollisionRect.Intersects(Player.CollisionRectangle))
            {
                Player.takeDamage();
                hit = true;
                //knockback = true;
                movementSpeed = 0;
            }
        }

        //Function to change states from idle
        public void ChangeFromIdle()
        {
            CheckCollisions();
            if ((Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 15 && (Player.CollisionRectangle.X > enemyPoint2.X + 15 || Player.CollisionRectangle.Right < enemyPoint.X - 15)))
            {
                enemyState = EnemyState.idle;
            }
            else if ((Math.Abs(currentPoint.X - Player.CollisionRectangle.X) <= collisionDistance) && (Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 15))
            {
                enemyState = EnemyState.attack;
                SoundEffects.Enemy1Attack.Play();
            }
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

        //Function to check for collisions between enemy 1 and player
        public void CheckCollisions()
        {
            if (!dead && Player.CollisionRectangle.Intersects(Enemy1CollisionRect))
            {
                if (Math.Abs(Player.CollisionRectangle.Bottom - Enemy1CollisionRect.Top) <= 5)
                {
                    enemyHP--;
                    SoundEffects.EnemyHurt.Play();
                    Player.Rebound(0.75f);
                    enemyColor = Color.Red * 0.6f;
                    elapsedRedColorTime = 0;
                }
            }
        }
        #endregion
    }
}