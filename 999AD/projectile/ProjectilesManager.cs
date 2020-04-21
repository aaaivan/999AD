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
        static List<Projectile> projectiles= new List<Projectile>(); //list of projectiles
        public static readonly float timeBetweenShots = 0.2f; //minimum time between shots
        static float elapsedShotTime=0f;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet)
        {
            Projectile.spritesheet = _spritesheet;
        }
        #endregion
        #region METHODS
        public static void Shoot(Vector2 position, Vector2 initialVelocity)
        {
            if (elapsedShotTime>timeBetweenShots)
            {
                projectiles.Add(new Projectile(position, initialVelocity));
                elapsedShotTime = 0;
            }
        }
        public static void Update(float elapsedTime)
        {
            elapsedShotTime += elapsedTime;
            for (int index = projectiles.Count-1; index>=0; index--)
            {
                if (projectiles[index].Active)
                    projectiles[index].Update(elapsedTime);
                else
                    projectiles.RemoveAt(index);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in projectiles)
            {
                p.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
