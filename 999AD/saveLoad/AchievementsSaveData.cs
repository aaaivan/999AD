using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    [Serializable]
    class AchievementsSaveData
    {
        #region DECLARATIONS
        public bool gameCompleted;
        public bool noDeath;
        public bool noHits;
        public float bestTime;
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
