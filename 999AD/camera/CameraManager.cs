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
    static class CameraManager
    {
        #region DECLARATIONS
        static Texture2D[] backgrounds = new Texture2D[(int)RoomsManager.Rooms.total];
        public static readonly float[] scaleByRoom = new float[(int)RoomsManager.Rooms.total] { 1, 1, 0.625f };
        static bool shaking = false;
        public static readonly int maxOffsetY = 2; //amplitude of the rumble
        static int offsetY = -2; //current offset
        static float timeBetweenOffsets = 0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime = 0f; //time elapsed since the last change in offsetY
        static float shakingTime; //tells for how many second the framing will shake
        static float elapsedShakingTime; //time elapsed since when the camera started shaking
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D[] _backgrounds)
        {
            backgrounds = _backgrounds;
            SwitchCamera(RoomsManager.Rooms.room1);
        }
        #endregion
        #region METHODS
        //move camera to another room
        public static void SwitchCamera(RoomsManager.Rooms room, bool lockOnPlayer = true)
        {
            Camera.Inizialize(backgrounds[(int)room], room, scaleByRoom[(int)room], lockOnPlayer);
        }
        //makes the camera shake for the time (in seconds) passed to the function as parameter
        public static void shakeForTime(float _shakingTime)
        {
            shakingTime = _shakingTime;
            elapsedShakingTime = 0f;
            shaking = true;
        }
        //move the camera up/down while shaking
        static void NextOffset(float elapsedTime)
        {
            offsetElapsedTime += elapsedTime;
            if (offsetElapsedTime >= timeBetweenOffsets)
            {
                offsetY *= -1;
                offsetElapsedTime = 0;
            }
        }
        public static void Update(float elapsedTime)
        {
            elapsedShakingTime += elapsedTime;
            if (elapsedShakingTime >= shakingTime)
                shaking = false;
            Camera.Update(maxOffsetY);
            if (shaking)
            {
                Camera.rectangle.Y += offsetY;
                NextOffset(elapsedTime);
            }
        }
        #endregion
    }
}
