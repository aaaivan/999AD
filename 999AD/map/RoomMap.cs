using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class RoomMap
    {
        #region DECLRATIONS
        public readonly int roomHeightTiles; //room width in tiles
        public readonly int roomWidthTiles; //room height in tiles
        public Tile[,] array; //array of all the tiles forming the room
        static bool removeRowSFX;
        static float timeBoforeRemovingNextTile;
        static float timer;
        static List<int[]> tilesToRemove;
        int removeTogether;
        #endregion
        #region CONSTRACTOR
        public RoomMap(int _roomHeightTiles, int _roomWidthTiles)
        {
            roomWidthTiles = _roomWidthTiles;
            roomHeightTiles = _roomHeightTiles;
            removeRowSFX = false;
            timeBoforeRemovingNextTile = 0;
            timer = 0f;
            tilesToRemove = new List<int[]>();
            removeTogether = 1;
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
                    for (int i = 0; i < removeTogether; i++)
                    {
                        if (tilesToRemove.Count == 0)
                        {
                            removeRowSFX = false;
                            break;
                        }
                        array[tilesToRemove[0][0], tilesToRemove[0][1]].tileType = Tile.TileType.empty;
                        tilesToRemove.RemoveAt(0);
                    }
                    timer = 0;
                }
            }
        }
        public void RemoveGroupOfTiles(List<int[]> _tilesToRemove, float _timeBeforeRemovingNextTile, int _removeTogether, float delay=0)
        {
            if (!removeRowSFX)
            {
                removeRowSFX = true;
                tilesToRemove = _tilesToRemove;
                timeBoforeRemovingNextTile = _timeBeforeRemovingNextTile;
                removeTogether = _removeTogether;
                timer = -delay;
            }
        }
        public bool HarmfulTileIntersectsRectangle(Rectangle collisionRect)
        {
            int topRow = MathHelper.Clamp(collisionRect.Y / Tile.tileSize, 0, roomHeightTiles - 1);
            int btmRow = MathHelper.Clamp((collisionRect.Bottom -1) / Tile.tileSize, 0, roomHeightTiles - 1);
            int leftCol = MathHelper.Clamp(collisionRect.X / Tile.tileSize, 0, roomWidthTiles - 1);
            int rightCol = MathHelper.Clamp((collisionRect.Right -1) / Tile.tileSize, 0, roomWidthTiles - 1);
            for (int row = topRow; row <= btmRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, col].isHarmful())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = Camera.Rectangle.Y/Tile.tileSize; row <= (Camera.Rectangle.Bottom-1) / Tile.tileSize; row++)
            {
                for (int col = Camera.Rectangle.X / Tile.tileSize; col <= (Camera.Rectangle.Right - 1) / Tile.tileSize; col++)
                {
                    if (row < 0 || row >= roomHeightTiles || col < 0 || col >= roomWidthTiles)
                        continue;
                    array[row, col].Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}