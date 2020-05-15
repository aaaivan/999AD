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
        List<Collectable> heartsBackUp;
        #endregion
        #region CONSTRUCTOR
        public CollectablesRoomManager(Collectable[] _collectables)
        {
            collectables = new List<Collectable>(_collectables);
            heartsBackUp = new List<Collectable>();
            foreach(Collectable collectable in collectables)
                if (collectable.type== Collectable.ItemType.heart)
                    heartsBackUp.Add(collectable.DeepCopy());
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            for (int i = collectables.Count - 1; i >= 0; i--)
            {
                if (collectables[i].Collected)
                {
                    if (collectables[i].type == Collectable.ItemType.heart)
                        Player.IncreaseHealth();
                    else
                        CollectablesManager.AddToInventory(collectables[i]);
                    collectables.RemoveAt(i);
                }
                else
                    collectables[i].Update(elapsedTime);
            }
        }
        public void AddCollectableToMap(Collectable collectable)
        {
            collectables.Add(collectable);
        }
        public void ResetHearts()
        {
            for (int i=collectables.Count-1; i>=0; i--)
            {
                if (collectables[i].type == Collectable.ItemType.heart)
                    collectables.RemoveAt(i);
            }
            foreach (Collectable heart in heartsBackUp)
            {
                collectables.Add(heart.DeepCopy());
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = collectables.Count - 1; i >= 0; i--)
            {
                if (Camera.Rectangle.Intersects(collectables[i].rectangle))
                collectables[i].Draw(spriteBatch);
            }
        }
        #endregion
    }
}
