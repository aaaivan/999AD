using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class MonologuesManager
    {
        #region DECLARATIONS
        public static MonologuesRoomManager[] monologuesRoomManagers;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(Texture2D spritesheet, SpriteFont spriteFont)
        {
            Monologue.Inizialize(spritesheet, spriteFont);
            MonologuesRoomManager.Inizialize(spritesheet);
            monologuesRoomManagers = new MonologuesRoomManager[(int)RoomsManager.Rooms.total]
            {
                new MonologuesRoomManager(  //tutorial0
                    new Monologue[]
                    {
                        new Monologue(new Rectangle(0,0,0,0),
                            new string[]
                            {
                                "A forsaken place, one of disgrace and\n" +
                                "mistakes. I came here wielding a sword,\n" +
                                "and left unarmed.",
                                "Never again shall I use such weapons, \n" +
                                "but nevertheless, I have skills I shall \n" +
                                "have to re-learn to aid me."
                            }),
                        new Monologue(new Rectangle(0,0,0,0),
                            new string[]
                            {
                                "Perhaps...\n" +
                                "A sword would have made things easier..."
                            }),
                    }),
                new MonologuesRoomManager(  //tutorial1
                    new Monologue[]
                    {
                        new Monologue(new Rectangle(592, 194, 16, 16),
                            new string[]
                            {
                                "\"Not all paths lead to the right way,\n"+
                                "yet the right way is the one with the\n" +
                                "most danger.",
                                "You will become a great saviour to\n" +
                                "mankind, but your praises will remain\n" +
                                "unsung.",
                                "If you still seek to redeem yourself,\n"+
                                "you will undoubtedly face the fourth\n" +
                                "horseman.\"",
                                "I wonder what this means..."
                            })
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
                        new Monologue(new Rectangle(193, 923, 87, 55),
                            new string[]
                            {
                                "Mmmh... Is seems like something would\n" +
                                "fit inside the two holes on this altar...",
                            }),
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
                new MonologuesRoomManager(  //descent
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
                        new Monologue(new Rectangle(0,0,0,0),
                            new string[]
                            {
                                "This place is coming down.\n" +
                                "I'd better hurry before I get stuck\n" +
                                "here."
                            })
                    }),
                new MonologuesRoomManager(  //escape1
                    new Monologue[]
                    {
                    }),
                new MonologuesRoomManager(  //escape2
                    new Monologue[]
                    {
                    }),
            };
        }
        #endregion
        #region PROPERTIES
        public static bool MonologuePlaying
        {
            get { return monologuesRoomManagers[(int)RoomsManager.CurrentRoom].IndexPlaying != -1; }
        }
        #endregion
    }
}