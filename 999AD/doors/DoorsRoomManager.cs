using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class DoorsRoomManager
    {
        #region DECLARATIONS
        public List<Door> doors; //list of doors in the current room
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
