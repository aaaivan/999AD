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
                        //new Enemy2(e2Spritesheet, new Vector2(150,169)),
                        //new Enemy2(e2Spritesheet, new Vector2(250,169))
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
            };
        }

        #endregion
    }
}