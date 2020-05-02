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
                new PlatformsRoomManager //room1
                (
                    new MovingPlatform[]
                    {
                    }
                ),
                new PlatformsRoomManager //room2
                (
                    new MovingPlatform[]
                    {
                    }
                ),
                new PlatformsRoomManager //finalBossRoom
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FireBallsManager.fireballsCenter, Vector2.Zero, 0.5f,0,false,0),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FireBallsManager.fireballsCenter, Vector2.Zero, 0.5f,0,false,120),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 205, FireBallsManager.fireballsCenter,Vector2.Zero, 0.5f,0,false,240),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 140, FireBallsManager.fireballsCenter,Vector2.Zero, -0.5f,0,false,180),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 140, FireBallsManager.fireballsCenter, Vector2.Zero,-0.5f,0,false,60),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 140, FireBallsManager.fireballsCenter,Vector2.Zero, -0.5f,0,false,300),
                        new MovingPlatform(MovingPlatform.TextureType.platform40_8, 0,
                                            new Vector2(100, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx+50),
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
                                        new Vector2(MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx-1280, MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomHeightPx-116),
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
                    }
                ),
                new PlatformsRoomManager //escape3
                (
                    new MovingPlatform[]
                    {
                    }
                )
            };
        }
        #endregion
    }
}
