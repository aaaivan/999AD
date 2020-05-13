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
    static class PlatformsManager
    {
        #region DECLARATIONS
        public static PlatformsRoomManager[] platformsRoomManagers; //list of platform managers for each room
        public static int platformIndex; //index of the platform ridden by the player
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet)
        {
            MovingPlatform.loadTextures(_spritesheet);
            platformsRoomManagers = new PlatformsRoomManager[(int)RoomsManager.Rooms.total]
            {
                new PlatformsRoomManager //tutorial0
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(172,180), new Vector2(0,0), 0, 0, false),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(172,220), new Vector2(0,0), 0, 0, false),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 40, new Vector2(632,160), new Vector2(0,0), 1f, 0, true, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 40, new Vector2(632,160), new Vector2(0,0), 1f, 0, true, 180),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 5, new Vector2(856,181), new Vector2(720,181), 3, 70),
                    }
                ),
                new PlatformsRoomManager //tutorial1
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(180, 112), new Vector2(180, 220), 0, 50, true, 0, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(252, 112), new Vector2(252, 220), 0, 50, true, 0, 0.2f),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(324, 112), new Vector2(324, 220), 0, 50, true, 0, 0.4f),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(396, 112), new Vector2(396, 220), 0, 50, true, 0, 0.6f),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(468, 112), new Vector2(468, 220), 0, 50, true, 0, 0.8f),
                    }
                ),
                new PlatformsRoomManager //tutorial2
                (
                    new MovingPlatform[]
                    {
                    }
                ),
                new PlatformsRoomManager //tutorial3
                (
                    new MovingPlatform[]
                    {
                    }
                ),
                new PlatformsRoomManager //tutorial4
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0, new Vector2(172, 132), new Vector2(172, 20), 0, 50, true, 0, 0),
                    }
                ),
                new PlatformsRoomManager //churchBellTower0
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(180, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-492),
                                            new Vector2(0,0), 0, 0, false, 0,0,0,false, true, 3, 3,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(256, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-540),
                                            new Vector2(0,0), 0, 0, false, 0,0,0,false, true, 3,3, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(332, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-588),
                                            new Vector2(0,0), 0, 0, false, 0,0,0,false, true, 3,3, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(358, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-690),
                                            new Vector2(278, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-690),
                                            0, 80),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(222, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-690),
                                            new Vector2(142, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-690),
                                            0, 80),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(304, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-828),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 0.8f, 1f),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8,0,
                                            new Vector2(412, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-828),
                                            new Vector2(412,MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower0].RoomHeightPx-978),
                                            0, 150),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 900), new Vector2(468, 700), 0, 100, true, 0,0,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(452, 492), new Vector2(452, 692), 0, 100, true, 0,0,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 348), new Vector2(468, 444), 0, 96, true, 0,0,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(452, 340), new Vector2(452, 244), 0, 96, true, 0,0,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 52), new Vector2(0,0), 0,0,false),
                    }
                ),
                new PlatformsRoomManager //churchBellTower1
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(234, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-104),
                                            new Vector2(0,0),
                                            0,0,true,0,0,0,false, true, 0.8f, 1f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(68, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-172),
                                            new Vector2(68, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-326), 0,80),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 80,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero, -1f,0,true,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 80,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero, -1f,0,true,120),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 80,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero, -1f,0,true,240),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero, 1,0,true,180),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero,1,0,true,60),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(224, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-352),
                                            Vector2.Zero, 1,0,true,300),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(68, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-696),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 45,
                                            new Vector2(150, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            new Vector2(300, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            2*MathHelper.Pi/3f,100,true,60),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 45,
                                            new Vector2(150, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            new Vector2(300, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            2*MathHelper.Pi/3f,100,true,180),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 45,
                                            new Vector2(150, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            new Vector2(300, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-776),
                                            2*MathHelper.Pi/3f,100,true,300),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                           new Vector2(76, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-1012),
                                           new Vector2(76, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower1].RoomHeightPx-1116),
                                           0, 80),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 1124), new Vector2(0,0), 0,0,false),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(452, 764), new Vector2(452, 268), 0, 124, true, 0,0,1),
                    }
                ),
                new PlatformsRoomManager //churchBellTower2
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(164, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-164),
                                            new Vector2(0,0),
                                            0,0,false, 0,0,0,false, true, 0.75f, 0.75f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(244, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-164),
                                            new Vector2(0,0),
                                            0,0,false, 0,0,0,false, true, 1.5f, 1.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(180, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-336),
                                            new Vector2(180, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-456),
                                            2*MathHelper.Pi/3f,80,true,90),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(180, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-336),
                                            new Vector2(180, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-456),
                                            2*MathHelper.Pi/3f,80,true,270),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(292, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-556),
                                            new Vector2(292, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-436),
                                            2*MathHelper.Pi/3f,80,true,90),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(292, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-556),
                                            new Vector2(292, MapsManager.maps[(int)RoomsManager.Rooms.churchBellTower2].RoomHeightPx-436),
                                            2*MathHelper.Pi/3f,80,true,270),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(356, 532),
                                            new Vector2(108, 532),
                                            0,512, true, 0,0,0.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(80, 332),
                                            new Vector2(80, 540),
                                            0,416, true, 0,0,0.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(248, 348),
                                            new Vector2(136, 348),
                                            0,224, true, 0,0,0.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(416, 348),
                                            new Vector2(304, 348),
                                            0,224, true, 0,0,0.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(416, 220),
                                            new Vector2(416, 308),
                                            0,176, true, 0,0,0.5f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(336, 220),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 0 ),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(252, 220),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(180, 204),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(108, 180),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(108, 132),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(180, 108),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(252, 92),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(332, 92),
                                            new Vector2(0, 0),
                                            0,0, false, 0,0,0,false, true, 1f, 1f, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 1204), new Vector2(0,0), 0,0,false),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(468, 228), new Vector2(0,0), 0,0,false),
                    }
                ),
                new PlatformsRoomManager //midBoss
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(460, 52), new Vector2(0,0),
                                            0,0,false),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(460, 108), new Vector2(0,0),
                                            0,0,false),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(460, 164), new Vector2(0,0),
                                            0,0,false),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(460, 220), new Vector2(0,0),
                                            0,0,false),
                    }
                ),
                new PlatformsRoomManager //churchGroundFloor0
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(572, 444),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1, 1,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(646, 412),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1, 1, 1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(724, 444),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1, 1,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(796, 284),
                                            new Vector2(668,284),
                                            0,80,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(484, 284),
                                            new Vector2(612,284),
                                            0,80,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(420, 284),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1, 1,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(310, 300),
                                            new Vector2(0,0),2, 0, true, 0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(310, 300),
                                            new Vector2(0,0),2, 0, true, 120),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 40,
                                            new Vector2(310, 300),
                                            new Vector2(0,0),2, 0, true, 240),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 284),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 268),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,0.2f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 252),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,0.4f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 236),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,0.6f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 220),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,0.8f),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 204),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,1),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                            new Vector2(28, 188),
                                            new Vector2(0,0),
                                            0,0,false,0,0,0,false, true, 1.1f, 0.3f,1.2f),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(108, 168),
                                            new Vector2(1044,168),
                                            0,60,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(140, 116),
                                            new Vector2(1076,116),
                                            0,60,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(172, 68),
                                            new Vector2(1108,68),
                                            0,60,true),
                    }
                ),
                new PlatformsRoomManager //churchAltarRoom
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(28, 444),
                                            new Vector2(28,388),
                                            0,96,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(76, 348),
                                            new Vector2(76,404),
                                            0,96,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(28, 308),
                                            new Vector2(76,308),
                                            0,96,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(76,268),
                                            new Vector2(28, 268),
                                            0,96,true),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 48,
                                            new Vector2(52, 180),
                                            new Vector2(52, 148),
                                            3*MathHelper.Pi/4, 24,true, 180 ),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 24,
                                            new Vector2(52, 180),
                                            new Vector2(52, 148),
                                            -3*MathHelper.Pi/4, 24,true, 180 ),
                        new MovingPlatform(MovingPlatform.TextureType.platform24_8, 24,
                                            new Vector2(52, 180),
                                            new Vector2(52, 148),
                                            -3*MathHelper.Pi/4, 24,true, 0 ),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(52, 36),
                                            new Vector2(52,60),
                                            0,24,true),
                    }
                ),
                new PlatformsRoomManager //church1stFloor0
                (
                    new MovingPlatform[]
                    {
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(412,204),
                                            new Vector2(636, 204),
                                            0,96,true),
                    }
                ),
                new PlatformsRoomManager //church2ndFloor0
                (
                    new MovingPlatform[]
                    {
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(156,MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-36),
                                            new Vector2(564, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-36),
                                            0,100,true),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 0),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 45),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 90),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 135),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 180),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 225),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 270),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 80,
                                              new Vector2(124, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              new Vector2(380, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-308),
                                              1, 32, true, 315),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 0),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 60),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 120),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 180),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 240),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 104,
                                              new Vector2(724, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-196),
                                              new Vector2(0,0),
                                              -1, 0, true, 300),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(956,MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-12),
                                            new Vector2(956, MapsManager.maps[(int)RoomsManager.Rooms.church2ndFloor0].RoomHeightPx-420),
                                            0,80,true),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1076, 100), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 0),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1108, 116), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 0.3f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1140, 132), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 0.6f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1172, 148), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 0.9f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1204, 164), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 1.2f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1236, 180), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 1.5f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(1268, 196), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2.2f, 0.8f, 1.8f),
                          new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(1324, 252),
                                            new Vector2(1068, 252),
                                            0,512,true, 0,0,1),
                    }
                ),
                new PlatformsRoomManager //descent
                (
                    new MovingPlatform[]
                    {
                    }
                ),
                new PlatformsRoomManager //finalBossRoom
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FinalBoss.fireballsCenter, Vector2.Zero, 0.5f,0,false,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FinalBoss.fireballsCenter, Vector2.Zero, 0.5f,0,false,120),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FinalBoss.fireballsCenter,Vector2.Zero, 0.5f,0,false,240),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 160, FinalBoss.fireballsCenter,Vector2.Zero, -0.5f,0,false,180),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 160, FinalBoss.fireballsCenter, Vector2.Zero,-0.5f,0,false,60),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 160, FinalBoss.fireballsCenter,Vector2.Zero, -0.5f,0,false,300),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(100, MapsManager.maps[(int)RoomsManager.Rooms.finalBoss].RoomHeightPx+50),
                                            new Vector2(100, 72), 0,30, false, 0,0,0,true)
                    }
                ),
                new PlatformsRoomManager //escape0
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.fallingFloor272_40, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-553, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx-20),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-553, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx+21),
                                        0,5, false, 0,0,0, true),
                        new MovingPlatform(MovingPlatform.TextureType.fallingFloor296_40, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-860, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx-20),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-860, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx+21),
                                        0,5, false, 0,0,0, true),
                        new MovingPlatform(MovingPlatform.TextureType.fallingFloor112_216, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-1280, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx+108),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-1280, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx-112),
                                        0,25, false, 0,0,0, true),
                        new MovingPlatform(MovingPlatform.TextureType.fallingFloor272_40, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-1568, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx+5),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-1568, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx-20),
                                        0,8, false, 0,0,0, true),
                    }
                ),
                new PlatformsRoomManager //escape1
                (
                    new MovingPlatform[]
                    {
                       new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx-76, 116),
                                            new Vector2(-40,116), 0, 52f, false, 0,0, 0, true),
                       new MovingPlatform(MovingPlatform.TextureType.fallingFloor184_216, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx-972, MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomHeightPx+32),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx-972, MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomHeightPx),
                                        0,10, false, 0,0,0, true),
                       new MovingPlatform(MovingPlatform.TextureType.fallingFloor184_216, 0,
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx-1156, MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomHeightPx+24),
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx-1156, MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomHeightPx-8),
                                        0,10, false, 0,0,0, true)
                    }
                ),
                new PlatformsRoomManager //escape2
                (
                    new MovingPlatform[]
                    {
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(396, 100), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 0),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(356, 116), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 0.4f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(316, 132), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 0.8f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(276, 148), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 1.2f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(236, 164), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 1.6f),
                          new MovingPlatform(MovingPlatform.TextureType.platform24_8, 0,
                                              new Vector2(196, 180), new Vector2(0,0),
                                              0,0,false, 0,0,0,false, true, 2f, 1, 2f),
                    }
                )
            };
        }
        #endregion
    }
}
