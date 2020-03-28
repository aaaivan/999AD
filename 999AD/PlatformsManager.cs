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
        public enum PlatformTextureType
        {
            texture1, texture2, texture3, total
        }
        public static Texture2D spritesheet; //textures of all the platforms
        public static List<Rectangle> sourceRectangles = new List<Rectangle>();
        public static List<PlatformsRoomManager> platformsRoomManagers = new List<PlatformsRoomManager>(); //list of platform managers
        public static void Inizialize(Texture2D _spritesheet, Rectangle[] _sourceRectangles,
            MovingPlatform[][] _movingPlatformInEachRoom, RotatingPlatform[][] _rotatingPlatformsInEachRoom)
        {
            spritesheet = _spritesheet;
            for (int i = 0; i < (int)PlatformTextureType.total; i++)
                sourceRectangles.Add(_sourceRectangles[i]);
            for (int i = 0; i < (int)RoomsManager.Rooms.total; i++)
                platformsRoomManagers.Add(new PlatformsRoomManager(_movingPlatformInEachRoom[i], _rotatingPlatformsInEachRoom[i]));
        }
    }
}
