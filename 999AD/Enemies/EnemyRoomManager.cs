using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class EnemyRoomManager
    {
        #region DECLARATIONS
        public Enemy1[] enemiesType1; // Enemies of Type 1 in a certain room
        public Enemy2[] enemiesType2; // Enemies of Type 2 in a certain room
        #endregion

        #region CONSTRUCTOR
        //Constructor for Enemy Room Manager
        //Takes a list of Enemy of Type 1 as a parameter
        public EnemyRoomManager(Enemy1[] EnemiesType1, Enemy2[] EnemiesType2)
        {
            enemiesType1 = EnemiesType1;
            enemiesType2 = EnemiesType2;
        }
        #endregion

        #region METHODS
        //Update Function
        public void Update(float elapsedTime)
        {
            //Updating each Enemy 1 in the list
            foreach (Enemy1 enemy in enemiesType1)
            {
                enemy.Update(elapsedTime);
            }

            //Updating each Enemy 2 in the list
            foreach(Enemy2 enemy in enemiesType2)
            {
                enemy.Update(elapsedTime);
            }
        }

        //Draw Function
        public void Draw(SpriteBatch spriteBatch)
        {
            //Drawing each Enemy 1 in the list if in view
            foreach (Enemy1 enemy in enemiesType1)
            {
                if(enemy.Enemy1DrawRect.Intersects(Camera.Rectangle))
                {
                    enemy.Draw(spriteBatch);
                }
            }

            //Drawing each Enemy 2 in the list if in view
            foreach (Enemy2 enemy in enemiesType2)
            {
                if(enemy.Enemy2DrawRect.Intersects(Camera.Rectangle))
                {
                    enemy.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
