using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    [Serializable]
    class GameSaveData
    {
        #region DECLARATIONS
        public float gameTime { get; private set; }
        public int deathsCount { get; private set; }
        public int hitsCount { get; private set; }
        public int currentRoom { get; private set; }
        public int previousRoom { get; private set; }
        public int health { get; private set; }
        public float[] playerPosition { get; private set; }
        public List<int> itemsOnMap { get; private set; }
        public List<int> itemsInInventory { get; private set; }
        public List<int> closedDoor { get; private set; }
        public bool midBossDead { get; private set; }
        public bool finalBossDead { get; private set; }
        public bool[] eventAlreadyHappened { get; private set; }
        #endregion
        #region CONSTRUCTOR
        public GameSaveData(float _time, int _deaths, int _hits, int _currRoom, int _prevRoom, int _health, float[] _position, List<int> _itemsOnMap,
            List<int> _itemsTypesInInvntory, List<int> _closedDoors, bool _midBossDead, bool _finalBossDead, bool[] _eventAlreadyHappened)
        {
            gameTime = _time;
            deathsCount = _deaths;
            hitsCount = _hits;
            playerPosition = _position;
            currentRoom = _currRoom;
            previousRoom = _prevRoom;
            health = _health;
            itemsOnMap = _itemsOnMap;
            itemsInInventory = _itemsTypesInInvntory;
            closedDoor = _closedDoors;
            midBossDead = _midBossDead;
            finalBossDead = _finalBossDead;
            eventAlreadyHappened = _eventAlreadyHappened;
        }
        #endregion
        #region METHODS
        public void ApplySaveData()
        {
            GameStats.Inizialize(gameTime, deathsCount, hitsCount);
            FinalBoss.Dead = finalBossDead;
            MidBoss.Dead = midBossDead;
            RoomsManager.CurrentRoom = (RoomsManager.Rooms)currentRoom;
            RoomsManager.PreviousRoom = (RoomsManager.Rooms)previousRoom;
            Player.healthPoints = health;
            Player.position = new Vector2 (playerPosition[0], playerPosition[1]);
            if (eventAlreadyHappened[(int)GameEvents.Events.unlockDoubleJump])
            {
                Player.doubleJumpUnlocked = true;
                AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.Rooms.churchBellTower0].AddAnimatedSprite(
                    new AnimatedSprite(new Vector2(211, 926), AnimatedSprite.SpriteType.displayDoubleJumpRelic, false));
            }
            if (eventAlreadyHappened[(int)GameEvents.Events.unlockWallJump])
            {
                Player.wallJumpUnlocked = true;
                AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.Rooms.churchBellTower0].AddAnimatedSprite(
                    new AnimatedSprite(new Vector2(239, 926), AnimatedSprite.SpriteType.displayWallJumpRelic, false));
            }
            foreach (CollectablesRoomManager r in CollectablesManager.collectablesRoomManagers)
            {
                for (int i=0; i< r.collectables.Count;i++)
                {
                    bool delete = true;
                    foreach(int j in itemsOnMap)
                    {
                        if (r.collectables[i].ID==j || r.collectables[i].ID == -1)
                        {
                            delete = false;
                            break;
                        }
                    }
                    if (delete)
                    {
                        r.collectables.RemoveAt(i);
                        i--;
                    }
                }
            }
            CollectablesManager.collectedItems.Clear();
            foreach (int i in itemsInInventory)
            {
                CollectablesManager.AddToInventory(new Collectable(Point.Zero, (Collectable.ItemType)i));
            }
            int room = 0;
            foreach (DoorsRoomManager r in DoorsManager.doorsRoomManagers)
            {
                for (int i = 0; i < r.doors.Count; i++)
                {
                    bool delete = true;
                    foreach (int j in closedDoor)
                    {
                        if (r.doors[i].ID == j)
                        {
                            delete = false;
                            break;
                        }
                    }
                    if (delete)
                    {
                        r.doors[i].OpenDoor(room);
                        r.doors.RemoveAt(i);
                        i--;
                    }
                }
                room++;
            }
            GameEvents.eventAlreadyHappened = eventAlreadyHappened;
            PlayerDeathManager.ResetVariables();
            CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
        }
        #endregion
    }
}
