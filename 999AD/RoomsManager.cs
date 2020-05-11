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
            tutorial0, tutorial1, tutorial2, tutorial3, midboss, finalBoss, escape0, escape1, total
        }
        static Rooms currentRoom = Rooms.midboss;
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
                case Rooms.tutorial0:
                    if (Player.position.X > MapsManager.maps[(int)Rooms.tutorial0].RoomWidthtPx)
                    {//move to room2
                        currentRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = 0;
                    }
                    break;
                case Rooms.tutorial1:
                    if (Player.position.X >= MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to finalBoss room
                        currentRoom = Rooms.finalBoss;
                        CameraManager.SwitchCamera(Rooms.finalBoss);
                        Player.position.X = MapsManager.maps[(int)Rooms.finalBoss].RoomWidthtPx-Player.width;
                        Player.position.Y = 0;
                    }
                    else if (Player.position.X + Player.width <= 0)
                    {//move to room1
                        currentRoom = Rooms.tutorial0;
                        CameraManager.SwitchCamera(Rooms.tutorial0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    break;
                case Rooms.finalBoss:
                    if (Player.position.X + Player.width <= 0)
                    {//move to room2
                        currentRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx - Player.width;
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
                FinalBoss.Update(elapsedTime);
            }
            if(currentRoom==Rooms.midboss)
            {
               MidBoss.Update(elapsedTime);
            }
            EnemyManager.enemyRoomManagers[(int)currentRoom].Update(elapsedTime);
            FireBallsManager.Update(elapsedTime);
            LavaGeyserManager.Update(elapsedTime);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Update(elapsedTime);
            CollectablesManager.collectablesRoomManagers[(int)currentRoom].Update(elapsedTime);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Update(elapsedTime);
            DoorsManager.doorsRoomManagers[(int)currentRoom].Update();
            AnimatedSpritesManager.animatedSpritesRoomManagers[(int)currentRoom].Update(elapsedTime);
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
            if(currentRoom==Rooms.midboss)
            {
                MidBoss.Draw(spriteBatch);
            }
            EnemyManager.enemyRoomManagers[(int)currentRoom].Draw(spriteBatch);
            FireBallsManager.Draw(spriteBatch);
            LavaGeyserManager.Draw(spriteBatch);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Draw(spriteBatch);
            DoorsManager.doorsRoomManagers[(int)currentRoom].Draw(spriteBatch);
            AnimatedSpritesManager.animatedSpritesRoomManagers[(int)currentRoom].Draw(spriteBatch);
        }
        #endregion
    }
}
