using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    static class Achievements
    {
        #region DECLARATIONS
        static SpriteFont spriteFont8;
        static SpriteFont spriteFont12;
        static public bool gameCompleted;
        static public bool noDeath;
        static public bool noHits;
        static public float bestTime;
        #endregion
        #region CONSTRUCTOR
        static public void Initialize(SpriteFont _spriteFont8, SpriteFont _spriteFont12)
        {
            spriteFont8 = _spriteFont8;
            spriteFont12 = _spriteFont12;
            LoadSaveManager.LoadHighScores();
        }
        static public void UpdateAchievements(bool _gameCompleted, bool _noDeaths, bool _noHits, float _bestTime)
        {
            gameCompleted = _gameCompleted;
            noDeath = _noDeaths;
            noHits = _noHits;
            bestTime = _bestTime;
        }
        #endregion
        #region METHODS
        static public void Draw(SpriteBatch spriteBatch)
        {
            int hh = (int)bestTime / 3600;
            int mm = ((int)bestTime / 60) % 60;
            int ss = (int)bestTime % 60;
            int decimals = (int)(bestTime * 1000) % 1000;
            string timeString = (bestTime == 0 ? ("-:-:-") : (hh + ":" + mm + ":" + ss + "." + decimals));
            Vector2 stringSize = spriteFont12.MeasureString(timeString);
            spriteBatch.DrawString(spriteFont8,
                gameCompleted ? "COMPLETED" : "NOT COMPLETED",
                new Vector2(200, 59), Color.White);
            spriteBatch.DrawString(spriteFont8,
                noDeath ? "COMPLETED" : "NOT COMPLETED",
                new Vector2(200, 81), Color.White);
            spriteBatch.DrawString(spriteFont8,
                noHits ? "COMPLETED" : "NOT COMPLETED",
                new Vector2(200, 103), Color.White);
            spriteBatch.DrawString(spriteFont12,
                timeString,
                new Vector2(Game1.gameWidth/2-stringSize.X/2, 160), Color.White);

        }
        #endregion
    }
}
