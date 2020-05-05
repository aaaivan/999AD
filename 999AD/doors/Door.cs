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
    class Door
    {
        #region DECLARATIONS
        public enum TextureType
        {
            redDoor, greenDoor, total
        }
        static Texture2D spritesheet;
        static Rectangle[] sourceRectangles;
        Point position;
        Collectable.ItemType key;
        TextureType textureType;
        bool closed = true;
        #endregion
        #region CONSTRUCTOR
        public Door (Point _position, TextureType _type, Collectable.ItemType _key)
        {
            position = _position;
            key = _key;
            textureType = _type;
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            sourceRectangles = new Rectangle[(int)TextureType.total]
                {
                    new Rectangle(0,0,16,40),
                    new Rectangle(16,0,16,40)
                };
        }
        #endregion
        #region PROPERTIES
        Rectangle InteractionRectangle
        {
            get
            {
                return new Rectangle(position.X-16,
                                    position.Y, 
                                    sourceRectangles[(int)textureType].Width+32, 
                                    sourceRectangles[(int)textureType].Height);
            }
        }
        Rectangle DrawRectangle
        {
            get
            {
                return new Rectangle(position.X,
                                    position.Y,
                                    sourceRectangles[(int)textureType].Width,
                                    sourceRectangles[(int)textureType].Height);
            }
        }
        public bool Closed
        {
            get { return closed; }
        }
        #endregion
        #region METHODS
        public void LockDoor(RoomsManager.Rooms room)
        {
            int topRow = (int)position.Y / Tile.tileSize;
            int btmRow = ((int)position.Y + sourceRectangles[(int)textureType].Height - 1) / Tile.tileSize;
            int leftCol = (int)position.X / Tile.tileSize;
            int rightCol = ((int)position.X + sourceRectangles[(int)textureType].Width - 1) / Tile.tileSize;
            for (int row = topRow; row <= btmRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    MapsManager.maps[(int)room].array[row, col].tileType = Tile.TileType.solidEmpty;
                }
            }
        }
        public void Update()
        {
            if (InteractionRectangle.Intersects(Player.CollisionRectangle) &&
                CollectablesManager.TryRemoveFromInventory(key))
            {
                closed = false;
                int topRow = (int)position.Y / Tile.tileSize;
                int btmRow = ((int)position.Y + sourceRectangles[(int)textureType].Height - 1) / Tile.tileSize;
                int leftCol = (int)position.X / Tile.tileSize;
                int rightCol = ((int)position.X + sourceRectangles[(int)textureType].Width - 1) / Tile.tileSize;
                for (int row = topRow; row <= btmRow; row++)
                {
                    for (int col = leftCol; col <= rightCol; col++)
                    {
                        MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, col].tileType = Tile.TileType.empty;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(DrawRectangle), sourceRectangles[(int)textureType], Color.White);
        }
        #endregion
    }
}
