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
        static Random rand= new Random();
        #endregion
        #region CONSTRUCTOR
        static public void Inizialize(Texture2D spritesheet)
        {
            LavaGeyser.spritesheet = spritesheet;
        }
        #endregion
        #region PROPERTIES
        public static int ActiveLavaGeysers
        {
            get { return lavaGeysers.Count; }
        }
        #endregion
        #region METHODS
        public static void ShootGeyser(float[] xPositions, float timeBeforeErupting )
        {
            foreach (float f in xPositions)
            {
                lavaGeysers.Add(new LavaGeyser(f, timeBeforeErupting));
            }
        }
        public static void SweepAcross(float timeBetweenEruptions, int safeAreas, float safeAreaMinX, float safeAreaMaxX, bool leftToright)
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
            if (leftToright)
            {
                for (int i =0, j=0; i<totalEruptions; i++)
                {
                    if (i==firstSafe)
                    {
                        i = i + safeAreas;
                    }
                    lavaGeysers.Add(new LavaGeyser((i+0.5f)* LavaGeyser.size, 1+j*timeBetweenEruptions));
                    j++;
                }
            }
            else
            {
                for (int i = 0, j=0; i < totalEruptions; i++)
                {
                    if (i == firstSafe)
                    {
                        i = i + safeAreas;
                    }
                    lavaGeysers.Add(new LavaGeyser((i + 0.5f) * LavaGeyser.size, 2 + timeBetweenEruptions*(totalEruptions - j - 1)));
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
        public static void Clear()
        {
            lavaGeysers.Clear();
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
