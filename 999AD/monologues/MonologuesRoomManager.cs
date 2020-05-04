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
    class MonologuesRoomManager
    {
        #region DECLARATIONS
        static Texture2D interactSymbol;
        static bool drawInteractSymbol = false;
        static Rectangle interactSymbolRectangle;
        Monologue[] monologues;
        int indexPlaying = -1;
        #endregion
        #region CONSTRUCTOR
        public MonologuesRoomManager(Monologue[] _monologues)
        {
            monologues = _monologues;
        }
        public static void Inizialize(Texture2D _interactSymbol)
        {
            interactSymbol = _interactSymbol;
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (indexPlaying == -1)
            {
                for (int i = 0; i < monologues.Length; i++)
                {
                    if (monologues[i].interactionRectangle.Intersects(Player.CollisionRectangle))
                    {
                        drawInteractSymbol = true;
                        interactSymbolRectangle = monologues[i].InteractSymbolLocation(interactSymbol.Width, interactSymbol.Height);
                        if (monologues[i].PlayAutomatically ||
                            (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)))
                        {
                            monologues[i].active = true;
                            indexPlaying = i;
                        }
                        return;
                    }
                }
                drawInteractSymbol = false;
            }
            else
            {
                if (!monologues[indexPlaying].active)
                {
                    indexPlaying = -1;
                    return;
                }
                monologues[indexPlaying].Update(elapsedTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (indexPlaying != -1)
                monologues[indexPlaying].Draw(spriteBatch);
            else if (drawInteractSymbol)
                spriteBatch.Draw(interactSymbol, Camera.RelativeRectangle(interactSymbolRectangle), Color.White);
        }
        #endregion
    }
}
