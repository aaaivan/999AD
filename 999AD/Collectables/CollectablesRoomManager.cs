﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class CollectablesRoomManager
    {
        #region DECLARATIONS
        public List<Collectable> collectables { get; private set; } //collectables in the current room
        List<Collectable> heartsBackUp; //copies of all the hearts collectables
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

        public void RemoveCollectablesFromMap()
        {
            for (int i=0; i<collectables.Count;)
            {
                if(collectables[i].type==Collectable.ItemType.heart)
                {
                    i++;
                    continue;
                }
                collectables.RemoveAt(i);
            }
        }

        //restore all the hearts in the current room
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
