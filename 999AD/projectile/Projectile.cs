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
        #region DECLARATION
        public static Texture2D spritesheet;
        Animation animation;
        public readonly static int width = 10;
        public readonly static int height = 10;
        Vector2 position;
        Vector2 velocity;
        bool active;
        #endregion
        #region CONSTRUCTOR
        public Projectile(Vector2 _position, Vector2 _initialVelocity)
        {
            position = _position;
            velocity = _initialVelocity;
            animation = new Animation(new Rectangle(0, 0, spritesheet.Width, spritesheet.Height), width, height, spritesheet.Width/width, 0.1f, true);
            active = true;
        }
        #endregion
        #region PROPERTIES
        //return the rectangle of the projectile
        Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
        public bool Active
        {
            get { return active; }
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
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(Rectangle), animation.Frame, Color.White);
        }
        #endregion
    }
}
