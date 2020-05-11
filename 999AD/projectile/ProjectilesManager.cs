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
    static class ProjectilesManager
    {
        #region DECLARATIONS
        public static List<Projectile> playerProjectiles= new List<Projectile>(); //list of the projectiles thrown by the player
        public static List<Projectile> midbossProjectiles = new List<Projectile>(); //list of the projectiles thrown by midboss
        public static List<Projectile> enemyProjectiles = new List<Projectile>(); //list of the projectiles thrown by enemy 2
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet)
        {
            Projectile.Inizialize(spritesheet);
        }
        #endregion
        #region METHODS
        public static void ShootPlayerProjectile(Vector2 position, Vector2 initialVelocity)
        {
                playerProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.holyWater));
        }

        //Function to make the midboss shoot a projectile
        public static void ShootBossProjectile(Vector2 position, Vector2 initialVelocity)
        {
            midbossProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.broccoli));
            //Broccoli - Placeholder
        }

        //Function to make the enemy 2 shoot a projectile
        public static void ShootEnemyProjectile(Vector2 position, Vector2 initialVelocity)
        {
            enemyProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.holyWater));
            //Holy water - Placeholder
        }

        public static void Update(float elapsedTime)
        {
            for (int index = playerProjectiles.Count-1; index>=0; index--)
            {
                if (playerProjectiles[index].active)
                    playerProjectiles[index].Update(elapsedTime);
                else
                    playerProjectiles.RemoveAt(index);
            }

            //Updates for midboss projectiles
            for(int index = midbossProjectiles.Count-1;index>=0;index--)
            {
                if (midbossProjectiles[index].active)
                    midbossProjectiles[index].Update(elapsedTime);
                else
                    midbossProjectiles.RemoveAt(index);
            }

            //Updates for enemy projectiles
            for(int index = enemyProjectiles.Count-1;index>=0;index--)
            {
                if (enemyProjectiles[index].active)
                    enemyProjectiles[index].Update(elapsedTime);
                else
                    enemyProjectiles.RemoveAt(index);
            }

        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in playerProjectiles)
            {
                p.Draw(spriteBatch);
            }

            //Draw for midboss projectiles
            foreach(Projectile p in midbossProjectiles)
            {
                p.Draw(spriteBatch);
            }

            //Draw for enemy projectiles
            foreach(Projectile p in enemyProjectiles)
            {
                p.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
