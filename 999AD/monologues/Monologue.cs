using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    class Monologue
    {
        #region DECLARATIONS
        public static Texture2D spritesheet;
        public static Rectangle sourceRectangle_dialogueBox;
        public static Rectangle sourceRectangle_arrow;
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
        #endregion
        #region CONSTRUCTOR
        public Monologue(Rectangle _interactionRectangle, string[] _sentences)
        {
            elapsedTimeBetweenChars = 0;
            stringDisplayed = "";
            currentSentence = 0;
            currentLength = 0;
            endOfSentence = false;
            active = false;
            interactionRectangle = _interactionRectangle;
            sentences = _sentences;
        }
        public static void Inizialize(Texture2D _spritesheet, SpriteFont _spriteFont)
        {
            spritesheet = _spritesheet;
            spriteFont = _spriteFont;
            sourceRectangle_dialogueBox = new Rectangle(24, 173, 360, 48);
            sourceRectangle_arrow = new Rectangle(384, 173, 8, 8);
            boxRectangle= new Rectangle((Game1.min_gameWidth - sourceRectangle_dialogueBox.Width) / 2, 10, sourceRectangle_dialogueBox.Width, sourceRectangle_dialogueBox.Height);
            stringPosition = new Vector2(boxRectangle.X + 50, boxRectangle.Y + 4);
            arrowRectangle = new Rectangle(boxRectangle.Right-4- sourceRectangle_arrow.Width, boxRectangle.Bottom - 4 - sourceRectangle_arrow.Height, sourceRectangle_arrow.Width, sourceRectangle_arrow.Height);
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (!active)
                return;
            if (currentSentence == sentences.Length)
            {
                active = false;
                currentSentence = 0;
                return;
            }
            if (endOfSentence)
            {
                if (Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter) ||
                (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
                {
                    endOfSentence = false;
                    currentSentence++;
                    currentLength = 0;
                }
            }
            else
            {
                if ((Game1.currentKeyboard.IsKeyDown(Keys.Enter) && !Game1.previousKeyboard.IsKeyDown(Keys.Enter)) ||
                (Game1.currentGamePad.Buttons.A == ButtonState.Pressed && Game1.previousGamePad.Buttons.A == ButtonState.Released))
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
            spriteBatch.Draw(spritesheet, boxRectangle,sourceRectangle_dialogueBox , Color.White);
            spriteBatch.DrawString(spriteFont, stringDisplayed, stringPosition, Color.White);
            if (currentSentence != sentences.Length - 1)
                spriteBatch.Draw(spritesheet, arrowRectangle,sourceRectangle_arrow, Color.White);
        }
        #endregion
    }
}
