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
        MovingPlatform[] backUpMovingPlatforms;
        #endregion
        #region CONSTRUCTOR
        public PlatformsRoomManager(MovingPlatform[] _movingPlatforms)
        {
            movingPlatforms = _movingPlatforms;
            backUpMovingPlatforms = new MovingPlatform[movingPlatforms.Length];
            for (int i = 0; i < movingPlatforms.Length; i++)
            {
                backUpMovingPlatforms[i] = movingPlatforms[i].DeepCopy();
            }
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
        public void Reset()
        {
            for (int i = 0; i < movingPlatforms.Length; i++)
                movingPlatforms[i] = backUpMovingPlatforms[i].DeepCopy();
        }
        #endregion
    }
}
