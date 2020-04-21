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
        public static Rectangle rectangle; //area framed in the camera
        static int roomWidth;
        static int roomHeight;
        public static bool lockOnPlayer=true; //if false, the camera will follow "pointLocked"
        public static Vector2 pointLocked=Vector2.Zero; //point followed by the camera
        static readonly Rectangle screenRectangle = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room)
        {
            background = _background;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            rectangle = new Rectangle(0, 0, (int)(Game1.screenWidth), (int)(Game1.screenHeight));
        }
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, Vector2 _pointLocked)
        {
            background = _background;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            lockOnPlayer = false;
            pointLocked = _pointLocked;
            rectangle = new Rectangle(0, 0, (int)(Game1.screenWidth), (int)(Game1.screenHeight));
        }
        #endregion
        #region METHODS
        //convert a rectangle from game world to screen coordinates for drawing
        public static Rectangle RelativeRectangle(Rectangle rect)
        {
            return new Rectangle(rect.X - rectangle.X,
                                 rect.Y - rectangle.Y,
                                 rect.Width,
                                 rect.Height);
        }
        public static Vector2 RelativeVector(Vector2 vector)
        {
            return new Vector2(vector.X - rectangle.X,
                               vector.Y - rectangle.Y);
        }
        public static void Update(GameTime gameTime, int maxOffsetY)
        {
            if (lockOnPlayer)
            {
                rectangle.X = (int)MathHelper.Clamp(Player.Center.X - Game1.screenWidth / 2f,
                                                         0,
                                                         roomWidth - Game1.screenWidth);
                rectangle.Y = (int)MathHelper.Clamp(Player.Center.Y - Game1.screenHeight / 2f,
                                                        maxOffsetY,
                                                        roomHeight - Game1.screenHeight - maxOffsetY);
            }
            else
            {
                pointLocked.X= MathHelper.Clamp(pointLocked.X,
                                                Game1.screenWidth/2f,
                                                roomWidth - Game1.screenWidth/2f);
                pointLocked.Y= MathHelper.Clamp(pointLocked.Y,
                                                Game1.screenHeight / 2f,
                                                roomHeight - Game1.screenHeight / 2f);
                rectangle.X = (int)(pointLocked.X - Game1.screenWidth / 2f);
                rectangle.Y = (int)(pointLocked.Y - Game1.screenHeight / 2f);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenRectangle, rectangle, Color.White);
        }
        #endregion
    }
}
