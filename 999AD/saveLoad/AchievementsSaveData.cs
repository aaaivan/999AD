using System;

namespace _999AD
{
    [Serializable]
    class AchievementsSaveData
    {
        #region DECLARATIONS
        public bool gameCompleted; //indicated whether the game has been ever completed
        public bool noDeath; //indicates whether the player has not died
        public bool noHits; //indicates whether the player has taken no hits
        public float bestTime; //store the best game time
        #endregion
        #region CONSTRUCTOR
        public AchievementsSaveData()
        {
            gameCompleted = false;
            noDeath = false;
            noHits = false;
            bestTime = 0;
        }
        public AchievementsSaveData(bool _gameCompleted, bool _noDeaths, bool _noHits, float _bestTime)
        {
            gameCompleted = _gameCompleted;
            noDeath = _noDeaths;
            noHits = _noHits;
            bestTime = _bestTime;
        }
        #endregion
        #region METHODS
        public void ApplySaveData()
        {
            Achievements.gameCompleted = gameCompleted;
            Achievements.noDeath = noDeath;
            Achievements.noHits = noHits;
            Achievements.bestTime = bestTime;
        }
        #endregion
    }
}
