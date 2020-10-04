using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class Collectable
    {
        public enum ItemType
        {
            doubleJump_powerup, wallJump_powerup,
            brassKey, goldKey, bronzeKey, silverKey,
            heart,  total
        }
        #region DECLARIONS
        public static int IDcounter;
        public int ID { get; private set; }
        static Texture2D spritesheet;
        static Animation[] animations;
        Animation animation;
        public readonly Rectangle rectangle;
        public readonly ItemType type; 
        bool collected;
        #endregion
        #region CONSTRUCTORS
        public Collectable(Point position, ItemType _collectableType)
        {
            if (_collectableType == ItemType.heart)
                ID = -1;
            else
            {
                ID = IDcounter;
                IDcounter++;
            }
            type = _collectableType;
            animation = animations[(int)type].DeepCopy();
            collected = false;
            rectangle = new Rectangle(position.X, position.Y, animation.Frame.Width, animation.Frame.Height);
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            animations = new Animation[(int)ItemType.total]
                {
                    new Animation(new Rectangle(0,40, 336, 35),24,35,14, 0.1f, true),
                    new Animation(new Rectangle(0,75, 336, 35),24,35,14, 0.1f, true),
                    new Animation(new Rectangle(0,129, 120, 22),12,22,10, 0.1f, true),
                    new Animation(new Rectangle(120,129, 120, 22),12,22,10, 0.1f, true),
                    new Animation(new Rectangle(0,151, 120, 22),12,22,10, 0.1f, true),
                    new Animation(new Rectangle(120,151, 120, 22),12,22,10, 0.1f, true),
                    new Animation(new Rectangle(0,110, 160, 19),16,19,10, 0.1f, true),
                };
        }
        #endregion
        #region PROPERTIES
        public bool Collected
        {
            get { return collected; }
        }
        public static Texture2D Sprites
        {
            get { return spritesheet; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
            if (rectangle.Intersects(Player.CollisionRectangle))
            {
                SoundEffects.PickUpItem.Play();
                collected = true;
                animation.Reset();
            }
        }
        public Collectable DeepCopy()
        {
            return new Collectable(new Point(rectangle.X, rectangle.Y), type);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(rectangle), animation.Frame, Color.White);
        }
        public void DrawInGUI(SpriteBatch spriteBatch, Vector2 screenPosition)
        {
            spriteBatch.Draw(spritesheet, screenPosition, animation.Frame, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(rectangle), animation.Frame, Color.White);
        }
        #endregion

    }
}
