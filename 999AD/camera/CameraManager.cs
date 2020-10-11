using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class CameraManager
    {
        #region DECLARATIONS
        static Texture2D[] backgrounds;
        public static readonly int maxOffsetY = 2; //amplitude of the rumble
        static bool shaking; //true if the camera is shaking
        static int offsetY; //current offset
        static readonly float timeBetweenOffsets = 0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime; //time elapsed since the last change in offsetY
        static float shakingTime; //tells for how many second the framing will shake
        static float elapsedShakingTime; //time elapsed since when the camera started shaking
        
        //the following variables are used to move the camera between 2 points
        //namely the player position and another point called pointLocked
        static float playerPositionWeight; //0->camera centered on pointLocked, 1->camera centered on Player
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
            Camera.Inizialize(backgrounds[(int)room], room);
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
                //move camera smoothly towards the final point, where:
                //final point = Player.Center * playerPositionWeight + pointLocked * (1 - playerPositionWeight)
                cameraTransitionProgression += elapsedTime / transitionDuration;
                if (cameraTransitionProgression >= 1)
                    cameraTransitionProgression = 1;
                transientPlayerPositionWeight = transientPlayerPositionWeight+(playerPositionWeight - transientPlayerPositionWeight) * cameraTransitionProgression;
            }
            Camera.Update(shaking, Player.Center*transientPlayerPositionWeight+pointLocked*(1-transientPlayerPositionWeight));
            if (shaking)
            {
                //apply displacement of the camera along y axis
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

        //set a new final point for the camera, where:
        //final point = Player.Center * playerPositionWeight + pointLocked * (1 - playerPositionWeight)
        public static void MoveCamera(float _playerPositionWeight, Vector2 _pointLocked, float _transitionDuration)
        {
            playerPositionWeight = _playerPositionWeight;
            pointLocked = _pointLocked;
            cameraTransitionProgression = 0;
            transitionDuration = _transitionDuration;
        }
        #endregion
    }
}
