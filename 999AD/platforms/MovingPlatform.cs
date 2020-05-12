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
    class MovingPlatform
    {
        #region DECLARATIONS
        public enum TextureType
        {
            platform40_8,
            platform24_8,
            fallingFloor272_40,
            fallingFloor296_40,
            fallingFloor112_216,
            fallingFloor184_216,
            total
        }
        public static Texture2D spritesheet; //textures of all the platforms
        public static Rectangle[] sourceRectangles;
        readonly TextureType textureType;
        Vector2 rotationCenter; //point around which the platform rotates
        public readonly int radius;
        readonly Vector2 centerStartingPoint;
        readonly Vector2 centerEndingPoint;
        public readonly int width; //width of the platform
        public readonly int height; //height of the platform
        Vector2 platformMidpointPosition;
        Vector2 platformMidpointPreviousPosition;
        float angleRadiants;
        readonly float angularSpeed; //radiants per second. positive->clockwise
        double normalizedLinearProgression; //0->rotationCenter is at the starting point, 1->rotationCenter is at the ending point, else platform it is somewhere in between
        float linearSpeed; //fraction of the total distance travelled every second
        readonly float centerRestingTime; //indicates for how many seconds the rotatationCenter rests at the starting and ending points
        float elapsedRestingTime;
        public bool active; //if false the platform does not move
        bool moveOnce;
        bool disappearing;
        bool transparent = false;
        float maxTransparentTime;
        float maxSolidTime;
        float elapsedTransparencyTime;
        static readonly float minAlpha = 0.15f;
        static readonly float alphaChangeSpeed = 3;//seconds^(-1)
        float alphaValue;
        float linearSpeed_pixelsPerSecond;
        float delay;
        #endregion
        #region CONSTRUCTOR
        public MovingPlatform(TextureType _textureType, int _radius,
            Vector2 _centerStartingPoint, Vector2 _centerEndingPoint, float _angularSpeed, float _linearSpeed_pixelsPerSecond,
            bool _active=true, float _startingAngleDegrees=0, float _normalizedLinearProression = 0, float _centerRestingTime = 0f, bool _moveOnce=false,
            bool _disappearing=false, float _maxTransparentTime=0, float _maxSolidTime=0, float _delay = 0)
        {
            textureType = _textureType;
            radius = _radius;
            centerStartingPoint = _centerStartingPoint;
            centerEndingPoint = _centerEndingPoint;
            width = sourceRectangles[(int)textureType].Width;
            height = sourceRectangles[(int)textureType].Height;
            angleRadiants = _startingAngleDegrees / 180 * MathHelper.Pi;
            angularSpeed = _angularSpeed;
            linearSpeed_pixelsPerSecond = _linearSpeed_pixelsPerSecond;
            linearSpeed = linearSpeed_pixelsPerSecond / (Vector2.Distance(centerEndingPoint, centerStartingPoint));
            centerRestingTime = _centerRestingTime;
            elapsedRestingTime = centerRestingTime;
            normalizedLinearProgression = MathHelper.Clamp(_normalizedLinearProression, 0, 1);
            active = _active;
            rotationCenter = Vector2.Lerp(centerStartingPoint, centerEndingPoint, (float)normalizedLinearProgression);
            platformMidpointPosition = new Vector2(rotationCenter.X + radius * (float)Math.Sin(angleRadiants),
                rotationCenter.Y - radius * (float)Math.Cos(angleRadiants));
            platformMidpointPreviousPosition = Vector2.Zero;
            moveOnce = _moveOnce;
            disappearing = _disappearing;
            maxTransparentTime = _maxTransparentTime;
            maxSolidTime = _maxSolidTime;
            delay = _delay;
            elapsedTransparencyTime = -delay;
            alphaValue = 1;
        }
        public static void loadTextures(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            sourceRectangles = new Rectangle[(int)TextureType.total]
            { 
              new Rectangle(0, 0, 40, 8),
              new Rectangle(0, 40, 24, 8),
              new Rectangle(0, 8, 272, 40),
              new Rectangle(0, 48, 296, 40),
              new Rectangle(0, 88, 112, 224),
              new Rectangle(112, 88, 184, 112)
            };
        }
        #endregion
        #region PROPERTIES
        //move the platform's rectangle given its new center
        public float AngleRadiants
        {
            get { return angleRadiants; }
        }
        public float AngularSpeed
        {
            get { return angularSpeed; }
        }
        public Vector2 Position
        {
            get { return new Vector2(platformMidpointPosition.X-0.5f*width, platformMidpointPosition.Y - 0.5f * height); }
        }
        public Vector2 Shift
        {
            get
            {
                if (!active)
                    return Vector2.Zero;
                else
                    return platformMidpointPosition - platformMidpointPreviousPosition;
            }
        }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)(platformMidpointPosition.X-0.5*width), (int)(platformMidpointPosition.Y - 0.5 * height), width, height); }
        }
        public bool Transparent
        {
            get { return transparent; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if(active)
            {
                platformMidpointPreviousPosition = platformMidpointPosition;
                if (elapsedRestingTime < centerRestingTime)
                {
                    elapsedRestingTime += elapsedTime;
                    return;
                }
                normalizedLinearProgression += linearSpeed * elapsedTime;
                if (normalizedLinearProgression > 1)
                {
                    normalizedLinearProgression = 1;
                    linearSpeed *= -1;
                    elapsedRestingTime = 0;
                    if (moveOnce)
                        active = false;
                }
                else if (normalizedLinearProgression < 0)
                {
                    normalizedLinearProgression = 0;
                    linearSpeed *= -1;
                    elapsedRestingTime = 0;
                    if (moveOnce)
                        active = false;
                }
                rotationCenter = Vector2.Lerp(centerStartingPoint, centerEndingPoint, (float)normalizedLinearProgression);
                angleRadiants += angularSpeed * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants<0)
                    angleRadiants += MathHelper.Pi * 2;
                platformMidpointPosition.X = rotationCenter.X + radius * (float)Math.Sin(angleRadiants);
            }
            platformMidpointPosition.Y = rotationCenter.Y - radius * (float)Math.Cos(angleRadiants);
            if (disappearing)
            {
                if (transparent)
                {
                    elapsedTransparencyTime += elapsedTime;
                    if (alphaValue>minAlpha)
                    {
                        alphaValue -= alphaChangeSpeed * elapsedTime;
                        if (alphaValue < minAlpha)
                            alphaValue = minAlpha;
                    }
                    if (elapsedTransparencyTime >= maxTransparentTime)
                    {
                        elapsedTransparencyTime = 0;
                        transparent = false;
                    }
                }
                else
                {
                    elapsedTransparencyTime += elapsedTime;
                    if (alphaValue <1)
                    {
                        alphaValue += alphaChangeSpeed * elapsedTime;
                        if (alphaValue > 1)
                            alphaValue = 1;
                    }
                    if (elapsedTransparencyTime >= maxSolidTime)
                    {
                            elapsedTransparencyTime = 0;
                            transparent = true;
                    }
                }
            }

        }
        public void RemovePlatform()
        {
            platformMidpointPosition.X = -width;
            platformMidpointPosition.Y = -height;
            active = false;
        }
        public MovingPlatform DeepCopy()
        {

            return new MovingPlatform(textureType, radius, centerStartingPoint, centerEndingPoint,
                angularSpeed, linearSpeed, active, angleRadiants*180/ MathHelper.Pi, linearSpeed_pixelsPerSecond,
                centerRestingTime, moveOnce, disappearing, maxTransparentTime, maxSolidTime, delay);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(Position, width, height),
                sourceRectangles[(int)textureType], Color.White*alphaValue);
        }
        #endregion
    }
}
