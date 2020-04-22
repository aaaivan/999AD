using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    static class GameEvents
    {
        public enum Events
        {
            terrainCollapseFinalBoss, activatePlatformsFinalBoss, total
        }
        public static bool[] eventAlreadyHappened = new bool[(int)Events.total];
        public static void Inizialize()
        {
            for (int i = 0; i < eventAlreadyHappened.Length; i++)
                eventAlreadyHappened[i] = false;
        }
        public static void playEvent(Events _event)
        {
            switch (_event)
            {
                case Events.terrainCollapseFinalBoss:
                    if (eventAlreadyHappened[(int)Events.terrainCollapseFinalBoss])
                        break;
                    else
                    {
                        List<int[]> tilesToRemove = new List<int[]>();
                        for (int col = 0; col < (MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles + 1) / 2; col++)
                        {
                            tilesToRemove.Add(new int[2] { 25, col });
                            tilesToRemove.Add(new int[2] { 25, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - col - 1 });
                        }
                        CameraManager.shakeForTime(MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles * 0.1f);
                        MapsManager.RemoveRowOfTiles(tilesToRemove, 0.1f);
                        eventAlreadyHappened[(int)Events.terrainCollapseFinalBoss] = true;
                    }
                    break;
                case Events.activatePlatformsFinalBoss:
                    if (eventAlreadyHappened[(int)Events.activatePlatformsFinalBoss])
                        break;
                    else
                    {
                        foreach (MovingPlatform p in PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms)
                            p.active = true;
                        eventAlreadyHappened[(int)Events.activatePlatformsFinalBoss] = true;
                    }
                    break;
            }
        }
        
    }
}
