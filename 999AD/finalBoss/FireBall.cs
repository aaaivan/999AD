using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _999AD
{
    class FireBall
    {
        #region DECLARATIONS
        static Texture2D spritesheet;
        public static readonly int size = 10;
        public static readonly float radialShootingVelocity = 400;
        public static readonly float shotLifeTime = 3;
        Vector2 rotationCenter;
        Animation animation;
        float radialDistance; //distace from center
        float angleRadiants;
        public float[] radialVelocities; //radial velocity for each phase
        public float[] angularVelocities; //angutar velocity for each phase
        float[] lifeTimes; //duration of each phase
        int phaseIndex;
        float elapsedLifeTime;
        bool targetPlayer;
        bool active;
        #endregion
        #region CONSTRUCTOR
        public FireBall(float _angleDegrees, float[] _radialVelocities, float[] _angularVelocities, float[] _lifeTimes,
            bool _targetPlayer = false, float centerOffsetX = 0, float centerOffsetY = 0, float _radialDistance = 0)
        {
            phaseIndex = 0;
            elapsedLifeTime = 0;
            active = true;
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
            animation= new Animation(new Rectangle(336,48,size*4, size), size, size, 4, 0.1f, true);
        }
        public static void Inizialize(Texture2D _spritesheet)
        {
            spritesheet = _spritesheet;
        }
        #endregion
        #region PROPERTIES
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
            if (elapsedLifeTime > lifeTimes[phaseIndex])
            {//switch to next phase
                    elapsedLifeTime = 0;
                    phaseIndex++;
                    if (phaseIndex== lifeTimes.Length)
                        active = false;
                    return;
            }
            else
            {//update fireball position
                elapsedLifeTime += elapsedTime;
                radialDistance += radialVelocities[phaseIndex] * elapsedTime;
                angleRadiants += angularVelocities[phaseIndex] * elapsedTime;
                if (angleRadiants >= MathHelper.Pi * 2)
                    angleRadiants -= MathHelper.Pi * 2;
                else if (angleRadiants < 0)
                    angleRadiants += MathHelper.Pi * 2;
                //if the fireball is meant to target the player
                //shoot if the shooting direction is close enough to the player position
                if (targetPlayer && phaseIndex== lifeTimes.Length-2 &&
                    checkPlayerAngle())
                {
                    targetPlayer = false;
                    phaseIndex = 0;
                    elapsedLifeTime = 0;
                    angularVelocities= new float[] { 0 };
                    radialVelocities = new float[] {radialShootingVelocity};
                    lifeTimes = new float[] { shotLifeTime };
                }


            }
        }

        //check whether the angle player-center-fireball is less than 5 degrees
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
