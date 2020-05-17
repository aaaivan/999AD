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
        static List<LavaGeyser> lavaGeysers;
        static readonly Random rand= new Random();
        #endregion
        #region CONSTRUCTOR
        static public void Inizialize(Texture2D spritesheet)
        {
            LavaGeyser.Inizialize(spritesheet);
            lavaGeysers = new List<LavaGeyser>();
        }
        public static void Reset()
        {
            lavaGeysers.Clear();
        }
        #endregion
        #region PROPERTIES
        public static int ActiveLavaGeysers
        {
            get { return lavaGeysers.Count; }
        }
        #endregion
        #region METHODS
        public static void ShootGeyser(float[] xPositions, float timeBeforeErupting, int initialYVelocity=-1400 )
        {
            foreach (float f in xPositions)
            {
                lavaGeysers.Add(new LavaGeyser(f, timeBeforeErupting, initialYVelocity));
            }
        }
        public static void SweepAcross(float delay, float timeBetweenEruptions, int safeAreas, float safeAreaMinX, float safeAreaMaxX, bool leftToRight)
        {
            int totalEruptions = (MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomWidthtPx - 1 + LavaGeyser.size) / LavaGeyser.size;
            int min = (int)(safeAreaMinX / LavaGeyser.size);
            int max = (int)(safeAreaMaxX / LavaGeyser.size);
            int firstSafe;
            if (max-min+1>=safeAreas)
                firstSafe = rand.Next(min, max + 1 - safeAreas);
            else
            {
                firstSafe = min;
                safeAreas = max - min + 1;
            }
            if (leftToRight)
            {
                for (int i =0, j=0; i<totalEruptions; i++)
                {
                    if (i==firstSafe)
                        i += safeAreas;
                    lavaGeysers.Add(new LavaGeyser((i+0.5f)* LavaGeyser.size, delay+j*timeBetweenEruptions));
                    j++;
                }
            }
            else
            {
                for (int i = totalEruptions-1, j=0; i >= 0; i--)
                {
                    if (i == firstSafe-1+safeAreas)
                        i -= safeAreas;
                    lavaGeysers.Add(new LavaGeyser((i + 0.5f) * LavaGeyser.size, delay + j * timeBetweenEruptions));
                    j++;
                }
            }
        }
        public static void EquallySpaced(float distance, float timeBeforeErupting, float offset)
        {
            int totalEruptions = (int)((MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomWidthtPx - 1 + 2 * distance) / (distance));
            for (int i = 0; i < totalEruptions; i++)
            {
                lavaGeysers.Add(new LavaGeyser((i + 0.5f + offset) * distance, timeBeforeErupting));
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
        public static bool LavaGeyserIntersectsRectangle(Rectangle collisionRect)
        {
            foreach (LavaGeyser lg in lavaGeysers)
            {
                if (lg.CollisionRectangle.Intersects(collisionRect))
                    return true;
            }
            return false;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (LavaGeyser lg in lavaGeysers)
                lg.Draw(spriteBatch);
        }
        #endregion
    }
}
