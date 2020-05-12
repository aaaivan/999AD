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
    class Monologue
    {
        #region DECLARATIONS
        public static Texture2D dialogueBox;
        public static Texture2D arrow;
        public static SpriteFont spriteFont;
        static Rectangle boxRectangle;
        static Vector2 stringPosition;
        static Rectangle arrowRectangle;
        static readonly float timeBetweenChars = 0.05f;
        public readonly Rectangle interactionRectangle;
        float elapsedTimeBetweenChars;
        string[] sentences;
        string stringDisplayed;
        int currentSentence;
        int currentLength;
        bool endOfSentence;
        public bool active;
        bool playAutomatically;
        #endregion
        #region CONSTRUCTOR
        public Monologue(Rectangle _interactionRectangle, string[] _sentences, bool _playAutomatically=false)
        {
            elapsedTimeBetweenChars = 0;
            stringDisplayed = "";
            currentSentence = 0;
            currentLength = 0;
            endOfSentence = false;
            active = false;
            interactionRectangle = _interactionRectangle;
            sentences = _sentences;
            playAutomatically = _playAutomatically;
        }
        public static void Inizialize(Texture2D _dialogueBox, Texture2D _arrow, SpriteFont _spriteFont)
        {
            dialogueBox = _dialogueBox;
            spriteFont = _spriteFont;
            arrow = _arrow;
            boxRectangle= new Rectangle((Game1.gameWidth - dialogueBox.Width) / 2, 10, dialogueBox.Width, dialogueBox.Height);
            stringPosition = new Vector2(boxRectangle.X + 50, boxRectangle.Y + 4);
            arrowRectangle = new Rectangle(boxRectangle.Right-4-arrow.Width, boxRectangle.Bottom - 4 - arrow.Height, arrow.Width, arrow.Height);
        }
        #endregion
        #region PROPERTIES
        public bool PlayAutomatically
        {
            get { return playAutomatically; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (currentSentence == sentences.Length)
            {
                active = false;
                currentSentence = 0;
                return;
            }
            if (endOfSentence)
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter))
                {
                    endOfSentence = false;
                    currentSentence++;
                    currentLength = 0;
                }
            }
            else
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter))
                {
                    currentLength = sentences[currentSentence].Length;
                    stringDisplayed = sentences[currentSentence].Substring(0, currentLength);
                    elapsedTimeBetweenChars = 0;
                    endOfSentence = true;
                    return;
                }
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
        public Rectangle InteractSymbolLocation(int width, int height)
        {
            return new Rectangle
                (
                    interactionRectangle.X +interactionRectangle.Width/2- width / 2,
                    interactionRectangle.Y - height,
                    width,
                    height
                );
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(dialogueBox, boxRectangle, Color.White);
            spriteBatch.DrawString(spriteFont, stringDisplayed, stringPosition, Color.White);
            if (currentSentence != sentences.Length - 1)
                spriteBatch.Draw(arrow, arrowRectangle, Color.White);
        }
        #endregion
    }
}
