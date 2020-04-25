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
            none,
            terrainCollapseFinalBoss,
            finalBossComesAlive,
            activatePlatformsFinalBoss,
            total
        }
        public static float[] eventsDuration = new float[(int)Events.total]
        {
            0,
            2,
            FinalBoss.bossAnimationDuration[(int)FinalBoss.BossAnimations.stoneToIdle],
            0
        };
        public static float elapsedEventsDuration = 0;
        public static bool[] eventAlreadyHappened = new bool[(int)Events.total];
        public static Events happening= Events.none;
        public static void Inizialize()
        {
            for (int i = 0; i < (int)Events.total; i++)
                eventAlreadyHappened[i] = false;
        }
        public static void Update(float elapsedTime)
        {
            if (happening== Events.none)
                return;
            elapsedEventsDuration += elapsedTime;
            if (elapsedEventsDuration >= eventsDuration[(int)happening])
            {
                eventAlreadyHappened[(int)happening] = true;
                elapsedEventsDuration = 0;
                happening = Events.none;
            }

        }
        public static void TriggerEvent(Events _event)
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
                        CameraManager.shakeForTime(eventsDuration[(int)Events.terrainCollapseFinalBoss]);
                        MapsManager.RemoveRowOfTiles(tilesToRemove, eventsDuration[(int)Events.terrainCollapseFinalBoss]/MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles);
                        happening = Events.terrainCollapseFinalBoss;
                        elapsedEventsDuration = 0;
                    }
                    break;
                case Events.finalBossComesAlive:
                    if (eventAlreadyHappened[(int)Events.finalBossComesAlive])
                        break;
                    CameraManager.shakeForTime(eventsDuration[(int)Events.finalBossComesAlive]);
                    FinalBoss.bossAnimation = FinalBoss.BossAnimations.stoneToIdle;
                    happening = Events.finalBossComesAlive;
                    elapsedEventsDuration = 0;
                    break;
                case Events.activatePlatformsFinalBoss:
                    if (eventAlreadyHappened[(int)Events.activatePlatformsFinalBoss])
                        break;
                    foreach (MovingPlatform p in PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms)
                        p.active = true;
                    happening= Events.activatePlatformsFinalBoss;
                    elapsedEventsDuration = 0;
                    break;
            }
        }
        
    }
}
