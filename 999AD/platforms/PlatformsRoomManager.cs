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
    class PlatformsRoomManager
    {
        #region DECLARATIONS
        public MovingPlatform[] rotatingPlatforms; //rotating platforms in a certain room
        #endregion
        #region CONSTRUCTOR
        public PlatformsRoomManager(MovingPlatform[] _rotatingPlatforms)
        {
            rotatingPlatforms = _rotatingPlatforms;
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            foreach (MovingPlatform platform in rotatingPlatforms)
                platform.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform platform in rotatingPlatforms)
            {
                if (platform.Rectangle.Intersects(Camera.rectangle))
                    platform.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
