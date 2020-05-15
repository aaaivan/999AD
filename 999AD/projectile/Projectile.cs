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
    class Projectile
    {
        public enum SpriteType
        {
            holyWater, lava, total
        }
        #region DECLARATION
        static Texture2D spritesheet;
        static Animation[] animations;
        Animation animation;
        public readonly int width;
        public readonly int height;
        Vector2 position;
        Vector2 velocity;
        public bool active;
        #endregion
        #region CONSTRUCTOR
        public Projectile(Vector2 _position, Vector2 _initialVelocity, SpriteType _spriteType)
        {
            position = _position;
            velocity = _initialVelocity;
            animation = animations[(int)_spriteType].DeepCopy();
            active = true;
            width = animation.Frame.Width;
            height = animation.Frame.Height;
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            animations = new Animation[(int)SpriteType.total]
                {
                    new Animation(new Rectangle(336,64,16,4), 4,4, 4, 0.1f, true),
                    new Animation(new Rectangle(336,58,24,6), 6,6, 4, 0.1f, true),
                };
        }
        #endregion
        #region PROPERTIES
        //return the rectangle of the projectile
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime); //update the animation
            Gravity.MoveDestructableObject(ref velocity, ref position, width, height, ref active, elapsedTime); //update the position
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(position, width, height), animation.Frame, Color.White);
        }
        #endregion
    }
}
