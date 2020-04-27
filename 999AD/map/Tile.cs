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
            empty, solidEmpty,
            grassTL, grassTR, grassBL, grassBR, grassT, grassR, grassB, grassL, grass,
            stoneTL, stoneTR, stoneBL, stoneBR, stoneT, stoneR, stoneB, stoneL, stone,
            dirtTL, dirtTR, dirtBL, dirtBR, dirtT, dirtR, dirtB, dirtL, dirt,
            marbleT, marbleR, marbleB, marbleL, marble,
            caveT, caveR, caveB, caveL, cave,
            spikeT, spikeR, spikeB, spikeL,
            total
        }
        public static readonly int tileSize=8;
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
        //inizialize the "spritesheet" variable and define the source rectangles
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
        #region PROPERTIES
        Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize); }
        }
        #endregion
        #region METHODS
        public bool isSolid()
        {
            if ((int)tileType>0 && (int)tileType<39)
                return true;
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (tileType == TileType.empty || tileType == TileType.solidEmpty)
                return;
            spriteBatch.Draw(spritesheet,Camera.RelativeRectangle(Rectangle), sourceRectangles[(int)tileType], Color.White);
        }
        public static void DrawAtLocation(SpriteBatch spriteBatch, int tileType, Vector2 screenPosition)
        {
            spriteBatch.Draw(spritesheet, screenPosition, sourceRectangles[tileType], Color.White);
        }
        #endregion
    }
}
