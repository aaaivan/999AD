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
        public enum Rooms
        {
            room1, room2, total
        }
        static Rooms currentRoom = Rooms.room1;
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
            /*switch (currentRoom)
            {
                case Rooms.room1:
                
            }*/
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (currentRoom)
            {
                case Rooms.room1:
                    MapsManager.maps[(int)Rooms.room1].Draw(spriteBatch);
                    break;
                case Rooms.room2:
                    MapsManager.maps[(int)Rooms.room2].Draw(spriteBatch);
                    break;
                default:
                    break;
            }
        }
    }
}
