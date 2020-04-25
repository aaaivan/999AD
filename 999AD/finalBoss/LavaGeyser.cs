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
        static public readonly int size= 96;
        static public int heigthBeforeErupting = 120;
        static public float initialYVelocity= -2000;
        Animation geyserTopAnim;
        Animation geyserBodyAnim;
        Vector2 positionTop;
        float yVelocity = 0;
        float timeBeforeErupting;
        float elapsedTimeBeforeErupting=0;
        bool erupted= false;
        #endregion
        #region CONSTRUCTOR
        public LavaGeyser(float xPosition, float _timeBeforeErupting)
        {
            positionTop = new Vector2(xPosition, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx);
            timeBeforeErupting = _timeBeforeErupting;
            geyserTopAnim = new Animation(new Rectangle(0, 0, spritesheet.Width, size), size, size, 2, 0.5f, true);
            geyserBodyAnim= new Animation(new Rectangle(0, size, spritesheet.Width, size), size, size, 2, 0.5f, true);
        }
        #endregion
        #region PROPERTIES
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)positionTop.X - size / 2,
                (int)positionTop.Y,
                size,
                MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx- (int)positionTop.Y); }
        }
        #endregion
        #region METHODS
        public bool isActive()
        {
            if (positionTop.Y > MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx)
                return false;
            return true;
        }
        public void Update(float elapsedTime)
        {
            geyserBodyAnim.Update(elapsedTime);
            geyserTopAnim.Update(elapsedTime);
            if (elapsedTimeBeforeErupting==0 )
            {
                if (positionTop.Y < MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting)
                {
                    positionTop.Y = MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting;
                    elapsedTimeBeforeErupting += elapsedTime;
                }
                else
                    positionTop.Y -= 0.5f;
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
                    positionTop.Y += yVelocity*elapsedTime;
                    yVelocity += Gravity.gravityAcceleration * elapsedTime;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(new Rectangle((int)positionTop.X - size / 2, (int)positionTop.Y, size,size)),
                geyserTopAnim.Frame,
                Color.White);
            for (int i=1; i<=(MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - (int)positionTop.Y + size -1)/size; i++)
            {
                spriteBatch.Draw(spritesheet,
                    Camera.RelativeRectangle(new Rectangle((int)positionTop.X - size / 2, (int)positionTop.Y+i*size, size, size)),
                    geyserBodyAnim.Frame,
                    Color.White);
            }
        }
        #endregion
    }
}
