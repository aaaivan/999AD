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
        static bool removeRowSFX = false;
        static float timeBoforeRemovingNextTile = 0;
        static float timer = 0f;
        static List<int[]> tilesToRemove = new List<int[]>();
        #endregion
        #region CONSTRACTOR
        public RoomMap(int _roomHeightTiles, int _roomWidthTiles)
        {
            roomWidthTiles = _roomWidthTiles;
            roomHeightTiles = _roomHeightTiles;
            array = new Tile[roomHeightTiles, roomWidthTiles]; //array of tiles
            for (int row = 0; row < roomHeightTiles; row++) //fill the array with empty tiles
            {
                for (int col = 0; col < roomWidthTiles; col++)
                    array[row, col] = new Tile(new Vector2(col * Tile.tileSize,
                        row * Tile.tileSize));
            }
        }
        #endregion
        #region PROPERTIES
        //room height in pixels
        public int RoomHeightPx
        {
            get { return roomHeightTiles * Tile.tileSize; }
        }
        //room width in pixels
        public int RoomWidthtPx
        {
            get { return roomWidthTiles* Tile.tileSize; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (removeRowSFX)
            {
                timer += elapsedTime;
                if (timer >= timeBoforeRemovingNextTile)
                {
                    array[tilesToRemove[0][0], tilesToRemove[0][1]].tileType = Tile.TileType.empty;
                    tilesToRemove.RemoveAt(0);
                    timer = 0;
                    if (tilesToRemove.Count == 0)
                    {
                        removeRowSFX = false;
                    }
                }
            }
        }
        public void RemoveRowOfTiles(List<int[]> _tilesToRemove, float _timeBeforeRemovingNextTile)
        {
            if (!removeRowSFX)
            {
                removeRowSFX = true;
                tilesToRemove = _tilesToRemove;
                timeBoforeRemovingNextTile = _timeBeforeRemovingNextTile;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = Camera.rectangle.Y/Tile.tileSize; row <= (Camera.rectangle.Bottom-1) / Tile.tileSize; row++)
            {
                for (int col = Camera.rectangle.X/Tile.tileSize; col <= (Camera.rectangle.Right-1) / Tile.tileSize; col++)
                    array[row, col].Draw(spriteBatch);
            }
        }
        #endregion
    }
}