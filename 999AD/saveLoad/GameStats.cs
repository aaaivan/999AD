using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _999AD
{
    static class GameStats
    {
        #region DECLARATIONS
        static public float gameTime { get; private set; }
        static public int deathsCount;
        static public int hitsCount;
        #endregion
        #region CONSTRUCTOR
        static public void Inizialize(float _gameTime = 0.0f, int _deathsCount=0, int _hitsCount=0 )
        {
            gameTime = _gameTime;
            deathsCount = _deathsCount;
            hitsCount = _hitsCount;
        }
        #endregion
        #region METHODS
        static public void Update(float elapsedTime)
        {
            gameTime += elapsedTime;
        }
        static public string  GetTimeString()
        {
            int hh = (int)gameTime / 3600;
            int mm = ((int)gameTime / 60) % 60;
            int ss = (int)gameTime % 60;
            int decimals = (int)(gameTime * 1000) % 1000;
            string timeString = hh + ":" + mm + ":" + ss + "." + decimals;
            return timeString;
        }
        #endregion
    }
}
