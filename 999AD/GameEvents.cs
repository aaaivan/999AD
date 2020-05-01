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
            escapeFinalBossRoom,
            lavaEruption1_escape0,
            moveFloor1_escape0,
            moveFloor2_escape0,
            lavaEruptionX,
            total
        }
        public static readonly float[] eventsDuration = new float[(int)Events.total]
        {
            0,
            2, 4, 0, 0,
            0, 3.5f, 0,0
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
            if (happening == Events.none)
            {
                eventHandler(elapsedTime);
                return;
            }
            elapsedEventsDuration += elapsedTime;
            if (elapsedEventsDuration >= eventsDuration[(int)happening])
            {
                eventAlreadyHappened[(int)happening] = true;
                elapsedEventsDuration = 0;
                happening = Events.none;
            }

        }
        //this function trigger events when a centain set of conditions is true
        static void eventHandler(float elapsedTime)
        {
            switch (RoomsManager.CurrentRoom)
            {
                case RoomsManager.Rooms.finalBoss:
                    if (Player.position.X <= 200 && !eventAlreadyHappened[(int)Events.terrainCollapseFinalBoss])
                        TriggerEvent(Events.terrainCollapseFinalBoss);
                    if (eventAlreadyHappened[(int)Events.terrainCollapseFinalBoss]&& !eventAlreadyHappened[(int)Events.finalBossComesAlive])
                        TriggerEvent(Events.finalBossComesAlive);
                    if (eventAlreadyHappened[(int)Events.finalBossComesAlive] && !eventAlreadyHappened[(int)Events.activatePlatformsFinalBoss])
                        TriggerEvent(Events.activatePlatformsFinalBoss);
                    if (FinalBoss.Dead && !eventAlreadyHappened[(int)Events.escapeFinalBossRoom])
                        TriggerEvent(Events.escapeFinalBossRoom);
                    break;
                case RoomsManager.Rooms.escape0:
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 248 && !eventAlreadyHappened[(int)Events.lavaEruption1_escape0])
                        TriggerEvent(Events.lavaEruption1_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 416 && !eventAlreadyHappened[(int)Events.moveFloor1_escape0])
                        TriggerEvent(Events.moveFloor1_escape0);
                    if (eventAlreadyHappened[(int)Events.moveFloor1_escape0] && !eventAlreadyHappened[(int)Events.moveFloor2_escape0])
                        TriggerEvent(Events.moveFloor2_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1352 && !eventAlreadyHappened[(int)Events.lavaEruptionX])
                        TriggerEvent(Events.lavaEruptionX);
                    break;
            }
        }
        public static void TriggerEvent(Events _event)
        {
            if (RoomsManager.CurrentRoom == RoomsManager.Rooms.finalBoss)
            {
                switch (_event)
                {
                    case Events.terrainCollapseFinalBoss:
                        List<int[]> tilesToRemove = new List<int[]>();
                        for (int col = 1; col < (MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles + 1) / 2; col++)
                        {
                            tilesToRemove.Add(new int[2] { 57, col });
                            tilesToRemove.Add(new int[2] { 57, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - col - 1 });
                            tilesToRemove.Add(new int[2] { 56, col });
                            tilesToRemove.Add(new int[2] { 56, MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles - col - 1 });
                        }
                        CameraManager.shakeForTime(eventsDuration[(int)Events.terrainCollapseFinalBoss]);
                        MapsManager.maps[(int)RoomsManager.CurrentRoom].RemoveGroupOfTiles(tilesToRemove, 0.03f, 3);
                        happening = Events.terrainCollapseFinalBoss;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.finalBossComesAlive:
                        CameraManager.shakeForTime(eventsDuration[(int)Events.finalBossComesAlive]);
                        FinalBoss.WakeUp();
                        happening = Events.finalBossComesAlive;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.activatePlatformsFinalBoss:
                        for (int i = 0; i < 6; i++)
                            PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[i].active = true;
                        happening = Events.activatePlatformsFinalBoss;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.escapeFinalBossRoom:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[6].active = true;
                        LavaGeyserManager.SweepAcross(2, 0.7f, 1, 0, 24, false);
                        happening = Events.escapeFinalBossRoom;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.escape0)
            {
                switch (_event)
                {
                    case Events.lavaEruption1_escape0:
                        LavaGeyserManager.ShootGeyser(new float[] { MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx -308 }, 0);
                        happening = Events.lavaEruption1_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.moveFloor1_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[0].active = true;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[1].active = true;
                        happening = Events.moveFloor1_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.moveFloor2_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[2].active = true;
                        happening = Events.moveFloor2_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruptionX:
                        LavaGeyserManager.ShootGeyser(new float[] { MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1388 }, 1.2f);
                        happening = Events.lavaEruptionX;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
        }
        
    }
}
