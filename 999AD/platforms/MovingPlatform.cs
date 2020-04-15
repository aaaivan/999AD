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
            texture1, texture2, texture3, total
        }
        public static Texture2D spritesheet; //textures of all the platforms
        public static Rectangle[] sourceRectangles;
        readonly TextureType textureType;
        Vector2 rotationCenter; //point around which the platform rotates
        readonly int radius;
        readonly Vector2 centerStartingPoint;
        readonly Vector2 centerEndingPoint;
        public readonly int width; //width of the platform
        public readonly int height; //height of the platform
        Vector2 platformMidpointPosition;
        Vector2 platformMidpointPreviousPosition=Vector2.Zero;
        float angleRadiants;
        readonly float angularSpeed; //radiants per second. positive->clockwise
        float normalizedLinearProgression; //0->rotationCenter is at the starting point, 1->rotationCenter is at the ending point, else platform it is somewhere in between
        float linearSpeed; //fraction of the total distance travelled every second
        readonly float centerRestingTime; //indicates for how many seconds the rotatationCenter rests at the starting and ending points
        float elapsedRestingTime = 0;
        bool active; //if false the platform does not move
        #endregion
        #region CONSTRUCTOR
        public MovingPlatform(TextureType _textureType, int _radius,
            Vector2 _centerStartingPoint, Vector2 _centerEndingPoint, float _angularSpeed, float _linearSpeed_pixelsPerSecond,
            float _centerRestingTime = 0f, float _startingAngleDegrees=0, float _normalizedLinearProression = 0, bool _active=true)
        {
            textureType = _textureType;
            radius = _radius;
            centerStartingPoint = _centerStartingPoint;
            centerEndingPoint = _centerEndingPoint;
            width = sourceRectangles[(int)textureType].Width;
            height = sourceRectangles[(int)textureType].Height;
            angleRadiants = _startingAngleDegrees / 180 * MathHelper.Pi;
            angularSpeed = _angularSpeed;
            linearSpeed = _linearSpeed_pixelsPerSecond / (Vector2.Distance(centerEndingPoint, centerStartingPoint));
            centerRestingTime = _centerRestingTime;
            normalizedLinearProgression = MathHelper.Clamp(_normalizedLinearProression, 0, 1);
            active = _active;
            rotationCenter = Vector2.Lerp(centerStartingPoint, centerEndingPoint, normalizedLinearProgression);
            platformMidpointPosition = new Vector2(rotationCenter.X + radius * (float)Math.Sin(angleRadiants),
                rotationCenter.Y + radius * (float)Math.Cos(angleRadiants));
        }
        public static void loadTextures(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            sourceRectangles = new Rectangle[(int)TextureType.total] { new Rectangle(0, 0, 100, 10),
                                                                       new Rectangle(0, 10, 100, 20),
                                                                       new Rectangle(0, 30, 100, 30) };
        }
        #endregion
        #region PROPERTIES
        //move the platform's rectangle given its new center
        public Vector2 Position
        {
            get { return new Vector2(platformMidpointPosition.X-0.5f*width, platformMidpointPosition.Y - 0.5f * height); }
        }
        public Vector2 Shift
        {
            get { return platformMidpointPosition - platformMidpointPreviousPosition; }
        }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)(platformMidpointPosition.X-0.5*width), (int)(platformMidpointPosition.Y - 0.5 * height), width, height); }
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            if(active)
            {
                platformMidpointPreviousPosition = platformMidpointPosition;
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (elapsedRestingTime < centerRestingTime)
                {
                    elapsedRestingTime += elapsedTime;
                    return;
                }
                normalizedLinearProgression += linearSpeed * elapsedTime;
                if (normalizedLinearProgression >= 1)
                {
                    normalizedLinearProgression = 1;
                    linearSpeed *= -1;
                    elapsedRestingTime = 0;
                }
                else if (normalizedLinearProgression <= 0)
                {
                    normalizedLinearProgression = 0;
                    linearSpeed *= -1;
                    elapsedRestingTime = 0;
                }
                rotationCenter= Vector2.Lerp(centerStartingPoint, centerEndingPoint, normalizedLinearProgression);
                angleRadiants += angularSpeed * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants<0)
                    angleRadiants += MathHelper.Pi * 2;
                platformMidpointPosition.X = rotationCenter.X + radius * (float)Math.Sin(angleRadiants);
                platformMidpointPosition.Y = rotationCenter.Y - radius * (float)Math.Cos(angleRadiants);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeVector(Position),
                sourceRectangles[(int)textureType], Color.White);
        }
        #endregion
    }
}
