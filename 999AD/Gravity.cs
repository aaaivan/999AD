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
    static class Gravity
    {
        #region DECLARATIONS
        public static float gravityAcceleration;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(float _gravityAcceleration)
        {
            gravityAcceleration = _gravityAcceleration;
        }
        #endregion
        public static void MoveDestructableObject(ref Vector2 velocity, ref Vector2 position, int width, int height, ref bool active, float elapsedTime, float maxYVelocity=500)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
            #region MOVE HORIZONTALLY
            position.X += velocity.X * elapsedTime;
            int topRow = MathHelper.Clamp(rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = MathHelper.Clamp((rectangle.Bottom - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = MathHelper.Clamp(rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = MathHelper.Clamp((rectangle.Right - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check right-hand side
            if (velocity.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        active = false;
                        return;
                    }
                }
            }
            //check left-hand side
            else if (velocity.X < 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, leftCol].isSolid())
                    {
                        active = false;
                        return;
                    }
                }
            }
            #endregion
            #region MOVE VERTICALLY
            velocity.Y += gravityAcceleration * elapsedTime;
            if (velocity.Y > maxYVelocity)
                velocity.Y = maxYVelocity;
            position.Y += velocity.Y * elapsedTime;
            topRow = MathHelper.Clamp(rectangle.Y / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = MathHelper.Clamp((rectangle.Bottom - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = MathHelper.Clamp(rectangle.X / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = MathHelper.Clamp((rectangle.Right - 1) / Tile.TileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check bottom
            if (velocity.Y > 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[btmRow, col].isSolid())
                    {
                        active = false;
                        return;
                    }
                }
                for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms.Count; i++)
                {
                    RotatingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].rotatingPlatforms[i];
                    if (p.Rectangle.Intersects(rectangle) &&
                        p.Rectangle.Bottom >= rectangle.Bottom)
                    {
                        active = false;
                        return;
                    }
                }
                for (int i = 0; i < PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms.Count; i++)
                {
                    MovingPlatform p = PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i];
                    if (p.Rectangle.Intersects(rectangle) &&
                        p.Rectangle.Bottom >= rectangle.Bottom)
                    {
                        active = false;
                        return;
                    }
                }
            }
            //check top
            else if (velocity.Y < 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[topRow, col].isSolid())
                    {
                        active = false;
                        return;
                    }
                }
            }
            #endregion
        }
    }
}
