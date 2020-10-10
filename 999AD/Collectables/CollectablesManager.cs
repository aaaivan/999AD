using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class CollectablesManager
    {
        #region DECLARATIONS
        public static CollectablesRoomManager[] collectablesRoomManagers;
        public static List<Collectable> collectedItems { get; private set; }
        static List<Collectable> itemsBeingUsed;
        static List<Vector2> targetPoints;
        static readonly float animationTime= 0.3f;
        static float elapsedAnimationTime;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet)
        {
            Collectable.IDcounter = 0;
            elapsedAnimationTime = 0;
            Collectable.Inizialize(spritesheet);
            collectablesRoomManagers = new CollectablesRoomManager[(int)RoomsManager.Rooms.total]
            {
                new CollectablesRoomManager(new Collectable[]   //tutorial0
                {
                    new Collectable(new Point(168, 198 ), Collectable.ItemType.brassKey),
                    new Collectable(new Point(628, 148), Collectable.ItemType.brassKey)
                }),
                new CollectablesRoomManager(new Collectable[]   //tutorial1
                {
                }),
                new CollectablesRoomManager(new Collectable[]   //tutorial2
                {
                    new Collectable(new Point(506, 150), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //tutorial3
                {
                }),
                new CollectablesRoomManager(new Collectable[]   //tutorial4
                {
                }),
                new CollectablesRoomManager(new Collectable[]   //churchBellTower0
                {
                    new Collectable(new Point(72, 960), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //churchBellTower1
                {
                    new Collectable(new Point(400, 614), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //churchBellTower2
                {
                    new Collectable(new Point(444, 120), Collectable.ItemType.silverKey),
                    new Collectable(new Point(56, 883), Collectable.ItemType.heart),
                    new Collectable(new Point(462, 63), Collectable.ItemType.heart),
                }),
                new CollectablesRoomManager(new Collectable[]   //midBoss
                {
                }),
                new CollectablesRoomManager(new Collectable[]   //churchGroundFloor0
                {
                    new Collectable(new Point(1160, 40), Collectable.ItemType.brassKey)
                }),
                new CollectablesRoomManager(new Collectable[]   //churchAltarRoom
                {
                    new Collectable(new Point(180, 456), Collectable.ItemType.bronzeKey),
                    new Collectable(new Point(40, 456), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //church1stFloor0
                {
                    new Collectable(new Point(40, 178), Collectable.ItemType.doubleJump_powerup),
                    new Collectable(new Point(689, 95), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //church2ndFloor0
                {
                    new Collectable(new Point(1062, 191), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //descent
                {
                    new Collectable(new Point(348, 188), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //finalBoss
                {
                    new Collectable(new Point(20,416), Collectable.ItemType.goldKey),
                    new Collectable(new Point(386, 416), Collectable.ItemType.heart)
                }),
                new CollectablesRoomManager(new Collectable[]   //escape0
                {
                }),
                new CollectablesRoomManager(new Collectable[]   //escape1
                {
                    new Collectable(new Point(26, 215), Collectable.ItemType.goldKey)
                }),
                new CollectablesRoomManager(new Collectable[]   //escape2
                {
                }),
            };
            collectedItems = new List<Collectable>();
            itemsBeingUsed = new List<Collectable>();
            targetPoints = new List<Vector2>();
        }
        #endregion
        #region METHODS
        public static void AddToInventory(Collectable collectable)
        {
            collectedItems.Add(collectable);
        }
        public static bool TryRemoveFromInventory(Collectable.ItemType itemType, Vector2 _targetPoint)
        {
            for (int i=collectedItems.Count-1;i>=0; i-- )
                if (collectedItems[i].type==itemType)
                {
                    itemsBeingUsed.Add(collectedItems[i]);
                    targetPoints.Add(_targetPoint);
                    collectedItems.RemoveAt(i);
                    return true;
                }
            return false;
        }
        public static void ResetHearts()
        {
            foreach (CollectablesRoomManager collectablesRoomManager in collectablesRoomManagers)
            {
                collectablesRoomManager.ResetHearts();
            }
        }
        public static void Update(float elapsedTime)
        {
            if (itemsBeingUsed.Count>0)
            {
                elapsedAnimationTime += elapsedTime;
                if (elapsedAnimationTime> animationTime)
                {
                    itemsBeingUsed.RemoveAt(0);
                    targetPoints.RemoveAt(0);
                    elapsedAnimationTime = 0;
                }
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            int padding = 2;
            int distanceFromScreenLeft = Game1.min_gameWidth-padding;
            for (int i = collectedItems.Count - 1; i >= 0; i--)
            {
                distanceFromScreenLeft -= collectedItems[i].rectangle.Width;
                collectedItems[i].DrawInGUI(spriteBatch, new Vector2(distanceFromScreenLeft, padding));
                distanceFromScreenLeft -= padding;
            }
            if (itemsBeingUsed.Count>0)
            {
                float scale = MathHelper.Lerp(3, 0, elapsedAnimationTime / animationTime);
                Rectangle drawRectangle = new Rectangle
                    ((int)(MathHelper.Lerp(Camera.Rectangle.Center.X, targetPoints[0].X, elapsedAnimationTime / animationTime) - itemsBeingUsed[0].rectangle.Width * scale / 2),
                    (int)(MathHelper.Lerp(Camera.Rectangle.Center.Y, targetPoints[0].Y, elapsedAnimationTime / animationTime) - itemsBeingUsed[0].rectangle.Height * scale / 2),
                    (int)(itemsBeingUsed[0].rectangle.Width * scale),
                    (int)(itemsBeingUsed[0].rectangle.Height * scale)
                    );
                itemsBeingUsed[0].Draw(spriteBatch, drawRectangle);
            }
        }
        #endregion
    }
}
