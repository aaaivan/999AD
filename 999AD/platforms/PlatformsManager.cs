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
        public static PlatformsRoomManager[] platformsRoomManagers; //list of platform managers for each room
        public static int platformIndex; //index of the platform ridden by the player
        public static void Inizialize(Texture2D _spritesheet)
        {
            MovingPlatform.loadTextures(_spritesheet);
            platformsRoomManagers = new PlatformsRoomManager[(int)RoomsManager.Rooms.total]
            {
                new PlatformsRoomManager //room1
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 50, new Vector2(100, 50), new Vector2(600, 100), 3, 100),
                        new MovingPlatform(MovingPlatform.TextureType.texture3, 50, new Vector2(800,400), Vector2.Zero, 3,0)
                    }
                ),
                new PlatformsRoomManager //room2
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.texture3, 0, new Vector2(80, 500), new Vector2(80, 600), 1, 500)
                    }
                ),
                new PlatformsRoomManager //finalBossRoom
                (
                    new MovingPlatform[]
                    {
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 340, new Vector2(496,576), Vector2.Zero, 1,0,false,0),
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 340, new Vector2(496,576), Vector2.Zero, 1,0,false,120),
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 340, new Vector2(496,576), Vector2.Zero, 1,0,false,240),
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 230, new Vector2(496,576), Vector2.Zero, -1,0,false,180),
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 230, new Vector2(496,576), Vector2.Zero, -1,0,false,60),
                        new MovingPlatform(MovingPlatform.TextureType.texture2, 230, new Vector2(496,576), Vector2.Zero, -1,0,false,300)
                    }
                )
            };
        }
    }
}
