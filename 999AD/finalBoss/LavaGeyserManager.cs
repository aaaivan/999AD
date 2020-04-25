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
    static class LavaGeyserManager
    {
        #region DECLARATIONS
        static List<LavaGeyser> lavaGeysers = new List<LavaGeyser>();
        #endregion
        #region CONSTRUCTOR
        static public void Inizialize(Texture2D spritesheet)
        {
            LavaGeyser.spritesheet = spritesheet;
        }
        #endregion
        #region METHODS
        public static void ShootGeyser(float[] xPositions, float timeBeforeErupting )
        {
            if (lavaGeysers.Count > 0)
                return;
            foreach (float f in xPositions)
            {
                lavaGeysers.Add(new LavaGeyser(f, timeBeforeErupting));
            }
        }
        public static void Update(float elapsedTime)
        {
            for (int i=lavaGeysers.Count-1; i>=0; i--)
            {
                if (lavaGeysers[i].isActive())
                    lavaGeysers[i].Update(elapsedTime);
                else
                    lavaGeysers.RemoveAt(i);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (LavaGeyser lg in lavaGeysers)
                lg.Draw(spriteBatch);
        }
        #endregion
    }
}
