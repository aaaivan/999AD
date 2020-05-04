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
            stoneToIdle, idle, spread, flap, withdraw, total
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
        static public readonly float recoveryTime= 15;
        static float elapsedRecoveryTime = 0;
        static public readonly int bossWidth = 180;
        static public readonly int bossHeight = 280;
        static Vector2 bossInitialMidPoint;
        static public Vector2 bossMidPoint;
        static public readonly int wingsRelativeYPosition = 100;
        static float YSpeed = 0;
        static BossAnimations bossAnimation = BossAnimations.stone;
        static WingAnimations rightWingAnimation = WingAnimations.idle;
        static WingAnimations leftWingAnimation = WingAnimations.idle;
        static WingTextures rightWingTexture = WingTextures.stone;
        static WingTextures leftWingTexture = WingTextures.stone;
        static Phases currentPhase = Phases.one;
        static readonly int maxBossHp = 10;
        static readonly int maxWingHP = 2;
        static int bossHP;
        static int rightWingHP;
        static int leftWingHP;
        static Random rand = new Random();
        static bool dead = false;
        static Color bossColor = Color.White;
        static readonly int framesOfDifferentColor = 5;
        static int frameCount = 0;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _bossSpritesheet, Texture2D[] _wingSpritesheets)
        {
            bossInitialMidPoint = new Vector2(FireBallsManager.fireballsCenter.X, FireBallsManager.fireballsCenter.Y - 30);
            bossSpritesheet = _bossSpritesheet;
            wingSpritesheets = _wingSpritesheets;
            bossHP = maxBossHp;
            rightWingHP = maxWingHP;
            leftWingHP = maxWingHP;
            bossMidPoint = bossInitialMidPoint+new Vector2(0,5);
            bossAnimations = new Animation[(int)BossAnimations.total]
                {
                    new Animation(new Rectangle(0,0,100, 150),100,150, 1, 0, true),
                    new Animation(new Rectangle(0,0,400, 150),100,150, 4, 1, false, true),
                    new Animation(new Rectangle(0,150,200, 150),100,150,2, 0.5f, true),
                    new Animation(new Rectangle(0,300,200, 150),100,150,2, 1f, false, true),
                    new Animation(new Rectangle(0,450,200, 150),100,150,2, 0.5f, true),
                    new Animation(new Rectangle(0,600,200, 150),100,150,2, 0.5f, true),
                };
            wingAnimations = new Animation[(int)WingAnimations.total]
            {
                    new Animation(new Rectangle[]{ new Rectangle(0,0,50,100),
                                                   new Rectangle(50,0,50,100),
                                                   new Rectangle(100,0,50,100),
                                                   new Rectangle(150, 0, 50, 100)}, 3, false, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,0,50,100) }, 0, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,100,50,100),
                                                   new Rectangle(50, 100,100,100),
                                                   new Rectangle(150, 100, 150, 100),
                                                   new Rectangle(300, 100, 200, 100)}, 0.3f, false, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,200,180,100),
                                                   new Rectangle(180, 200,190,100),
                                                   new Rectangle(370, 200, 200, 100)}, 0.3f, true),
                    new Animation(new Rectangle[]{ new Rectangle(0,300,200,100),
                                                   new Rectangle(200, 300,150,100),
                                                   new Rectangle(350, 300, 100, 100),
                                                   new Rectangle(450, 300, 50, 100)}, 0.3f, false, true),
            };
        }
        #endregion
        #region PROPERTIES
        public static Rectangle BossCollisionRectangle
        {
            get { return new Rectangle((int)(bossMidPoint.X - bossWidth / 2), (int)(bossMidPoint.Y - bossHeight / 2), bossWidth, bossHeight); }
        }
        public static Rectangle BossDrawRectangle //improve: adjust based on sprite
        {
            get
            {
                return new Rectangle((int)(bossMidPoint.X - bossAnimations[(int)bossAnimation].Frame.Width / 2),
                                     (int)(bossMidPoint.Y - bossAnimations[(int)bossAnimation].Frame.Height / 2),
                                     bossAnimations[(int)bossAnimation].Frame.Width,
                                     bossAnimations[(int)bossAnimation].Frame.Height);
            }
        }
        public static Rectangle RightWingCollisionRectangle //improve: adjust based on sprite
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X,
                                    (int)(bossMidPoint.Y - wingsRelativeYPosition),
                                    wingAnimations[(int)rightWingAnimation].Frame.Width,
                                    8);
            }
        }
        public static Rectangle LeftWingCollisionRectangle //improve: adjust based on sprite
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X - wingAnimations[(int)leftWingAnimation].Frame.Width,
                              (int)(bossMidPoint.Y - wingsRelativeYPosition),
                              wingAnimations[(int)leftWingAnimation].Frame.Width,
                              8);
            }
        }

        public static Rectangle RightWingDrawRectangle
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X,
                                    (int)(bossMidPoint.Y - wingsRelativeYPosition),
                                    wingAnimations[(int)rightWingAnimation].Frame.Width,
                                    wingAnimations[(int)rightWingAnimation].Frame.Height);
            }
        }
        public static Rectangle LeftWingDrawRectangle
        {
            get
            {
                return new Rectangle((int)bossMidPoint.X - wingAnimations[(int)leftWingAnimation].Frame.Width,
                              (int)(bossMidPoint.Y - wingsRelativeYPosition),
                              wingAnimations[(int)leftWingAnimation].Frame.Width,
                              wingAnimations[(int)leftWingAnimation].Frame.Height);
            }
        }
        public static bool Dead
        {
            get { return dead; }
        }
        #endregion
        #region METHODS
        public static void WakeUp()
        {
            bossAnimation = BossAnimations.stoneToIdle;
            rightWingAnimation = WingAnimations.stoneToIdle;
            leftWingAnimation = WingAnimations.stoneToIdle;
        } 
        public static void Update(float elapsedTime)
        {
            if (dead||bossAnimation==BossAnimations.stone)
                return;
            if (bossAnimation==BossAnimations.idle || bossAnimation == BossAnimations.recovering)
            {
                bossMidPoint.Y += YSpeed;
                YSpeed += (bossInitialMidPoint.Y - bossMidPoint.Y)*0.2f*elapsedTime;
            }

            bossAnimations[(int)bossAnimation].Update(elapsedTime);
            wingAnimations[(int)rightWingAnimation].Update(elapsedTime);
            wingAnimations[(int)leftWingAnimation].Update(elapsedTime);
            if (!bossAnimations[(int)bossAnimation].Active)
            {
                bossAnimation = BossAnimations.idle;
            }
            if (!wingAnimations[(int)rightWingAnimation].Active)
            {
                if (rightWingAnimation == WingAnimations.spread)
                    rightWingAnimation = WingAnimations.flap;
                else
                {
                    if (rightWingAnimation == WingAnimations.stoneToIdle)
                        rightWingTexture = WingTextures.healthy;
                    rightWingAnimation = WingAnimations.idle;
                }
            }
            if (!wingAnimations[(int)leftWingAnimation].Active)
            {
                if (leftWingAnimation == WingAnimations.spread)
                    leftWingAnimation = WingAnimations.flap;
                else
                {
                    if (leftWingAnimation == WingAnimations.stoneToIdle)
                        leftWingTexture = WingTextures.healthy;
                    leftWingAnimation = WingAnimations.idle;
                }
            }
            /*//debug only
            if (Game1.currentKeyboard.IsKeyDown(Keys.Down) && !Game1.previousKeyboard.IsKeyDown(Keys.Down))
                bossHP--;
            if (Game1.currentKeyboard.IsKeyDown(Keys.Left) && !Game1.previousKeyboard.IsKeyDown(Keys.Left))
                DamageWing(false);
            if (Game1.currentKeyboard.IsKeyDown(Keys.Right) && !Game1.previousKeyboard.IsKeyDown(Keys.Right))
                DamageWing(true);
            //end debug only*/
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
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.recovering;
                                    LavaGeyserManager.Clear();
                                    FireBallsManager.Clear();
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                                    break;
                                int attack = rand.Next(4);
                                if (attack == 0)
                                {
                                    FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(3, 6, 1 - (float)bossHP / maxBossHp), (float)(0.5 + rand.NextDouble()), 0.5f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 1)
                                {
                                    FireBallsManager.ThrowInAllDirections((int)MathHelper.Lerp(3, 7, 1 - (float)bossHP / maxBossHp), FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 2)
                                {
                                    int num = (int)MathHelper.Lerp(1, 3, 1 - (float)bossHP / maxBossHp);
                                    int[] platforms = new int[num];
                                    for (int i = 0; i < num; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) % PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, (int)MathHelper.Lerp(1, 6, 1 - (float)bossHP / maxBossHp), 2);
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
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.recovering;
                                    LavaGeyserManager.Clear();
                                    FireBallsManager.Clear();
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                                    break;
                                int attack = rand.Next(3);
                                if (attack == 0)
                                {
                                    attack = rand.Next(3);
                                    if (attack == 0)
                                    {
                                        FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(5, 11, 1 - (float)bossHP / maxBossHp), (float)(1 + rand.NextDouble()), 0.5f);
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
                                        int num = (int)MathHelper.Lerp(2, 4, 1 - (float)bossHP / maxBossHp);
                                        int[] platforms = new int[num];
                                        for (int i = 0; i < num; i++)
                                        {
                                            int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                            while (platforms.Contains(nextPlatform))
                                            {
                                                nextPlatform = (nextPlatform + 1) % PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
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
                                    float angle = MathHelper.Lerp(60, 120, 1 - (float)bossHP / maxBossHp);
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
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.recovering;
                                    LavaGeyserManager.Clear();
                                    FireBallsManager.Clear();
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                                    break;
                                int attack = rand.Next(4);
                                if (attack == 0)
                                {
                                    attack = rand.Next(2);
                                    if (attack == 0)
                                    {
                                        float angle = MathHelper.Lerp(120, 240, 1 - (float)bossHP / maxBossHp);
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
                                            nextPlatform = (nextPlatform + 1) % PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
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
                                    if ((float)bossHP / maxBossHp < 0.3f)
                                        LavaGeyserManager.EquallySpaced(LavaGeyser.size * 1.5f, 2, 0);
                                    else
                                        LavaGeyserManager.ShootGeyser(
                                        new float[] { rand.Next((int)FireBallsManager.fireballsCenter.X - 350, (int)FireBallsManager.fireballsCenter.X + 350) },
                                        3);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                break;
                            }
                        case Phases.four:
                            {
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.recovering;
                                    LavaGeyserManager.Clear();
                                    FireBallsManager.Clear();
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.spread;
                                        wingAnimations[(int)WingAnimations.spread].Reset();
                                    }
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                                    break;
                                int attack = rand.Next(5);
                                if (attack == 0)
                                {
                                    LavaGeyserManager.SweepAcross(
                                        1,
                                        0.5f,
                                        (int)MathHelper.Lerp(2, 4, (float)bossHP / maxBossHp),
                                        244,
                                        524,
                                        rand.Next(2) == 0);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 1)
                                {
                                    float distance = LavaGeyser.size * MathHelper.Lerp(2, 3, (float)bossHP / maxBossHp);
                                    LavaGeyserManager.EquallySpaced(distance, 2, distance * (float)rand.NextDouble());
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else if (attack == 2)
                                {
                                    int[] platforms = new int[2];
                                    for (int i = 0; i < 2; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) % PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length;
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, 6, 5);
                                    FireBallsManager.Spiral(30, 10, 1, FireBall.radialShootingVelocity * 0.8f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                else
                                {
                                    attack = rand.Next(3);
                                    LavaGeyserManager.ShootGeyser(new float[] { rand.Next(300, 700) }, 3);
                                    if (attack == 0)
                                    {
                                        FireBallsManager.ThrowAtPlayer(
                                            (int)MathHelper.Lerp(3, 7, 1 - (float)bossHP / maxBossHp),
                                            1.5f,
                                            2);
                                    }
                                    else if (attack == 2)
                                    {
                                        FireBallsManager.ThrowInAllDirections(
                                            (int)MathHelper.Lerp(3, 7, 1 - (float)bossHP / maxBossHp),
                                            FireBall.radialShootingVelocity,
                                            2);
                                    }
                                    else
                                    {
                                        FireBallsManager.Sweep(1, 8);
                                    }
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                }
                                break;
                            }
                    }
                    break;
                case BossAnimations.attack:
                    {
                        bossMidPoint.Y = bossInitialMidPoint.Y+5;
                        break;
                    }
                case BossAnimations.recovering:
                    {
                        elapsedRecoveryTime += elapsedTime;
                        if(elapsedRecoveryTime>= recoveryTime)
                        {
                            bossAnimation = BossAnimations.idle;
                            if (rightWingAnimation != WingAnimations.idle)
                            {
                                rightWingAnimation = WingAnimations.withdraw;
                                wingAnimations[(int)rightWingAnimation].Reset();
                            }
                            if (leftWingAnimation != WingAnimations.idle)
                            {
                                leftWingAnimation = WingAnimations.withdraw;
                                wingAnimations[(int)leftWingAnimation].Reset();
                            }
                            elapsedRecoveryTime = 0;
                            bossHP = maxBossHp / 2;
                            break;
                        }
                        switch(currentPhase)
                        {
                            case Phases.one:
                                if (rightWingHP+leftWingHP==3)
                                {

                                    bossHP = maxBossHp;
                                    elapsedRecoveryTime = 0;
                                    currentPhase = Phases.two;
                                    bossAnimation = BossAnimations.attack;
                                    if (rightWingAnimation != WingAnimations.idle)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)rightWingAnimation].Reset();
                                    }
                                    if (leftWingAnimation != WingAnimations.idle)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    FireBallsManager.AddGhostFireball(3);
                                }
                                break;
                            case Phases.two:
                                if (rightWingHP + leftWingHP == 2)
                                {
                                    bossHP = maxBossHp;
                                    elapsedRecoveryTime = 0;
                                    currentPhase = Phases.three;
                                    bossAnimation = BossAnimations.attack;
                                    if (rightWingAnimation != WingAnimations.idle)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)rightWingAnimation].Reset();
                                    }
                                    if (leftWingAnimation != WingAnimations.idle)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    FireBallsManager.ThrowAtPlayer(20,2,0.2f);
                                }
                                break;
                            case Phases.three:
                                if (rightWingHP + leftWingHP == 1)
                                {
                                    bossHP = maxBossHp;
                                    elapsedRecoveryTime = 0;
                                    currentPhase = Phases.four;
                                    bossAnimation = BossAnimations.attack;
                                    if (rightWingAnimation != WingAnimations.idle)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)rightWingAnimation].Reset();
                                    }
                                    if (leftWingAnimation != WingAnimations.idle)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    LavaGeyserManager.SweepAcross(1,0.2f, 100, 244, 524, true);
                                    LavaGeyserManager.SweepAcross(6,0.2f, 100, 244, 524, false);
                                }
                                break;
                            case Phases.four:
                                if (rightWingHP + leftWingHP == 0)
                                {
                                    if (rightWingAnimation != WingAnimations.idle)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)rightWingAnimation].Reset();
                                    }
                                    if (leftWingAnimation != WingAnimations.idle)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                        wingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    bossAnimation = BossAnimations.falling;
                                    elapsedRecoveryTime = 0;
                                    FireBallsManager.Clear();
                                    LavaGeyserManager.Clear();
                                }
                                break;
                        }
                        break;
                    }
                case BossAnimations.falling:
                    {
                        bossMidPoint.Y += YSpeed * elapsedTime;
                        YSpeed += Gravity.gravityAcceleration*elapsedTime;
                        if (BossDrawRectangle.Y>MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx)
                        {
                            dead = true;
                        }
                        break;
                    }
            }
        }
        public static void DamageWing(bool rightWing)
        {
            if (rightWing)
            {
                if (rightWingHP > 0)
                {
                    rightWingHP--;
                    if (rightWingHP == 1)
                        rightWingTexture = WingTextures.damaged;
                    else if (rightWingHP == 0)
                        rightWingTexture = WingTextures.dead;
                }
            }
            else
            {
                if (leftWingHP > 0)
                {
                    leftWingHP--;
                    if (leftWingHP == 1)
                        leftWingTexture = WingTextures.damaged;
                    else if (leftWingHP == 0)
                        leftWingTexture = WingTextures.dead;
                }
            }

        }
        public static bool WingHitByReactangle(Rectangle collisionRect, float yVelocity, float elapsedTime)
        {
            if (yVelocity <= 0)
                return false;
            Rectangle spaceSpanned;
            spaceSpanned = new Rectangle(collisionRect.X,
                                             (int)(collisionRect.Y - yVelocity * elapsedTime),
                                             collisionRect.Width,
                                             (int)(yVelocity * elapsedTime));
            if (rightWingAnimation == WingAnimations.flap)
            {
                if (RightWingCollisionRectangle.Intersects(spaceSpanned))
                {
                    DamageWing(true);
                    return true;
                }
            }
            if (leftWingAnimation == WingAnimations.flap)
            {
                if (LeftWingCollisionRectangle.Intersects(spaceSpanned))
                {
                    DamageWing(false);
                    return true;
                }
            }
            return false;
        }
        public static bool BossHitByRectangle(Rectangle collisionRect)
        {
            if (bossAnimation == BossAnimations.idle || bossAnimation == BossAnimations.attack)
            {
                if (BossCollisionRectangle.Intersects(collisionRect))
                {
                    bossHP-=1;
                    bossColor = Color.Red * 0.8f;
                    return true;
                }
            }
            return false;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (dead)
                return;
            spriteBatch.Draw(wingSpritesheets[(int)rightWingTexture], Camera.RelativeRectangle(RightWingDrawRectangle), wingAnimations[(int)rightWingAnimation].Frame, Color.White);
            spriteBatch.Draw(wingSpritesheets[(int)leftWingTexture], Camera.RelativeRectangle(LeftWingDrawRectangle), wingAnimations[(int)leftWingAnimation].Frame, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 1); ;
            spriteBatch.Draw(bossSpritesheet, Camera.RelativeRectangle(BossDrawRectangle), bossAnimations[(int)bossAnimation].Frame, bossColor);
            if (frameCount >= 5)
            {
                frameCount = 0;
                bossColor = Color.White;
            }
            else
                frameCount++;
        }
        #endregion

    }
}
