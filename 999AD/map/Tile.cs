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
        public static readonly int tileSize=32;
        static Texture2D spritesheet; //set by MapsManager
        static Rectangle[] sourceRectangles = new Rectangle[(int)TileType.total]; //filled by the Inizialize() function, which is called by MapsManager
        public TileType tileType;
        Vector2 position;
        #endregion
        #region CONSTRUCTORS
        public Tile(Vector2 _position)
        {
            tileType = TileType.empty;
            position = _position;
        }
        public Tile(Vector2 _position, TileType _tileType)
        {
            tileType = _tileType;
            position = _position;
        }
        public static void LoadTextures(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            for (int i = 0; i < (int)TileType.total; i++)
                sourceRectangles[i]=new Rectangle(((i % (spritesheet.Width/tileSize)) * tileSize),
                    i / (spritesheet.Width / tileSize) * tileSize,
                    tileSize,
                    tileSize);
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
            if (tileType == TileType.empty || tileType == TileType.solidEmpty)
                return;
            spriteBatch.Draw(spritesheet,Camera.RelativeVector(position), sourceRectangles[(int)tileType], Color.White);
        }
        #endregion
    }
}
