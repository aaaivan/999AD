using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        static Animation[] enemyAnimations;

        //Size Variables
        public readonly int width = 14;
        public readonly int height = 14;

        //Position Variables
        Vector2 currentPoint;
        Vector2 enemyPoint;
        bool isFacingRight = false;

        //Movement Variables
        float movementSpeed;

        //Distance between which enemy notices the player
        readonly int collisionDistance = 50;
        bool hit = false;

        //Variables for knocking back the player
        readonly int knockbackDistance = 60;
        int distanceKnocked;
        bool knockback = false;

        public EnemyState enemyState = EnemyState.idle;
        readonly int maxHP = 2;
        int enemyHP;
        bool dead = false;
        Color enemyColor = Color.White;
        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy 1
        //Takes a spritesheet and a vector as parameters
        public Enemy1(Texture2D EnemySheet, Vector2 EnemyPoint)
        {
            enemyPoint = EnemyPoint;
            enemySheet = EnemySheet;
            enemyColor = Color.White;

            enemyHP = maxHP;
            currentPoint = enemyPoint;

            movementSpeed = 0f;
            distanceKnocked = 0;

            enemyAnimations = new Animation[(int)EnemyState.total]
            {
                new Animation(new Rectangle(0,0,56,14),14,14,2,1f,true),
                new Animation(new Rectangle(0,14,28,14),14,14,2,1f,true),
                new Animation(new Rectangle(0,28,28,14),14,14,2,1f,false, true)
            };
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
            //Updating the animation of the enemy sprite
            enemyAnimations[(int)enemyState].Update(elapsedTime);

            //If the enemy HP goes below 0, its state will be set to death
            //This will play the death animation
            if(enemyHP<0)
            {
                enemyState = EnemyState.death;
            }

            //Updating the direction the enemy faces
            if(Enemy1CollisionRect.X + 5 < Player.CollisionRectangle.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            //Switch case statement for the enemyState
            switch(enemyState)
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
            if(dead)
            {
                return;
            }

            spriteBatch.Draw(enemySheet, Camera.RelativeRectangle(Enemy1DrawRect), enemyAnimations[(int)enemyState].Frame, enemyColor, 0f, Vector2.Zero, isFacingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

        }

        //Function to handle Attack
        public void Attack(float elapsedTime)
        {
            if(!hit)
            {
                movementSpeed = 100;
                if(isFacingRight)
                {
                    currentPoint.X += movementSpeed * elapsedTime;
                }
                else
                {
                    currentPoint.X -= movementSpeed * elapsedTime;
                }
                PlayerHitByEnemy1();
            }

            else
            {
                KnockBack();
            }

        }

        //Function that handles the player knock back
        public void KnockBack()
        {
            if(!isFacingRight)
            {
                if(knockback)
                {
                    Player.position.X -= 2;
                    distanceKnocked += 2;
                }
            }
            else
            {
                if(knockback)
                {
                    Player.position.X += 2;
                    distanceKnocked += 2;
                }
            }

            if(distanceKnocked>=knockbackDistance)
            {
                knockback = false;
                hit = false;
                distanceKnocked = 0;
                enemyState = EnemyState.idle;
            }
        }

        //Function that returns boolean if the enemy is hit by projectile
        public bool Enemy1HitByRect(Rectangle collisionRect)
        {
            if(Enemy1CollisionRect.Intersects(collisionRect))
            {
                enemyHP--;
                enemyColor = Color.Red * 0.3f;
                return true;
            }
            return false;
        }

        //Function to handle if Player is hit by Enemy 1
        public void PlayerHitByEnemy1()
        {
            if(Enemy1CollisionRect.Intersects(Player.CollisionRectangle))
            {
                Player.takeDamage();
                hit = true;
                knockback = true;
                movementSpeed = 0;
            }
        }

        //Function to change states from idle
        public void ChangeFromIdle()
        {
            if((Math.Abs(currentPoint.X - Player.CollisionRectangle.X) <= collisionDistance)&&(Math.Abs(currentPoint.Y - Player.CollisionRectangle.Y) < 15))
            {
                enemyState = EnemyState.attack;
            }
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
        #endregion
    }
}
