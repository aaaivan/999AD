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
    class LevelEditor
    {
        int currentTileType=0;
        int hoveredTileType = 0;
        int currentRoomNumber = 0;
        string message="LEVEL EDITOR MODE\nClick on a tile to change it to the selected type.\nPress 'M' to access the menu.\n\nEnter to begin.";
        int userInput=0;
        bool start = true;
        bool menu = false;
        bool changeRoom = false;
        bool changeTile = false;
        bool solidView = false;
        bool deadlyView = false;
        string roomInfo="";
        string tileTypeInfo = "";
        string tilePositionInfo="";
        SpriteFont arial32;
        SpriteFont arial16;
        Texture2D whiteTexture;
        public LevelEditor(SpriteFont _arial32, SpriteFont _arial16, Texture2D _whiteTexture)
        {
            arial32 = _arial32;
            arial16 = _arial16;
            whiteTexture = _whiteTexture;
            Camera.lockOnPlayer = false;
            CameraManager.SwitchCamera((RoomsManager.Rooms)currentRoomNumber);
        }

        bool GetInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad0) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad0))
            {
                userInput = userInput * 10 + 0;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad1) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad1))
            {
                userInput = userInput * 10 + 1;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad2) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad2))
            {
                userInput = userInput * 10 + 2;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad3) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad3))
            {
                userInput = userInput * 10 + 3;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad4) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad4))
            {
                userInput = userInput * 10 + 4;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad5) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad5))
            {
                userInput = userInput * 10 + 5;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad6) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad6))
            {
                userInput = userInput * 10 + 6;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad7) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad7))
            {
                userInput = userInput * 10 + 7;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad8) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad8))
            {
                userInput = userInput * 10 + 8;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad9) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad9))
            {
                userInput = userInput * 10 + 9;
                return true;
            }
            return false;
        }
        void ProcessInput()
        {
            if (start && Game1.currentKeyboard.IsKeyDown(Keys.Enter) )
            {
                menu = true;
                changeRoom = false;
                changeTile = false;
                start = false;
                message = "Menu. Press:\nR-Change room\nT-Change tile type\nS-Toggle solid tiles view\nD-Toggle deadly tiles view";
            }
            if (!menu && Game1.currentKeyboard.IsKeyDown(Keys.M))
            {
                menu = true;
                changeRoom = false;
                changeTile = false;
                message = "Menu. Press:\nR-Change room\nT-Change tile type\nS-Toggle solid tiles view\nD-Toggle deadly tiles view";
                return;
            }
            if (menu && Game1.currentKeyboard.IsKeyDown(Keys.R))
            {
                menu = false;
                changeRoom = true;
                changeTile = false;
                message = "Room index: ";
                return;
            }
            else if (menu && Game1.currentKeyboard.IsKeyDown(Keys.T))
            {
                menu = false;
                changeRoom = false;
                changeTile = true;
                message = "Tile index: ";
                return;
            }
            if (menu && Game1.currentKeyboard.IsKeyDown(Keys.S))
            {
                menu = false;
                changeRoom = false;
                changeTile = false;
                solidView = !solidView;
                return;
            }
            if (menu && Game1.currentKeyboard.IsKeyDown(Keys.D))
            {
                menu = false;
                changeRoom = false;
                changeTile = false;
                deadlyView = !deadlyView;
                return;
            }
            if (changeRoom || changeTile)
            {
                if (GetInput())
                {
                    if (changeRoom)
                    {
                        message = "Room index: " + userInput;
                        return;
                    }
                    else if (changeTile)
                    {
                        message = "Tile index: " + userInput;
                        return;
                    }
                }
                if (changeRoom && Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                {
                    currentRoomNumber = userInput < (int)RoomsManager.Rooms.total ? userInput : ((int)RoomsManager.Rooms.total - 1);
                    CameraManager.SwitchCamera((RoomsManager.Rooms)currentRoomNumber);
                    userInput = 0;
                    menu = false;
                    changeRoom = false;
                    changeTile = false;
                    return;

                }
                else if (changeTile && Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                {
                    currentTileType = userInput < (int)Tile.TileType.total ? userInput : ((int)Tile.TileType.total - 1);
                    menu = false;
                    changeRoom = false;
                    changeTile = false;
                    return;
                }
            }
        }
        public void Update(MouseState mouseState)
        {
            ProcessInput();
            if (!menu && !changeRoom && !changeTile)
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Up))
                    Camera.pointLocked.Y -= 3;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
                    Camera.pointLocked.Y += 3;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Right))
                    Camera.pointLocked.X += 3;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
                    Camera.pointLocked.X -= 3;
                int tileRow = 0;
                int tileCol = 0;
                Point mouseLocation = new Point(
                    MathHelper.Clamp(mouseState.X, 0, Game1.screenWidth-1),
                    MathHelper.Clamp(mouseState.Y, 0, Game1.screenHeight-1)
                    );
                tileCol = (mouseLocation.X + Camera.rectangle.X) / Tile.tileSize;
                tileRow = (mouseLocation.Y + Camera.rectangle.Y) / Tile.tileSize;
                tileCol = MathHelper.Clamp(tileCol, 0, MapsManager.maps[currentRoomNumber].roomWidthTiles - 1);
                tileRow = MathHelper.Clamp(tileRow, 0, MapsManager.maps[currentRoomNumber].roomHeightTiles - 1);
                hoveredTileType = (int)MapsManager.maps[currentRoomNumber].array[tileRow, tileCol].tileType;
                if (mouseState.LeftButton==ButtonState.Pressed)
                {
                    MapsManager.maps[currentRoomNumber].array[tileRow, tileCol].tileType = (Tile.TileType)currentTileType;
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    MapsManager.maps[currentRoomNumber].array[tileRow, tileCol].tileType = (Tile.TileType)0;
                }
                roomInfo = "Current room: " + currentRoomNumber + "(" + (RoomsManager.Rooms)currentRoomNumber + ")";
                tileTypeInfo = "Tile selected: " + currentTileType + "("+(Tile.TileType)currentTileType+")";
                tilePositionInfo = "Tile hovered: row "+tileRow+", col "+tileCol+"\n Type: " + hoveredTileType + "(" + (Tile.TileType)hoveredTileType + ")";
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (start)
            {
                spriteBatch.DrawString(arial32,
                        message,
                        new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2) - arial32.MeasureString(message) / 2,
                        Color.Black);
                return;
            }
            Camera.Draw(spriteBatch);
            MapsManager.maps[currentRoomNumber].Draw(spriteBatch);
            if (solidView)
            {
                for (int row = Camera.rectangle.Y / Tile.tileSize; row <= (Camera.rectangle.Bottom - 1) / Tile.tileSize; row++)
                {
                    for (int col = Camera.rectangle.X / Tile.tileSize; col <= (Camera.rectangle.Right - 1) / Tile.tileSize; col++)
                    {
                        if (MapsManager.maps[currentRoomNumber].array[row, col].isSolid())
                            spriteBatch.Draw(whiteTexture, Camera.RelativeVector(new Vector2(col, row)*Tile.tileSize), Color.Red*0.3f);
                    }
                }
            }
            spriteBatch.Draw(whiteTexture, new Rectangle(0, Game1.screenHeight, Game1.screenWidth, 40), Color.LightGray);
            spriteBatch.DrawString(arial16,
                                    tileTypeInfo,
                                    new Vector2(Game1.screenWidth / 2, Game1.screenHeight + 20) - arial16.MeasureString(tileTypeInfo) / 2,
                                    Color.Black);
            spriteBatch.DrawString(arial16,
                                    tilePositionInfo,
                                    new Vector2(Game1.screenWidth -10- arial16.MeasureString(tilePositionInfo).X,
                                                Game1.screenHeight + 20 - arial16.MeasureString(tilePositionInfo).Y / 2),
                                    Color.Black);
            spriteBatch.DrawString(arial16,
                                    roomInfo,
                                    new Vector2(10, Game1.screenHeight + 20 - arial16.MeasureString(roomInfo).Y / 2),
                                    Color.Black);
            if (menu||changeRoom||changeTile)
            {
                spriteBatch.Draw(
                        whiteTexture,
                        new Rectangle((int)(Game1.screenWidth / 2 - arial32.MeasureString(message).X / 2 - 10),
                                       (int)(Game1.screenHeight / 2 - arial32.MeasureString(message).Y / 2 - 10),
                                       (int)arial32.MeasureString(message).X+20,
                                       (int)arial32.MeasureString(message).Y+20),
                        Color.LightGray);

                spriteBatch.DrawString(arial32,
                        message,
                        new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2) - arial32.MeasureString(message) / 2,
                        Color.Black);
            }

        }
    }
}
