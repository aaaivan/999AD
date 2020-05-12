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
    class LavaGeyser
    {
        #region DECLARATIONS
        static public Texture2D spritesheet;
        static public Texture2D whiteTexture;
        static public readonly int size= 24;
        static public readonly int heigthBeforeErupting = 40;
        float initialYVelocity;
        float yVelocity;
        Animation geyserTopAnim;
        Animation geyserBodyAnim;
        Vector2 position;
        float timeBeforeErupting;
        float elapsedTimeBeforeErupting;
        bool erupted;
        #endregion
        #region CONSTRUCTOR
        public LavaGeyser(float xCenterPosition, float _timeBeforeErupting, int _initialYvelocity=-1400)
        {
            yVelocity = 0;
            initialYVelocity = _initialYvelocity;
            elapsedTimeBeforeErupting = 0;
            erupted = false;
            position = new Vector2(xCenterPosition - 0.5f*size, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx);
            timeBeforeErupting = _timeBeforeErupting;
            geyserTopAnim = new Animation(new Rectangle(0, 0, spritesheet.Width, size), size, size, 2, 0.5f, true);
            geyserBodyAnim= new Animation(new Rectangle(0, size, spritesheet.Width, size), size, size, 2, 0.5f, true);
        }
        public static void Inizialize(Texture2D _spritesheet, Texture2D _white)
        {
            spritesheet = _spritesheet;
            whiteTexture = _white;
        }
        #endregion
        #region PROPERTIES
        public Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X,
                (int)position.Y+size/3,
                size,
                MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx- (int)position.Y-size/3); }
        }
        #endregion
        #region METHODS
        public bool isActive()
        {
            if (position.Y > MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx)
                return false;
            return true;
        }
        public void Update(float elapsedTime)
        {
            geyserBodyAnim.Update(elapsedTime);
            geyserTopAnim.Update(elapsedTime);
            if (elapsedTimeBeforeErupting==0 )
            {
                if (position.Y < MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting)
                {
                    position.Y = MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting;
                    elapsedTimeBeforeErupting += elapsedTime;
                }
                else
                    position.Y -= 2f;
            }
            else
            {
                if (!erupted)
                {
                    if (elapsedTimeBeforeErupting > timeBeforeErupting)
                    {
                        yVelocity = initialYVelocity;
                        erupted = true;
                    }
                    else
                        elapsedTimeBeforeErupting += elapsedTime;
                }
                else
                {
                    position.Y += yVelocity*elapsedTime;
                    yVelocity += Gravity.gravityAcceleration * elapsedTime;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whiteTexture,
                Camera.RelativeRectangle(new Vector2(position.X,0),size, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx),
                Color.Red * 0.1f);
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(position, size,size),
                geyserTopAnim.Frame,
                Color.White);
            for (int i=1; i<=(MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - (int)position.Y + size -1)/size; i++)
            {
                spriteBatch.Draw(spritesheet,
                    Camera.RelativeRectangle(position+new Vector2(0,i*size), size, size),
                    geyserBodyAnim.Frame,
                    Color.White);
            }
        }
        #endregion
    }
}
