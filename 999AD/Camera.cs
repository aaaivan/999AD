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
    static class Camera
    {
        #region DECLARATIONS
        static Texture2D background;
        static Rectangle rectangle; //area framed in the camera
        static int roomWidth;
        static int roomHeight;
        static float scale; //zoom in if >1, zoom out if <1
        static bool lockOnPlayer=true; //if false, the camera will follow "pointLocked"
        static Vector2 pointLocked=Vector2.Zero; //point followed by the camera
        static Rectangle screenRectangle = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);
        static Vector2 screenCenter = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
        public static bool shaking =false;
        static int maxOffsetY =2; //amplitude of the rumble
        static int offsetY =-2; //current offset
        static float timeBetweenOffsets =0.1f; //tells how ofter the offset is changed in sign (smaller time->faster rumble)
        static float offsetElapsedTime = 0f; //time elapsed since the last change in offsetY
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, float _scale=1)
        {
            background = _background;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            scale = _scale;
            rectangle = new Rectangle(0, 0, (int)(Game1.screenWidth / scale), (int)(Game1.screenHeight / scale));
        }
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, Vector2 _pointLocked, float _scale=1)
        {
            background = _background;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            lockOnPlayer = false;
            pointLocked = _pointLocked;
            scale = _scale;
            rectangle = new Rectangle(0, 0, (int)(Game1.screenWidth / scale), (int)(Game1.screenHeight / scale));
        }
        #endregion
        #region PROPERTIES
        //return the center of the framing
        static Vector2 FramingCenter
        {
            get{ return new Vector2(rectangle.X + Game1.screenWidth / (2 * scale), rectangle.Y + Game1.screenHeight / (2 * scale)); }
        }
        static public float Scale
        {
            get { return scale; }
            set
            {
                rectangle.Width = (int)(Game1.screenWidth / scale);
                rectangle.Height = (int)(Game1.screenHeight / scale);
                scale = value;
            }
        }
        #endregion
        #region METHODS
        //convert a rectangle from game world to screen coordinates for drawing
        public static Rectangle DrawRectangle(Rectangle rect)
        {
            if (scale==1f)
                return new Rectangle(rect.X - rectangle.X,
                                    rect.Y - rectangle.Y,
                                    rect.Width,
                                    rect.Height);
            return new Rectangle((int)(screenCenter.X+(rect.X-FramingCenter.X)*scale),
                                 (int)(screenCenter.Y + (rect.Y - FramingCenter.Y) * scale),
                                 (int)(rect.Width * scale),
                                 (int)(rect.Height * scale));
        }
        static void NextOffset(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            offsetElapsedTime += elapsedTime;
            if (offsetElapsedTime >= timeBetweenOffsets)
            {
                offsetY *= -1;
                offsetElapsedTime = 0;
            }
        }
        public static void Update(GameTime gameTime)
        {
            if (lockOnPlayer)
            {
                rectangle.X = (int)MathHelper.Clamp(Player.Center.X - Game1.screenWidth / (2 * scale),
                                                         0,
                                                         roomWidth - Game1.screenWidth / scale);
                rectangle.Y = (int)MathHelper.Clamp(Player.Center.Y - Game1.screenHeight / (2 * scale),
                                                        maxOffsetY,
                                                        roomHeight - Game1.screenHeight / scale - maxOffsetY);
            }
            else
            {
                rectangle.X = (int)MathHelper.Clamp(pointLocked.X - Game1.screenWidth / (2 * scale),
                                                         0,
                                                         roomWidth - Game1.screenWidth / scale);
                rectangle.Y = (int)MathHelper.Clamp(pointLocked.Y - Game1.screenHeight / (2 * scale),
                                                        maxOffsetY,
                                                        roomHeight - Game1.screenHeight / scale - maxOffsetY);
            }
            if (shaking)
            {
                rectangle.Y += offsetY;
                NextOffset(gameTime);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenRectangle, rectangle, Color.White);
        }
        #endregion
    }
}
