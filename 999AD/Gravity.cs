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
    static class Gravity
    {
        #region DECLARATIONS
        public static float gravityAcceleration;
        static float wallFriction;
        static float airFriction;
        #endregion
        #region CONSTRUCTOR
        public static void Inizialize(float _gravityAcceleration, float _wallFriction, float _airFriction)
        {
            gravityAcceleration = _gravityAcceleration;
            wallFriction = _wallFriction;
            airFriction = _airFriction;
        }
        #endregion
    }
}
