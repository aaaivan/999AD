using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace _999AD
{
    static class FinalBoss
    {
        public enum BossAnimations
        {
            stone, stoneToIdle, idle, attack,startRecovering, recovering, endRecovering, falling, total
        }
        public enum WingAnimations
        {
            stoneIdle, stoneToIdle, idle, spread, flap, withdraw, withdrawDead, total
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
        static Animation[] rightWingAnimations;
        static Animation[] leftWingAnimations;
        static public readonly float recoveryTime= 15;
        static float elapsedRecoveryTime;
        static public readonly float attackAnimationTime = 3;
        static float elapsedAttackAnimationTime;
        static public readonly int bossWidth = 80;
        static public readonly int bossHeight = 128;
        static public readonly int wingWidth = 136;
        static public readonly int wingHeight = 8;
        static public readonly Vector2 fireballsCenter = new Vector2(384, 338);
        static Vector2 bossInitialMidPoint;
        static public Vector2 bossMidPoint;
        static public readonly int wingsRelativeYPosition = -2;
        static float YSpeed;
        static BossAnimations bossAnimation;
        static WingAnimations rightWingAnimation;
        static WingAnimations leftWingAnimation;
        static WingTextures rightWingTexture;
        static WingTextures leftWingTexture;
        static Phases currentPhase;
        static readonly int maxBossHp = 20;
        static readonly int maxWingHP = 2;
        static int bossHP;
        static int rightWingHP;
        static int leftWingHP;
        static readonly Random rand = new Random();
        static bool dead = false;
        static Color bossColor;
        static readonly int framesOfDifferentColor = 5;
        static int frameCount;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _bossSpritesheet, Texture2D[] _wingSpritesheets)
        {
            Reset();
            bossSpritesheet = _bossSpritesheet;
            wingSpritesheets = _wingSpritesheets;
            bossAnimations = new Animation[(int)BossAnimations.total]
                {
                    new Animation(new Rectangle(0,0,288, 256),288,256, 1, 0, false, true),
                    new Animation(new Rectangle(0,0,2016, 1024),288,256, 22, 0.2f, false, true),
                    new Animation(new Rectangle(288,768,1728, 256),288,256,6, 0.2f, true),
                    new Animation(new Rectangle(0,1024,1728, 256),288,256,6, 0.2f, true),
                    new Animation(new Rectangle(0,1280,2016, 256),288,256,7, 0.2f, false, true),
                    new Animation(new Rectangle(0,1536,1728, 256),288,256,6, 0.2f, true),
                    new Animation(new Rectangle(0,1792,2016, 256),288,256,7, 0.2f, false, true),
                    new Animation(new Rectangle(0,1536,1728, 256),288,256,6, 0.2f, true),
                };
            rightWingAnimations = new Animation[(int)WingAnimations.total]
                {
                    new Animation(new Rectangle(0,0,144, 256), 144, 256, 1, 0f, false, true),
                    new Animation(new Rectangle(0,0,1584, 512), 144, 256, 22, 0.2f, false, true),
                    new Animation(new Rectangle(0,0,864, 256), 144, 256, 6, 0.2f, true),
                    new Animation(new Rectangle(0,768,1008, 256), 144, 256, 7, 0.2f, false, true),
                    new Animation(new Rectangle(0,512,864, 256), 144, 256, 6, 0.2f, true),
                    new Animation(new Rectangle(0,768,1008, 256), 144, 256, 7, 0.2f, false, true),
                    new Animation(new Rectangle[]{new Rectangle(864, 768, 144, 256),
                                                  new Rectangle(720, 768, 144, 256),
                                                  new Rectangle(576, 768, 144, 256),
                                                  new Rectangle(432, 768, 144, 256),
                                                  new Rectangle(288, 768, 144, 256),
                                                  new Rectangle(144, 768, 144, 256),
                                                  new Rectangle(0, 768, 144, 256),
                                                    }, 0.2f, false, true),
                };
            leftWingAnimations= new Animation[(int)WingAnimations.total]
                {
                    new Animation(new Rectangle(0,0,144, 256), 144, 256, 1, 0f, false, true),
                    new Animation(new Rectangle(0,0,1584, 512), 144, 256, 22, 0.2f, false, true),
                    new Animation(new Rectangle(0,0,864, 256), 144, 256, 6, 0.2f, true),
                    new Animation(new Rectangle(0,768,1008, 256), 144, 256, 7, 0.2f, false, true),
                    new Animation(new Rectangle(0,512,864, 256), 144, 256, 6, 0.2f, true),
                    new Animation(new Rectangle(0,768,1008, 256), 144, 256, 7, 0.2f, false, true),
                                        new Animation(new Rectangle[]{new Rectangle(864, 768, 144, 256),
                                                  new Rectangle(720, 768, 144, 256),
                                                  new Rectangle(576, 768, 144, 256),
                                                  new Rectangle(432, 768, 144, 256),
                                                  new Rectangle(288, 768, 144, 256),
                                                  new Rectangle(144, 768, 144, 256),
                                                  new Rectangle(0, 768, 144, 256),
                                                    }, 0.2f, false, true),
                };
        }
        public static void Reset()
        {
            elapsedRecoveryTime = 0;
            elapsedAttackAnimationTime = 0;
            YSpeed = 0;
            bossAnimation = BossAnimations.stone;
            rightWingAnimation = WingAnimations.stoneIdle;
            leftWingAnimation = WingAnimations.stoneIdle;
            rightWingTexture = WingTextures.stone;
            leftWingTexture = WingTextures.stone;
            currentPhase = Phases.one;
            bossColor = Color.White;
            frameCount = 0;
            bossInitialMidPoint = new Vector2(fireballsCenter.X, fireballsCenter.Y - 13);
            bossHP = maxBossHp;
            rightWingHP = maxWingHP;
            leftWingHP = maxWingHP;
            bossMidPoint = bossInitialMidPoint + new Vector2(0, 5);
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
                if (Player.position.X<bossMidPoint.X || bossAnimation== BossAnimations.stone)
                    return new Rectangle((int)(bossMidPoint.X - 126),
                                     (int)(bossMidPoint.Y - 134),
                                     bossAnimations[(int)bossAnimation].Frame.Width,
                                     bossAnimations[(int)bossAnimation].Frame.Height);
                else
                    return new Rectangle((int)(bossMidPoint.X - 162),
                                         (int)(bossMidPoint.Y - 134),
                                         bossAnimations[(int)bossAnimation].Frame.Width,
                                         bossAnimations[(int)bossAnimation].Frame.Height);
            }
        }
        public static Rectangle RightWingCollisionRectangle //improve: adjust based on sprite
        {
            get
            {
                if (bossAnimation != BossAnimations.recovering)
                    return new Rectangle(0, 0, 0, 0);
                if (Player.position.X < bossMidPoint.X)

                    return new Rectangle((int)bossMidPoint.X+18,
                                    (int)(bossMidPoint.Y - wingsRelativeYPosition),
                                    wingWidth,
                                    wingHeight);
                else
                    return new Rectangle((int)bossMidPoint.X - 18,
                                    (int)(bossMidPoint.Y - wingsRelativeYPosition),
                                    wingWidth,
                                    wingHeight);
            }
        }
        public static Rectangle LeftWingCollisionRectangle //improve: adjust based on sprite
        {
            get
            {
                if (bossAnimation != BossAnimations.recovering)
                    return new Rectangle(0, 0, 0, 0);
                if (Player.position.X < bossMidPoint.X)

                    return new Rectangle((int)bossMidPoint.X + 18-wingWidth,
                                    (int)(bossMidPoint.Y + wingsRelativeYPosition),
                                    wingWidth,
                                    wingHeight);
                else
                    return new Rectangle((int)bossMidPoint.X - 18-wingWidth,
                                    (int)(bossMidPoint.Y + wingsRelativeYPosition),
                                    wingWidth,
                                    wingHeight);
            }
        }

        public static Rectangle RightWingDrawRectangle
        {
            get
            {
                if (Player.position.X < bossMidPoint.X || bossAnimation == BossAnimations.stone)
                    return new Rectangle((int)bossMidPoint.X+38,
                                    (int)(bossMidPoint.Y + wingsRelativeYPosition-118),
                                    rightWingAnimations[(int)rightWingAnimation].Frame.Width,
                                    rightWingAnimations[(int)rightWingAnimation].Frame.Height);
                else
                    return new Rectangle((int)bossMidPoint.X -2,
                        (int)(bossMidPoint.Y + wingsRelativeYPosition-118),
                        rightWingAnimations[(int)rightWingAnimation].Frame.Width,
                        rightWingAnimations[(int)rightWingAnimation].Frame.Height);

            }
        }
        public static Rectangle LeftWingDrawRectangle
        {
            get
            {
                if (Player.position.X < bossMidPoint.X || bossAnimation == BossAnimations.stone)
                    return new Rectangle((int)bossMidPoint.X - leftWingAnimations[(int)leftWingAnimation].Frame.Width-2,
                              (int)(bossMidPoint.Y + wingsRelativeYPosition-118),
                              leftWingAnimations[(int)leftWingAnimation].Frame.Width,
                              leftWingAnimations[(int)leftWingAnimation].Frame.Height);
                else
                    return new Rectangle((int)bossMidPoint.X - leftWingAnimations[(int)leftWingAnimation].Frame.Width-38,
                              (int)(bossMidPoint.Y + wingsRelativeYPosition-118),
                              leftWingAnimations[(int)leftWingAnimation].Frame.Width,
                              leftWingAnimations[(int)leftWingAnimation].Frame.Height);

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
            bossAnimations[(int)bossAnimation].Reset();
            rightWingAnimation = WingAnimations.stoneToIdle;
            leftWingAnimation = WingAnimations.stoneToIdle;
            rightWingAnimations[(int)rightWingAnimation].Reset();
            leftWingAnimations[(int)leftWingAnimation].Reset();
            MediaPlayer.Play(SoundEffects.FinaBossSoundTrack);
            MediaPlayer.IsRepeating = true;
        }
        public static void Update(float elapsedTime)
        {
            if (dead||bossAnimation==BossAnimations.stone)
                return;
            if (bossAnimation!=BossAnimations.stone &&
                bossAnimation != BossAnimations.stoneToIdle
                && bossAnimation != BossAnimations.falling)
            {
                bossMidPoint.Y += YSpeed;
                YSpeed += (bossInitialMidPoint.Y - bossMidPoint.Y)*0.2f*elapsedTime;
            }
            bossAnimations[(int)bossAnimation].Update(elapsedTime);
            rightWingAnimations[(int)rightWingAnimation].Update(elapsedTime);
            leftWingAnimations[(int)leftWingAnimation].Update(elapsedTime);
            if (!bossAnimations[(int)bossAnimation].Active)
            {
                if (bossAnimation == BossAnimations.startRecovering)
                {
                    bossAnimation = BossAnimations.recovering;
                    rightWingAnimation = WingAnimations.flap;
                    leftWingAnimation = WingAnimations.flap;
                    bossAnimations[(int)bossAnimation].Reset();
                    rightWingAnimations[(int)rightWingAnimation].Reset();
                    leftWingAnimations[(int)leftWingAnimation].Reset();
                }
                else
                {
                    bossAnimation = BossAnimations.idle;
                    rightWingAnimation = WingAnimations.idle;
                    leftWingAnimation = WingAnimations.idle;
                    if (rightWingTexture == WingTextures.stone || leftWingTexture== WingTextures.stone)
                    {
                        rightWingTexture = WingTextures.healthy;
                        leftWingTexture = WingTextures.healthy;
                    }
                    bossAnimations[(int)bossAnimation].Reset();
                    rightWingAnimations[(int)rightWingAnimation].Reset();
                    leftWingAnimations[(int)leftWingAnimation].Reset();
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
                    if (!bossAnimations[(int)bossAnimation].IsLastFrame())
                        break;
                    switch (currentPhase)
                    {
                        case Phases.one:
                            {
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.startRecovering;
                                    bossAnimations[(int)BossAnimations.startRecovering].Reset();
                                    LavaGeyserManager.Reset();
                                    FireBallsManager.Reset();
                                    rightWingAnimation = WingAnimations.spread;
                                    leftWingAnimation = WingAnimations.spread;
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                    break;
                                }
                                if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                                    break;
                                int attack = rand.Next(4);
                                if (attack == 0)
                                {
                                    FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(3, 6, 1 - (float)bossHP / maxBossHp), 2+(float)rand.NextDouble(), 1f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else if (attack == 1)
                                {
                                    FireBallsManager.ThrowInAllDirections((int)MathHelper.Lerp(3, 7, 1 - (float)bossHP / maxBossHp), FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();

                                }
                                else if (attack == 2)
                                {
                                    int num = (int)MathHelper.Lerp(1, 3, 1 - (float)bossHP / maxBossHp);
                                    int[] platforms = new int[num];
                                    for (int i = 0; i < num; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) % (PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, (int)MathHelper.Lerp(1, 6, 1 - (float)bossHP / maxBossHp), 2);
                                    FireBallsManager.AddGhostFireball(9);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else
                                {
                                    FireBallsManager.Sweep((float)(0.4 + 0.2*rand.NextDouble()), rand.Next(3, 5));
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                SoundEffects.FinalBossAttack.Play();
                                break;
                            }
                        case Phases.two:
                            {
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.startRecovering;
                                    bossAnimations[(int)BossAnimations.startRecovering].Reset();
                                    LavaGeyserManager.Reset();
                                    FireBallsManager.Reset();
                                    rightWingAnimation = WingAnimations.spread;
                                    leftWingAnimation = WingAnimations.spread;
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
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
                                        FireBallsManager.ThrowAtPlayer((int)MathHelper.Lerp(4, 9, 1 - (float)bossHP / maxBossHp), (float)(3 + rand.NextDouble()), 1f);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    else if (attack == 1)
                                    {
                                        FireBallsManager.ThrowInAllDirections(6, FireBall.radialShootingVelocity, (float)(1 + rand.NextDouble()));
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    else
                                    {
                                        int num = (int)MathHelper.Lerp(2, 4, 1 - (float)bossHP / maxBossHp);
                                        int[] platforms = new int[num];
                                        for (int i = 0; i < num; i++)
                                        {
                                            int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                            while (platforms.Contains(nextPlatform))
                                            {
                                                nextPlatform = (nextPlatform + 1) % (PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                            }
                                            platforms[i] = nextPlatform;
                                        }
                                        FireBallsManager.TargetPlatform(platforms, 6, 2);
                                        FireBallsManager.AddGhostFireball(7);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                }
                                else if (attack == 1)
                                {
                                    float angle = MathHelper.Lerp(60, 120, 1 - (float)bossHP / maxBossHp);
                                    FireBallsManager.TrowWithinCircularSector(
                                        (int)(angle / 6),
                                        FireBall.radialShootingVelocity * 0.8f,
                                        (float)(0.25 + 0.5*rand.NextDouble()),
                                        angle);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else
                                {
                                    FireBallsManager.RandomSweep((float)(0.8f + 0.2*rand.NextDouble()), 2, 4);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                SoundEffects.FinalBossAttack.Play();
                                break;
                            }
                        case Phases.three:
                            {
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.startRecovering;
                                    bossAnimations[(int)BossAnimations.startRecovering].Reset();
                                    LavaGeyserManager.Reset();
                                    FireBallsManager.Reset();
                                    rightWingAnimation = WingAnimations.spread;
                                    leftWingAnimation = WingAnimations.spread;
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
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
                                            (float)(0.4 + 0.2*rand.NextDouble()),
                                            angle);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    else
                                    {
                                        FireBallsManager.RandomSweep((float)(1+0.5*rand.NextDouble()), 4, 5);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                }
                                else if (attack == 1)
                                {
                                    int num = 2;
                                    int[] platforms = new int[num];
                                    for (int i = 0; i < num; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) % (PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
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
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    else if (attack == 1)
                                    {
                                        FireBallsManager.ThrowInAllDirections(6, FireBall.radialShootingVelocity, (float)(0.5 + rand.NextDouble()));
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                    else
                                    {
                                        FireBallsManager.TrowWithinCircularSector(
                                            20,
                                            FireBall.radialShootingVelocity * 0.8f,
                                            (float)(0.5 + 0.1*rand.NextDouble()),
                                            120);
                                        bossAnimation = BossAnimations.attack;
                                        bossAnimations[(int)BossAnimations.attack].Reset();
                                        rightWingAnimations[(int)rightWingAnimation].Reset();
                                        leftWingAnimations[(int)leftWingAnimation].Reset();
                                    }
                                }
                                else if (attack == 2)
                                {
                                    FireBallsManager.Spiral(30, 20, MathHelper.Lerp(0.3f, 0.5f, (float)bossHP / maxBossHp), FireBall.radialShootingVelocity * 0.4f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else
                                {
                                    if ((float)bossHP / maxBossHp < 0.3f)
                                        LavaGeyserManager.EquallySpaced(LavaGeyser.size * 7, 2, 0);
                                    else
                                        LavaGeyserManager.ShootGeyser(
                                        new float[] { rand.Next((int)fireballsCenter.X - 350, (int)fireballsCenter.X + 350) },
                                        3);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                SoundEffects.FinalBossAttack.Play();
                                break;
                            }
                        case Phases.four:
                            {
                                if (bossHP <= 0)
                                {
                                    bossAnimation = BossAnimations.startRecovering;
                                    bossAnimations[(int)BossAnimations.startRecovering].Reset();
                                    LavaGeyserManager.Reset();
                                    FireBallsManager.Reset();
                                    rightWingAnimation = WingAnimations.spread;
                                    leftWingAnimation = WingAnimations.spread;
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
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
                                        (int)MathHelper.Lerp(4, 10, (float)bossHP / maxBossHp),
                                        244,
                                        524,
                                        rand.Next(2) == 0);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else if (attack == 1)
                                {
                                    LavaGeyserManager.EquallySpaced(LavaGeyser.size *6, 2, LavaGeyser.size * 6 * (float)rand.NextDouble());
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                else if (attack == 2)
                                {
                                    int[] platforms = new int[2];
                                    for (int i = 0; i < 2; i++)
                                    {
                                        int nextPlatform = rand.Next(PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                        while (platforms.Contains(nextPlatform))
                                        {
                                            nextPlatform = (nextPlatform + 1) % (PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Length-1);
                                        }
                                        platforms[i] = nextPlatform;
                                    }
                                    FireBallsManager.TargetPlatform(platforms, 6, 5);
                                    FireBallsManager.Spiral(30, 20, 0.3f, FireBall.radialShootingVelocity * 0.4f);
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
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
                                            1);
                                    }
                                    else if (attack == 2)
                                    {
                                        FireBallsManager.ThrowInAllDirections(
                                            (int)MathHelper.Lerp(3, 7, 1 - (float)bossHP / maxBossHp),
                                            FireBall.radialShootingVelocity,
                                            1);
                                    }
                                    else
                                    {
                                        FireBallsManager.Sweep(0.5f, 5);
                                    }
                                    bossAnimation = BossAnimations.attack;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                }
                                SoundEffects.FinalBossAttack.Play();
                                break;
                            }
                    }
                    break;
                case BossAnimations.attack:
                    elapsedAttackAnimationTime += elapsedTime;
                    if (elapsedAttackAnimationTime>= attackAnimationTime)
                    {
                        elapsedAttackAnimationTime = 0;
                        bossAnimation = BossAnimations.idle;
                        bossAnimations[(int)BossAnimations.endRecovering].Reset();
                        rightWingAnimations[(int)rightWingAnimation].Reset();
                        leftWingAnimations[(int)leftWingAnimation].Reset();
                    }
                    break;
                case BossAnimations.startRecovering:
                    break;
                case BossAnimations.recovering:
                    {
                        if (bossAnimations[(int)bossAnimation].FrameIndex == 0
                            && bossAnimations[(int)bossAnimation].FreviousFrameIndex != 0)
                            SoundEffects.FinalBossRecover.Play();
                        else if (bossAnimations[(int)bossAnimation].FrameIndex == 2 &&
                            bossAnimations[(int)bossAnimation].FreviousFrameIndex != 2)
                            SoundEffects.FinalBossAwaken.Play();
                        elapsedRecoveryTime += elapsedTime;
                        if(elapsedRecoveryTime>= recoveryTime)
                        {
                            bossAnimation = BossAnimations.endRecovering;
                            if (rightWingHP>0)
                            {
                                rightWingAnimation = WingAnimations.withdraw;
                            }
                            else
                                rightWingAnimation = WingAnimations.withdrawDead;
                            if (leftWingHP>0)
                            {
                                leftWingAnimation = WingAnimations.withdraw;
                            }
                            else
                                leftWingAnimation = WingAnimations.withdrawDead;
                            bossAnimations[(int)BossAnimations.endRecovering].Reset();
                            rightWingAnimations[(int)rightWingAnimation].Reset();
                            leftWingAnimations[(int)leftWingAnimation].Reset();
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
                                    bossAnimation = BossAnimations.endRecovering;
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        rightWingAnimation = WingAnimations.withdrawDead;
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        leftWingAnimation = WingAnimations.withdrawDead;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                    SoundEffects.FinalBossHurt.Play();
                                    FireBallsManager.AddGhostFireball(3);
                                }
                                break;
                            case Phases.two:
                                if (rightWingHP + leftWingHP == 2)
                                {
                                    bossHP = maxBossHp;
                                    elapsedRecoveryTime = 0;
                                    currentPhase = Phases.three;
                                    bossAnimation = BossAnimations.endRecovering;
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        rightWingAnimation = WingAnimations.withdrawDead;
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        leftWingAnimation = WingAnimations.withdrawDead;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                    SoundEffects.FinalBossHurt.Play();
                                    FireBallsManager.ThrowAtPlayer(20,2,0.1f);
                                }
                                break;
                            case Phases.three:
                                if (rightWingHP + leftWingHP == 1)
                                {
                                    bossHP = maxBossHp;
                                    elapsedRecoveryTime = 0;
                                    currentPhase = Phases.four;
                                    bossAnimation = BossAnimations.endRecovering;
                                    if (rightWingHP > 0)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        rightWingAnimation = WingAnimations.withdrawDead;
                                    if (leftWingHP > 0)
                                    {
                                        leftWingAnimation = WingAnimations.withdraw;
                                    }
                                    else
                                        leftWingAnimation = WingAnimations.withdrawDead;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                    SoundEffects.FinalBossHurt.Play();
                                    LavaGeyserManager.SweepAcross(1,0f, 100, 159, 609, true);
                                    CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                                        new Collectable(new Point(376, 475), Collectable.ItemType.heart));
                                }
                                break;
                            case Phases.four:
                                if (rightWingHP + leftWingHP == 0)
                                {
                                    if (Player.CollisionRectangle.Right > bossMidPoint.X)
                                    {
                                        rightWingAnimation = WingAnimations.withdraw;
                                        leftWingAnimation = WingAnimations.withdrawDead;
                                    }
                                    else
                                    {
                                        rightWingAnimation = WingAnimations.withdrawDead;
                                        leftWingAnimation = WingAnimations.withdraw;
                                    }
                                    bossAnimation = BossAnimations.falling;
                                    bossAnimations[(int)BossAnimations.attack].Reset();
                                    rightWingAnimations[(int)rightWingAnimation].Reset();
                                    leftWingAnimations[(int)leftWingAnimation].Reset();
                                    elapsedRecoveryTime = 0;
                                    SoundEffects.FinalBossHurt.Play();
                                    FireBallsManager.Reset();
                                    LavaGeyserManager.Reset();
                                }
                                break;
                        }
                        break;
                    }
                case BossAnimations.endRecovering:
                    break;
                case BossAnimations.falling:
                    {
                        bossMidPoint.Y += YSpeed * elapsedTime;
                        YSpeed += Gravity.gravityAcceleration*elapsedTime;
                        if (BossDrawRectangle.Y>MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx)
                        {
                            dead = true;
                            MediaPlayer.Stop();
                        }
                        break;
                    }
            }
        }
        public static bool DamageWing(bool rightWing)
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
                    return true;
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
                    return true;
                }
            }
            return false;
        }
        public static bool WingHitByReactangle(Rectangle collisionRect, float yVelocity, float elapsedTime)
        {
            if (yVelocity <= 0)
                return false;
            Rectangle spaceSpanned;
            spaceSpanned = new Rectangle(collisionRect.X,
                                             (int)(collisionRect.Bottom - yVelocity * elapsedTime),
                                             collisionRect.Width,
                                             (int)(yVelocity * elapsedTime));
            if (rightWingAnimation == WingAnimations.flap && rightWingHP>0)
            {
                if (RightWingCollisionRectangle.Intersects(spaceSpanned))
                {
                    return DamageWing(true);
                }
            }
            if (leftWingAnimation == WingAnimations.flap && leftWingHP>0)
            {
                if (LeftWingCollisionRectangle.Intersects(spaceSpanned))
                {
                    return DamageWing(false);
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
            spriteBatch.Draw(wingSpritesheets[(int)rightWingTexture], Camera.RelativeRectangle(RightWingDrawRectangle), rightWingAnimations[(int)rightWingAnimation].Frame, Color.White);
            spriteBatch.Draw(wingSpritesheets[(int)leftWingTexture], Camera.RelativeRectangle(LeftWingDrawRectangle), leftWingAnimations[(int)leftWingAnimation].Frame, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 1); ;
            spriteBatch.Draw(bossSpritesheet,
                Camera.RelativeRectangle(BossDrawRectangle),
                bossAnimations[(int)bossAnimation].Frame, bossColor, 0,
                Vector2.Zero,
                (Player.position.X<bossMidPoint.X || bossAnimation== BossAnimations.stone)?SpriteEffects.None: SpriteEffects.FlipHorizontally, 0);
            if (frameCount >= framesOfDifferentColor)
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
