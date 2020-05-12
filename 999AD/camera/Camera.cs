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
        public static Vector2 position; //area framed in the camera
        static Rectangle screenRectangle;
        static int roomWidth;
        static int roomHeight;
        static float scale;
        #endregion
        #region CONSTRUCTORS
        public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room, float _scale)
        {
            background = _background;
            scale = _scale;
            roomWidth = MapsManager.maps[(int)_room].RoomWidthtPx;
            roomHeight = MapsManager.maps[(int)_room].RoomHeightPx;
            position = new Vector2(0, CameraManager.maxOffsetY);
            screenRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
        }
        #endregion
        #region PROPERTIES
        public static float Scale
        {
            get { return scale; }
        }
        public static Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)(screenRectangle.Width / scale), (int)(screenRectangle.Height / scale)); }
        }
        #endregion
        #region METHODS
        //convert a rectangle from game world to screen coordinates for drawing
        public static Rectangle RelativeRectangle(Vector2 _worldPosition, int _width, int _height)
        {
            return new Rectangle((int)((_worldPosition.X - position.X) * scale),
                                 (int)((_worldPosition.Y - position.Y) * scale),
                                 (int)(_width * scale),
                                 (int)(_height * scale));
        }
        public static Rectangle RelativeRectangle(Rectangle rect)
        {
            return new Rectangle((int)((rect.X - position.X) * scale),
                                 (int)((rect.Y - position.Y) * scale),
                                 (int)(rect.Width * scale),
                                 (int)(rect.Height * scale));
        }
        public static void Update(bool shaking, Vector2 pointLocked)
        {
            pointLocked.X = MathHelper.Clamp(pointLocked.X,
                                            Game1.gameWidth / (2f * scale),
                                            roomWidth - Game1.gameWidth / (2f * scale));
            int offset = shaking ? CameraManager.maxOffsetY : 0;
            pointLocked.Y = MathHelper.Clamp(pointLocked.Y,
                                            Game1.gameHeight / (2f * scale) + offset,
                                            roomHeight - Game1.gameHeight / (2f * scale) - offset);
            position.X = (pointLocked.X - Game1.gameWidth / (2f * scale));
            position.Y = (pointLocked.Y - Game1.gameHeight / (2f * scale));
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, screenRectangle, Rectangle, Color.White);
        }
        #endregion
    }
}
