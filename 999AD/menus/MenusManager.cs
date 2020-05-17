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
    static class MenusManager
    {
        #region DECLARATIONS
        public enum MenuType
        {
            titleScreen, controls, credits, pause, confirmQuit, total
        }
        public static Menu[] menus;
        #endregion
        #region COSTRUCTOR
        public static void Initialize(Texture2D spritesheet_options, Texture2D[] backgrounds)
        {
            Menu.Initialize(spritesheet_options);
            menus = new Menu[(int)MenuType.total]
            {
                new Menu(backgrounds[0],//title screen
                            new Rectangle[]
                            {
                                new Rectangle(0,0,120,24),  //new game
                                new Rectangle(0,24,120,24), //controls
                                new Rectangle(0,48,120,24), //credits
                                new Rectangle(0,72,120,24), //quit
                            },
                            new Game1.GameStates[]
                            {
                                Game1.GameStates.intro,
                                Game1.GameStates.controls,
                                Game1.GameStates.credits,
                                Game1.GameStates.quit,
                            },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]
                            {
                                new Rectangle(132,100,120,24),
                                new Rectangle(132,124,120,24),
                                new Rectangle(132,148,120,24),
                                new Rectangle(132,172,120,24),
                            }),
                new Menu(backgrounds[1],//controls
                            new Rectangle[]{new Rectangle(0, 192, 48, 24) },
                            new Game1.GameStates[]{Game1.GameStates.titleScreen },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]{ new Rectangle(24, 184, 48, 24) }),
                new Menu(backgrounds[2],//credits
                            new Rectangle[]{new Rectangle(0, 192, 48, 24) },
                            new Game1.GameStates[]{Game1.GameStates.titleScreen },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]{ new Rectangle(24, 184, 48, 24) }),
                new Menu(backgrounds[3],//pause
                            new Rectangle[]
                            {
                                new Rectangle(0, 96, 120, 24),
                                new Rectangle(0, 120, 120, 24),
                            },
                            new Game1.GameStates[]
                            {
                                Game1.GameStates.playing,
                                Game1.GameStates.confirmQuit
                            },
                            Game1.GameStates.playing,
                            new Rectangle[]
                            {
                                new Rectangle(132,124,120,24),
                                new Rectangle(132,148,120,24),
                            }),
                new Menu(backgrounds[4],//confirmQuit
                            new Rectangle[]
                            {
                                new Rectangle (24, 168, 72, 24),
                                new Rectangle (24, 144, 72, 24),
                            },
                            new Game1.GameStates[]
                            {
                                Game1.GameStates.pause,
                                Game1.GameStates.titleScreen,
                            },
                            Game1.GameStates.pause,
                            new Rectangle[]
                            {
                                new Rectangle(156,124,72,24), //no
                                new Rectangle(156,148,72,24), //yes
                            }),
            };
        }
        #endregion

    }
}
