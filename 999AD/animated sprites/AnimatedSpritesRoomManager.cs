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
    class AnimatedSpritesRoomManager
    {
        #region DECLARATIONS
        List<AnimatedSprite> animatedSprites;
        List<AnimatedSprite> temporaryAnimaterSprites;
        #endregion
        #region CONSTRUCTOR
        public AnimatedSpritesRoomManager(AnimatedSprite[] _animatedSprites)
        {
            animatedSprites = new List<AnimatedSprite>(_animatedSprites);
            temporaryAnimaterSprites= new List<AnimatedSprite>(_animatedSprites);
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            for (int i = animatedSprites.Count - 1; i >= 0; i--)
            {
                if (animatedSprites[i].Active)
                    animatedSprites[i].Update(elapsedTime);
                else
                    animatedSprites.RemoveAt(i);
            }
            for (int i =0; i< temporaryAnimaterSprites.Count; i++)
            {
                    temporaryAnimaterSprites[i].Update(elapsedTime);
            }

        }
        public void AddAnimatedSprite(AnimatedSprite animatedSprite)
        {
            animatedSprites.Add(animatedSprite);
        }
        public void ClearTemporaryAnimatedSprites()
        {
            temporaryAnimaterSprites.Clear();
        }
        public void AddTemporaryAnimatedSprites(List<AnimatedSprite> animatedSprites)
        {
            if (temporaryAnimaterSprites.Count > 0)
                return;
            temporaryAnimaterSprites = animatedSprites;
        }
        public void DrawInFront(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite animatedSprite in animatedSprites)
                if (animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
            foreach (AnimatedSprite animatedSprite in temporaryAnimaterSprites)
                if (animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
        }
        public void DrawOnTheBack(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite animatedSprite in animatedSprites)
                if (!animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
            foreach (AnimatedSprite animatedSprite in temporaryAnimaterSprites)
                if (!animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
        }
        #endregion
    }
}
