using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class Torch
    {
        #region DECLARATIONS
        static Texture2D spritesheet;
        static Animation litAnim;
        static Animation unlitAnim;
        Animation animation;
        Vector2 position;
        public bool isLit { get; private set; }
        float timeStaysUnlit;
        float elapsedTimeUnlit;
        #endregion
        #region CONSTRUCTOR
        static public void Initialize(Texture2D _spritesheet, Animation _lit, Animation _unlit)
        {
            spritesheet = _spritesheet;
            litAnim = _lit;
            unlitAnim = _unlit;
        }
        public Torch(Vector2 _position)
        {
            position = _position;
            isLit = true;
            animation = litAnim.DeepCopy();
            timeStaysUnlit = 30;
            elapsedTimeUnlit = 0;
        }
        #endregion
        #region METHODS
        public Rectangle DrawRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, animation.Frame.Width, animation.Frame.Height);
        }
        public Rectangle CollisionRectangle()
        {
            return new Rectangle((int)position.X+1, (int)position.Y+14, 14, 14);
        }
        public bool TorchHitByRect(Rectangle rect)
        {
            if (isLit && CollisionRectangle().Intersects(rect))
            {
                isLit = false;
                animation = unlitAnim.DeepCopy();
                return true; 
            }
            return false;
        }

        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
            if(!isLit)
            {
                elapsedTimeUnlit += elapsedTime;
                if (elapsedTimeUnlit>= timeStaysUnlit)
                {
                    isLit = true;
                    animation = litAnim.DeepCopy();
                    elapsedTimeUnlit = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(DrawRectangle()), animation.Frame, Color.White);
        }
        #endregion
    }
}
