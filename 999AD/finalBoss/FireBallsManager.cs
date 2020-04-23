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
    public static class FireBallsManager
    {
        #region DECLARATIONS
        static List<FireBall> fireballs = new List<FireBall>();
        static Random rand = new Random();
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D spritesheet)
        {
            FireBall.spritesheet = spritesheet;
        }
        public static void ThrowAtPlayer(int amount, float angularVelocity)
        {
            if (fireballs.Count > 0)
                return;
            for (int i =0; i<amount; i++)
            fireballs.Add(new FireBall(360f/amount*i, new float[] { 20,0, 0 }, new float[] { angularVelocity, angularVelocity, angularVelocity }, new float[] { 2,2*i, 3 }, true));
        }
        public static void ThrowInAllDirections(int amount, float shotVelocity, float angularVelocity)
        {
            if (fireballs.Count > 0)
                return;
            float time = 2 + 2 * (float)rand.NextDouble();
            for (int i = 0; i < amount; i++)
                fireballs.Add(new FireBall(360f / amount * i,
                    new float[] { 20, 0, shotVelocity },
                    new float[] { angularVelocity, angularVelocity, 0 },
                    new float[] { 2, time , 5 }
                    ));
        }
        public static void TrowWithinCircularSector(int amount, float shotVelocity, float angularVelocity, float amplitudeDegrees)
        {
            if (fireballs.Count > 0)
                return;
            float time = 3 + 2 * (float)rand.NextDouble();
            float angle = rand.Next(360);
            for (int i = 0; i < amount; i++)
                fireballs.Add(new FireBall(angle+amplitudeDegrees/amount * i,
                    new float[] { 20, 0, shotVelocity },
                    new float[] { angularVelocity, angularVelocity, 0 },
                    new float[] { 2, time, 5 }
                    ));
        }
        public static void Sweep(float angularVelocity, float lifetime)
        {
            if (fireballs.Count > 0)
                return;
            int angle = rand.Next(0, 360);
            int sign = 1 - 2 * rand.Next(0, 2);
            for (int i = 0; i < 10; i++)
                fireballs.Add(new FireBall(angle,
                    new float[] { 0,0,100, 0}, 
                    new float[] { 0,0,0, sign*angularVelocity}, 
                    new float[] { 0.2f*i,0.2f*i,5-0.4f*i, lifetime }));
        }
        public static void RandomSweep(float angularVelocity, float maxTimeWithoutInverion, int numberOfInversions)
        {
            if (fireballs.Count > 0)
                return;
            int angle = rand.Next(0, 360);
            int sign = 1 - 2 * rand.Next(0, 2);
            float[] times = new float[numberOfInversions+1];
            float[] angularVel = new float[numberOfInversions+1];
            float[] radialVel = new float[numberOfInversions+1];
            for (int i=0; i<= numberOfInversions; i++)
            {
                times[i] = maxTimeWithoutInverion*(float)rand.NextDouble();
                angularVel[i] = sign * angularVelocity;
                sign *= -1;
                radialVel[i] = 0;
            }
            for (int i = 0; i < 10; i++)
                fireballs.Add(new FireBall(angle,
                    new float[] { 0, 0, 100}.Concat(radialVel).ToArray(),
                    new float[] { 0, 0, 0}.Concat(angularVel).ToArray(),
                    new float[] { 0.2f * i, 0.2f * i, 5 - 0.4f * i}.Concat(times).ToArray()
                    ));
        }
        public static void Spiral(int amount, float angleOffset, float timeBetweenShots, float shotVelocity)
        {
            if (fireballs.Count > 0)
                return;
            for (int i = 0; i < amount; i++)
                fireballs.Add(new FireBall(angleOffset * i,
                    new float[] { 0,  shotVelocity},
                    new float[] { 0,0 },
                    new float[] { timeBetweenShots*i, 4 }
                    ));
        }
        public static void TargetPlatform(int[] platformIndexes, int amount, float lifetime)
        {
            if (fireballs.Count > 0)
                return;
            foreach (int i in platformIndexes)
            {
                float angle = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].AngleRadiants;
                float angularSpeed = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].AngularSpeed;
                float platformWidth = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].width;
                float platformHeight = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].height;
                float radius = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].radius;
                for (int count = 0; count < amount; count++)
                {
                    fireballs.Add(new FireBall(
                    angle*180/MathHelper.Pi,
                    new float[] { 0 },
                    new float[] { angularSpeed },
                    new float[] { lifetime },
                    false,
                    MathHelper.Lerp(-platformWidth / 2f, platformWidth / 2f, (count+0.5f) / amount),
                    -platformHeight/2,
                    radius));
                }
            }
        }
        #endregion
        #region METHODS
        public static void Update(float elapsedTime)
        {
            for (int i=fireballs.Count-1; i>=0; i--)
            {
                if (fireballs[i].Active)
                    fireballs[i].Update(elapsedTime);
                else
                    fireballs.RemoveAt(i);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (FireBall fb in fireballs)
                fb.Draw(spriteBatch);
        }
        #endregion
    }
}
