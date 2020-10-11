using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class CutscenesManager
    {
        #region DECLARATIONS
        public enum CutsceneType
        {
            intro, ending, total
        }
        public static Cutscene[] cutscenes; //list of all cutscenes
        #endregion
        #region CONSTRUCTOR
        public static void Initialize(Texture2D enemySpritesheet, Texture2D playerSpritesheet, SpriteFont spriteFont)
        {
            Cutscene.Initialize(spriteFont);
            cutscenes = new Cutscene[(int)CutsceneType.total]
                {
                    new Cutscene(enemySpritesheet,
                                new Animation(new Rectangle(0,0,420,48),42,48,10,0.2f,true),
                                new Vector2(171,10),
                                new Vector2(10,84),
                                new string[]
                                {
                                    "\n\nNOTE: THIS GAME AUTOSAVES AT EVERY SCREEN TRANSITION." +
                                    "\n\n\n" +
                                    "                Press ENTER to start.",
                                    "Life starts, ends, and is accompanied by a great\n" +
                                    "many... mysteries. What concerns you right now,\n" +
                                    "Wandering Knight, is the end of said life,\n\n" +
                                    "                   ALL life.",
                                    "The year has turned to a most concerning of numbers,\n" +
                                    "999A.D., his most holy Pope Sylvester II - a man\n" +
                                    "of both great knowledge and great faith - fears that\n" +
                                    "such an auspicious date brings with it a threat to\n" +
                                    "all we hold dear.",
                                    "Missives, like flocks of paper birds have been\n" +
                                    "sent forth detailing rituals of evil at locations\n" +
                                    "left to fester in rot and injustice, rituals set to\n" +
                                    "bring foul creations never seen before to our\n" +
                                    "doorstep.",
                                    "You, Wandering Knight, know of one such that may\n"+
                                    "well have slipped the net, and seek to rectify past\n" +
                                    "wrongs before it is too late.",
                                }),
                    new Cutscene(playerSpritesheet,
                                new Animation( new Rectangle(0, 24, 160,24), 16, 24, 10, 0.06f, true),
                                new Vector2(184,10),
                                new Vector2(10,84),
                                new string[]
                                {
                                    "The cacophony of masonry and age exalts your exit\n" +
                                    "from the now-demolished church...A duty completed,\n" +
                                    "sunlight washes over your exhausted self.",
                                    "Wandering Knight, lost to history your actions may\n" +
                                    "become, perhaps your true penance is the thanks you\n" +
                                    "rightfully deserve, but you are no less for it.\n" +
                                    "There were more of these foul beasts, some may yet\n" +
                                    "have survived. Wander ever onwards, towards a\n" +
                                    "brighter tomorrow.",
                                    ""
                                }),
                };
        }
        #endregion
    }
}
