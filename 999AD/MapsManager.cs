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
        static int tileSize;
        public static Texture2D spritesheet;
        public static List<Rectangle> sourceRectangles = new List<Rectangle>();
        public static List<Map> maps = new List<Map>();
        public static void Inizialize(int _tileSize, Texture2D _spritesheet)
        {
            tileSize = _tileSize;
            spritesheet = _spritesheet;
            for (int i=0; i<(int)Tile.TileType.total; i++)
                sourceRectangles.Add(new Rectangle(((i % (int)Tile.TileType.total) * tileSize),
                    (i / (int)Tile.TileType.total),
                    tileSize,
                    tileSize));
            loadMaps();
        }
        public static int TileSize
        {
            get { return tileSize; }
        }
        static void loadMaps()
        {
            maps.Add(new Map(20, 60));
            maps.Add(new Map(20, 30));
        }
    }
}
