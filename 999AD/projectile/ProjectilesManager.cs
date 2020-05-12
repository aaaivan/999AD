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
        public static List<Projectile> playerProjectiles; //list of the projectiles thrown by the player
        public static List<Projectile> midbossProjectiles; //list of the projectiles thrown by midboss
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet)
        {
            Projectile.Inizialize(spritesheet);
            playerProjectiles = new List<Projectile>();
            midbossProjectiles = new List<Projectile>();
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
        public static void Update(float elapsedTime)
        {
            //update for player projectiles
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
        }
        public static void Reset()
        {
            playerProjectiles.Clear();
            midbossProjectiles.Clear();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in playerProjectiles)
            {
                p.Draw(spriteBatch);
            }

            foreach(Projectile p in midbossProjectiles)
            {
                p.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
