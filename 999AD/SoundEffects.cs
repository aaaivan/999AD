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
        private SoundEffectInstance pShootInstance;
        private SoundEffectInstance pHurtInstance;
        #endregion

        #region ENEMY1-SOUND-EFFECTS
        //SFX for the first enemy
        private SoundEffectInstance e1AttackInstance;
        #endregion

        #region ENEMY2-SOUND-EFFECTS
        //SFX for the second enemy
        private SoundEffectInstance e2MeleeInstance; // Same as e1Attack
        private SoundEffectInstance e2AttackInstance;
        private SoundEffectInstance enemyHurtInstance; // Shared with enemy 1
        #endregion

        #region MIDBOSS-SOUND-EFFECTS
        //SFX for the mid boss
        private SoundEffectInstance midMoveInstance;
        private SoundEffectInstance midAttackInstance;
        private SoundEffectInstance midHurtInstance;
        #endregion

        #region FINALBOSS-SOUND-EFFECTS
        //SFX for the final boss
        private SoundEffectInstance finAttackInstance;
        private SoundEffectInstance finHurtInstance;
        private SoundEffectInstance finAwakenInstance; // Spread its wings
        private SoundEffectInstance finRecoverInstance; // Breath heavily
        #endregion
        #endregion

        #region CONSTRUCTOR
        public void Initialise
            (
            SoundEffect pJump, SoundEffect pShoot, SoundEffect pHurt,
            SoundEffect enemyAttack, SoundEffect enemyHurt,
            SoundEffect e2Attack,
            SoundEffect midMove, SoundEffect midAttack, SoundEffect midHurt,
            SoundEffect finAttack, SoundEffect finHurt, SoundEffect finAwaken, SoundEffect finRecover
            )
        {
            //Player Variables
            pJumpInstance = pJump.CreateInstance();
            pShootInstance = pShoot.CreateInstance();
            pHurtInstance = pHurt.CreateInstance();

            //Enemy Variables
            //Enemy 1
            e1AttackInstance = enemyAttack.CreateInstance();

            //Enemy 2
            e2MeleeInstance = enemyAttack.CreateInstance();
            e2AttackInstance = e2Attack.CreateInstance();
            enemyHurtInstance = enemyHurt.CreateInstance();

            //Midboss
            midMoveInstance = midMove.CreateInstance();
            midAttackInstance = midAttack.CreateInstance();
            midHurtInstance = midHurt.CreateInstance();

            //Final boss
            finAttackInstance = finAttack.CreateInstance();
            finHurtInstance = finHurt.CreateInstance();
            finAwakenInstance = finAwaken.CreateInstance();
            finRecoverInstance = finRecover.CreateInstance();

        }
        #endregion

        #region PROPERTIES
        #region PLAYER
        public SoundEffectInstance PlayerJump
        {
            get { return pJumpInstance; }
        }

        public SoundEffectInstance PlayerAttack
        {
            get { return pShootInstance; }
        }

        public SoundEffectInstance PlayerHurt
        {
            get { return pHurtInstance; }
        }
        #endregion

        #region ENEMY 1
        public SoundEffectInstance Enemy1Attack
        {
            get { return e1AttackInstance; }
        }

        #endregion

        #region ENEMY 2
        public SoundEffectInstance Enemy2Melee
        {
            get { return e2MeleeInstance; }
        }

        public SoundEffectInstance Enemy2Attack
        {
            get { return e2AttackInstance; }
        }

        public SoundEffectInstance Enemy2Hurt
        {
            get { return enemyHurtInstance; }
        }
        #endregion

        #region MIDBOSS
        public SoundEffectInstance MidbossMove
        {
            get { return midMoveInstance; }
        }

        public SoundEffectInstance MidbossAttack
        {
            get { return midAttackInstance; }
        }

        public SoundEffectInstance MidbossHurt
        {
            get { return midHurtInstance; }
        }
        #endregion

        #region FINAL BOSS
        public SoundEffectInstance FinalBossAttack
        {
            get { return finAttackInstance; }
        }

        public SoundEffectInstance FinalBossHurt
        {
            get { return finHurtInstance; }
        }

        public SoundEffectInstance FinalBossAwaken
        {
            get { return finAwakenInstance; }
        }

        public SoundEffectInstance FinalBossRecover
        {
            get { return finRecoverInstance; }
        }
        #endregion
        #endregion
    }
}

/*
 *  class Sounds
    {
        private SoundEffectInstance laserSoundInstance;
        private SoundEffectInstance explosionSoundInstance;

        public void Initialise(SoundEffect laserSound, SoundEffect explosionSound)
        {
            laserSoundInstance = laserSound.CreateInstance();
            explosionSoundInstance = explosionSound.CreateInstance();
        }

        public SoundEffectInstance LASER
        {
            get { return laserSoundInstance; }
        }

        public SoundEffectInstance EXPLOSION
        {
            get { return explosionSoundInstance; }
        }
    }
 */
