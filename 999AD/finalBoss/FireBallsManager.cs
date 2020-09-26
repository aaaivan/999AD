using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    public static class FireBallsManager
    {
        #region DECLARATIONS
        static public Vector2 fireballsCenter;
        static Texture2D spritesheet;
        static Rectangle laserSourceRect;
        static List<int> targetedPlatforms;
        static float relativeLaserProgression=0;
        static readonly float laserVelocity = 2f;
        static readonly float laserPersistanceTime = 4;
        static float elapsedLaserPersistanceTime;
        static int fireballsPerPlatform;
        static float targetPlatformLifetime;
        static List<FireBall> fireballs;
        static readonly Random rand = new Random();
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            laserSourceRect = new Rectangle(409, 41, 30, 180);
            FireBall.Inizialize(_spritesheet);
            fireballsCenter = FinalBoss.fireballsCenter;
            targetedPlatforms = new List<int>();
            fireballs = new List<FireBall>();
            Reset();
        }
        public static void Reset()
        {
            targetedPlatforms.Clear();
            fireballs.Clear();
            elapsedLaserPersistanceTime = 0;
            fireballsPerPlatform = 0;
            targetPlatformLifetime = 0;
        }
        #endregion
        #region PROPERTIES
        public static int NumberOfActiveFireballs
        {
            get { return fireballs.Count; }
        }
        #endregion
        #region FIREBALL PATTERNS
        public static void ThrowAtPlayer(int amount, float angularVelocity, float timeBtweenShots)
        {
            for (int i =0; i<amount; i++)
            fireballs.Add(new FireBall(360f/amount*i, new float[] { 20,0, 0, FireBall.radialShootingVelocity }, new float[] { angularVelocity, angularVelocity, angularVelocity,0 }, new float[] { 2,2+timeBtweenShots*i,5,4 }, true));
        }
        public static void ThrowInAllDirections(int amount, float shotVelocity, float angularVelocity)
        {
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
            int angle = rand.Next(0, 360);
            int sign = 1 - 2 * rand.Next(0, 2);
            for (int i = 0; i < 15; i++)
                fireballs.Add(new FireBall(angle,
                    new float[] { 0,100, 0}, 
                    new float[] { 0,0, sign*angularVelocity}, 
                    new float[] { 0.2f*i,3-0.2f*i, lifetime }));
        }
        public static void RandomSweep(float angularVelocity, float maxTimeWithoutInverion, int numberOfInversions)
        {
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
            for (int i = 0; i < 15; i++)
                fireballs.Add(new FireBall(angle,
                    new float[] { 0, 100}.Concat(radialVel).ToArray(),
                    new float[] { 0, 0}.Concat(angularVel).ToArray(),
                    new float[] { 0.2f * i, 3 - 0.2f * i}.Concat(times).ToArray()
                    ));
        }
        public static void Spiral(int amount, float angleOffset, float timeBetweenShots, float shotVelocity)
        {
            for (int i = 0; i < amount; i++)
                fireballs.Add(new FireBall(angleOffset * i,
                    new float[] { 0,  shotVelocity},
                    new float[] { 0,0 },
                    new float[] { timeBetweenShots*i, 4 }
                    ));
        }
        public static void TargetPlatform(int[] platformIndexes, int _fireballsPerPlatform, float lifetime)
        {
            if (targetedPlatforms.Count != 0)
                return;
            elapsedLaserPersistanceTime = 0;
            targetedPlatforms = new List<int>(platformIndexes);
            relativeLaserProgression = 0;
            fireballsPerPlatform = _fireballsPerPlatform;
            targetPlatformLifetime = lifetime;
        }
        static void TargetPlatformGo(int amount, float lifetime)
        {
            foreach (int i in targetedPlatforms)
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
        public static void AddGhostFireball(float lifetime)
        {
            fireballs.Add(new FireBall(0, 
                new float[] { 0 }, 
                new float[] { 0 }, 
                new float[] { lifetime }, 
                false, 0, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx));
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
            if (targetedPlatforms.Count!=0)
            {
                if (relativeLaserProgression < 1)
                {
                    relativeLaserProgression += laserVelocity * elapsedTime;
                    if (relativeLaserProgression > 1)
                        relativeLaserProgression = 1;
                }
                else
                {
                    elapsedLaserPersistanceTime += elapsedTime;
                    if (elapsedLaserPersistanceTime> laserPersistanceTime)
                    {
                        TargetPlatformGo(fireballsPerPlatform, targetPlatformLifetime);
                        targetedPlatforms.Clear();
                    }
                }
            }

        }
        public static bool FireballIntersectsRectangle(Rectangle collisionRect)
        {
            for (int i = fireballs.Count-1; i>=0; i--)
            {
                if (fireballs[i].CollisionRectangle.Intersects(collisionRect))
                {
                    fireballs.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (int i in targetedPlatforms)
            {
                spriteBatch.Draw(spritesheet,
                    Camera.RelativeRectangle( new Rectangle((int)fireballsCenter.X,
                                  (int)(fireballsCenter.Y-laserSourceRect.Width/2),
                                  laserSourceRect.Width,
                                  (int)((PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].radius+20) * relativeLaserProgression))
                    ),
                    laserSourceRect,
                    Color.White,
                    PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].AngleRadiants+MathHelper.Pi,
                    new Vector2(laserSourceRect.Width / 2, 0),
                    SpriteEffects.None,
                    1);
            }
            foreach (FireBall fb in fireballs)
                fb.Draw(spriteBatch);
        }
        #endregion
    }
}
