using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class Door
    {
        #region DECLARATIONS
        public enum TextureType
        {
            brassDoor, goldDoor, bronzeDoor, silverDoor,  total
        }
        static public int IDcounter;
        public int ID { get; private set; } //used to keep track of doors that have beeen opened
        static Texture2D spritesheet;
        //each source rectangle define the position of the image of the door in the spritesheet
        static Rectangle[] sourceRectangles;
        Point position;//position of the door in world coordinates
        Collectable.ItemType key;//type of collectable that opens the door
        TextureType textureType;
        bool closed;
        #endregion
        #region CONSTRUCTOR
        public Door (Point _position, TextureType _type, Collectable.ItemType _key)
        {
            ID = IDcounter;
            IDcounter++;
            position = _position;
            key = _key;
            textureType = _type;
            closed = true;
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            //define the source rectangle for each door type
            sourceRectangles = new Rectangle[(int)TextureType.total]
                {
                    new Rectangle(0,0,16,40),
                    new Rectangle(112,0,16,40),
                    new Rectangle(224,0,16,40),
                    new Rectangle(336,0,16,40),
                };
        }
        #endregion
        #region PROPERTIES

        //the player can open the door if they are inside this rectangle
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

        //rectangle within which the door is drawn
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
            //make all the tiles on top of the door to type "solidEmpty" (invisible and NOT traversable)
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
            //if the player is within the interaction rectangle of the door
            //and they have the proper key, then open the door
            if (InteractionRectangle.Intersects(Player.CollisionRectangle) &&
                CollectablesManager.TryRemoveFromInventory(key,new Vector2(DrawRectangle.Center.X, DrawRectangle.Center.Y)))
            {
                OpenDoor((int)RoomsManager.CurrentRoom);
                AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.CurrentRoom].AddTempAnimatedSprite(
                    new AnimatedSprite(new Vector2(position.X, position.Y), AnimatedSprite.GetDoorAnimation(textureType)));
            }
        }

        public void OpenDoor(int roomType)
        {
            //turn all the tiles on top of the door to type "empty" (invisible and traversable)
            closed = false;
            int topRow = (int)position.Y / Tile.tileSize;
            int btmRow = ((int)position.Y + sourceRectangles[(int)textureType].Height - 1) / Tile.tileSize;
            int leftCol = (int)position.X / Tile.tileSize;
            int rightCol = ((int)position.X + sourceRectangles[(int)textureType].Width - 1) / Tile.tileSize;
            for (int row = topRow; row <= btmRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    MapsManager.maps[roomType].array[row, col].tileType = Tile.TileType.empty;
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
