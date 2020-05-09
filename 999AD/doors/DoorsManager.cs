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
                        new Door(new Point(200, 176), Door.TextureType.greenDoor, Collectable.ItemType.coin)
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial1
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial2
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //tutorial3
                    {
                    }
                    ),
                    new DoorsRoomManager(new Door[] //midboss
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
                    )
                };
            for (int room = 0; room < (int)RoomsManager.Rooms.total; room++)
                foreach (Door door in doorsRoomManagers[room].doors)
                    door.LockDoor((RoomsManager.Rooms)room);
        }
        #endregion
    }
}
