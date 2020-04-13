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
    class RotatingPlatform
    {
        #region DECLARATIONS
        PlatformsManager.PlatformTextureType textureType;
        Vector2 center; //point around which the platform rotates
        int radius;
        public readonly int width; //width of the platform
        public readonly int height; //height of the platform
        Vector2 platformCenterPosition;
        Vector2 platformCenterPreviousPosition=Vector2.Zero;
        float angleRadiants;
        float angularSpeed; //radiants per second. positive->clockwise
        bool active; //if false the platform does not move
        #endregion
        #region CONSTRUCTOR
        public RotatingPlatform(PlatformsManager.PlatformTextureType _textureType, Vector2 _center, int _radius,
            int _width, int _height, float _angularSpeed, float _startingAngleDegrees=0, bool _active=true)
        {
            textureType = _textureType;
            center = _center;
            radius = _radius;
            width = _width;
            height = _height;
            angleRadiants = _startingAngleDegrees / 180 * MathHelper.Pi;
            angularSpeed = _angularSpeed;
            active = _active;
            platformCenterPosition = new Vector2(center.X + radius * (float)Math.Sin(angleRadiants),
                center.Y + radius * (float)Math.Cos(angleRadiants));
        }
        #endregion
        #region PROPERTIES
        //move the platform's rectangle given its new center
        public Vector2 Position
        {
            get { return new Vector2(platformCenterPosition.X-0.5f*width, platformCenterPosition.Y - 0.5f * height); }
        }
        public Vector2 Shift
        {
            get { return platformCenterPosition - platformCenterPreviousPosition; }
        }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)(platformCenterPosition.X-0.5*width), (int)(platformCenterPosition.Y - 0.5 * height), width, height); }
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            if(active)
            {
                platformCenterPreviousPosition = platformCenterPosition;
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                angleRadiants += angularSpeed * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants<0)
                    angleRadiants += MathHelper.Pi * 2;
                platformCenterPosition.X = center.X + radius * (float)Math.Sin(angleRadiants);
                platformCenterPosition.Y = center.Y - radius * (float)Math.Cos(angleRadiants);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlatformsManager.spritesheet, Camera.DrawRectangle(Rectangle),
                PlatformsManager.sourceRectangles[(int)textureType], Color.White);
        }
        #endregion
    }
}
