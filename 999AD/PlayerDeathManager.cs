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
    static class PlayerDeathManager
    {
        #region DECLARATIONS
        static SpriteFont spriteFont;
        static readonly string youDied= "YOU DIED";
        static readonly string enterToContinue= "Press Enter to continue.";
        static readonly float timeBeforeResuming= 4;
        static float elapsedTimeBeforeResuming;
        #endregion
        #region CONSTRUCTOR
        public static void Initialize(SpriteFont _spriteFont)
        {
            spriteFont = _spriteFont;
            elapsedTimeBeforeResuming = 0;
        }
        #endregion
        #region METHODS
        public static void Update(float elapsedTime)
        {
            if (elapsedTimeBeforeResuming < timeBeforeResuming)
            {
                elapsedTimeBeforeResuming += elapsedTime;
                return;
            }
            else if(Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            {
                elapsedTimeBeforeResuming = 0;
                Game1.currentGameState = Game1.GameStates.playing;
                CameraManager.Reset();
                LavaGeyserManager.Reset();
                FireBallsManager.Reset();
                ProjectilesManager.Reset();
                EnemyManager.Reset();
                if (!MidBoss.Dead)
                    MidBoss.Reset();
                CollectablesManager.ResetHearts();
                GameEvents.Reset();
                Player.ReplenishHealth();
                switch (RoomsManager.CurrentRoom)
                {
                    case RoomsManager.Rooms.tutorial0:
                        if (RoomsManager.PreviousRoom== RoomsManager.Rooms.tutorial4)
                            Player.position= new Vector2(167,153);
                        else if (RoomsManager.PreviousRoom== RoomsManager.Rooms.tutorial1)
                            Player.position= new Vector2(903,185);
                        break;
                    case RoomsManager.Rooms.tutorial1:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial0)
                            Player.position = new Vector2(18, 185);
                        else
                            Player.position = new Vector2(550, 193);
                        break;
                    case RoomsManager.Rooms.tutorial2:
                        if (Player.position.Y<80)
                        {
                            RoomsManager.CurrentRoom = RoomsManager.Rooms.tutorial1;
                            RoomsManager.PreviousRoom = RoomsManager.Rooms.tutorial0;
                            Player.position = new Vector2(18, 185);
                        }
                        else
                        Player.position = new Vector2(388, 153);
                        break;
                    case RoomsManager.Rooms.tutorial3:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
                            Player.position = new Vector2(776, 193);
                        else
                            Player.position = new Vector2(91, 193);
                        break;
                    case RoomsManager.Rooms.tutorial4:
                        Player.position = new Vector2(166, 153);
                        break;
                    case RoomsManager.Rooms.churchBellTower0:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower1)
                        {
                            if (Player.position.X < 436 && Player.position.X >= 60)
                                Player.position = new Vector2(405, 161);
                            else if (Player.position.X >= 436)
                                Player.position = new Vector2(456, 961);
                            else
                                Player.position = new Vector2(23, 961);
                        }
                        else if (RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial3)
                            Player.position = new Vector2(23, 961);
                        else
                            Player.position = new Vector2(456, 961);
                        break;
                    case RoomsManager.Rooms.churchBellTower1:
                        if (Player.position.X >= 436)
                            Player.position = new Vector2(458, 1097);
                        else if (Player.position.X < 60)
                            Player.position = new Vector2(23, 1097);
                        else
                        {
                            if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
                                Player.position = new Vector2(372, 1097);
                            else
                                Player.position = new Vector2(111, 121);
                        }
                        break;
                    case RoomsManager.Rooms.churchBellTower2:
                        if (Player.position.X >= 436 && Player.position.Y > 110)
                        {
                                Player.position = new Vector2(464, 1177);
                        }
                        else if (Player.position.X >= 60)
                        {
                            if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower1)
                                Player.position = new Vector2(119, 1177);
                            else
                                Player.position = new Vector2(457, 65);
                        }
                        else
                            Player.position = new Vector2(23, 1177);
                        break;
                    case RoomsManager.Rooms.midBoss:
                        Player.position = new Vector2(452, 25);
                        break;
                    case RoomsManager.Rooms.churchGroundFloor0:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
                            Player.position = new Vector2(22, 465);
                        else
                            Player.position = new Vector2(1160, 465);
                        break;
                    case RoomsManager.Rooms.churchAltarRoom:
                        if (Player.position.X< 104)
                            Player.position = new Vector2(22, 465);
                        else
                            Player.position = new Vector2(1160, 465);
                        break;
                    case RoomsManager.Rooms.church1stFloor0:
                        if (Player.position.X < 1288)
                        {
                            if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchAltarRoom)
                                Player.position = new Vector2(1208, 217);
                            else
                                Player.position = new Vector2(18, 217);
                        }
                        else
                            Player.position = new Vector2(1320, 217);
                        break;
                    case RoomsManager.Rooms.church2ndFloor0:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
                            Player.position = new Vector2(37, 481);
                        else
                            Player.position = new Vector2(1289, 481);
                        break;
                    case RoomsManager.Rooms.descent:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchAltarRoom)
                            Player.position = new Vector2(339, 121);
                        else
                            Player.position = new Vector2(318, 233);
                        break;
                    case RoomsManager.Rooms.finalBoss:
                        if (!FinalBoss.Dead)
                            Player.position = new Vector2(734, 41);
                        else
                            Player.position = new Vector2(17, 41);
                        break;
                    case RoomsManager.Rooms.escape0:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.finalBoss)
                            Player.position = new Vector2(1726, 129);
                        else
                            Player.position = new Vector2(18, 49);
                        break;

                    case RoomsManager.Rooms.escape1:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape0)
                            Player.position = new Vector2(1320, 89);
                        else
                            Player.position = new Vector2(12, 289);
                        break;
                    case RoomsManager.Rooms.escape2:
                        if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape1)
                            Player.position = new Vector2(500, 289);
                        else
                            Player.position = new Vector2(21, 129);
                        break;

                }
            }
        }
        #endregion
    }
}
