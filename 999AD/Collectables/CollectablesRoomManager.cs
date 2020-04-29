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
    class CollectablesRoomManager
    {
        #region DECLARATIONS
        List<Collectable> collectables;
        #endregion
        #region CONSTRUCTOR
        public CollectablesRoomManager(Collectable[] _collectables)
        {
            collectables = new List<Collectable>(_collectables);
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            for (int i= collectables.Count-1; i>=0; i--)
            {
                if (collectables[i].Collected)
                {
                    CollectablesManager.AddToInventory(collectables[i].type);
                    collectables.RemoveAt(i);
                }
                else
                    collectables[i].Update(elapsedTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = collectables.Count - 1; i >= 0; i--)
            {
                if (Camera.rectangle.Intersects(collectables[i].Rectangle))
                collectables[i].Draw(spriteBatch);
            }
        }
        #endregion
    }
}
