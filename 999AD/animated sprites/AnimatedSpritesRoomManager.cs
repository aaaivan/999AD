using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class AnimatedSpritesRoomManager
    {
        #region DECLARATIONS
        List<AnimatedSprite> tempAnimatedSprites; //animated sprites destroyed at the end of the animation
        List<AnimatedSprite> animaterSprites; //animated sprites added and destroyed manually
        #endregion
        #region CONSTRUCTOR
        public AnimatedSpritesRoomManager(AnimatedSprite[] _animatedSprites)
        {
            tempAnimatedSprites = new List<AnimatedSprite>(_animatedSprites);
            animaterSprites= new List<AnimatedSprite>(_animatedSprites);
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            for (int i = tempAnimatedSprites.Count - 1; i >= 0; i--)
            {
                if (tempAnimatedSprites[i].Active)
                    tempAnimatedSprites[i].Update(elapsedTime);
                else
                    tempAnimatedSprites.RemoveAt(i);
            }
            for (int i =0; i< animaterSprites.Count; i++)
            {
                    animaterSprites[i].Update(elapsedTime);
            }
        }
        public void AddTempAnimatedSprite(AnimatedSprite animatedSprite)
        {
            tempAnimatedSprites.Add(animatedSprite);
        }
        public void ClearAnimatedSprites()
        {
            animaterSprites.Clear();
        }
        public void AddAnimatedSprites(List<AnimatedSprite> animatedSprites)
        {
            animaterSprites = animatedSprites;
        }

        //Draw in front of the character
        public void DrawInFront(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite animatedSprite in tempAnimatedSprites)
                if (animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
            foreach (AnimatedSprite animatedSprite in animaterSprites)
                if (animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
        }

        //draw behind the character
        public void DrawOnTheBack(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite animatedSprite in tempAnimatedSprites)
                if (!animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
            foreach (AnimatedSprite animatedSprite in animaterSprites)
                if (!animatedSprite.DrawInFront)
                    animatedSprite.Draw(spriteBatch);
        }
        #endregion
    }
}
