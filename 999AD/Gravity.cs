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
        public static void MoveDestructableObject(ref Vector2 velocity, ref Vector2 position, int width, int height, ref bool active, float elapsedTime, float maxYVelocity=2000)
        {
            #region MOVE HORIZONTALLY
            position.X += velocity.X * elapsedTime;
            int topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int btmRow = (int)MathHelper.Clamp((position.Y + height - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            int leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            int rightCol = (int)MathHelper.Clamp((position.X + width - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check right-hand side
            if (velocity.X > 0)
            {
                for (int row = topRow; row <= btmRow; row++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, rightCol].isSolid())
                    {
                        position.X = rightCol * Tile.tileSize - width;
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
                        position.X = (leftCol + 1) * Tile.tileSize;
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
            topRow = (int)MathHelper.Clamp(position.Y / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            btmRow = (int)MathHelper.Clamp((position.Y + height - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles - 1);
            leftCol = (int)MathHelper.Clamp(position.X / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            rightCol = (int)MathHelper.Clamp((position.X + width - 1) / Tile.tileSize, 0, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - 1);
            //check bottom
            if (velocity.Y > 0)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[btmRow, col].isSolid())
                    {
                        position.Y = btmRow * Tile.tileSize - height;
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
                        position.Y = (topRow + 1) * Tile.tileSize;
                        active = false;
                        return;
                    }
                }
            }
            #endregion
        }
    }
}
