using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class MenusManager
    {
        #region DECLARATIONS
        public enum MenuType
        {
            titleScreen, controls, achievements, credits, pause, confirmQuit, doubleJump, wallJump,  total
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
                                new Rectangle(0,0,160,24),  //new game
                                new Rectangle(0,216,160,24),  //load game
                                new Rectangle(0,24,160,24), //controls
                                new Rectangle(0,240,160,24), //achievements
                                new Rectangle(0,48,160,24), //credits
                                new Rectangle(0,72,160,24), //quit
                            },
                            new Game1.GameStates[]
                            {
                                Game1.GameStates.intro,
                                Game1.GameStates.loadGame,
                                Game1.GameStates.controls,
                                Game1.GameStates.achievements,
                                Game1.GameStates.credits,
                                Game1.GameStates.quit,
                            },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]
                            {
                                new Rectangle(112,64,160,24),
                                new Rectangle(112,88,160,24),
                                new Rectangle(112,112,160,24),
                                new Rectangle(112,136,160,24),
                                new Rectangle(112,160,160,24),
                                new Rectangle(112,184,160,24),
                            }),
                new Menu(backgrounds[1],//controls
                            new Rectangle[]{new Rectangle(20, 192, 48, 24) },
                            new Game1.GameStates[]{Game1.GameStates.titleScreen },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]{ new Rectangle(24, 184, 48, 24) }),
                new Menu(backgrounds[7],//achievements
                            new Rectangle[]{new Rectangle(20, 192, 48, 24) },
                            new Game1.GameStates[]{Game1.GameStates.titleScreen },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]{ new Rectangle(24, 184, 48, 24) }),
                new Menu(backgrounds[2],//credits
                            new Rectangle[]{new Rectangle(20, 192, 48, 24) },
                            new Game1.GameStates[]{Game1.GameStates.titleScreen },
                            Game1.GameStates.titleScreen,
                            new Rectangle[]{ new Rectangle(24, 184, 48, 24) }),
                new Menu(backgrounds[3],//pause
                            new Rectangle[]
                            {
                                new Rectangle(0, 96, 160, 24),
                                new Rectangle(0, 120, 160, 24),
                            },
                            new Game1.GameStates[]
                            {
                                Game1.GameStates.playing,
                                Game1.GameStates.confirmQuit
                            },
                            Game1.GameStates.playing,
                            new Rectangle[]
                            {
                                new Rectangle(112,88,160,24),
                                new Rectangle(112,124,160,24),
                            }),
                new Menu(backgrounds[4],//confirmQuit
                            new Rectangle[]
                            {
                                new Rectangle (44, 168, 72, 24),
                                new Rectangle (44, 144, 72, 24),
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
                new Menu(backgrounds[5],
                            new Rectangle[]{new Rectangle(0, 96, 160, 24) },
                            new Game1.GameStates[]{ Game1.GameStates.playing},
                            Game1.GameStates.playing,
                            new Rectangle[]{ new Rectangle(112,124, 160, 24) }
                            ),
                new Menu(backgrounds[6],
                            new Rectangle[]{new Rectangle(0, 96, 160, 24) },
                            new Game1.GameStates[]{ Game1.GameStates.playing},
                            Game1.GameStates.playing,
                            new Rectangle[]{ new Rectangle(112,124, 160, 24) }
                            ),
            };
        }
        #endregion

    }
}
