using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class DoorsRoomManager
    {
        #region DECLARATIONS
        public List<Door> doors;
        #endregion
        #region CONSTRUCTOR
        public DoorsRoomManager(Door[] _doors)
        {
            doors = new List<Door> (_doors);
        }
        #endregion
        #region METHODS
        public void Update()
        {
            for(int i= doors.Count-1; i>=0; i--)
            {
                if (doors[i].Closed)
                    doors[i].Update();
                else
                    doors.RemoveAt(i);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Door door in doors)
                door.Draw(spriteBatch);
        }
        #endregion
    }
}
