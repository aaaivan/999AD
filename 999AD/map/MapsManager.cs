using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class MapsManager
    {
        #region DECLARATIONS
        public static RoomMap[] maps = new RoomMap[(int)RoomsManager.Rooms.total];

        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D _spritesheet)
        {
            Tile.LoadTextures( _spritesheet);
            loadMaps();
        }
        #endregion
        #region METHODS
        //fill the list "maps" with the maps read from the corresponding text files
        static void loadMaps()
        {
            for (int room=0; room< (int)RoomsManager.Rooms.total; room++)
            {
                string line = "";
                List<List<int>> tileMap = new List<List<int>>();
                string[] row;
                try
                {
                    StreamReader inputStream = new StreamReader("mapFiles\\mapRoom_" + (RoomsManager.Rooms)room + ".txt");
                    using (inputStream)
                    {
                        line = inputStream.ReadLine();
                        int count = 0;
                        while (line != null)
                        {
                            tileMap.Add(new List<int>());
                            row = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string tile in row)
                            {
                                tileMap[count].Add(int.Parse(tile));
                            }
                            line = inputStream.ReadLine();
                            count++;
                        }
                        maps[room] = new RoomMap(tileMap.Count, tileMap[0].Count);
                        for (int i = 0; i < tileMap.Count; i++)
                        {
                            for (int j = 0; j < tileMap[0].Count; j++)
                            {
                                if (tileMap[i][j]!=0)
                                    maps[room].array[i, j].tileType = (Tile.TileType)tileMap[i][j];
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    maps[room] = new RoomMap(
                        (Game1.gameHeight + 2 * CameraManager.maxOffsetY + (int)(Tile.tileSize ) - 1) / (int)(Tile.tileSize ),
                        (Game1.gameWidth + (int)(Tile.tileSize) - 1) / (int)(Tile.tileSize ));
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    maps[room] = new RoomMap(
                        (Game1.gameHeight + 2* CameraManager.maxOffsetY + (int)(Tile.tileSize) - 1) / (int)(Tile.tileSize),
                        (Game1.gameWidth + (int)(Tile.tileSize ) - 1) / (int)(Tile.tileSize ));
                }
            }
        }
        public static void resetMap(RoomsManager.Rooms roomName)
        {
            string line = "";
            List<List<int>> tileMap = new List<List<int>>();
            string[] row;
            try
            {
                StreamReader inputStream = new StreamReader("mapRoom_" + roomName + ".txt");
                using (inputStream)
                {
                    line = inputStream.ReadLine();
                    int count = 0;
                    while (line != null)
                    {
                        tileMap.Add(new List<int>());
                        row = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string tile in row)
                        {
                            tileMap[count].Add(int.Parse(tile));
                        }
                        line = inputStream.ReadLine();
                        count++;
                    }
                    maps[(int)roomName] = new RoomMap(tileMap.Count, tileMap[0].Count);
                    for (int i = 0; i < tileMap.Count; i++)
                    {
                        for (int j = 0; j < tileMap[0].Count; j++)
                        {
                            if (tileMap[i][j] != 0)
                                maps[(int)roomName].array[i, j].tileType = (Tile.TileType)tileMap[i][j];
                        }
                    }
                }
            }
            catch (IOException)
            {
                return;          
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }
        #endregion
    }
}
