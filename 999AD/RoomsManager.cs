﻿using System;
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
            room1, room2, total
        }
        static Rooms currentRoom = Rooms.room1;
        #endregion
        #region METHODS
        //state machine managing the movement between rooms
        static void switchRoom()
        {
            if (currentRoom==Rooms.room1)
            {
                if (Player.rectangle.X> MapsManager.maps[(int)Rooms.room1].RoomWidthtPx)
                {
                    currentRoom = Rooms.room2;
                    CameraManager.SwitchCamera(Rooms.room2);
                    Player.rectangle.X = 0;
                    CameraManager.shakeForTime(5f);
                }
            }
            else if (currentRoom == Rooms.room2)
            {
                if (Player.rectangle.Right <= 0)
                {
                    currentRoom = Rooms.room1;
                    CameraManager.SwitchCamera(Rooms.room1);
                    Player.rectangle.X = MapsManager.maps[(int)Rooms.room1].RoomWidthtPx - Player.rectangle.Width;
                }
            }
        }
        public static void Update(GameTime gameTime)
        {
            switchRoom();
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Update(gameTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            MapsManager.maps[(int)currentRoom].Draw(spriteBatch);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Draw(spriteBatch);
        }
        #endregion
    }
}
