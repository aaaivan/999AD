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
    static class Collisions
    {
        public static void Update(float elapsedTiem)
        {
            PlayerCollisions(elapsedTiem);
            PlayerProjectilesCollisions();
            BossProjectilesCollisions();
        }
        static void PlayerCollisions(float elapsedTime)
        {
            #region COLLISION PLAYER-HARMFUL TILES
            if (MapsManager.maps[(int)RoomsManager.CurrentRoom].HarmfulTileIntersectsRectangle(Player.CollisionRectangle))
            {
                Player.takeDamage();
                return;
            }
            #endregion
            #region COLLISIONS PLAYER-FIREBALLS
            if (FireBallsManager.FireballIntersectsRectangle(Player.CollisionRectangle))
            {
                Player.takeDamage();
                return;
            }
            #endregion
            #region COLLISION PLAYER-LAVA GEYSER
            if (LavaGeyserManager.LavaGeyserIntersectsRectangle(Player.CollisionRectangle))
            {
                Player.takeDamage();
                return;
            }
            #endregion
            #region COLLISION PLAYER-FINAL BOSS WINGS
            if(RoomsManager.CurrentRoom==RoomsManager.Rooms.finalBoss)
            {
                if (FinalBoss.WingHitByReactangle(Player.CollisionRectangle, Player.JumpSpeed.Y, elapsedTime))
                    Player.Rebound(2);
            }
            #endregion
            #region COLLISION PLAYER-MIDBOSS
            //If the player is in the midboss room, and comes into contact with the midboss whilst it is moving
            //Player will take damage
            if(RoomsManager.CurrentRoom==RoomsManager.Rooms.midboss)
            {
                if (MidBoss.bossState == MidBoss.BossState.move && Player.CollisionRectangle.Intersects(MidBoss.BossCollisionRect))
                    Player.takeDamage();
            }
            #endregion
        }
        static void PlayerProjectilesCollisions()
        {
            foreach(Projectile projectile in ProjectilesManager.playerProjectiles)
            {
                if (RoomsManager.CurrentRoom==RoomsManager.Rooms.finalBoss)
                {
                    if (FinalBoss.BossHitByRectangle(projectile.Rectangle))
                        projectile.active = false;
                }

                if(RoomsManager.CurrentRoom==RoomsManager.Rooms.midboss)
                {
                    if (MidBoss.BossHitByRect(projectile.Rectangle))
                        projectile.active = false;
                }
            }
        }

        static void BossProjectilesCollisions()
        {
            foreach(Projectile projectile in ProjectilesManager.midbossProjectiles)
            {
                if(RoomsManager.CurrentRoom==RoomsManager.Rooms.midboss)
                {
                    if(MidBoss.PlayerHitByRect(projectile.Rectangle))
                    {
                        projectile.active = false;
                    }
                }
            }
        }
    }
}