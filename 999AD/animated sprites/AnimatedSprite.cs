using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class AnimatedSprite
    {
        #region DECLARATIONS
        public enum SpriteType
        {
            sign_tutorial1,
            invisibleTile,
            openBrassDoor, openGoldDoor, openBronzeDoor, openSilverDoor,
            displayDoubleJumpRelic, displayWallJumpRelic,
            total
        }
        static Texture2D spritesheet;
        static Animation[] animations;
        Vector2 position;
        Animation animation;
        bool removeWhenInactive;
        bool drawInFront;
        #endregion
        #region CONSTRUCTORS
        public AnimatedSprite(Vector2 _position, SpriteType _spriteType, bool _removeWhenInactive=true, bool _drawInfront=false)
        {
            position = _position;
            removeWhenInactive = _removeWhenInactive;
            animation = animations[(int)_spriteType].DeepCopy();
            drawInFront = _drawInfront;
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            animations = new Animation[(int)SpriteType.total]
            {
                new Animation(new Rectangle[]{ new Rectangle(336, 68, 16,16) }, 0, false),
                new Animation(new Rectangle(336,40,64, 8),8,8,8, 0.1f, true),
                new Animation(new Rectangle(0,0, 112, 40), 16, 40, 7, 0.1f, false, true),
                new Animation(new Rectangle(112,0, 112, 40), 16, 40, 7, 0.1f, false, true),
                new Animation(new Rectangle(224,0, 112, 40), 16, 40, 7, 0.1f, false, true),
                new Animation(new Rectangle(336,0, 112, 40), 16, 40, 7, 0.1f, false, true),
                new Animation(new Rectangle(0,40, 24, 24), 24, 24, 1, 0, false),
                new Animation(new Rectangle(0,75, 24, 24), 24, 24, 1, 0, false),
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
        public bool DrawInFront
        {
            get { return drawInFront; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
        }
        public static SpriteType GetDoorAnimation(Door.TextureType doorType)
        {
            switch (doorType)
            {
                case Door.TextureType.brassDoor:
                    return SpriteType.openBrassDoor;
                case Door.TextureType.goldDoor:
                    return SpriteType.openGoldDoor;
                case Door.TextureType.silverDoor:
                    return SpriteType.openSilverDoor;
                case Door.TextureType.bronzeDoor:
                    return SpriteType.openBronzeDoor;
                default:
                    return SpriteType.openBrassDoor;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (DrawRectangle.Intersects(Camera.Rectangle))
                spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(DrawRectangle), animation.Frame, Color.White);
        }
        #endregion
    }
}
