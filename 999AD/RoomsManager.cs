﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    static class RoomsManager
    {
        #region DECLARATIONS
        public enum Rooms
        {
            tutorial0, tutorial1, tutorial2, tutorial3, tutorial4,
            churchBellTower0, churchBellTower1, churchBellTower2, midBoss,
            churchGroundFloor0, churchAltarRoom,
            church1stFloor0, church2ndFloor0,
            finalBoss, escape0, escape1, total
        }
        static Rooms currentRoom = Rooms.churchBellTower0;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize()
        {
            CameraManager.SwitchCamera(currentRoom);
        }
        #endregion
        #region PROPERTIES
        public static Rooms CurrentRoom
        {
            get { return currentRoom; }
        }
        #endregion
        #region METHODS
        //state machine managing the movement between rooms
        static void switchRoom()
        {
            switch (currentRoom)
            {
                case Rooms.tutorial0:
                    if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to tutorial1
                        currentRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = 0;
                    }
                    break;
                case Rooms.tutorial1:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx+20)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.Y = 0;
                    }
                    else if (Player.position.X + Player.width < 0)
                    {//move to tutorial0
                        currentRoom = Rooms.tutorial0;
                        CameraManager.SwitchCamera(Rooms.tutorial0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    else if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to tutorial3
                        currentRoom = Rooms.tutorial3;
                        CameraManager.SwitchCamera(Rooms.tutorial3);
                        Player.position.X = 0;
                    }
                    break;
                case Rooms.tutorial2:
                    if (Player.position.Y+Player.height<0)
                    {
                        if (Player.position.X>= MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx)
                        {//move to tutorial3
                            currentRoom = Rooms.tutorial3;
                            CameraManager.SwitchCamera(Rooms.tutorial3);
                            Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                            Player.position.X -= MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx;
                        }
                        else
                        {//move to tutorial1
                            currentRoom = Rooms.tutorial1;
                            CameraManager.SwitchCamera(Rooms.tutorial1);
                            Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        }
                    }
                    else if (Player.position.X+Player.width<0)
                    {//moveto tutorial4
                        currentRoom = Rooms.tutorial4;
                        CameraManager.SwitchCamera(Rooms.tutorial4);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    break;
                case Rooms.tutorial3:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.X += MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx;
                        Player.position.Y = -10;
                    }
                    else if (Player.position.X<0)
                    {//move to tutorial1
                        currentRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx-Player.width;
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.X = 0;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx- MapsManager.maps[(int)Rooms.tutorial3].RoomHeightPx;
                    }
                    break;
                case Rooms.tutorial4:
                    if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.X = 0;
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move to tutorial0
                        currentRoom = Rooms.tutorial0;
                        CameraManager.SwitchCamera(Rooms.tutorial0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                    }
                    break;
                case Rooms.churchBellTower0:
                    if (Player.position.X+Player.width<0)
                    {//move to tutorial3
                        currentRoom = Rooms.tutorial3;
                        CameraManager.SwitchCamera(Rooms.tutorial3);
                        Player.position.X= MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move up to churchBellTower1
                        currentRoom = Rooms.churchBellTower1;
                        CameraManager.SwitchCamera(Rooms.churchBellTower1);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {
                        if (Player.position.Y> 496)
                        {//move to churchGroundFloor0
                            currentRoom = Rooms.churchGroundFloor0;
                            CameraManager.SwitchCamera(Rooms.churchGroundFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                        }
                        else if (Player.position.Y>248)
                        {//church1stFloor0
                            currentRoom = Rooms.church1stFloor0;
                            CameraManager.SwitchCamera(Rooms.church1stFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx + MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                        }
                        else
                        {//church2ndFloor0
                            currentRoom = Rooms.church2ndFloor0;
                            CameraManager.SwitchCamera(Rooms.church2ndFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                                 + MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                                 + MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx
                                                 - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                        }

                    }
                    break;
                case Rooms.churchBellTower1:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y = 0;
                    }
                    else if (Player.position.Y + Player.height < 0)
                    {//move up to chirchBellTower2
                        currentRoom = Rooms.churchBellTower2;
                        CameraManager.SwitchCamera(Rooms.churchBellTower2);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                    }
                    break;
                case Rooms.churchBellTower2:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower1
                        currentRoom = Rooms.churchBellTower1;
                        CameraManager.SwitchCamera(Rooms.churchBellTower1);
                        Player.position.Y = 0;
                    }
                    else if (Player.position.Y + Player.height < 0)
                    {//move up to midBoss
                        currentRoom = Rooms.midBoss;
                        CameraManager.SwitchCamera(Rooms.midBoss);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                    }
                    break;
                case Rooms.midBoss:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower1
                        currentRoom = Rooms.churchBellTower2;
                        CameraManager.SwitchCamera(Rooms.churchBellTower2);
                        Player.position.Y = 0;
                    }
                    break;
                case Rooms.churchGroundFloor0:
                    if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx;
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to churchAltarRoom
                        currentRoom = Rooms.churchAltarRoom;
                        CameraManager.SwitchCamera(Rooms.churchAltarRoom);
                        Player.position.X = 0;
                    }
                    break;
                case Rooms.churchAltarRoom:
                    if (Player.position.X+Player.width<0)
                    {//move to churchGroundFloor0
                        currentRoom = Rooms.churchGroundFloor0;
                        CameraManager.SwitchCamera(Rooms.churchGroundFloor0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move to church1stFloor0
                        currentRoom = Rooms.church1stFloor0;
                        CameraManager.SwitchCamera(Rooms.church1stFloor0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        Player.position.X += MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomWidthtPx;
                    }
                    break;
                case Rooms.church1stFloor0:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to churchAltarRoom
                        currentRoom = Rooms.churchAltarRoom;
                        CameraManager.SwitchCamera(Rooms.churchAltarRoom);
                        Player.position.Y = 0;
                        Player.position.X -= MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomWidthtPx;
                    }
                    else if (Player.position.Y +Player.height <0)
                    {//move to church2ndFloor0
                        currentRoom = Rooms.church2ndFloor0;
                        CameraManager.SwitchCamera(Rooms.church2ndFloor0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                    }
                    else if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx;
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx-Player.width;
                    }
                    break;
                case Rooms.church2ndFloor0:
                    if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx
                                             -MapsManager.maps[(int)Rooms.church2ndFloor0].RoomHeightPx;
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {
                        Player.position.Y = 0;
                    }
                    break;
            }
        }
        public static void Update(float elapsedTime)
        {
            switchRoom();
            CameraManager.Update(elapsedTime);
            MapsManager.maps[(int)currentRoom].Update(elapsedTime);
            if (currentRoom== Rooms.finalBoss)
            {
                FinalBoss.Update(elapsedTime);
            }
            FireBallsManager.Update(elapsedTime);
            LavaGeyserManager.Update(elapsedTime);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Update(elapsedTime);
            CollectablesManager.collectablesRoomManagers[(int)currentRoom].Update(elapsedTime);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Update(elapsedTime);
            DoorsManager.doorsRoomManagers[(int)currentRoom].Update();
            AnimatedSpritesManager.animatedSpritesRoomManagers[(int)currentRoom].Update(elapsedTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            MapsManager.maps[(int)currentRoom].Draw(spriteBatch);
            Player.Draw(spriteBatch);
            ProjectilesManager.Draw(spriteBatch);
            PlatformsManager.platformsRoomManagers[(int)currentRoom].Draw(spriteBatch);
            CollectablesManager.collectablesRoomManagers[(int)currentRoom].Draw(spriteBatch);
            if (currentRoom == Rooms.finalBoss)
            {
                FinalBoss.Draw(spriteBatch);
            }
            FireBallsManager.Draw(spriteBatch);
            LavaGeyserManager.Draw(spriteBatch);
            MonologuesManager.monologuesRoomManagers[(int)currentRoom].Draw(spriteBatch);
            DoorsManager.doorsRoomManagers[(int)currentRoom].Draw(spriteBatch);
            AnimatedSpritesManager.animatedSpritesRoomManagers[(int)currentRoom].Draw(spriteBatch);
        }
        #endregion
    }
}
