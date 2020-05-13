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
    static class RoomsManager
    {
        #region DECLARATIONS
        public enum Rooms
        {
            tutorial0, tutorial1, tutorial2, tutorial3, tutorial4,
            churchBellTower0, churchBellTower1, churchBellTower2, midBoss,
            churchGroundFloor0, churchAltarRoom,
            church1stFloor0, church2ndFloor0, descent,
            finalBoss, escape0, escape1, escape2,
            total
        }
        static Rooms currentRoom;
        static Rooms previousRoom;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize()
        {
            currentRoom = Rooms.finalBoss;
            previousRoom = Rooms.tutorial4;
            CameraManager.SwitchCamera(currentRoom);
        }
        #endregion
        #region PROPERTIES
        public static Rooms CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; }
        }
        public static Rooms PreviousRoom
        {
            get { return previousRoom; }
            set { previousRoom = value; }
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
                        previousRoom = Rooms.tutorial0;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to tutorial4
                        currentRoom = Rooms.tutorial4;
                        previousRoom = Rooms.tutorial0;
                        CameraManager.SwitchCamera(Rooms.tutorial4);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.tutorial1:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        previousRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X + Player.width < 0)
                    {//move to tutorial0
                        currentRoom = Rooms.tutorial0;
                        previousRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to tutorial3
                        currentRoom = Rooms.tutorial3;
                        previousRoom = Rooms.tutorial1;
                        CameraManager.SwitchCamera(Rooms.tutorial3);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.tutorial2:
                    if (Player.position.Y+Player.height<0)
                    {
                        if (Player.position.X>= MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx)
                        {//move to tutorial3
                            currentRoom = Rooms.tutorial3;
                            previousRoom = Rooms.tutorial2;
                            CameraManager.SwitchCamera(Rooms.tutorial3);
                            Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                            Player.position.X -= MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx;
                            FireBallsManager.Reset();
                            LavaGeyserManager.Reset();
                            ProjectilesManager.Reset();
                        }
                        else
                        {//move to tutorial1
                            currentRoom = Rooms.tutorial1;
                            previousRoom = Rooms.tutorial2;
                            CameraManager.SwitchCamera(Rooms.tutorial1);
                            Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                            FireBallsManager.Reset();
                            LavaGeyserManager.Reset();
                            ProjectilesManager.Reset();
                        }
                    }
                    else if (Player.position.X+Player.width<0)
                    {//moveto tutorial4
                        currentRoom = Rooms.tutorial4;
                        previousRoom = Rooms.tutorial2;
                        CameraManager.SwitchCamera(Rooms.tutorial4);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.tutorial3:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        previousRoom = Rooms.tutorial3;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.X += MapsManager.maps[(int)Rooms.tutorial1].RoomWidthtPx;
                        Player.position.Y = -10;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X<0)
                    {//move to tutorial1
                        currentRoom = Rooms.tutorial1;
                        previousRoom = Rooms.tutorial3;
                        CameraManager.SwitchCamera(Rooms.tutorial1);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx-Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        previousRoom = Rooms.tutorial3;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.X = 0;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx- MapsManager.maps[(int)Rooms.tutorial3].RoomHeightPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.tutorial4:
                    if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to tutorial2
                        currentRoom = Rooms.tutorial2;
                        previousRoom = Rooms.tutorial4;
                        CameraManager.SwitchCamera(Rooms.tutorial2);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move to tutorial0
                        currentRoom = Rooms.tutorial0;
                        previousRoom = Rooms.tutorial4;
                        CameraManager.SwitchCamera(Rooms.tutorial0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to escape2
                        currentRoom = Rooms.escape2;
                        previousRoom = Rooms.tutorial4;
                        CameraManager.SwitchCamera(Rooms.escape2);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.churchBellTower0:
                    if (Player.position.X+Player.width<0)
                    {//move to tutorial3
                        currentRoom = Rooms.tutorial3;
                        previousRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.tutorial3);
                        Player.position.X= MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move up to churchBellTower1
                        currentRoom = Rooms.churchBellTower1;
                        previousRoom = Rooms.churchBellTower0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower1);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {
                        if (Player.position.Y> 496)
                        {//move to churchGroundFloor0
                            currentRoom = Rooms.churchGroundFloor0;
                            previousRoom = Rooms.churchBellTower0;
                            CameraManager.SwitchCamera(Rooms.churchGroundFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                            FireBallsManager.Reset();
                            LavaGeyserManager.Reset();
                            ProjectilesManager.Reset();
                        }
                        else if (Player.position.Y>248)
                        {//church1stFloor0
                            currentRoom = Rooms.church1stFloor0;
                            previousRoom = Rooms.churchBellTower0;
                            CameraManager.SwitchCamera(Rooms.church1stFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx + MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                            FireBallsManager.Reset();
                            LavaGeyserManager.Reset();
                            ProjectilesManager.Reset();
                        }
                        else
                        {//church2ndFloor0
                            currentRoom = Rooms.church2ndFloor0;
                            previousRoom = Rooms.churchBellTower0;
                            CameraManager.SwitchCamera(Rooms.church2ndFloor0);
                            Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                                 + MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                                 + MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx
                                                 - MapsManager.maps[(int)Rooms.churchBellTower0].RoomHeightPx;
                            Player.position.X = 0;
                            FireBallsManager.Reset();
                            LavaGeyserManager.Reset();
                            ProjectilesManager.Reset();
                        }

                    }
                    break;
                case Rooms.churchBellTower1:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        previousRoom = Rooms.churchBellTower1;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y + Player.height < 0)
                    {//move up to chirchBellTower2
                        currentRoom = Rooms.churchBellTower2;
                        previousRoom = Rooms.churchBellTower1;
                        CameraManager.SwitchCamera(Rooms.churchBellTower2);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.churchBellTower2:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower1
                        currentRoom = Rooms.churchBellTower1;
                        previousRoom = Rooms.churchBellTower2;
                        CameraManager.SwitchCamera(Rooms.churchBellTower1);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y + Player.height < 0)
                    {//move up to midBoss
                        currentRoom = Rooms.midBoss;
                        previousRoom = Rooms.churchBellTower2;
                        CameraManager.SwitchCamera(Rooms.midBoss);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.midBoss:
                    if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move down to chirchBellTower1
                        currentRoom = Rooms.churchBellTower2;
                        previousRoom = Rooms.midBoss;
                        CameraManager.SwitchCamera(Rooms.churchBellTower2);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.churchGroundFloor0:
                    if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        previousRoom = Rooms.churchGroundFloor0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to churchAltarRoom
                        currentRoom = Rooms.churchAltarRoom;
                        previousRoom = Rooms.churchGroundFloor0;
                        CameraManager.SwitchCamera(Rooms.churchAltarRoom);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.churchAltarRoom:
                    if (Player.position.X+Player.width<0)
                    {//move to churchGroundFloor0
                        currentRoom = Rooms.churchGroundFloor0;
                        previousRoom = Rooms.churchAltarRoom;
                        CameraManager.SwitchCamera(Rooms.churchGroundFloor0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y+Player.height<0)
                    {//move to church1stFloor0
                        currentRoom = Rooms.church1stFloor0;
                        previousRoom = Rooms.churchAltarRoom;
                        CameraManager.SwitchCamera(Rooms.church1stFloor0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        Player.position.X += MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomWidthtPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to descent
                        currentRoom = Rooms.descent;
                        previousRoom = Rooms.churchAltarRoom;
                        CameraManager.SwitchCamera(Rooms.descent);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.church1stFloor0:
                    if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to churchAltarRoom
                        currentRoom = Rooms.churchAltarRoom;
                        previousRoom = Rooms.church1stFloor0;
                        CameraManager.SwitchCamera(Rooms.churchAltarRoom);
                        Player.position.Y = 0;
                        Player.position.X -= MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomWidthtPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y +Player.height <0)
                    {//move to church2ndFloor0
                        currentRoom = Rooms.church2ndFloor0;
                        previousRoom = Rooms.church1stFloor0;
                        CameraManager.SwitchCamera(Rooms.church2ndFloor0);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        previousRoom = Rooms.church1stFloor0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx;
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx-Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.church2ndFloor0:
                    if (Player.position.X+Player.width<0)
                    {//move to churchBellTower0
                        currentRoom = Rooms.churchBellTower0;
                        previousRoom = Rooms.church2ndFloor0;
                        CameraManager.SwitchCamera(Rooms.churchBellTower0);
                        Player.position.Y += MapsManager.maps[(int)currentRoom].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.church1stFloor0].RoomHeightPx
                                             - MapsManager.maps[(int)Rooms.churchGroundFloor0].RoomHeightPx
                                             -MapsManager.maps[(int)Rooms.church2ndFloor0].RoomHeightPx;
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to church1stFloor0
                        currentRoom = Rooms.church1stFloor0;
                        previousRoom = Rooms.church2ndFloor0;
                        CameraManager.SwitchCamera(Rooms.church1stFloor0);
                        Player.position.Y = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.descent:
                    if (Player.position.Y+Player.height<0)
                    {//move to churchAltarRoom
                        currentRoom = Rooms.churchAltarRoom;
                        previousRoom = Rooms.descent;
                        CameraManager.SwitchCamera(Rooms.churchAltarRoom);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx)
                    {//move to finalBoss
                        currentRoom = Rooms.finalBoss;
                        previousRoom = Rooms.descent;
                        Game1.Zoom0Dot5();
                        CameraManager.SwitchCamera(Rooms.finalBoss);
                        Player.position.Y = 0;
                        Player.position.X += MapsManager.maps[(int)currentRoom].RoomWidthtPx - MapsManager.maps[(int)Rooms.descent].RoomWidthtPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    break;
                case Rooms.finalBoss:
                    if (Player.position.X+Player.width<0)
                    {//move to escape0
                        currentRoom = Rooms.escape0;
                        previousRoom = Rooms.finalBoss;
                        Game1.Zoom1();
                        CameraManager.SwitchCamera(Rooms.escape0);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx-Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if(Player.position.Y+Player.height<0) 
                    {//move to descent
                        currentRoom = Rooms.descent;
                        previousRoom = Rooms.finalBoss;
                        Game1.Zoom1();
                        CameraManager.SwitchCamera(Rooms.descent);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx - Player.height;
                        Player.position.X += MapsManager.maps[(int)currentRoom].RoomWidthtPx - MapsManager.maps[(int)Rooms.finalBoss].RoomWidthtPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y> MapsManager.maps[(int)currentRoom].RoomHeightPx+24)
                    {
                        Player.takeDamage(Player.maxHealthPoints, true);
                    }
                    break;
                case Rooms.escape0:
                    if (Player.position.X > MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to finalBoss
                        currentRoom = Rooms.finalBoss;
                        previousRoom = Rooms.escape0;
                        Game1.Zoom0Dot5();
                        CameraManager.SwitchCamera(Rooms.finalBoss);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X + Player.width<0)
                    {//move to escape1
                        currentRoom = Rooms.escape1;
                        previousRoom = Rooms.escape0;
                        CameraManager.SwitchCamera(Rooms.escape1);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        Player.position.Y += MapsManager.maps[(int)Rooms.descent].RoomHeightPx - MapsManager.maps[(int)Rooms.tutorial2].RoomHeightPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx + 24)
                    {
                        Player.takeDamage(Player.maxHealthPoints, true);
                    }
                    break;
                case Rooms.escape1:
                    if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to escape0
                        currentRoom = Rooms.escape0;
                        previousRoom = Rooms.escape1;
                        CameraManager.SwitchCamera(Rooms.escape0);
                        Player.position.X = 0;
                        Player.position.Y-= MapsManager.maps[(int)Rooms.descent].RoomHeightPx - MapsManager.maps[(int)Rooms.tutorial2].RoomHeightPx;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.X+Player.width<0)
                    {//move to escape2
                        currentRoom = Rooms.escape2;
                        previousRoom = Rooms.escape1;
                        CameraManager.SwitchCamera(Rooms.escape2);
                        Player.position.X = MapsManager.maps[(int)currentRoom].RoomWidthtPx - Player.width;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx + 24)
                    {
                        Player.takeDamage(Player.maxHealthPoints, true);
                    }
                    break;
                case Rooms.escape2:
                    if (Player.position.X> MapsManager.maps[(int)currentRoom].RoomWidthtPx)
                    {//move to escape1
                        currentRoom = Rooms.escape1;
                        previousRoom = Rooms.escape2;
                        CameraManager.SwitchCamera(Rooms.escape1);
                        Player.position.X = 0;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if(Player.position.Y+Player.height<0)
                    {//move to tutorial4
                        currentRoom = Rooms.tutorial4;
                        previousRoom = Rooms.escape2;
                        CameraManager.SwitchCamera(Rooms.tutorial4);
                        Player.position.Y = MapsManager.maps[(int)currentRoom].RoomHeightPx-Player.height;
                        FireBallsManager.Reset();
                        LavaGeyserManager.Reset();
                        ProjectilesManager.Reset();
                    }
                    else if (Player.position.Y > MapsManager.maps[(int)currentRoom].RoomHeightPx + 24)
                    {
                        Player.takeDamage(Player.maxHealthPoints, true);
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
            if (currentRoom == Rooms.midBoss)
                MidBoss.Update(elapsedTime);
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
            if(currentRoom==Rooms.midBoss)
            {
                MidBoss.Draw(spriteBatch);
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
