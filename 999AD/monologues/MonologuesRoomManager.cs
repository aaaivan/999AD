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
        #region PROPERTIES
        public int IndexPlaying
        {
            get { return indexPlaying; }
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
                        interactSymbolRectangle = monologues[i].InteractSymbolLocation(sourceRectangle_interactSymbol.Width, sourceRectangle_interactSymbol.Height);
                        if ((Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)) ||
                            (Game1.currentGamePad.Buttons.X == ButtonState.Pressed && Game1.previousGamePad.Buttons.X == ButtonState.Released))
                        {
                            monologues[i].active = true;
                            indexPlaying = i;
                            Player.haltInput = true;
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
                    Player.haltInput = false;
                    return;
                }
                monologues[indexPlaying].Update(elapsedTime);
            }
        }
        public void PlayMonologue(int index)
        {
            if (indexPlaying != -1 || index<0 || index>=monologues.Length)
                return;
            monologues[index].active = true;
            indexPlaying = index;
            Player.haltInput = true;
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
