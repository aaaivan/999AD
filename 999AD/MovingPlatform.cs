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
        PlatformsManager.PlatformTextureType textureType;
        Vector2 startingPoint;
        Vector2 endingPoint;
        int width;
        int height;
        float relativeProgression;
        float normalizedSpeed;
        float restingTime;
        float elapsedRestingTime=0;
        Rectangle rectangle;
        bool active;
        #endregion
        #region COSTRUCTOR
        public MovingPlatform(PlatformsManager.PlatformTextureType _textureType, Vector2 _startingPoint,
                            Vector2 _endingPoint, int _width, int _height, float _normalizedSpeed,
                            float _restingTime=0f, bool _active= true, float _relativeProgression=0)
        {
            textureType = _textureType;
            startingPoint = _startingPoint;
            endingPoint = _endingPoint;
            width = _width;
            height = _height;
            normalizedSpeed = _normalizedSpeed;
            restingTime = _restingTime;
            relativeProgression = _relativeProgression;
            active = _active;
            rectangle = new Rectangle((int)MathHelper.Lerp(startingPoint.X, endingPoint.X, relativeProgression),
                                    (int)MathHelper.Lerp(startingPoint.Y, endingPoint.Y, relativeProgression),
                                    width,
                                    height);
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (elapsedRestingTime < restingTime)
                {
                    elapsedRestingTime += elapsedTime;
                    return;
                }
                relativeProgression += normalizedSpeed * elapsedTime;
                if (relativeProgression>=1)
                {
                    relativeProgression = 1;
                    normalizedSpeed *= -1;
                    elapsedRestingTime = 0;
                }
                else if(relativeProgression<=0)
                {
                    relativeProgression = 0;
                    normalizedSpeed *= -1;
                    elapsedRestingTime = 0;
                }
                rectangle.X = (int)MathHelper.Lerp(startingPoint.X, endingPoint.X, relativeProgression);
                rectangle.Y = (int)MathHelper.Lerp(startingPoint.Y, endingPoint.Y, relativeProgression);
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
