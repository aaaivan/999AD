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
        static Texture2D background; //the background of the room
        public static Rectangle rectangle; //area framed in the camera
        static int roomWidth; //in pixels
        static int roomHeight; //in pixels
        static bool lockOnPlayer; //if false, the camera will follow "pointLocked"
        public static Vector2 pointLocked= new Vector2(0,0); //point followed by the camera
        static readonly Rectangle screenRectangle = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);
        static float scale;
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, float _scale, bool _lockOnPlayer)
        {
            background = _background;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            scale = _scale;
            rectangle = new Rectangle(0, 0, (int)(Game1.screenWidth/scale), (int)(Game1.screenHeight/scale));
            lockOnPlayer = _lockOnPlayer;
        }
        #endregion
        #region PROPERTIES
        public static float Scale
        {
            get { return scale; }
        }
        #endregion
        #region METHODS
        //convert a rectangle from game world to screen coordinates for drawing
        public static Rectangle RelativeRectangle(Rectangle rect)
        {
            return new Rectangle((int)((rect.X - rectangle.X)*scale),
                                 (int)((rect.Y - rectangle.Y)*scale),
                                 (int)(rect.Width*scale),
                                 (int)(rect.Height*scale));
        }
        public static void Update(int maxOffsetY)
        {
            if (lockOnPlayer) //follow the player if lock on player is true
            {
                rectangle.X = (int)MathHelper.Clamp(Player.Center.X - Game1.screenWidth / (2f*scale),
                                                         0,
                                                         roomWidth - Game1.screenWidth/scale);
                rectangle.Y = (int)MathHelper.Clamp(Player.Center.Y - Game1.screenHeight / (2f * scale),
                                                        maxOffsetY,
                                                        roomHeight - Game1.screenHeight/scale - maxOffsetY);
            }
            else //follow point locked otherwise
            {
                pointLocked.X= MathHelper.Clamp(pointLocked.X,
                                                Game1.screenWidth/ (2f * scale),
                                                roomWidth - Game1.screenWidth/ (2f * scale));
                pointLocked.Y= MathHelper.Clamp(pointLocked.Y,
                                                Game1.screenHeight / (2f * scale)+maxOffsetY,
                                                roomHeight - Game1.screenHeight / (2f * scale)-maxOffsetY);
                rectangle.X = (int)(pointLocked.X - Game1.screenWidth / (2f * scale));
                rectangle.Y = (int)(pointLocked.Y - Game1.screenHeight / (2f * scale));
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenRectangle, rectangle, Color.White);
        }
        #endregion
    }
}
