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
        List<MovingPlatform> movingPlatforms = new List<MovingPlatform>();
        List<RotatingPlatform> rotatingPlatforms = new List<RotatingPlatform>();
        #endregion
        #region CONSTRUCTOR
        public PlatformsRoomManager(MovingPlatform[] _movingPlatforms, RotatingPlatform[] _rotatingPlatforms)
        {
            foreach (MovingPlatform platform in _movingPlatforms)
                movingPlatforms.Add(platform);
            foreach (RotatingPlatform platform in _rotatingPlatforms)
                rotatingPlatforms.Add(platform);
        }
        #endregion
        #region METHODS
        public void Update(GameTime gameTime)
        {
            foreach (MovingPlatform platform in movingPlatforms)
                platform.Update(gameTime);
            foreach (RotatingPlatform platform in rotatingPlatforms)
                platform.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform platform in movingPlatforms)
                platform.Draw(spriteBatch);
            foreach (RotatingPlatform platform in rotatingPlatforms)
                platform.Draw(spriteBatch);
        }
        #endregion

    }
}
