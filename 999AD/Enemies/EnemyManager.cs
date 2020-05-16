using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace _999AD
{
    static class EnemyManager
    {
        #region DECLARATIONS
        public static EnemyRoomManager[] enemyRoomManagers; //List of Enemy Managers for each room

        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy Room Manager
        //Takes a spritesheet as a parameter
        public static void Initialise(Texture2D e1Spritesheet, Texture2D e2Spritesheet)
        {
            Enemy1.Inizialize(e1Spritesheet);
            Enemy2.Inizialize(e2Spritesheet);
            Reset();
        }
        public static void Reset()
        {
            enemyRoomManagers = new EnemyRoomManager[(int)RoomsManager.Rooms.total]
            {
                new EnemyRoomManager //Tutorial 0
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // Tutorial 1
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // Tutorial 2
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // Tutorial 3
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(200, 192), new Vector2(500, 192)),
                        new Enemy1(new Vector2(250, 87), new Vector2(450, 87)),
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // Tutorial 4
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // churchBellTower0
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // churchBellTower1
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(130,120), new Vector2(230,120)),
                        new Enemy1(new Vector2(110,616), new Vector2(220,616))
                    },
                    new Enemy2[]
                    {
                        new Enemy2(new Vector2(100,937), new Vector2(125,937))
                    }
                ),
                new EnemyRoomManager // churchBellTower2
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                        new Enemy2(new Vector2(120,1177), new Vector2(400,1177))
                    }
                ),
                new EnemyRoomManager // Midboss
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // churchGroundFloor0
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(250,400), new Vector2(375,400)),
                        new Enemy1(new Vector2(875,375), new Vector2(925,375)),
                        new Enemy1(new Vector2(80,304), new Vector2(180,304))
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // churchAltarRoom
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // church1stFloor0
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(860,200), new Vector2(985,200))
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // church2ndFloor0
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(1125,344), new Vector2(1275,344))
                    },
                    new Enemy2[]
                    {
                        new Enemy2(new Vector2(1150,441), new Vector2(1300,441))
                    }
                ),

                new EnemyRoomManager // descent
                (
                    new Enemy1[]
                    {
                        new Enemy1(new Vector2(155,120), new Vector2(320,120)),
                        new Enemy1(new Vector2(170,232), new Vector2(170,232))
                    },
                    new Enemy2[]
                    {
                        new Enemy2(new Vector2(220,232), new Vector2(320,232))
                    }
                ),

                new EnemyRoomManager // Final Boss
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // Escape 0
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // Escape 1
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // Escape 2
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
            };
        }
        #endregion
    }
}