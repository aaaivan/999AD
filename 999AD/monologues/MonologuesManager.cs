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
                        new Monologue(new Rectangle(264, 142, 55, 78),
                        new string[]
                        {
                            "Wha...?!",
                            "...",
                            "So... you have been sent here on a quest to\nsave the world, you say..." ,
                            "I see. I have one teensy-weensy, but ever\nso crucial little, tiny feat for you...",
                            "GO BACK HOME, YOU FOOL!"})
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