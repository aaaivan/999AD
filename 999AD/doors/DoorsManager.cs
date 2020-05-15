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
    static class DoorsManager
    {
        #region DECLARATIONS
        public static DoorsRoomManager[] doorsRoomManagers;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet)
        {
            Door.Inizialize(spritesheet);
            doorsRoomManagers = new DoorsRoomManager[(int)RoomsManager.Rooms.total]
                {
                    new DoorsRoomManager(new Door[] //tutorial0
                    {
                        new Door(new Point(912, 168), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial1
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial2
                    {
                        new Door(new Point(458,136), Door.TextureType.brassDoor, Collectable.ItemType.brassKey),
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial3
                    {
                        new Door(new Point(520, 72), Door.TextureType.brassDoor, Collectable.ItemType.brassKey),
                        new Door(new Point(768, 176), Door.TextureType.silverDoor, Collectable.ItemType.silverKey),

                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial4
                    {
                        new Door(new Point(120, 136), Door.TextureType.goldDoor, Collectable.ItemType.goldKey),
                    }
                    ),
                    new DoorsRoomManager(new Door[] //churchBellTower0
                    {
                        new Door(new Point(472,200), Door.TextureType.silverDoor, Collectable.ItemType.silverKey)
                    }
                    ),
                    new DoorsRoomManager(new Door[] //churchBellTower1
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //churchBellTower2
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //midBoss
                    {
                        new Door(new Point(48, 176), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
                    }
                    ),
                    new DoorsRoomManager(new Door[] //churchGroundFloor0
                    {
                        new Door(new Point(1184, 448), Door.TextureType.brassDoor, Collectable.ItemType.brassKey),
                    }
                    ),
                    new DoorsRoomManager(new Door[] //churchAltarRoom
                    {
                        new Door(new Point(96, 448), Door.TextureType.bronzeDoor, Collectable.ItemType.bronzeKey)
                    }
                    ),
                    new DoorsRoomManager(new Door[] //church1stFloor0
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //church2ndFloor0
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //descent
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //finalBoss
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //escape0
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //escape1
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //escape2
                    {
                    }
                    )
                };
            for (int room = 0; room < (int)RoomsManager.Rooms.total; room++)
                foreach (Door door in doorsRoomManagers[room].doors)
                    door.LockDoor((RoomsManager.Rooms)room);
        }
        #endregion
    }
}
