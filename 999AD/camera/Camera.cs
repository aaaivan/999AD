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
        static bool lockOnPlayer; //if false, the camera will follow "pointLocked"
        public static Vector2 pointLocked= new Vector2(0,0); //point followed by the camera
        static readonly Rectangle screenRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
        static int roomWidth;
        static int roomHeight;
        static float scale;
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, float _scale, bool _lockOnPlayer)
        {
            background = _background;
            scale = _scale;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            rectangle = new Rectangle(0, CameraManager.maxOffsetY, (int)(Game1.gameWidth/scale), (int)(Game1.gameHeight/scale));
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
        public static void Update(bool shaking)
        {
            if (lockOnPlayer) //follow the player if lock on player is true
            {
                rectangle.X = (int)MathHelper.Clamp(Player.Center.X - Game1.gameWidth / (2f*scale),
                                                         0,
                                                         roomWidth - Game1.gameWidth/scale);
                rectangle.Y = (int)MathHelper.Clamp(Player.Center.Y+Player.height/2 - Game1.gameHeight / (2f * scale),
                                                        CameraManager.maxOffsetY,
                                                        roomHeight - Game1.gameHeight/scale - CameraManager.maxOffsetY);
            }
            else //follow point locked otherwise
            {
                pointLocked.X= MathHelper.Clamp(pointLocked.X,
                                                Game1.gameWidth/ (2f * scale),
                                                roomWidth - Game1.gameWidth/ (2f * scale));
                int offset = shaking ? CameraManager.maxOffsetY : 0;
                pointLocked.Y= MathHelper.Clamp(pointLocked.Y,
                                                Game1.gameHeight / (2f * scale)+ offset,
                                                roomHeight - Game1.gameHeight / (2f * scale)- offset);
                rectangle.X = (int)(pointLocked.X - Game1.gameWidth / (2f * scale));
                rectangle.Y = (int)(pointLocked.Y - Game1.gameHeight / (2f * scale));
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenRectangle, rectangle, Color.White);
        }
        #endregion
    }
}
