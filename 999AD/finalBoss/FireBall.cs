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
    class FireBall
    {
        #region DECLARATIONS
        static Texture2D spritesheet;
        public static readonly int size = 10;
        public static readonly float radialShootingVelocity = 600;
        public static readonly float shotLifeTime = 3;
        Vector2 rotationCenter;
        Animation animation;
        float radialDistance;
        float angleRadiants;
        public float[] radialVelocities;
        public float[] angularVelocities;
        float[] lifeTimes; //seconds
        int index = 0;
        float elapsedLifeTime = 0;
        bool targetPlayer;
        bool active=true;
        #endregion
        #region CONSTRUCTOR
        public FireBall(float _angleDegrees, float[] _radialVelocities, float[] _angularVelocities, float[] _lifeTimes,
            bool _targetPlayer = false, float centerOffsetX = 0, float centerOffsetY = 0, float _radialDistance = 0)
        {
            rotationCenter = FireBallsManager.fireballsCenter+new Vector2(centerOffsetX, centerOffsetY);
            radialDistance = _radialDistance;
            angleRadiants = _angleDegrees / 180*(float)Math.PI;
            if (angleRadiants >= MathHelper.Pi * 2)
                angleRadiants -= MathHelper.Pi * 2;
            else if (angleRadiants < 0)
                angleRadiants += MathHelper.Pi * 2;
            radialVelocities = _radialVelocities;
            angularVelocities = _angularVelocities;
            lifeTimes = _lifeTimes;
            targetPlayer = _targetPlayer;
            animation= new Animation(new Rectangle(0,0,spritesheet.Width, spritesheet.Height), size, size, spritesheet.Width / size, 0.2f, true);
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
        }
        #endregion
        #region PRPERTIES
        public bool Active
        {
            get { return active; }
        }
        public Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)(Center.X-size*0.5f), (int)(Center.Y - size * 0.5f), size, size); }
        }
        Vector2 Center
        {
            get { return rotationCenter + radialDistance * new Vector2((float)Math.Sin(angleRadiants), -(float)Math.Cos(angleRadiants)); }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            animation.Update(elapsedTime);
            if (elapsedLifeTime > lifeTimes[index])
            {
                    elapsedLifeTime = 0;
                    index++;
                    if (index== lifeTimes.Length)
                        active = false;
                    return;
            }
            else
            {
                elapsedLifeTime += elapsedTime;
                radialDistance += radialVelocities[index] * elapsedTime;
                angleRadiants += angularVelocities[index] * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants < 0)
                    angleRadiants += MathHelper.Pi * 2;
                if (targetPlayer && index== lifeTimes.Length-2 &&
                    checkPlayerAngle())
                {
                    targetPlayer = false;
                    index = 0;
                    elapsedLifeTime = 0;
                    angularVelocities= new float[] { 0 };
                    radialVelocities = new float[] {radialShootingVelocity};
                    lifeTimes = new float[] { shotLifeTime };
                }


            }
        }
        bool checkPlayerAngle()
        {
            Vector2 v = Player.Center-rotationCenter;
            v.Normalize();
            float playerAngle = (float)Math.Acos(Vector2.Dot(v, new Vector2(0, -1)));
            playerAngle = v.X > 0 ? playerAngle : MathHelper.Pi * 2 - playerAngle;
            if (Math.Abs(playerAngle - angleRadiants) < 5f / 180 * Math.PI)
                return true;
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, Camera.RelativeRectangle(new Vector2(Center.X - size * 0.5f, Center.Y - size * 0.5f), size, size), animation.Frame, Color.White);
        }
        #endregion
    }
}
