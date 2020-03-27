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
    static class MapsManager
    {
        #region DECLARATIONS
        public static List<RoomMap> maps = new List<RoomMap>();
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(int _tileSize, Texture2D _spritesheet)
        {
            Tile.Inizialize(_tileSize, _spritesheet);
            loadMaps();
        }
        #endregion
        #region METHODS
        //fill the list maps with the maps for all the rooms
        static void loadMaps()
        {
            maps.Add(new RoomMap(20, 60));
            maps.Add(new RoomMap(20, 30));
            for (int i = 0; i < maps[0].roomWidthTiles; i++)
                maps[0].array[maps[0].roomHeightTiles - 1, i].tileType = Tile.TileType.solid;
            for (int i = 0; i < maps[1].roomWidthTiles; i++)
                maps[1].array[maps[1].roomHeightTiles - 1, i].tileType = Tile.TileType.solid;
        }
        #endregion
    }
}
