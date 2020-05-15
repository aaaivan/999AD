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
                        new Enemy1(new Vector2(150, 200), new Vector2(400, 200)),
                        new Enemy1( new Vector2(200, 200), new Vector2(500, 200)),
                        new Enemy1( new Vector2(250, 200), new Vector2(600, 200))
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
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // churchBellTower2
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // Midboss
                (
                    new Enemy1[]
                    {
                        //For testing purposes only
                        //new Enemy1(e1Spritesheet, new Vector2(150, 168)),
                        //new Enemy1(e1Spritesheet, new Vector2(200, 168)),
                        //new Enemy1(e1Spritesheet, new Vector2(250, 168))
                    },
                    new Enemy2[]
                    {
                        //For testing purposes only
                        //new Enemy2(e2Spritesheet, new Vector2(150,190)),
                        //new Enemy2(e2Spritesheet, new Vector2(250,190))
                    }
                ),
                new EnemyRoomManager // churchGroundFloor0
                (
                    new Enemy1[]
                    {
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
                    },
                    new Enemy2[]
                    {
                    }
                ),
                new EnemyRoomManager // church2ndFloor0
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
                    }
                ),

                new EnemyRoomManager // descent
                (
                    new Enemy1[]
                    {
                    },
                    new Enemy2[]
                    {
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