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
    static class ProjectilesManager
    {
        #region DECLARATIONS
        public static List<Projectile> playerProjectiles= new List<Projectile>(); //list of the projectiles thrown by the player
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
        public static void Update(float elapsedTime)
        {
            for (int index = playerProjectiles.Count-1; index>=0; index--)
            {
                if (playerProjectiles[index].active)
                    playerProjectiles[index].Update(elapsedTime);
                else
                    playerProjectiles.RemoveAt(index);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in playerProjectiles)
            {
                p.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
