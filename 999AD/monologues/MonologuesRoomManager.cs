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
        static Texture2D spritesheet;
        static Rectangle sourceRectangle_interactSymbol;
        static bool drawInteractSymbol;
        static Rectangle interactSymbolRectangle;
        Monologue[] monologues;
        int indexPlaying;
        #endregion
        #region CONSTRUCTOR
        public MonologuesRoomManager(Monologue[] _monologues)
        {
            drawInteractSymbol = false;
            indexPlaying = -1;
            monologues = _monologues;
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
            sourceRectangle_interactSymbol = new Rectangle(384, 181, 7, 20);
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
                        interactSymbolRectangle = monologues[i].InteractSymbolLocation(spritesheet.Width, spritesheet.Height);
                        if (monologues[i].PlayAutomatically ||
                            (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)) ||
                            (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
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
                spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(interactSymbolRectangle), sourceRectangle_interactSymbol, Color.White);
        }
        #endregion
    }
}
