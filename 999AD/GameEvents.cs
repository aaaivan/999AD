﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _999AD
{
    static class GameEvents
    {
        public enum Events
        {
            none,

            introMonologue,

            keySpawns1_tutorial3, key1Collected_tutorial3, keySpawns2_tutorial3, key2Collected_tutorial3,

            unlockDoubleJump, showDoubleJumpScreen, unlockWallJump, showWallJumpScreen,

            keyAndPowerUpSpawn_midBoss, pUpCollected_midBoss,

            activateMovingPlatforms_churchGroundFloor0, resetMovingPlatforms_churchGroundFloor0,

            showInvisibleTiles_church1stFloor0, hide_InvisibleTiles_church1stFloor0,

            spawnKey_altar, keyCollected_altar,

            terrainCollapseFinalBoss, finalBossComesAlive, activatePlatformsFinalBoss, escapeFinalBossRoom,

            monologue_escape0,
            lavaEruption1_escape0, lowerFloor1_escape0, removeFloor1_escape0,
            lavaEruption2_escape0, lavaEruption3_escape0, lavaEruption4_escape0,
            raiseFloor2_escape0, lavaEruption5_escape0, raiseFloor3_escape0, lavaEruption6_escape0,

            activatePlatform_escape1, raiseFloor1_escape1, lavaEruption1_escape1,

            lavaEruption_escape2,

            finalMonologue, finalCutscene,
            total
        }
        public static readonly float[] eventsDuration = new float[(int)Events.total] //duration of each event in seconds
        {
            0,//none

            0,

            0,0,0,0,

            0.5f,0,0.5f,0,

            0,0,

            0,0,

            0,0,

            0,0,

            2, 4, 0, 0,

            0,
            0, 3.1f, 0,
            2.5f, 1f, 0,
            0, 0, 4.2f, 0,

            0,0,0,
            2.2f,
            0,0
        };
        public static float elapsedEventsDuration;
        public static bool[] eventAlreadyHappened; //indicates whther the n-th event has already happened (true) or not (false)
        public static Events happening;
        public static void Inizialize()
        {
            elapsedEventsDuration = 0;
            eventAlreadyHappened = new bool[(int)Events.total];
            for (int i = 0; i < (int)Events.total; i++)
                eventAlreadyHappened[i] = false;
            happening = Events.none;
        }

        //reset some events upon player death
        public static void Reset()
        {
            elapsedEventsDuration = 0;
            happening = Events.none;
            if (eventAlreadyHappened[(int)Events.keySpawns1_tutorial3] && !eventAlreadyHappened[(int)Events.key1Collected_tutorial3])
            {
                CollectablesManager.collectablesRoomManagers[(int)RoomsManager.Rooms.tutorial3].RemoveCollectablesFromMap();
                eventAlreadyHappened[(int)Events.keySpawns1_tutorial3] = false;
            }
            else if (eventAlreadyHappened[(int)Events.keySpawns2_tutorial3] && !eventAlreadyHappened[(int)Events.key2Collected_tutorial3])
            {
                CollectablesManager.collectablesRoomManagers[(int)RoomsManager.Rooms.tutorial3].RemoveCollectablesFromMap();
                eventAlreadyHappened[(int)Events.keySpawns2_tutorial3] = false;
            }
            else if (eventAlreadyHappened[(int)Events.keyAndPowerUpSpawn_midBoss] &&
                !eventAlreadyHappened[(int)Events.pUpCollected_midBoss])
            {
                CollectablesManager.collectablesRoomManagers[(int)RoomsManager.Rooms.midBoss].AddCollectableToMap(
                    new Collectable(new Point(280, 140), Collectable.ItemType.wallJump_powerup));
            }
            else if (eventAlreadyHappened[(int)Events.spawnKey_altar] &&
                !eventAlreadyHappened[(int)Events.keyCollected_altar])
            {
                CollectablesManager.collectablesRoomManagers[(int)RoomsManager.Rooms.churchAltarRoom].AddCollectableToMap(
                    new Collectable(new Point(230, 400), Collectable.ItemType.silverKey));
            }
            else if(FinalBoss.Dead)
            {
                List<int[]> tilesToRemove = new List<int[]>();
                for (int col = 1; col < (MapsManager.maps[(int)RoomsManager.Rooms.finalBoss].roomWidthTiles + 1) / 2; col++)
                {
                    tilesToRemove.Add(new int[2] { 57, col });
                    tilesToRemove.Add(new int[2] { 57, MapsManager.maps[(int)RoomsManager.Rooms.finalBoss].roomWidthTiles - col - 1 });
                    tilesToRemove.Add(new int[2] { 56, col });
                    tilesToRemove.Add(new int[2] { 56, MapsManager.maps[(int)RoomsManager.Rooms.finalBoss].roomWidthTiles - col - 1 });
                }
                MapsManager.maps[(int)RoomsManager.Rooms.finalBoss].RemoveGroupOfTiles(tilesToRemove, 0, 100);
            }
            switch (RoomsManager.CurrentRoom)
            {
                case RoomsManager.Rooms.finalBoss:
                    if (!FinalBoss.Dead)
                    {
                        eventAlreadyHappened[(int)Events.terrainCollapseFinalBoss] = false;
                        eventAlreadyHappened[(int)Events.finalBossComesAlive] = false;
                        eventAlreadyHappened[(int)Events.activatePlatformsFinalBoss] = false;
                        eventAlreadyHappened[(int)Events.escapeFinalBossRoom] = false;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].Reset();
                        MapsManager.resetMap(RoomsManager.CurrentRoom);
                        FinalBoss.Reset();
                    }
                    break;
                case RoomsManager.Rooms.escape0:
                    if (RoomsManager.PreviousRoom== RoomsManager.Rooms.finalBoss)
                    {
                        eventAlreadyHappened[(int)Events.lavaEruption1_escape0] = false;
                        eventAlreadyHappened[(int)Events.lowerFloor1_escape0] = false;
                        eventAlreadyHappened[(int)Events.removeFloor1_escape0] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption2_escape0] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption3_escape0] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption4_escape0] = false;
                        eventAlreadyHappened[(int)Events.raiseFloor2_escape0] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption5_escape0] = false;
                        eventAlreadyHappened[(int)Events.raiseFloor3_escape0] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption6_escape0] = false;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].Reset();
                        MapsManager.resetMap(RoomsManager.CurrentRoom);
                    }
                    break;
                case RoomsManager.Rooms.escape1:
                    if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape0)
                    {
                        eventAlreadyHappened[(int)Events.activatePlatform_escape1] = false;
                        eventAlreadyHappened[(int)Events.raiseFloor1_escape1] = false;
                        eventAlreadyHappened[(int)Events.lavaEruption1_escape1] = false;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].Reset();
                    }
                    break;
            }
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
            {//when an event happens, start cowntdown based on its duration
             //at the end of the countdown set th eevent that is happening to none
                eventAlreadyHappened[(int)happening] = true;
                elapsedEventsDuration = 0;
                happening = Events.none;
            }
        }

        //this function check some condition to determine whether an event should be triggered
        static void eventHandler(float elapsedTime)
        {
            switch (RoomsManager.CurrentRoom)
            {
                case RoomsManager.Rooms.tutorial0:
                    if (!eventAlreadyHappened[(int)Events.introMonologue])
                        TriggerEvent(Events.introMonologue);
                    else if (FinalBoss.Dead && Player.position.X < 200 && Player.position.Y <= 176 - Player.height &&
                        !eventAlreadyHappened[(int)Events.finalMonologue])
                        TriggerEvent(Events.finalMonologue);
                    else if (eventAlreadyHappened[(int)Events.finalMonologue] && !MonologuesManager.MonologuePlaying)
                        TriggerEvent(Events.finalCutscene);
                    break;
                case RoomsManager.Rooms.tutorial3:
                    if (EnemyManager.enemyRoomManagers[(int)RoomsManager.CurrentRoom].enemiesType1[0].Dead &&
                        !eventAlreadyHappened[(int)Events.keySpawns1_tutorial3])
                        TriggerEvent(Events.keySpawns1_tutorial3);
                    else if (eventAlreadyHappened[(int)Events.keySpawns1_tutorial3] && !eventAlreadyHappened[(int)Events.key1Collected_tutorial3] &&
                        CollectablesManager.collectedItems.Count==1)
                        TriggerEvent(Events.key1Collected_tutorial3);
                    else if (EnemyManager.enemyRoomManagers[(int)RoomsManager.CurrentRoom].enemiesType1[1].Dead &&
                        !eventAlreadyHappened[(int)Events.keySpawns2_tutorial3])
                        TriggerEvent(Events.keySpawns2_tutorial3);
                    else if (eventAlreadyHappened[(int)Events.keySpawns2_tutorial3] && !eventAlreadyHappened[(int)Events.key2Collected_tutorial3] &&
                        CollectablesManager.collectedItems.Count == 1)
                        TriggerEvent(Events.key2Collected_tutorial3);
                    break;
                case RoomsManager.Rooms.churchBellTower0:
                    if (Player.CollisionRectangle.Intersects(new Rectangle(196, 908, 96, 76)) &&
                        CollectablesManager.TryRemoveFromInventory(Collectable.ItemType.doubleJump_powerup, new Vector2(223, 938)) &&
                        !eventAlreadyHappened[(int)Events.unlockDoubleJump])
                        TriggerEvent(Events.unlockDoubleJump);
                    else if (eventAlreadyHappened[(int)Events.unlockDoubleJump] && !eventAlreadyHappened[(int)Events.showDoubleJumpScreen])
                        TriggerEvent(Events.showDoubleJumpScreen);
                    else if (Player.CollisionRectangle.Intersects(new Rectangle(196, 908, 96, 76)) &&
                        CollectablesManager.TryRemoveFromInventory(Collectable.ItemType.wallJump_powerup, new Vector2(251, 938)) &&
                        !eventAlreadyHappened[(int)Events.unlockWallJump])
                        TriggerEvent(Events.unlockWallJump);
                    else if (eventAlreadyHappened[(int)Events.unlockWallJump] && !eventAlreadyHappened[(int)Events.showWallJumpScreen])
                        TriggerEvent(Events.showWallJumpScreen);
                    break;
                case RoomsManager.Rooms.midBoss:
                    if (MidBoss.Dead && !eventAlreadyHappened[(int)Events.keyAndPowerUpSpawn_midBoss])
                        TriggerEvent(Events.keyAndPowerUpSpawn_midBoss);
                    else if (eventAlreadyHappened[(int)Events.keyAndPowerUpSpawn_midBoss] && CollectablesManager.collectedItems.Count > 0)
                    {
                        if (!eventAlreadyHappened[(int)Events.pUpCollected_midBoss])
                        {
                            foreach (Collectable collectable in CollectablesManager.collectedItems)
                            {
                                if (collectable.type == Collectable.ItemType.wallJump_powerup)
                                { 
                                    TriggerEvent(Events.pUpCollected_midBoss);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case RoomsManager.Rooms.churchGroundFloor0:
                    if (Player.position.X >= 88 - Player.width && Player.position.Y < 178)
                    {
                        if (Player.IsOnMovingPlatform && !eventAlreadyHappened[(int)Events.activateMovingPlatforms_churchGroundFloor0])
                            TriggerEvent(Events.activateMovingPlatforms_churchGroundFloor0);
                    }
                    else if (!eventAlreadyHappened[(int)Events.resetMovingPlatforms_churchGroundFloor0])
                        TriggerEvent(Events.resetMovingPlatforms_churchGroundFloor0);
                    break;
                case RoomsManager.Rooms.church1stFloor0:
                    if ((Player.position.X < 1032 && Player.position.X >= 1000) ||
                        (Player.position.X < 684 && Player.position.X >= 664) ||
                        (Player.position.X < 364 && Player.position.X >= 344 && Player.position.Y>106))
                    {
                        if (!eventAlreadyHappened[(int)Events.showInvisibleTiles_church1stFloor0])
                        {
                            TriggerEvent(Events.showInvisibleTiles_church1stFloor0);
                        }
                    }
                    else if (!eventAlreadyHappened[(int)Events.hide_InvisibleTiles_church1stFloor0])
                        TriggerEvent(Events.hide_InvisibleTiles_church1stFloor0);
                    break;
                case RoomsManager.Rooms.churchAltarRoom:
                    if (TorchManager.AllTorchesUnlit() && !eventAlreadyHappened[(int)Events.spawnKey_altar])
                        TriggerEvent(Events.spawnKey_altar);
                    else if (eventAlreadyHappened[(int)Events.spawnKey_altar] && 
                        !eventAlreadyHappened[(int)Events.keyCollected_altar] && 
                        CollectablesManager.collectedItems.Count>0)
                    {
                        foreach (Collectable c in CollectablesManager.collectedItems)
                            if (c.type == Collectable.ItemType.silverKey)
                            { 
                                TriggerEvent(Events.keyCollected_altar);
                                break;
                            }
                    }
                    break;
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
                    if (Player.position.X < 1752 && !eventAlreadyHappened[(int)Events.monologue_escape0])
                        TriggerEvent(Events.monologue_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 248 && !eventAlreadyHappened[(int)Events.lavaEruption1_escape0])
                        TriggerEvent(Events.lavaEruption1_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 416 && !eventAlreadyHappened[(int)Events.lowerFloor1_escape0])
                        TriggerEvent(Events.lowerFloor1_escape0);
                    if (!PlatformsManager.platformsRoomManagers[(int)RoomsManager.Rooms.escape0].movingPlatforms[1].active
                        && !eventAlreadyHappened[(int)Events.removeFloor1_escape0] && eventAlreadyHappened[(int)Events.lowerFloor1_escape0])
                        TriggerEvent(Events.removeFloor1_escape0);
                    if (eventAlreadyHappened[(int)Events.lowerFloor1_escape0] && !eventAlreadyHappened[(int)Events.lavaEruption2_escape0])
                        TriggerEvent(Events.lavaEruption2_escape0);
                    if (eventAlreadyHappened[(int)Events.lavaEruption2_escape0] && !eventAlreadyHappened[(int)Events.lavaEruption3_escape0])
                        TriggerEvent(Events.lavaEruption3_escape0);
                    if (eventAlreadyHappened[(int)Events.lavaEruption3_escape0] && !eventAlreadyHappened[(int)Events.lavaEruption4_escape0])
                        TriggerEvent(Events.lavaEruption4_escape0);
                    if (eventAlreadyHappened[(int)Events.lowerFloor1_escape0] && !eventAlreadyHappened[(int)Events.raiseFloor2_escape0])
                        TriggerEvent(Events.raiseFloor2_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1352 && !eventAlreadyHappened[(int)Events.lavaEruption5_escape0])
                        TriggerEvent(Events.lavaEruption5_escape0);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1448 && !eventAlreadyHappened[(int)Events.raiseFloor3_escape0])
                        TriggerEvent(Events.raiseFloor3_escape0);
                    if (eventAlreadyHappened[(int)Events.raiseFloor3_escape0] && !eventAlreadyHappened[(int)Events.lavaEruption6_escape0])
                        TriggerEvent(Events.lavaEruption6_escape0);
                    break;
                case RoomsManager.Rooms.escape1:
                    if (Player.IsOnMovingPlatform && !eventAlreadyHappened[(int)Events.activatePlatform_escape1])
                        TriggerEvent(Events.activatePlatform_escape1);
                    if (Player.position.X < MapsManager.maps[(int)RoomsManager.Rooms.escape1].RoomWidthtPx - 881 && !eventAlreadyHappened[(int)Events.raiseFloor1_escape1])
                        TriggerEvent(Events.raiseFloor1_escape1);
                    if (Player.position.X < 104 && !eventAlreadyHappened[(int)Events.lavaEruption1_escape1])
                        TriggerEvent(Events.lavaEruption1_escape1);
                    break;
                case RoomsManager.Rooms.escape2:
                    if (Player.position.X+Player.width< 424)
                    {
                        if (!eventAlreadyHappened[(int)Events.lavaEruption_escape2])
                            TriggerEvent(Events.lavaEruption_escape2);
                        else
                            eventAlreadyHappened[(int)Events.lavaEruption_escape2] = false;
                    }
                    break;
            }
        }
        
        //trigger the specified event
        public static void TriggerEvent(Events _event)
        {
            if (RoomsManager.CurrentRoom == RoomsManager.Rooms.tutorial0)
            {
                switch (_event)
                {
                    case Events.introMonologue:
                        MonologuesManager.monologuesRoomManagers[(int)RoomsManager.CurrentRoom].PlayMonologue(0);
                        happening = Events.introMonologue;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.finalMonologue:
                        MonologuesManager.monologuesRoomManagers[(int)RoomsManager.CurrentRoom].PlayMonologue(1);
                        happening = Events.finalMonologue;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.finalCutscene:
                        Game1.currentGameState = Game1.GameStates.ending;
                        CutscenesManager.cutscenes[1].ChangeSentence
                            (CutscenesManager.cutscenes[1].GetNUmOfSentences()-1, "Your Time: " + GameStats.GetTimeString()+
                            "                                                                                            ");
                        happening = Events.finalCutscene;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.tutorial3)
            {
                switch (_event)
                {
                    case Events.keySpawns1_tutorial3:
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                            new Collectable(new Point(350, 132), Collectable.ItemType.brassKey));
                        happening = Events.keySpawns1_tutorial3;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.key1Collected_tutorial3:
                        eventAlreadyHappened[(int)Events.key1Collected_tutorial3] = true;
                        break;
                    case Events.keySpawns2_tutorial3:
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                            new Collectable(new Point(350, 46), Collectable.ItemType.silverKey));
                        happening = Events.keySpawns2_tutorial3;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.key2Collected_tutorial3:
                        eventAlreadyHappened[(int)Events.key2Collected_tutorial3] = true;
                        break;

                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.churchBellTower0)
            {
                switch (_event)
                {
                    case Events.unlockDoubleJump:
                        Player.doubleJumpUnlocked = true;
                        AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.CurrentRoom].AddTempAnimatedSprite(
                            new AnimatedSprite(new Vector2(211, 926), AnimatedSprite.AnimationType.displayDoubleJumpRelic, false));
                        happening = Events.unlockDoubleJump;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.showDoubleJumpScreen:
                        Game1.currentGameState = Game1.GameStates.doubleJump;
                        happening = Events.showDoubleJumpScreen;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.unlockWallJump:
                        Player.wallJumpUnlocked = true;
                        AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.CurrentRoom].AddTempAnimatedSprite(
                            new AnimatedSprite(new Vector2(239, 926), AnimatedSprite.AnimationType.displayWallJumpRelic, false));
                        happening = Events.unlockWallJump;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.showWallJumpScreen:
                        Game1.currentGameState = Game1.GameStates.wallJump;
                        happening = Events.showWallJumpScreen;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.midBoss)
            {
                switch (_event)
                {
                    case Events.keyAndPowerUpSpawn_midBoss:
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                            new Collectable(new Point(280, 140), Collectable.ItemType.wallJump_powerup));
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                            new Collectable(new Point(216, 148), Collectable.ItemType.brassKey));
                        happening = Events.keyAndPowerUpSpawn_midBoss;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.pUpCollected_midBoss:
                        eventAlreadyHappened[(int)Events.pUpCollected_midBoss] = true;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.churchGroundFloor0)
            {
                switch (_event)
                {
                    case Events.activateMovingPlatforms_churchGroundFloor0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[16].active = true;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[17].active = true;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[18].active = true;
                        happening = Events.activateMovingPlatforms_churchGroundFloor0;
                        eventAlreadyHappened[(int)Events.resetMovingPlatforms_churchGroundFloor0] = false;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.resetMovingPlatforms_churchGroundFloor0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[16].Reset(false);
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[17].Reset(false);
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[18].Reset(false);
                        happening = Events.resetMovingPlatforms_churchGroundFloor0;
                        eventAlreadyHappened[(int)Events.activateMovingPlatforms_churchGroundFloor0] = false;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.church1stFloor0)
            {
                switch (_event)
                {
                    case Events.showInvisibleTiles_church1stFloor0:
                        List<AnimatedSprite> animatedSprites = new List<AnimatedSprite>();
                        for (int row = 0; row < MapsManager.maps[(int)RoomsManager.CurrentRoom].roomHeightTiles; row++)
                        {
                            for (int col = 0; col < MapsManager.maps[(int)RoomsManager.CurrentRoom].roomWidthTiles; col++)
                            {
                                if (MapsManager.maps[(int)RoomsManager.CurrentRoom].array[row, col].tileType == Tile.TileType.solidEmpty)
                                {
                                    animatedSprites.Add(new AnimatedSprite(new Vector2(col * Tile.tileSize, row * Tile.tileSize),
                                        AnimatedSprite.AnimationType.invisibleTile));
                                }
                            }
                        }
                        AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.CurrentRoom].AddAnimatedSprites(animatedSprites);
                        if (Player.position.X < 1032 && Player.position.X >= 1000)
                            CameraManager.MoveCamera(0, new Vector2(856, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx), 8);
                        else if (Player.position.X < 684 && Player.position.X >= 664)
                            CameraManager.MoveCamera(0, new Vector2(528, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx), 8);
                        else if (Player.position.X < 364 && Player.position.X >= 344)
                            CameraManager.MoveCamera(0, new Vector2(204, MapsManager.maps[(int)RoomsManager.CurrentRoom].RoomHeightPx), 8);
                        happening = Events.showInvisibleTiles_church1stFloor0;
                        eventAlreadyHappened[(int)Events.hide_InvisibleTiles_church1stFloor0] = false;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.hide_InvisibleTiles_church1stFloor0:
                        AnimatedSpritesManager.animatedSpritesRoomManagers[(int)RoomsManager.CurrentRoom].ClearAnimatedSprites();
                        CameraManager.MoveCamera(1, CameraManager.pointLocked, 8);
                        happening = Events.hide_InvisibleTiles_church1stFloor0;
                        eventAlreadyHappened[(int)Events.showInvisibleTiles_church1stFloor0] = false;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.churchAltarRoom)
            {
                switch (_event)
                {
                    case Events.spawnKey_altar:
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].AddCollectableToMap(
                            new Collectable(new Point(230, 400), Collectable.ItemType.silverKey));
                        eventAlreadyHappened[(int)Events.spawnKey_altar] = true;
                        break;
                    case Events.keyCollected_altar:
                        eventAlreadyHappened[(int)Events.keyCollected_altar] = true;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.finalBoss)
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
                        CollectablesManager.collectablesRoomManagers[(int)RoomsManager.CurrentRoom].RemoveCollectablesFromMap();
                        CameraManager.shakeForTime(eventsDuration[(int)Events.terrainCollapseFinalBoss]);
                        CameraManager.MoveCamera(0.3f, FinalBoss.fireballsCenter, 1);
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
                        CameraManager.MoveCamera(1, FinalBoss.fireballsCenter, 1);
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
                    case Events.monologue_escape0:
                        MonologuesManager.monologuesRoomManagers[(int)RoomsManager.CurrentRoom].PlayMonologue(0);
                        happening = Events.monologue_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption1_escape0:
                        LavaGeyserManager.ShootGeyser(new float[] { MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 308 }, 0, -1000);
                        List<int[]> tilesToRemove = new List<int[]>();
                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tilesToRemove.Add(new int[2] {
                                    MapsManager.maps[(int)RoomsManager.Rooms.escape0].roomHeightTiles - i-1,
                                    MapsManager.maps[(int)RoomsManager.Rooms.escape0].roomWidthTiles - 40 + j
                                });
                            }
                        }
                        MapsManager.maps[(int)RoomsManager.CurrentRoom].RemoveGroupOfTiles(tilesToRemove, 0.01f, 3, 0.3f);
                        happening = Events.lavaEruption1_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lowerFloor1_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[0].active = true;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[1].active = true;
                        happening = Events.lowerFloor1_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.removeFloor1_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[0].RemovePlatform();
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[1].RemovePlatform();
                        happening = Events.removeFloor1_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption2_escape0:
                        LavaGeyserManager.ShootGeyser(new float[]
                        {
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 664,
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 640
                        }, 0);
                        happening = Events.lavaEruption2_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption3_escape0:
                        LavaGeyserManager.ShootGeyser(new float[]
                        {
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1032,
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1008
                        }, 0, -600);
                        happening = Events.lavaEruption3_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption4_escape0:
                        LavaGeyserManager.ShootGeyser(new float[]
                        {
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 972,
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 948
                        }, 0, -600);
                        happening = Events.lavaEruption4_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.raiseFloor2_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[2].active = true;
                        happening = Events.raiseFloor2_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption5_escape0:
                        LavaGeyserManager.ShootGeyser(new float[] { MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1388 }, 1.5f, -800);
                        happening = Events.lavaEruption5_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.raiseFloor3_escape0:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.CurrentRoom].movingPlatforms[3].active = true;
                        happening = Events.raiseFloor3_escape0;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption6_escape0:
                        LavaGeyserManager.ShootGeyser(new float[]
                        {
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1688,
                            MapsManager.maps[(int)RoomsManager.Rooms.escape0].RoomWidthtPx - 1664
                        }, 0);
                        happening = Events.lavaEruption6_escape0;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.escape1)
            {
                switch (_event)
                {
                    case Events.activatePlatform_escape1:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.Rooms.escape1].movingPlatforms[0].active = true;
                        happening = Events.activatePlatform_escape1;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.raiseFloor1_escape1:
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.Rooms.escape1].movingPlatforms[1].active = true;
                        PlatformsManager.platformsRoomManagers[(int)RoomsManager.Rooms.escape1].movingPlatforms[2].active = true;
                        happening = Events.raiseFloor1_escape1;
                        elapsedEventsDuration = 0;
                        break;
                    case Events.lavaEruption1_escape1:
                        LavaGeyserManager.SweepAcross(0.2f, 0, 10, 0, 96, true);
                        happening = Events.lavaEruption1_escape1;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
            else if (RoomsManager.CurrentRoom == RoomsManager.Rooms.escape2)
            {
                switch (_event)
                {
                    case Events.lavaEruption_escape2:
                        LavaGeyserManager.ShootGeyser(new float[] { 396 }, 0f);
                        LavaGeyserManager.ShootGeyser(new float[] { 376 }, 0.2f);
                        LavaGeyserManager.ShootGeyser(new float[] { 356 }, 0.4f);
                        LavaGeyserManager.ShootGeyser(new float[] { 336 }, 0.6f);
                        LavaGeyserManager.ShootGeyser(new float[] { 316 }, 0.8f);
                        LavaGeyserManager.ShootGeyser(new float[] { 296 }, 1f);
                        LavaGeyserManager.ShootGeyser(new float[] { 276 }, 1.2f);
                        LavaGeyserManager.ShootGeyser(new float[] { 256 }, 1.4f);
                        LavaGeyserManager.ShootGeyser(new float[] { 236 }, 1.6f);
                        LavaGeyserManager.ShootGeyser(new float[] { 216 }, 1.8f);
                        LavaGeyserManager.ShootGeyser(new float[] { 196 }, 2f);
                        LavaGeyserManager.ShootGeyser(new float[] { 176 }, 2.2f);
                        LavaGeyserManager.ShootGeyser(new float[] { 156 }, 2.4f);
                        LavaGeyserManager.ShootGeyser(new float[] { 136 }, 2.6f);
                        LavaGeyserManager.ShootGeyser(new float[] { 116 }, 2.8f);
                        LavaGeyserManager.ShootGeyser(new float[] { 96 }, 3f);
                        happening = Events.lavaEruption_escape2;
                        elapsedEventsDuration = 0;
                        break;
                }
            }
        }
        
    }
}
