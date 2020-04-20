using System;
using System.IO;
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
        enum MenuState
        {
            start, none, main, rooms, tiles, view, pickRoom, pickRoomSize, pickResizingDirection , pickTileIndex, selectRandomTiles
        }
        int currentTileType=0;
        int hoveredTileType = 0;
        int currentRoomNumber = 0;
        int widthTiles=0;
        int heightTiles=0;
        MenuState menu= MenuState.start;
        string message= "LEVEL EDITOR MODE\nLeft click on a tile to change it to the selected type.\nRight click on a tile to remove it.\nPress 'M' to access the menu.\n\nEnter to begin.";
        int userInputInt=0;
        string userInputString = "";
        bool randomMode=false;
        bool solidView = false;
        bool deadlyView = false;
        List<int> randomTiles=new List<int>();
        string roomInfo="";
        string tileTypeInfo = "";
        string tilePositionInfo="";
        SpriteFont arial32;
        SpriteFont arial14;
        Texture2D whiteTexture;
        Random rand = new Random();
        public LevelEditor(SpriteFont _arial32, SpriteFont _arial16, Texture2D _whiteTexture)
        {
            arial32 = _arial32;
            arial14 = _arial16;
            whiteTexture = _whiteTexture;
            Camera.lockOnPlayer = false;
            CameraManager.SwitchCamera((RoomsManager.Rooms)currentRoomNumber);
        }

        void saveMaps()
        {
            for (int room=0; room< (int)RoomsManager.Rooms.total; room++)
            {
                StreamWriter outputStream = new StreamWriter("mapRoom_" + room + ".txt");
                try
                {
                    using (outputStream)
                    {
                        for (int row=0; row< MapsManager.maps[room].roomHeightTiles; row++)
                        {
                            for (int col = 0; col < MapsManager.maps[room].roomWidthTiles; col++)
                            {
                                outputStream.Write((int)MapsManager.maps[room].array[row, col].tileType + ",");
                            }
                            outputStream.WriteLine();
                        }
                    }
                }
                catch (IOException)
                {
                    message = "Save FAILED!\nAsk Ivan.";
                }
            }
        }
        Point TileFromPointerLocation(MouseState mouseState)
        {
            int tileRow = 0;
            int tileCol = 0;
            tileCol = (mouseState.X + Camera.rectangle.X) / Tile.tileSize;
            tileRow = (mouseState.Y + Camera.rectangle.Y) / Tile.tileSize;
            tileCol = MathHelper.Clamp(tileCol, 0, MapsManager.maps[currentRoomNumber].roomWidthTiles - 1);
            tileRow = MathHelper.Clamp(tileRow, 0, MapsManager.maps[currentRoomNumber].roomHeightTiles - 1);
            return new Point(tileCol, tileRow);
        }
        bool getDirectionalInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.Up))
            {
                userInputString = "U" + "," + userInputString[2];
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
            {
                userInputString = "D" + "," + userInputString[2];
                return true;
            }
            if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
            {
                userInputString = userInputString[0] + "," + "L";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.Right))
            {
                userInputString = userInputString[0] + "," + "R";
                return true;
            }
            return false;
        }
        bool GetIntInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad0) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad0))
            {
                userInputInt = userInputInt * 10 + 0;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad1) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad1))
            {
                userInputInt = userInputInt * 10 + 1;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad2) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad2))
            {
                userInputInt = userInputInt * 10 + 2;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad3) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad3))
            {
                userInputInt = userInputInt * 10 + 3;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad4) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad4))
            {
                userInputInt = userInputInt * 10 + 4;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad5) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad5))
            {
                userInputInt = userInputInt * 10 + 5;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad6) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad6))
            {
                userInputInt = userInputInt * 10 + 6;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad7) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad7))
            {
                userInputInt = userInputInt * 10 + 7;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad8) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad8))
            {
                userInputInt = userInputInt * 10 + 8;
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad9) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad9))
            {
                userInputInt = userInputInt * 10 + 9;
                return true;
            }
            return false;
        }
        bool GetStringInput()
        {
            if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad0) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad0))
            {
                userInputString += "0";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad1) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad1))
            {
                userInputString += "1";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad2) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad2))
            {
                userInputString += "2";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad3) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad3))
            {
                userInputString += "3";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad4) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad4))
            {
                userInputString += "4";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad5) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad5))
            {
                userInputString += "5";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad6) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad6))
            {
                userInputString += "6";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad7) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad7))
            {
                userInputString += "7";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad8) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad8))
            {
                userInputString += "8";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad9) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad9))
            {
                userInputString += "9";
                return true;
            }
            else if (Game1.currentKeyboard.IsKeyDown(Keys.OemComma) && !Game1.previousKeyboard.IsKeyDown(Keys.OemComma))
            {
                userInputString += ",";
                return true;
            }
            return false;
        }
        void MenuLoop(MouseState mouseState, MouseState previousMouseState,int tilesPerRow, int infoBoxHeighPx)
        {
            switch (menu)
            {
                case MenuState.start:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                    {
                        menu = MenuState.none;
                        message = "";
                    }
                    break;
                case MenuState.none:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.M))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    break;
                case MenuState.main:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.R))
                    {
                        menu = MenuState.rooms;
                        message = "Press:\nA-Change room\nB-Change room size";

                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.T))
                    {
                        menu = MenuState.tiles;
                        message = "Press:\nA-Change tile type\nB-Random tile mode";
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.V))
                    {
                        menu = MenuState.view;
                        message = "Press:\nA-Highlight solid tiles\nB-Highlight deadly tiles";
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.S))
                    {
                        menu = MenuState.none;
                        message = "";
                        saveMaps();
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.none;
                        message = "";
                    }
                    break;
                case MenuState.rooms:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                    {
                        menu = MenuState.pickRoom;
                        message = "Room index: ";
                        userInputInt = 0;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.B))
                    {
                        menu = MenuState.pickRoomSize;
                        message = "Room size in tiles (format: width,height): ";
                        userInputString = "";
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    break;
                case MenuState.tiles:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                    {
                        menu = MenuState.pickTileIndex;
                        message = "Tile index: ";
                        userInputInt = 0;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.B))
                    {
                        menu = MenuState.selectRandomTiles;
                        message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\n" +
                            "You can also click the tiles displayed on the right.\nIndexes: ";
                        userInputString = "";
                        randomTiles.Clear();
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    break;
                case MenuState.view:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.A))
                    {
                        menu = MenuState.none;
                        message = "";
                        solidView = !solidView;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.B))
                    {
                        menu = MenuState.none;
                        message = "";
                        deadlyView=!deadlyView;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    break;
                case MenuState.pickRoom:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    else if (GetIntInput())
                        message = "Room index: " + userInputInt;
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                    {
                        currentRoomNumber = userInputInt < (int)RoomsManager.Rooms.total ? userInputInt : ((int)RoomsManager.Rooms.total - 1);
                        CameraManager.SwitchCamera((RoomsManager.Rooms)currentRoomNumber);
                        userInputInt = 0;
                        menu = MenuState.none;
                        message = "";
                    }
                    break;
                case MenuState.pickRoomSize:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    else if (GetStringInput())
                        message = "Room size in tiles (format: width,height): " + userInputString;
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                    {
                        string[] arr = userInputString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        if (arr.Length != 2)
                            break;
                        widthTiles =int.Parse(arr[0]);
                        widthTiles = MathHelper.Clamp(widthTiles,(Game1.screenWidth + Tile.tileSize-1)/ Tile.tileSize, 200);
                        heightTiles = int.Parse(arr[1]);
                        heightTiles = MathHelper.Clamp(heightTiles, (Game1.screenHeight + Tile.tileSize - 1) / Tile.tileSize, 200);
                        menu = MenuState.pickResizingDirection;
                        userInputString = "U,R";
                        message = "Use the arrows to select the edges (top/bottom and left/right)\n" +
                            "to shift to resize the room.\n"+userInputString;
                    }
                    break;
                case MenuState.pickResizingDirection:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                        userInputString = "";
                        heightTiles = 0;
                        widthTiles = 0;
                    }
                    else if (getDirectionalInput())
                    {
                        message = "Use the arrows to select the edges (top/bottom and left/right)\n" +
                            "to shift to resize the room.\n" + userInputString;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter))
                    {
                        RoomMap newMap = new RoomMap(heightTiles, widthTiles);
                        int oldHeightTiles = MapsManager.maps[currentRoomNumber].roomHeightTiles;
                        int oldWidthTiles= MapsManager.maps[currentRoomNumber].roomWidthTiles;
                        for (int row = 0; row < Math.Min(oldHeightTiles, heightTiles); row++)
                        {
                            for (int col = 0; col < Math.Min(oldWidthTiles, widthTiles); col++)
                            {
                                if (userInputString == "D,R")
                                    newMap.array[row, col].tileType = MapsManager.maps[currentRoomNumber].array[row, col].tileType;
                                else if (userInputString == "U,R")
                                    newMap.array[heightTiles - 1 - row, col].tileType = MapsManager.maps[currentRoomNumber].array[oldHeightTiles - 1 - row, col].tileType;
                                else if (userInputString == "D,L")
                                    newMap.array[row, widthTiles - 1 - col].tileType = MapsManager.maps[currentRoomNumber].array[row, oldWidthTiles - 1 - col].tileType;
                                else if (userInputString == "U,L")
                                    newMap.array[heightTiles - 1 - row, widthTiles - 1 - col].tileType = MapsManager.maps[currentRoomNumber].array[oldHeightTiles - 1 - row, oldWidthTiles - 1 - col].tileType;
                            }
                        }
                        MapsManager.maps[currentRoomNumber] = newMap;
                        CameraManager.SwitchCamera((RoomsManager.Rooms)currentRoomNumber);
                        menu = MenuState.none;
                        message = "";
                        userInputString = "";
                        heightTiles = 0;
                        widthTiles = 0;
                    }
                    break;
                case MenuState.pickTileIndex:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    else if (GetIntInput())
                        message = "Tile index: " + userInputInt;
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                    {
                        currentTileType = userInputInt < (int)Tile.TileType.total ? userInputInt : ((int)Tile.TileType.total - 1);
                        userInputInt = 0;
                        menu = MenuState.none;
                        message = "";
                        randomMode = false;
                    }
                    break;
                case MenuState.selectRandomTiles:
                    if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
                    {
                        menu = MenuState.main;
                        message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
                    }
                    else if (GetStringInput())
                        message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\n" +
                            "You can also click the tiles displayed on the right.\nIndexes: "+userInputString;
                    else if (mouseState.X >= Game1.screenWidth && mouseState.X < Game1.screenWidth + Tile.tileSize * tilesPerRow &&
                        mouseState.Y >= 0 && mouseState.Y < Game1.screenHeight &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        previousMouseState.LeftButton!= ButtonState.Pressed)
                    {
                        int newTileType = (mouseState.X - Game1.screenWidth) / Tile.tileSize + (mouseState.Y / Tile.tileSize * tilesPerRow);
                        userInputString += (","+newTileType + ",");
                        message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\n" +
                            "You can also click the tiles displayed on the right.\nIndexes: " + userInputString;
                    }
                    else if (Game1.currentKeyboard.IsKeyDown(Keys.Enter))
                    {
                        randomTiles.Clear();
                        string[] arr = userInputString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        userInputString = "";
                        if (arr.Length == 0)
                            break;
                        else if (arr.Length==1)
                        {
                            menu = MenuState.none;
                            message = "";
                            randomMode = false;
                            int tile = int.Parse(arr[0]);
                            currentTileType= tile < (int)Tile.TileType.total ? tile : ((int)Tile.TileType.total - 1);
                            break;
                        }
                        foreach (string s in arr)
                        {
                            int tile = int.Parse(s);
                            tile = tile < (int)Tile.TileType.total ? tile : ((int)Tile.TileType.total - 1);
                            randomTiles.Add(tile);
                        }
                        randomMode = true;
                        menu = MenuState.none;
                        message = "";
                    }
                    break;
                default:
                    break;
            }
        }
        public void Update(MouseState mouseState, MouseState previousMouseState, int tilesPerRow, int infoBoxHeighPx)
        {
            MenuLoop(mouseState, previousMouseState, tilesPerRow, infoBoxHeighPx);
            if (menu== MenuState.none)
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Up))
                    Camera.pointLocked.Y -= 10;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
                    Camera.pointLocked.Y += 10;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Right))
                    Camera.pointLocked.X += 10;
                if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
                    Camera.pointLocked.X -= 10;
                if (mouseState.X >= 0 && mouseState.X < Game1.screenWidth &&
                    mouseState.Y >= 0 && mouseState.Y < Game1.screenHeight)
                {
                    Point tile = TileFromPointerLocation(mouseState);
                    hoveredTileType = (int)MapsManager.maps[currentRoomNumber].array[tile.Y, tile.X].tileType;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (randomMode)
                        {
                            Point previousTile = TileFromPointerLocation(previousMouseState);
                            if (previousTile != tile || previousMouseState.LeftButton != ButtonState.Pressed)
                            {
                                int index = rand.Next(randomTiles.Count);
                                MapsManager.maps[currentRoomNumber].array[tile.Y, tile.X].tileType = (Tile.TileType)randomTiles[index];
                            }
                        }
                        else
                            MapsManager.maps[currentRoomNumber].array[tile.Y, tile.X].tileType = (Tile.TileType)currentTileType;
                    }
                    else if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        MapsManager.maps[currentRoomNumber].array[tile.Y, tile.X].tileType = (Tile.TileType)0;
                    }
                    tilePositionInfo = "Tile hovered: row " + tile.Y + ", col " + tile.X + "\n Type: " + hoveredTileType + "(" + (Tile.TileType)hoveredTileType + ")";
                }
                else if (mouseState.X >= Game1.screenWidth && mouseState.X < Game1.screenWidth+Tile.tileSize*tilesPerRow)
                {
                    if(mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton!=ButtonState.Pressed)
                    {
                        int newTileType = (mouseState.X - Game1.screenWidth) / Tile.tileSize + (mouseState.Y / Tile.tileSize * tilesPerRow);
                        if (newTileType< (int)Tile.TileType.total)
                        {
                            randomMode = false;
                            currentTileType = newTileType;
                        }
                    }
                }
                roomInfo = "Current room: " + currentRoomNumber + "(" + (RoomsManager.Rooms)currentRoomNumber + ")";
                if (randomMode)
                    tileTypeInfo = "Tile selected: random";
                else
                    tileTypeInfo = "Tile selected: " + currentTileType + "("+(Tile.TileType)currentTileType+")";
            }
        }
        public void Draw(SpriteBatch spriteBatch, int tilesPerRow, int infoBoxHeighPx)
        {
            if (menu== MenuState.start)
            {
                spriteBatch.DrawString(arial32,
                        message,
                        new Vector2((Game1.screenWidth+tilesPerRow*Tile.tileSize) / 2, Game1.screenHeight / 2) - arial32.MeasureString(message) / 2,
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
            spriteBatch.Draw(whiteTexture, new Rectangle(0, Game1.screenHeight, Game1.screenWidth,  infoBoxHeighPx), Color.LightGray);
            spriteBatch.DrawString(arial14,
                                    tileTypeInfo,
                                    new Vector2(Game1.screenWidth / 2, Game1.screenHeight + 20) - arial14.MeasureString(tileTypeInfo) / 2,
                                    Color.Black);
            spriteBatch.DrawString(arial14,
                                    tilePositionInfo,
                                    new Vector2(Game1.screenWidth -10- arial14.MeasureString(tilePositionInfo).X,
                                                Game1.screenHeight + 20 - arial14.MeasureString(tilePositionInfo).Y / 2),
                                    Color.Black);
            spriteBatch.DrawString(arial14,
                                    roomInfo,
                                    new Vector2(10, Game1.screenHeight + 20 - arial14.MeasureString(roomInfo).Y / 2),
                                    Color.Black);
            if (menu!= MenuState.start && menu!=MenuState.none)
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
            spriteBatch.Draw(whiteTexture, new Rectangle(Game1.screenWidth, 0, Tile.tileSize * tilesPerRow, Game1.screenHeight+ infoBoxHeighPx), Color.LightGray);
            for (int i=0; i<(int)Tile.TileType.total; i++)
            {
                Tile.DrawAtLocation(spriteBatch, i,new Vector2(i % tilesPerRow, i / tilesPerRow) * Tile.tileSize+new Vector2(Game1.screenWidth,0));
            }
        }
    }
}
