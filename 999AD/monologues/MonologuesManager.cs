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
    static class MonologuesManager
    {
        #region DECLARATIONS
        public static MonologuesRoomManager[] monologuesRoomManagers;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D dialogueBox,Texture2D arrow, Texture2D interactSymbol, SpriteFont spriteFont)
        {
            Monologue.Inizialize(dialogueBox,arrow, spriteFont);
            MonologuesRoomManager.Inizialize(interactSymbol);
            monologuesRoomManagers = new MonologuesRoomManager[(int)RoomsManager.Rooms.total]
            {
                new MonologuesRoomManager(  //tutorial0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //tutorial1
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //tutorial2
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //tutorial3
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //tutorial4
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //churchBellTower0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //churchBellTower1
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //churchBellTower2
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //midBoss
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //churchGroundFloor0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //churchAltarRoom
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //church1stFloor0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //church2ndFloor0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //finalBoss
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //escape0
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //escape1
                    new Monologue[]
                    {
                    }),
            };
        }
        #endregion
    }
}