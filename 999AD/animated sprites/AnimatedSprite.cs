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
    class AnimatedSprite
    {
        #region DECLARATIONS
        public enum SpriteType
        {
            invisibleTile, total
        }
        static Texture2D spritesheet;
        static Animation[] animations;
        Vector2 position;
        Animation animation;
        bool removeWhenInactive;
        #endregion
        #region CONSTRUCTORS
        public AnimatedSprite(Vector2 _position, SpriteType _spriteType, bool _removeWhenInactive=true)
        {
            position = _position;
            removeWhenInactive = _removeWhenInactive;
            animation = animations[(int)_spriteType].DeepCopy();
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            animations = new Animation[(int)SpriteType.total]
            {
                new Animation(new Rectangle(0,0,64, 8),8,8,8, 0.2f, true),
            };
        }
        #endregion
        #region PROPERTIES
        Rectangle DrawRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, animation.Frame.Width, animation.Frame.Height); }
        }
        public bool Active
        {
            get { return (!removeWhenInactive) || animation.Active; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (DrawRectangle.Intersects(Camera.Rectangle))
                spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(DrawRectangle), animation.Frame, Color.White);
        }
        #endregion
    }
}
