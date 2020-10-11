using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _999AD
{
    static class LoadSaveManager
    {
        static string folderPath;
        static string filePathGame;
        static string filePathScores;
        static public void Inizialize()
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+ "\\999ad";
            filePathGame = folderPath + "\\game.fun";
            filePathScores = folderPath + "\\scores.fun";
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if(!folder.Exists)
            {
                folder.Create();
            }
        }
        static public void SaveGameProgress()
        {
            List<int> collectables = new List<int>();
            foreach(CollectablesRoomManager r in CollectablesManager.collectablesRoomManagers)
            {
                foreach (Collectable c in r.collectables)
                {
                    if (c.ID != -1)
                        collectables.Add(c.ID);
                }
            }
            List<int> inventory = new List<int>();
            foreach (Collectable c in CollectablesManager.collectedItems)
            {
                inventory.Add((int)c.type);
            }
            List<int> doors = new List<int>();
            foreach (DoorsRoomManager r in DoorsManager.doorsRoomManagers)
            {
                foreach (Door d in r.doors)
                {
                    doors.Add(d.ID);
                }
            }
            GameSaveData saveData = new GameSaveData(GameStats.gameTime, GameStats.deathsCount, GameStats.hitsCount,
                (int)RoomsManager.CurrentRoom, (int)RoomsManager.PreviousRoom, Player.healthPoints, new float[2] { Player.position.X, Player.position.Y },
                collectables, inventory, doors, MidBoss.Dead, FinalBoss.Dead, GameEvents.eventAlreadyHappened);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = new FileStream(filePathGame, FileMode.Create);
            binaryFormatter.Serialize(file, saveData);
            file.Close();
        }
        static public void SaveHighScores(AchievementsSaveData achievements)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = new FileStream(filePathScores, FileMode.Create);
            binaryFormatter.Serialize(file, achievements);
            file.Close();
            Achievements.UpdateAchievements(
                achievements.gameCompleted,
                achievements.noDeath,
                achievements.noHits,
                achievements.bestTime);
        }
        static public void DeleteSaveFile()
        {
            if(File.Exists(filePathGame))
            {
                File.Delete(filePathGame);
            }
        }

        public static bool LoadGame()
        {
            if (File.Exists(filePathGame))
            {
                FileStream file = new FileStream(filePathGame, FileMode.Open);
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    GameSaveData saveData = binaryFormatter.Deserialize(file) as GameSaveData;
                    saveData.ApplySaveData();
                    file.Close();
                    return true;
                }
                catch(Exception e)
                {
                    file.Dispose();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static void LoadHighScores()
        {
            if (File.Exists(filePathScores))
            {
                FileStream file = new FileStream(filePathScores, FileMode.Open);
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    AchievementsSaveData saveData = binaryFormatter.Deserialize(file) as AchievementsSaveData;
                    Achievements.UpdateAchievements(
                        saveData.gameCompleted,
                        saveData.noDeath,
                        saveData.noHits,
                        saveData.bestTime);
                    file.Close();
                }
                catch(Exception e)
                {
                    Achievements.UpdateAchievements(
                        false,
                        false,
                        false,
                        0);
                }
            }
            else
            {
                Achievements.UpdateAchievements(
                    false,
                    false,
                    false,
                    0);
            }
        }
    }
}
