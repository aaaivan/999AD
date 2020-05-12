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
        static Texture2D[] backgrounds;
        public static readonly float[] scaleByRoom = new float[(int)RoomsManager.Rooms.total]
        {
            1, //tutorial0
            1, //tutorial1
            1, //tutorial2
            1, //tutorial3
            1, //tutorial4
            1, //churchBellTower0
            1, //churchBellTower1
            1, //churchBellTower2
            1, //midBoss
            1, //churchGroundFloor0
            1, //churchAltarRoom
            1, //church1stFloor0
            1, //church2ndFloor0
            1, //finalBoss
            0.625f, //finalBoss
            1, //escape0
            1, //escape1
            1 //escape2
        };
        public static readonly int maxOffsetY = 2; //amplitude of the rumble
        static bool shaking;
        static int offsetY; //current offset
        static readonly float timeBetweenOffsets = 0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime; //time elapsed since the last change in offsetY
        static float shakingTime; //tells for how many second the framing will shake
        static float elapsedShakingTime; //time elapsed since when the camera started shaking
        static float playerPositionWeight;
        static float transientPlayerPositionWeight;
        public static Vector2 pointLocked;
        static float cameraTransitionProgression;
        static float transitionDuration;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D[] _backgrounds)
        {
            backgrounds = _backgrounds;
            Reset();
            SwitchCamera(RoomsManager.Rooms.tutorial0);
        }
        public static void Reset()
        {
            offsetY = -maxOffsetY;
            shaking = false;
            cameraTransitionProgression = 1;
            offsetElapsedTime = 0f;
            transientPlayerPositionWeight = 1;
            playerPositionWeight = 1;
            pointLocked = new Vector2(0, 0);
            cameraTransitionProgression = 1;
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
