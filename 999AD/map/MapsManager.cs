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
                    StreamReader inputStream = new StreamReader("mapRoom_" + room + ".txt");
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
                    maps[room] = new RoomMap((Game1.screenHeight+Tile.tileSize-1)/Tile.tileSize, (Game1.screenWidth + Tile.tileSize - 1) / Tile.tileSize) ;
                }
                catch(System.ArgumentOutOfRangeException)
                {
                    maps[room] = new RoomMap((Game1.screenHeight + Tile.tileSize - 1) / Tile.tileSize, (Game1.screenWidth + Tile.tileSize - 1) / Tile.tileSize);
                }
            }
        }
        #endregion
    }
}
