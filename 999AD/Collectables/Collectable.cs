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
    class Collectable
    {
        public enum ItemType
        {
            coin, key, total
        }
        #region DECLARIONS
        static Texture2D spritesheet;
        static Animation[] animations;
        Animation animation;
        public readonly Rectangle rectangle;
        public readonly ItemType type; 
        bool collected;
        #endregion
        #region CONSTRUCTORS
        public Collectable(Rectangle _rectangle, ItemType _collectableType)
        {
            rectangle = _rectangle;
            type = _collectableType;
            animation = animations[(int)type].DeepCopy();
            collected = false;
        }
        public static void Inizialize(Texture2D _spritesheet, Animation[] _animations)
        {
            spritesheet = _spritesheet;
            animations = _animations;
        }
        #endregion
        #region PROPERTIES
        public bool Collected
        {
            get { return collected; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
            if (rectangle.Intersects(Player.CollisionRectangle))
                collected = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(spritesheet, Camera.RelativeRect(rectangle), animation.Frame, Color.White);
        }
        #endregion

    }
}
