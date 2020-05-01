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
    static class CollectablesManager
    {
        #region DECLARATIONS
        public static CollectablesRoomManager[] collectablesRoomManagers;
        public static Dictionary<Collectable.ItemType, int> collectedItems= new Dictionary<Collectable.ItemType, int>();
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet)
        {
            Collectable.Inizialize(spritesheet,
                new Animation[(int)Collectable.ItemType.total]
                {
                    new Animation(new Rectangle(0, 0, 64, 16),16,16,4, 0.5f, true),
                    new Animation(new Rectangle(0, 16, 64, 16),16,16,4, 0.5f, true)
                });
            collectablesRoomManagers = new CollectablesRoomManager[(int)RoomsManager.Rooms.total]
            {
                new CollectablesRoomManager(new Collectable[]
                {
                    new Collectable(new Rectangle(100, 150,16,16), Collectable.ItemType.coin),
                    new Collectable(new Rectangle(300, 150, 16, 16), Collectable.ItemType.key)
                }),
                new CollectablesRoomManager(new Collectable[]{ }),
                new CollectablesRoomManager(new Collectable[]{ }),
                new CollectablesRoomManager(new Collectable[]{ }),
                new CollectablesRoomManager(new Collectable[]{ }),
                new CollectablesRoomManager(new Collectable[]{ }),
                new CollectablesRoomManager(new Collectable[]{ })
            };
            for (int i = 0; i < (int)Collectable.ItemType.total; i++)
                collectedItems.Add((Collectable.ItemType)i, 0);
        }
        #endregion
        #region METHODS
        public static void AddToInventory(Collectable.ItemType itemType)
        {
            collectedItems[itemType]++;
        }
        public static bool TryRemoveFromInventory(Collectable.ItemType itemType)
        {
            if (collectedItems[itemType]>0)
            {
                collectedItems[itemType]--;
                return true;
            }
            return false;
        }
        #endregion
    }
}
