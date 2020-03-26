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
        public enum TileType
        {
            empty, solidEmpty, solid, total
        }
        public TileType tileType;
        Rectangle rectangle;
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
        #region METHODS
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MapsManager.spritesheet,
                Camera.DrawRectangle(rectangle),
                MapsManager.sourceRectangles[(int)(tileType)],
                Color.White);
        }
        #endregion
    }
}
