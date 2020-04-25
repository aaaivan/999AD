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
    static class FinalBoss
    {
        public enum BossAnimations
        {
            stone, stoneToIdle, idle, attack, recovering, falling, total
        }
        public enum WingAnimations
        {
            idle, spread, flap, withdraw, total
        }
        public enum WingTextures
        {
            stone, healthy, damaged, dead, total
        }
        public enum Phases
        {
           one, two, three, four, total
        }
        #region DECLARATIONS
        static Texture2D bossSpritesheet;
        static Texture2D[] wingSpritesheets;
        static Animation[] bossAnimations;
        static Animation[] wingAnimations;
        static public readonly float[] bossAnimationDuration = new float[(int)BossAnimations.total] { -1, 4, -1, 2, 15, -1 };
        static float elapsedAnimationTime = 0;
        static public readonly int bossWidth = 180;
        static public readonly int bossHeight = 280;
        static public readonly Vector2 bossMidPoint = new Vector2(496, 520);
        static public readonly int wingsRelativeYPosition = 200;
        static public BossAnimations bossAnimation= BossAnimations.stone;
        static WingAnimations rightWingAnimation= WingAnimations.idle;
        static WingAnimations leftWingAnimation= WingAnimations.idle;
        static WingTextures rightWingTexture = WingTextures.stone;
        static WingTextures leftWingTexture = WingTextures.stone;
        static Phases currentPhase= Phases.one;
        static readonly int maxBossHp=10;
        static int bossHP;
        static int rightWingHP = (int)Phases.total / 2;
        static int leftWingHP = (int)Phases.total / 2;
        static Random rand = new Random();
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _bossSpritesheet, Texture2D[] _wingSpritesheets)
        {
            bossSpritesheet = _bossSpritesheet;
            wingSpritesheets = _wingSpritesheets;
            bossHP = maxBossHp;
            bossAnimations = new Animation[(int)BossAnimations.total]
                {
                    new Animation(new Rectangle(0,0,200, 300),200,300,1, 0.5f, false),
                    new Animation(new Rectangle(0,300,200, 300),200,300,4, 1.5f, false, true),
                    new Animation(new Rectangle(0,600,200, 300),200,300,2, 0.5f, true),
                    new Animation(new Rectangle(0,900,200, 300),200,300,2, 0.5f, false, true),
                    new Animation(new Rectangle(0,1200,200, 300),200,300,2, 0.5f, true),
                    new Animation(new Rectangle(0,1500,200, 300),200,300,2, 0.5f, true),
                };
            wingAnimations = new Animation[(int)WingAnimations.total]
            {
                    new Animation(new Rectangle[]{ new Rectangle(0,0,50,100) }, 0, false, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,100,50,100),
                                                   new Rectangle(50, 100,100,100),
                                                   new Rectangle(150, 100, 150, 100),
                                                   new Rectangle(300, 100, 200, 100)}, 0.3f, false, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,200,180,100),
                                                   new Rectangle(180, 200,190,100),
                                                   new Rectangle(370, 200, 200, 100)}, 0.3f, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,300,200,100),
                                                   new Rectangle(50, 300,150,100),
                                                   new Rectangle(150, 300, 100, 100),
                                                   new Rectangle(300, 300, 50, 100)}, 0.3f, false, true),
            };
        }
        #endregion
        #region PROPERTIES
        public static Rectangle BossCollisionRectangle
        {
            get { return new Rectangle((int)(bossMidPoint.X - bossWidth / 2), (int)(bossMidPoint.Y - bossHeight / 2), bossWidth, bossHeight); }
        }
        public static Rectangle BossDrawRectangle
        {
            get
            {
                return new Rectangle((int)(bossMidPoint.X - bossAnimations[(int)bossAnimation].Frame.Width / 2),
                                     (int)(bossMidPoint.Y - bossAnimations[(int)bossAnimation].Frame.Height / 2),
                                     bossAnimations[(int)bossAnimation].Frame.Width,
                                     bossAnimations[(int)bossAnimation].Frame.Height);
            }
        }
        public static Rectangle RightWingRectangle
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X,
                                    (int)(bossMidPoint.Y - wingsRelativeYPosition),
                                    wingAnimations[(int)rightWingAnimation].Frame.Width,
                                    wingAnimations[(int)rightWingAnimation].Frame.Height);
            }
        }
        public static Rectangle LeftWingRectangle
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X- wingAnimations[(int)leftWingAnimation].Frame.Width,
                              (int)(bossMidPoint.Y - wingsRelativeYPosition),
                              wingAnimations[(int)leftWingAnimation].Frame.Width,
                              wingAnimations[(int)leftWingAnimation].Frame.Height);
            }
        }
        #endregion
        #region METHODS
        public static void Update(float elapsedTime)
        {
            bossAnimations[(int)bossAnimation].Update(elapsedTime);
            if (bossAnimationDuration[(int)bossAnimation]!=-1)
            {
                elapsedAnimationTime += elapsedTime;
                if (elapsedAnimationTime>= bossAnimationDuration[(int)bossAnimation])
                {
                    bossAnimation = BossAnimations.idle;
                    elapsedAnimationTime = 0;
                }
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
                bossHP--;
            if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
                leftWingHP--;
            if (Game1.currentKeyboard.IsKeyDown(Keys.Right))
                rightWingHP--;
            switch (bossAnimation)
            {
                case BossAnimations.stone:
                    break;
                case BossAnimations.stoneToIdle:
                    break;
                case BossAnimations.idle:
                    switch (currentPhase)
                    {
                        case Phases.one:
                            {
                                if (bossHP == 0)
                                {
                                    bossHP = maxBossHp;
                                    bossAnimation = BossAnimations.recovering;
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0)
                                    break;
                                int attack = rand.Next(4);
                                if (attack == 0)
                                {
                                    FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(3, 5, (float)bossHP / maxBossHp), (float)(0.5 + rand.NextDouble()), 1.5f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 1)
                                {
                                    FireBallsManager.ThrowInAllDirections((int)MathHelper.Lerp(3, 7, (float)bossHP / maxBossHp), FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 2)
                                {
                                    int num = (int)MathHelper.Lerp(1, 3, (float)bossHP / maxBossHp);
                                    int[] platforms = new int[num];
                                    for (int i = 0; i < num; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) / PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, (int)MathHelper.Lerp(1, 6, (float)bossHP / maxBossHp), 2);
                                    FireBallsManager.AddGhostFireball(9);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else
                                {
                                    FireBallsManager.Sweep((float)(0.5 + rand.NextDouble()), rand.Next(3, 7));
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                break;
                            }
                        case Phases.two:
                            {
                                if (bossHP == 0)
                                {
                                    bossHP = maxBossHp;
                                    bossAnimation = BossAnimations.recovering;
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0)
                                    break;
                                int attack = rand.Next(3);
                                if (attack == 0)
                                {
                                    attack = rand.Next(3);
                                    if (attack == 0)
                                    {
                                        FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(5, 11, (float)bossHP / maxBossHp), (float)(1 + rand.NextDouble()), 0.5f);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                    else if (attack == 1)
                                    {
                                        FireBallsManager.ThrowInAllDirections(6, FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                    else
                                    {
                                        int num = (int)MathHelper.Lerp(2, 4, (float)bossHP / maxBossHp);
                                        int[] platforms = new int[num];
                                        for (int i = 0; i < num; i++)
                                        {
                                            int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                            while (platforms.Contains(nextPlatform))
                                            {
                                                nextPlatform = (nextPlatform + 1) / PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
                                            }
                                            platforms[i] = nextPlatform;
                                        }
                                        FireBallsManager.TargetPlatform(platforms, 6, 2);
                                        FireBallsManager.AddGhostFireball(9);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                }
                                else if (attack == 1)
                                {
                                    float angle = MathHelper.Lerp(60, 120, (float)bossHP / maxBossHp);
                                    FireBallsManager.TrowWithinCircularSector(
                                        (int)(angle / 6),
                                        FireBall.radialShootingVelocity * 0.8f,
                                        (float)(0.5 + rand.NextDouble()),
                                        angle);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else
                                {
                                    FireBallsManager.RandomSweep((float)(1 + rand.NextDouble()), 3, 4);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                break;
                            }
                        case Phases.three:
                            {
                                if (bossHP == 0)
                                {
                                    bossHP = maxBossHp;
                                    bossAnimation = BossAnimations.recovering;
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0)
                                    break;
                                int attack = rand.Next(4);
                                if (attack == 0)
                                {
                                    attack = rand.Next(2);
                                    if (attack == 0)
                                    {
                                        float angle = MathHelper.Lerp(120, 240, (float)bossHP / maxBossHp);
                                        FireBallsManager.TrowWithinCircularSector(
                                            (int)(angle / 6),
                                            FireBall.radialShootingVelocity * 0.8f,
                                            (float)(1.5 + rand.NextDouble()),
                                            angle);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                    else
                                    {
                                        FireBallsManager.RandomSweep((float)(1 + rand.NextDouble()), 4, 5);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                }
                                else if (attack == 1)
                                {
                                    int num = 2;
                                    int[] platforms = new int[num];
                                    for (int i = 0; i < num; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) / PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, 6, 5);
                                    attack = rand.Next(3);
                                    if (attack == 0)
                                    {
                                        FireBallsManager.ThrowAtPlayer(6, (float)(1 + rand.NextDouble()), 0.5f);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                    else if (attack == 1)
                                    {
                                        FireBallsManager.ThrowInAllDirections(6, FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                    else
                                    {
                                        FireBallsManager.TrowWithinCircularSector(
                                            20,
                                            FireBall.radialShootingVelocity * 0.8f,
                                            (float)(0.5 + rand.NextDouble()),
                                            120);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                    }
                                }
                                else if (attack == 2)
                                {
                                    FireBallsManager.Spiral(30, 10, MathHelper.Lerp(0.5f, 1.5f, (float)bossHP / maxBossHp), FireBall.radialShootingVelocity * 0.8f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else
                                {
                                    LavaGeyserManager.ShootGeyser(
                                        new float[] { rand.Next((int)FireBallsManager.fireballsCenter.X - 350, (int)FireBallsManager.fireballsCenter.X + 350) },
                                        3);
                                }
                                break;
                            }
                    }
                    break;
                case BossAnimations.attack:
                    break;
                case BossAnimations.recovering:
                    break;
                case BossAnimations.falling:
                    break;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
        }
        #endregion

    }
}
