using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace _999AD
{
    class SoundEffects
    {
        #region DECLARATIONS
        #region PLAYER-SOUND-EFFECTS
        //SFX for the Player
        private SoundEffectInstance pJumpInstance;
        private Song pShootInstance;
        private SoundEffectInstance pHurtInstance;
        #endregion

        #region ENEMY1-SOUND-EFFECTS
        //SFX for the first enemy
        private Song e1AttackInstance;
        #endregion

        #region ENEMY2-SOUND-EFFECTS
        //SFX for the second enemy
        private Song e2MeleeInstance; // Same as e1Attack
        private SoundEffectInstance e2AttackInstance;
        private SoundEffectInstance enemyHurtInstance; // Shared with enemy 1
        #endregion

        #region MIDBOSS-SOUND-EFFECTS
        //SFX for the mid boss
        private Song midMoveInstance;
        private SoundEffectInstance midAttackInstance;
        private Song midHurtInstance;
        #endregion

        #region FINALBOSS-SOUND-EFFECTS
        //SFX for the final boss
        private Song finAttackInstance;
        private Song finHurtInstance;
        private Song finAwakenInstance; // Spread its wings
        private Song finRecoverInstance; // Breath heavily
        #endregion
        #endregion

        #region CONSTRUCTOR
        public void Initialise
            (
            SoundEffect pJump, Song pShoot, SoundEffect pHurt,
            Song enemyAttack, SoundEffect enemyHurt,
            SoundEffect e2Attack,
            Song midMove, SoundEffect midAttack, Song midHurt,
            Song finAttack, Song finHurt, Song finAwaken, Song finRecover
            )
        {
            //Player Variables
            pJumpInstance = pJump.CreateInstance();
            pShootInstance = pShoot;
            pHurtInstance = pHurt.CreateInstance();

            //Enemy Variables
            //Enemy 1
            e1AttackInstance = enemyAttack;

            //Enemy 2
            e2MeleeInstance = enemyAttack;
            e2AttackInstance = e2Attack.CreateInstance();
            enemyHurtInstance = enemyHurt.CreateInstance();

            //Midboss
            midMoveInstance = midMove;
            midAttackInstance = midAttack.CreateInstance();
            midHurtInstance = midHurt;

            //Final boss
            finAttackInstance = finAttack;
            finHurtInstance = finHurt;
            finAwakenInstance = finAwaken;
            finRecoverInstance = finRecover;

        }
        #endregion

        #region PROPERTIES
        //Returns Songs and SoundEffects to the other classes to allow them to play the required effects
        #region PLAYER
        public SoundEffectInstance PlayerJump
        {
            get { return pJumpInstance; }
        }

        public Song PlayerAttack
        {
            get { return pShootInstance; }
        }

        public SoundEffectInstance PlayerHurt
        {
            get { return pHurtInstance; }
        }
        #endregion

        #region ENEMY 1
        public Song Enemy1Attack
        {
            get { return e1AttackInstance; }
        }

        #endregion

        #region ENEMY 2
        public Song Enemy2Melee
        {
            get { return e2MeleeInstance; }
        }

        public SoundEffectInstance Enemy2Attack
        {
            get { return e2AttackInstance; }
        }

        //Shared for both Enemy 1 and 2
        public SoundEffectInstance EnemyHurt
        {
            get { return enemyHurtInstance; }
        }
        #endregion

        #region MIDBOSS
        public Song MidbossMove
        {
            get { return midMoveInstance; }
        }

        public SoundEffectInstance MidbossAttack
        {
            get { return midAttackInstance; }
        }

        public Song MidbossHurt
        {
            get { return midHurtInstance; }
        }
        #endregion

        #region FINAL BOSS
        public Song FinalBossAttack
        {
            get { return finAttackInstance; }
        }

        public Song FinalBossHurt
        {
            get { return finHurtInstance; }
        }

        public Song FinalBossAwaken
        {
            get { return finAwakenInstance; }
        }

        public Song FinalBossRecover
        {
            get { return finRecoverInstance; }
        }
        #endregion
        #endregion
    }
}