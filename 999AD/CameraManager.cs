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
    static class CameraManager
    {
        static List<Texture2D> backgrounds = new List<Texture2D>();
        static bool shaking = false;
        static int maxOffsetY = 2; //amplitude of the rumble
        static int offsetY = -2; //current offset
        static float timeBetweenOffsets = 0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime = 0f; //time elapsed since the last change in offsetY
        static float shakingTime;
        static float elapsedShakingTime;
        public static void Inizialize(Texture2D[] _backgrounds)
        {
            for (int i=0; i<(int)RoomsManager.Rooms.total; i++)
            {
                backgrounds.Add(_backgrounds[i]);
            }
            SwitchCamera(RoomsManager.Rooms.room1);
        }
        public static void SwitchCamera(RoomsManager.Rooms _room, int scale=1)
        {
            Camera.Inizialize(backgrounds[(int)_room], _room, scale);
        }
        public static void shakeForTime(float _shakingTime)
        {
            shakingTime = _shakingTime;
            elapsedShakingTime = 0f;
            shaking = true;
        }
        static void NextOffset(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            offsetElapsedTime += elapsedTime;
            if (offsetElapsedTime >= timeBetweenOffsets)
            {
                offsetY *= -1;
                offsetElapsedTime = 0;
            }
        }
        public static void Update(GameTime gameTime)
        {
            elapsedShakingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedShakingTime >= shakingTime)
                shaking = false;
            Camera.Update(gameTime, maxOffsetY);
            if (shaking)
            {
                Camera.rectangle.Y += offsetY;
                NextOffset(gameTime);
            }
        }
    }
}
