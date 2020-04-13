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
    class RoomMap
    {
        #region DECLRATIONS
        public readonly int roomHeightTiles; //room width in tiles
        public readonly int roomWidthTiles; //room height in tiles
        public Tile[,] array; //array of all the tiles forming the room
        #endregion
        #region CONSTRACTOR
        public RoomMap(int _roomHeightTiles, int _roomWidthTiles)
        {
            roomWidthTiles = _roomWidthTiles;
            roomHeightTiles = _roomHeightTiles;
            array = new Tile[roomHeightTiles, roomWidthTiles]; //array of tiles
            for (int row=0; row< roomHeightTiles; row++) //fill the array with empty tiles
            {
                for (int col = 0; col < roomWidthTiles; col++)
                    array[row, col] = new Tile(new Vector2(col * Tile.TileSize,
                        row * Tile.TileSize));
            }
        }
        #endregion
        #region PROPERTIES
        //room height in pixels
        public int RoomHeightPx
        {
            get { return roomHeightTiles * Tile.TileSize; }
        }
        //room width in pixels
        public int RoomWidthtPx
        {
            get { return roomWidthTiles* Tile.TileSize; }
        }
        #endregion
        #region METHODS
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = Camera.rectangle.Y/Tile.TileSize; row <= (Camera.rectangle.Bottom-1) / Tile.TileSize; row++)
            {
                for (int col = Camera.rectangle.X/Tile.TileSize; col <= (Camera.rectangle.Right-1) / Tile.TileSize; col++)
                    array[row, col].Draw(spriteBatch);
            }
        }
        #endregion
    }
}