using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class Tile
    {
        #region DECLARATIONS
        public enum TileType
        {
            empty, solidEmpty,

            caveOutTL, caveOutT, caveOutTR, dirtOutTL, dirtOutT, dirtOutTR, marbleOutTL, marbleOutT, marbleOutTR,
            caveSpikeL, caveSpikeR, caveOutL, caveOut, caveOutR, dirtOutL, dirtOut, dirtOutR, marbleOutL, marbleOut, marbleOutR,
            caveSpikeT, caveSpikeB, caveOutBL, caveOutB, caveOutBR, dirtOutBL, dirtOutB, dirtOutBR, marbleOutBL, marbleOutB, marbleOutBR,
            marbleSpikeL, marbleSpikeR, caveInTL, caveInT, caveInTR, dirtInTL, dirtInT, dirtInTR, marbleInTL, marbleInT, marbleInTR,
            marbleSpikeT, marbleSpikeB, caveInL, caveIn, caveInR, dirtInL, dirtIn, dirtInR, marbleInL, marbleIn, marbleInR,
            dirtSpikeL, dirtSpikeR, caveInBL, caveInB, caveInBR, dirtInBL, dirtInB, dirtInBR, marbleInBL, marbleInB, marbleInBR,
            dirtSpikeT, dirtSpikeB, total
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
            if ((int)tileType >= 1 && !isHarmful())
                return true;
            return false;
        }
        public bool isHarmful()
        {
            if ((int)tileType >= 11)
            {
                if ((int)tileType%11==0 || (int)tileType % 11==1)
                    return true;
            }
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
