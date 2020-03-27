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
    class RotatingPlatform
    {
        #region DECLARATIONS
        PlatformsManager.PlatformTextureType textureType;
        Point center;
        int radius;
        int width;
        int height;
        float angleRadiants;
        float angularSpeed; //clockwise positive speed
        Rectangle rectangle;
        bool active;
        #endregion
        #region CONSTRUCTOR
        public RotatingPlatform(PlatformsManager.PlatformTextureType _textureType, Point _center, int _radius,
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
            rectangle = new Rectangle(center.X - width / 2, center.Y - radius - height / 2, width, height);
        }
        #endregion
        #region PROPERTIES
        Vector2 RectangleCenter
        {
            set
            {
                rectangle.X = (int)(value.X - width / 2);
                rectangle.Y = (int)(value.Y - height / 2);
            }
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            if(active)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                angleRadiants += angularSpeed * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants<0)
                    angleRadiants += MathHelper.Pi * 2;
                RectangleCenter = new Vector2(center.X + radius * (float)Math.Sin(angleRadiants),
                                            center.Y + radius * (float)Math.Cos(angleRadiants));
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlatformsManager.spritesheet, Camera.DrawRectangle(rectangle),
                PlatformsManager.sourceRectangles[(int)textureType], Color.White);
        }
        #endregion
    }
}