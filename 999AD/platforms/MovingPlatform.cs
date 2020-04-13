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
        public readonly int width; //width of the platform
        public readonly int height; //height of the platform
        float relativeProgression; //0->platform is at the starting point, 1->platform is at the ending point, else platform is somewhere in between
        float previousRelativeProgression=0;
        float normalizedSpeed; //fraction of th etotal distance travelled every second
        float restingTime; //indicates for how many seconds the platform rests at th estarting and ending points
        float elapsedRestingTime=0;
        bool active; //if false the platform does not move
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
        }
        #endregion
        #region PROPERTIES
        public Vector2 Position
        {
            get
            {
                return new Vector2(MathHelper.Lerp(startingPoint.X, endingPoint.X, relativeProgression),
                    MathHelper.Lerp(startingPoint.Y, endingPoint.Y, relativeProgression));
            }
        }
        public Vector2 Shift
        {
            get
            {
                return Position- new Vector2(MathHelper.Lerp(startingPoint.X, endingPoint.X, previousRelativeProgression),
                                             MathHelper.Lerp(startingPoint.Y, endingPoint.Y, previousRelativeProgression));
            }
        }
        public Rectangle Rectangle
        {
            get
            { return new Rectangle((int)MathHelper.Lerp(startingPoint.X, endingPoint.X, relativeProgression),
                                    (int)MathHelper.Lerp(startingPoint.Y, endingPoint.Y, relativeProgression),
                                    width,
                                    height);
            }
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                previousRelativeProgression = relativeProgression;
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
