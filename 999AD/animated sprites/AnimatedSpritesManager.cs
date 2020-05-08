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
    static class AnimatedSpritesManager
    {
        #region DECLARATIONS
        public static AnimatedSpritesRoomManager[] animatedSpritesRoomManagers;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize (Texture2D spritesheet)
        {
            AnimatedSprite.Inizialize(spritesheet);
            animatedSpritesRoomManagers = new AnimatedSpritesRoomManager[(int)RoomsManager.Rooms.total]
            {
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //tutorial0
                {
                    new AnimatedSprite(new Vector2(100,100), AnimatedSprite.SpriteType.loading)
                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //tutorial1
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //tutorial2
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //tutorial3
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //tutorial4
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //church
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //finalBoss
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //escape0
                {

                }),
                new AnimatedSpritesRoomManager(new AnimatedSprite[] //escape1
                {

                })
            };
        }
        #endregion
    }
}
