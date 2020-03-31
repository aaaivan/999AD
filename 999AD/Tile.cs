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
    class Tile
    {
        #region DECLARATIONS
        public enum TileType
        {
            empty, solidEmpty, solid, total
        }
        static int tileSize; //set by MapsManager
        static Texture2D spritesheet; //set by MapsManager
        static List<Rectangle> sourceRectangles = new List<Rectangle>(); //filled by MapsManager
        public TileType tileType;
        Rectangle rectangle;
        #endregion
        #region CONSTRUCTORS
        public Tile(Rectangle _rectangle, TileType _tileType)
        {
            tileType = _tileType;
            rectangle = _rectangle;
        }
        public Tile(Rectangle _rectangle)
        {
            tileType = TileType.empty;
            rectangle = _rectangle;
        }
        public static void Inizialize(int _tileSize, Texture2D _spritesheet)
        {
            tileSize = _tileSize;
            spritesheet = _spritesheet;
            for (int i = 0; i < (int)TileType.total; i++)
                sourceRectangles.Add(new Rectangle(((i % (spritesheet.Width/tileSize)) * tileSize),
                    i / (spritesheet.Width / tileSize) * tileSize,
                    tileSize,
                    tileSize));
        }
        #endregion
        #region PROPERTIES
        //return the tile size
        public static int TileSize
        {
            get { return tileSize; }
        }
        #endregion
        #region METHODS
        public bool isSolid()
        {
            if (tileType == TileType.solid || tileType == TileType.solidEmpty)
                return true;
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet,
                Camera.DrawRectangle(rectangle),
                sourceRectangles[(int)(tileType)],
                Color.White);
        }
        #endregion
    }
}
