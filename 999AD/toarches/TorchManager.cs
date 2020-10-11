using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    static class TorchManager
    {
        #region DECLARATIONS
        public static List<Torch> torches; //list of all toarches in the churchAltarRoom
        #endregion
        #region CONSTRUCTOR
        static public void Initialize(Texture2D spritesheet)
        {
            Torch.Initialize(spritesheet,
                new Animation(new Rectangle(0, 0, 128, 32), 16, 32, 8, 0.1f, true),
                new Animation(new Rectangle(0, 32, 128, 32), 16, 32, 1, 0, false, true));
            torches = new List<Torch>();
            torches.Add(new Torch(new Vector2(46,440)));
            torches.Add(new Torch(new Vector2(230,300)));

            torches.Add(new Torch(new Vector2(160, 220)));
            torches.Add(new Torch(new Vector2(300, 220)));
            torches.Add(new Torch(new Vector2(160, 380)));
            torches.Add(new Torch(new Vector2(300, 380)));

            torches.Add(new Torch(new Vector2(230, 160)));
            torches.Add(new Torch(new Vector2(230, 440)));
            torches.Add(new Torch(new Vector2(130, 300)));
            torches.Add(new Torch(new Vector2(330, 300)));
        }
        #endregion
        #region METHODS
        static public void Update(float elapsedTime)
        {
            if (RoomsManager.CurrentRoom != RoomsManager.Rooms.churchAltarRoom)
                return;
            foreach (Torch t in torches)
                t.Update(elapsedTime);
        }

        //check whether all toarches are unlit
        static public bool AllTorchesUnlit()
        {
            foreach (Torch t in torches)
                if (t.isLit)
                    return false;
            return true;
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            if (RoomsManager.CurrentRoom != RoomsManager.Rooms.churchAltarRoom)
                return;
            foreach (Torch t in torches)
                t.Draw(spriteBatch);
        }
        #endregion

    }
}
