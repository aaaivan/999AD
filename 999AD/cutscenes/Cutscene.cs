using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    class Cutscene
    {
        #region DECLARATIONS
        static SpriteFont spriteFont;
        Animation animation;
        Texture2D animationSpritesheet;
        Vector2 animationPosition;
        string[] sentences;
        Vector2 stringPosition;
        static readonly float timeBetweenChars = 0.02f;
        float elapsedTimeBetweenChars;
        string stringDisplayed;
        int currentSentence;
        int currentLength;
        bool endOfSentence;
        public bool active;
        #endregion
        #region CONSTRUCTOR
        public Cutscene(Texture2D _animationSpritesheet, Animation _animation, Vector2 _animationPosition,
            Vector2 _stringPosition, string[] _sentences)
        {
            animationSpritesheet = _animationSpritesheet;
            animation = _animation;
            animationPosition = _animationPosition;
            stringPosition = _stringPosition;
            sentences = _sentences;
            elapsedTimeBetweenChars = 0;
            stringDisplayed = "";
            currentSentence = 0;
            currentLength = 0;
            endOfSentence = false;
            active = true;
        }
        public static void Initialize(SpriteFont _spriteFont)
        {
            spriteFont = _spriteFont;
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (!active)
                return;
            animation.Update(elapsedTime);
            if (currentSentence == sentences.Length)
            {
                active = false;
                currentSentence = 0;
                return;
            }
            if (endOfSentence)
            {
                if ((Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)) ||
                (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
                {
                    endOfSentence = false;
                    currentSentence++;
                    currentLength = 0;
                }
            }
            else
            {
                elapsedTimeBetweenChars += elapsedTime;
                if (elapsedTimeBetweenChars >= timeBetweenChars)
                {
                    currentLength++;
                    stringDisplayed = sentences[currentSentence].Substring(0, currentLength);
                    elapsedTimeBetweenChars = 0;
                    if (currentLength == sentences[currentSentence].Length)
                        endOfSentence = true;
                }
            }
        }
        public void ChangeSentence(int index, string newMessage)
        {
            sentences[index] = newMessage;
        }
        public int GetNUmOfSentences()
        {
            return sentences.Length;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animationSpritesheet, animationPosition, animation.Frame, Color.White);
            spriteBatch.DrawString(spriteFont, stringDisplayed, stringPosition, Color.White);
        }
        #endregion
    }
}
