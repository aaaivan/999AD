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
    class Map
    {

        int roomHeightTiles;
        int roomWidthTiles;
        Tile[,] array;
        public Map(int _roomHeightTiles, int _roomWidthTiles)
        {
            roomWidthTiles = _roomWidthTiles;
            roomHeightTiles = _roomHeightTiles;
            array = new Tile[roomHeightTiles, roomWidthTiles];
            for (int row=0; row< roomHeightTiles; row++)
            {
                for (int col = 0; col < roomWidthTiles; col++)
                    array[row, col] = new Tile(new Rectangle(col * MapsManager.TileSize,
                        row * MapsManager.TileSize,
                        MapsManager.TileSize,
                        MapsManager.TileSize));
            }
        }
        #region PROPERTIES
        public int RoomHeightPx
        {
            get { return roomHeightTiles * MapsManager.TileSize; }
        }
        public int RoomWidthtPx
        {
            get { return roomWidthTiles*MapsManager.TileSize; }
        }
        #endregion
        #region METHODS
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < roomHeightTiles; row++)
            {
                for (int col = 0; col < roomWidthTiles; col++)
                    array[row, col].Draw(spriteBatch);
            }
        }
        #endregion
    }
}