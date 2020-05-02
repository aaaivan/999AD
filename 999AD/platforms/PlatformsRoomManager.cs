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
        public MovingPlatform[] movingPlatforms; //moving platforms in a certain room
        #endregion
        #region CONSTRUCTOR
        public PlatformsRoomManager(MovingPlatform[] _rotatingPlatforms)
        {
            movingPlatforms = _rotatingPlatforms;
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            foreach (MovingPlatform platform in movingPlatforms)
                platform.Update(elapsedTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform platform in movingPlatforms)
            {
                if (platform.Rectangle.Intersects(Camera.Rectangle))
                    platform.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
