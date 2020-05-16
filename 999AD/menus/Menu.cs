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
    class Menu
    {
        #region DECLARATIONS
        static Texture2D spritesheet_options;
        Texture2D backgroungImage;
        Rectangle[] sourceRectangles_options;
        Rectangle[] positionOnScreen_options;
        int currentOption;
        int totalNumberOfOptions;
        Game1.GameStates[] followingGameState;
        Game1.GameStates previousGameState;
        #endregion
        #region CONSTRUCTORS
        static public void Initialize(Texture2D spritesheet)
        {
            spritesheet_options = spritesheet;
        }
        public Menu(Texture2D _backgroundImage, Rectangle[] _sourceRectangles_options, Game1.GameStates[] _followingGameStates,
            Game1.GameStates _previousGameState, Rectangle[] _positionOnScreen_options)
        {
            backgroungImage = _backgroundImage;
            sourceRectangles_options = _sourceRectangles_options;
            currentOption = 0;
            totalNumberOfOptions = sourceRectangles_options.Length;
            followingGameState = _followingGameStates;
            previousGameState = _previousGameState;
            positionOnScreen_options = _positionOnScreen_options;
        }
        #endregion
        #region METHODS
        public void Update()
        {
            //check which option is currently selected
            if ((Game1.currentKeyboard.IsKeyDown(Keys.Down) && !Game1.previousKeyboard.IsKeyDown(Keys.Down)) || 
                (Game1.currentGamePad.DPad.Down == ButtonState.Pressed && Game1.previousGamePad.DPad.Down == ButtonState.Released))
                currentOption = (currentOption + 1) % totalNumberOfOptions;
            else if ((Game1.currentKeyboard.IsKeyDown(Keys.Up) && !Game1.previousKeyboard.IsKeyDown(Keys.Up)) ||
                (Game1.currentGamePad.DPad.Up == ButtonState.Pressed && Game1.previousGamePad.DPad.Up == ButtonState.Released))
                currentOption = (currentOption - 1 + totalNumberOfOptions) % totalNumberOfOptions;
            else if (Game1.currentMouseState.X!= Game1.previousMouseState.X || Game1.currentMouseState.Y != Game1.previousMouseState.Y)
            {
                for (int i=0; i<totalNumberOfOptions; i++)
                {
                    if (positionOnScreen_options[i].Contains(new Point(Game1.currentMouseState.X/Game1.scale, Game1.currentMouseState.Y / Game1.scale)))
                        currentOption = i;
                }
            }
            //check if the player has confirmed the option
            if ((Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)) ||
                (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
            {
                Game1.currentGameState = followingGameState[currentOption];
                currentOption = 0;
            }
            else if (Game1.currentMouseState.LeftButton == ButtonState.Pressed && Game1.previousMouseState.LeftButton == ButtonState.Released)
            {
                if (positionOnScreen_options[currentOption].Contains(new Point(Game1.currentMouseState.X/Game1.scale, Game1.currentMouseState.Y / Game1.scale)))
                {
                    Game1.currentGameState = followingGameState[currentOption];
                    currentOption = 0;
                }
            }
            else if ((Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back)) || (Game1.currentGamePad.Buttons.B == ButtonState.Pressed && Game1.previousGamePad.Buttons.B == ButtonState.Released))
            {
                if (Game1.currentGameState!= previousGameState)
                {
                    Game1.currentGameState = previousGameState;
                    currentOption = 0;
                }
            }
        }
        public void Reset()
        {
            currentOption = 0;
        }
        public void Draw(SpriteBatch spriteBatch, float alphaValue=1)
        {
            spriteBatch.Draw(backgroungImage, new Vector2(0, 0), Color.White*alphaValue);
            for (int i=0; i<totalNumberOfOptions; i++)
            {
                spriteBatch.Draw(spritesheet_options, positionOnScreen_options[i], sourceRectangles_options[i], currentOption != i ? Color.White : Color.Red);
            }
        }
        #endregion
    }
}
