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
    static class RoomsManager
    {
        #region DECLARATIONS
        public enum Rooms
        {
            room1, room2, finalBoss, total
        }
        static Rooms currentRoom = Rooms.room1;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize()
        {
            CameraManager.SwitchCamera(currentRoom);
        }
        #endregion
        #region PROPERTIES
        public static Rooms CurrentRoom
        {
            get { return currentRoom; }
        }
        #endregion
        #region METHODS
        //state machine managing the movement between rooms
        static void switchRoom()
        {
            switch (currentRoom)
            {
                case Rooms.room1:
                    if (Player.position.X > MapsManager.maps[(int)Rooms.room1].RoomWidthtPx)
                    {//move to room2
                        currentRoom = Rooms.room2;
                        CameraManager.SwitchCamera(Rooms.room2);
                        Player.position.X = 0;
                        CameraManager.shakeForTime(5f);
                    }
                    break;
                case Rooms.room2:
                    if (Player.position.X >= MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to finalBoss room
                        currentRoom = Rooms.finalBoss;
                        CameraManager.SwitchCamera(Rooms.finalBoss);
                        Player.position.X = 0;
                        Player.position.Y += 9*Tile.tileSize;
                    }
                    else if (Player.position.X + Player.width <= 0)
                    {//move to room1
                        currentRoom = Rooms.room1;
                        CameraManager.SwitchCamera(Rooms.room1);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    break;
                case Rooms.finalBoss:
                    if (Player.position.X + Player.width <= 0)
                    {//move to room2
                        currentRoom = Rooms.room2;
                        CameraManager.SwitchCamera(Rooms.room2);
                        Player.position.X = MapsManager.maps[(int)Rooms.room2].RoomWidthtPx - Player.width;
                        Player.position.Y -= 9 * Tile.tileSize;
                    }
                    break;
            }
        }
        //this function trigger events when a centain set of conditions is true
        static void eventHandler(float elapsedTime)
        {
            switch (currentRoom)
            {
                case Rooms.finalBoss:
                    if (Player.position.X>=5*Tile.tileSize && Player.position.Y>=25*Tile.tileSize-Player.height*2)
                        GameEvents.playEvent(GameEvents.Events.terrainCollapseFinalBoss);
                    if (GameEvents.eventAlreadyHappened[(int)GameEvents.Events.terrainCollapseFinalBoss] &&
                        MapsManager.NumberOfTilesToRemove == 0)
                        GameEvents.playEvent(GameEvents.Events.activatePlatformsFinalBoss);
                    
                    break;
            }
        }
        public static void Update(float elapsedTime)
        {
            switchRoom();
            eventHandler(elapsedTime);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Update(elapsedTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            MapsManager.maps[(int)currentRoom].Draw(spriteBatch);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Draw(spriteBatch);
        }
        #endregion
    }
}
