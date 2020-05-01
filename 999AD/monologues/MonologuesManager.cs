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
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                        new Monologue(new Rectangle(123, 100, 48, 66), new string[]{"How dare you disturb my sleep?!", "FUCKING HELL!" })
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(
                    new Monologue[]
                    {
                    })
            };
        }
        #endregion
    }
}
