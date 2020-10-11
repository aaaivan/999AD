using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
        public List<int> itemsOnMap { get; private set; } //id of collectable not yet collected
        public List<int> itemsInInventory { get; private set; } //typr of collectables in the inventory
        public List<int> closedDoor { get; private set; } //doors that have not been opened yet
        public bool midBossDead { get; private set; }
        public bool finalBossDead { get; private set; }
        public bool[] eventAlreadyHappened { get; private set; } //store the events that have already been triggered (true=triggered, false= not triggered)
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
            RoomsManager.PreviousRoom =(RoomsManager.Rooms)previousRoom;
            Player.healthPoints = health;
            Player.position = new Vector2 (playerPosition[0], playerPosition[1]);
            if (eventAlreadyHappened[(int)GameEvents.Events.unlockDoubleJump])
            {//enable double jump if it was already unlocked
                Player.doubleJumpUnlocked = true;
                AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.Rooms.churchBellTower0].AddTempAnimatedSprite(
                    new AnimatedSprite(new Vector2(211, 926), AnimatedSprite.AnimationType.displayDoubleJumpRelic, false));
            }
            if (eventAlreadyHappened[(int)GameEvents.Events.unlockWallJump])
            {//enable wall jump if it was already unlocked
                Player.wallJumpUnlocked = true;
                AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.Rooms.churchBellTower0].AddTempAnimatedSprite(
                    new AnimatedSprite(new Vector2(239, 926), AnimatedSprite.AnimationType.displayWallJumpRelic, false));
            }
            foreach (CollectablesRoomManager r in CollectablesManager.collectablesRoomManagers)
            {//remove collectables already collected
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
            {//place items in inventory
                CollectablesManager.AddToInventory(new Collectable(Point.Zero, (Collectable.ItemType)i));
            }
            int room = 0;
            foreach (DoorsRoomManager r in DoorsManager.doorsRoomManagers)
            {//remove doors that have been opend already
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
