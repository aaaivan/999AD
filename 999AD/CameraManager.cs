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
            Camera.shaking = true;
        }
        public static void Update(GameTime gameTime)
        {
            elapsedShakingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedShakingTime >= shakingTime)
                Camera.shaking = false;
            Camera.Update(gameTime);
        }
    }
}
