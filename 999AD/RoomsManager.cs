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
            room1, room2, finalBoss, escape0, escape1, escape2, escape3, total
        }
        static Rooms currentRoom = Rooms.escape2;
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
                    }
                    break;
                case Rooms.room2:
                    if (Player.position.X >= MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to finalBoss room
                        currentRoom = Rooms.finalBoss;
                        CameraManager.SwitchCamera(Rooms.finalBoss);
                        Player.position.X = MapsManager.maps[(int)Rooms.finalBoss].RoomWidthtPx-Player.width;
                        Player.position.Y = 0;
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
        public static void Update(float elapsedTime)
        {
            switchRoom();
            CameraManager.Update(elapsedTime);
            MapsManager.maps[(int)currentRoom].Update(elapsedTime);
            if (currentRoom== Rooms.finalBoss)
            {
                Camera.pointLocked.X = Player.Center.X;
                Camera.pointLocked.Y = (Player.Center.Y + 2 * FireBallsManager.fireballsCenter.Y) / 3f;
                FinalBoss.Update(elapsedTime);
            }
            FireBallsManager.Update(elapsedTime);
            LavaGeyserManager.Update(elapsedTime);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Update(elapsedTime);
            CollectablesManager.collectablesRoomManagers[(int)currentRoom].Update(elapsedTime);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Update(elapsedTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            MapsManager.maps[(int)currentRoom].Draw(spriteBatch);
            Player.Draw(spriteBatch);
            ProjectilesManager.Draw(spriteBatch);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Draw(spriteBatch);
            CollectablesManager.collectablesRoomManagers[(int)currentRoom].Draw(spriteBatch);
            if (currentRoom == Rooms.finalBoss)
            {
                FinalBoss.Draw(spriteBatch);
            }
            FireBallsManager.Draw(spriteBatch);
            LavaGeyserManager.Draw(spriteBatch);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Draw(spriteBatch);
        }
        #endregion
    }
}
