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
        #region DECLARATIONS
        static Texture2D[] backgrounds = new Texture2D[(int)RoomsManager.Rooms.total];
        public static readonly float[] scaleByRoom = new float[(int)RoomsManager.Rooms.total]
        {
            1, //tutorial0
            1, //tutorial1
            1, //tutorial2
            1, //tutorial3
            0.5f, //finalBoss
            1, //escape0
            1 //escape1
        };
        public static readonly int maxOffsetY = 2; //amplitude of the rumble
        static bool shaking = false;
        static int offsetY; //current offset
        static float timeBetweenOffsets = 0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime = 0f; //time elapsed since the last change in offsetY
        static float shakingTime; //tells for how many second the framing will shake
        static float elapsedShakingTime; //time elapsed since when the camera started shaking
        static float playerPositionWeight=1;
        static float transientPlayerPositionWeight = 1;
        public static Vector2 pointLocked = new Vector2(0,0);
        static float cameraTransitionProgression = 1;
        static float transitionDuration;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D[] _backgrounds)
        {
            Camera.Inizialize();
            backgrounds = _backgrounds;
            offsetY = -maxOffsetY;
            SwitchCamera(RoomsManager.Rooms.tutorial0);
        }
        #endregion
        #region METHODS
        //move camera to another room
        public static void SwitchCamera(RoomsManager.Rooms room, float _playerPositionWeight=1)
        {
            playerPositionWeight = _playerPositionWeight;
            transientPlayerPositionWeight = playerPositionWeight;
            cameraTransitionProgression = 1;
            Camera.Inizialize(backgrounds[(int)room], room, scaleByRoom[(int)room]);
        }
        //makes the camera shake for the time (in seconds) passed to the function as parameter
        public static void shakeForTime(float _shakingTime)
        {
            shakingTime = _shakingTime;
            elapsedShakingTime = 0f;
            shaking = true;
        }
        public static void Update(float elapsedTime)
        {
            if (cameraTransitionProgression<1)
            {
                cameraTransitionProgression += elapsedTime / transitionDuration;
                if (cameraTransitionProgression >= 1)
                    cameraTransitionProgression = 1;
                transientPlayerPositionWeight = transientPlayerPositionWeight+(playerPositionWeight - transientPlayerPositionWeight) * cameraTransitionProgression;
            }
            Camera.Update(shaking, Player.Center*transientPlayerPositionWeight+pointLocked*(1-transientPlayerPositionWeight));
            if (shaking)
            {
                Camera.position.Y += offsetY;
                elapsedShakingTime += elapsedTime;
                if (elapsedShakingTime >= shakingTime)
                    shaking = false;
                else
                {
                    offsetElapsedTime += elapsedTime;
                    if (offsetElapsedTime >= timeBetweenOffsets)
                    {
                        offsetY *= -1;
                        offsetElapsedTime = 0;
                    }
                }
            }
        }
        public static void MoveCamera(float _playerPositionWeight, Vector2 _pointLocked, float _transitionDuration)
        {
            transientPlayerPositionWeight = playerPositionWeight;
            playerPositionWeight = _playerPositionWeight;
            pointLocked = _pointLocked;
            cameraTransitionProgression = 0;
            transitionDuration = _transitionDuration;
        }
        #endregion
    }
}
