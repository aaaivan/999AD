﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class LavaGeyser
    {
        #region DECLARATIONS
        static public Texture2D spritesheet;
        static public Rectangle whiteTexture;
        static public readonly int size= 24;
        static public readonly int heigthBeforeErupting = 40;
        float initialYVelocity;
        float yVelocity;
        static Rectangle geyserTop; //source rect on sritesheet
        static Rectangle geyserBody; //source rect on sritesheet
        Vector2 position;
        float timeBeforeErupting;
        float elapsedTimeBeforeErupting;
        bool erupted;
        #endregion
        #region CONSTRUCTOR
        public LavaGeyser(float xCenterPosition, float _timeBeforeErupting, int _initialYvelocity=-1200)
        {
            yVelocity = 0;
            initialYVelocity = _initialYvelocity;
            elapsedTimeBeforeErupting = 0;
            erupted = false;
            position = new Vector2(xCenterPosition - 0.5f*size, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx);
            timeBeforeErupting = _timeBeforeErupting;
            geyserTop = new Rectangle(0,173, size, size);
            geyserBody= new Rectangle(0, 197, size, size);
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            whiteTexture = new Rectangle(400, 40, 8,8);
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
            //if cowntown to eruption has not started,
            //slowly move geyser up until it reaches heightBeforeErupting
            if (elapsedTimeBeforeErupting<=0.0001f )
            {
                if (position.Y <= MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting)
                {
                    position.Y = MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - heigthBeforeErupting;
                    elapsedTimeBeforeErupting += elapsedTime;//start cowntdown
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
                        yVelocity = initialYVelocity;//shoot geyser
                        erupted = true;
                    }
                    else
                        elapsedTimeBeforeErupting += elapsedTime;
                }
                else
                {
                    //let gravity act on geyser
                    position.Y += yVelocity*elapsedTime;
                    yVelocity += Gravity.gravityAcceleration * elapsedTime;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(new Vector2(position.X,0),size, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx),
                whiteTexture,
                Color.Red * 0.3f);
            spriteBatch.Draw(spritesheet,
                Camera.RelativeRectangle(position, size,size),
                geyserTop,
                Color.White);
            for (int i=1; i<=(MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx - (int)position.Y + size -1)/size; i++)
            {
                spriteBatch.Draw(spritesheet,
                    Camera.RelativeRectangle(position+new Vector2(0,i*size), size, size),
                    geyserBody,
                    Color.White);
            }
        }
        #endregion
    }
}
