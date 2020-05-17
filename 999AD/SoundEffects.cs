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
    static class SoundEffects
    {
        #region DECLARATIONS
        #region PLAYER-SOUND-EFFECTS
        //SFX for the Player
        static private SoundEffect pJumpInstance;
        static private SoundEffect pShootInstance;
        static private SoundEffect pHurtInstance;
        static private SoundEffect pickUpItem;
        #endregion

        #region ENEMY1-SOUND-EFFECTS
        //SFX for the first enemy
        static private SoundEffect e1AttackInstance;
        #endregion

        #region ENEMY2-SOUND-EFFECTS
        //SFX for the second enemy
        static private SoundEffect e2MeleeInstance; // Same as e1Attack
        static private SoundEffect e2AttackInstance;
        static private SoundEffect enemyHurtInstance; // Shared with enemy 1
        #endregion

        #region MIDBOSS-SOUND-EFFECTS
        //SFX for the mid boss
        static private SoundEffect midMoveInstance;
        static private SoundEffect midAttackInstance;
        static private SoundEffect midHurtInstance;
        #endregion

        #region FINALBOSS-SOUND-EFFECTS
        //SFX for the final boss
        static private SoundEffect finAttackInstance;
        static private SoundEffect finHurtInstance;
        static private SoundEffect finAwakenInstance; // Spread its wings
        static private SoundEffect finRecoverInstance; // Breath heavily
        private static Song finalBossMusic;
        #endregion
        #endregion

        #region CONSTRUCTOR
        static public void Initialise
            (
            SoundEffect pJump, SoundEffect pShoot, SoundEffect pHurt, SoundEffect pickUp,
            SoundEffect enemyAttack, SoundEffect enemyHurt,
            SoundEffect e2Attack,
            SoundEffect midMove, SoundEffect midAttack, SoundEffect midHurt,
            SoundEffect finAttack, SoundEffect finHurt, SoundEffect finAwaken, SoundEffect finRecover, Song finMusic
            )
        {
            //Player Variables
            pJumpInstance = pJump;
            pShootInstance = pShoot;
            pHurtInstance = pHurt;
            pickUp = pickUpItem;

            //Enemy Variables
            //Enemy 1
            e1AttackInstance = enemyAttack;

            //Enemy 2
            e2MeleeInstance = enemyAttack;
            e2AttackInstance = e2Attack;
            enemyHurtInstance = enemyHurt;

            //Midboss
            midMoveInstance = midMove;
            midAttackInstance = midAttack;
            midHurtInstance = midHurt;

            //Final boss
            finAttackInstance = finAttack;
            finHurtInstance = finHurt;
            finAwakenInstance = finAwaken;
            finRecoverInstance = finRecover;
            finalBossMusic = finMusic;

        }
        #endregion

        #region PROPERTIES
        //Returns Songs and SoundEffects to the other classes to allow them to play the required effects
        #region PLAYER
        static public SoundEffectInstance PlayerJump
        {
            get { return pJumpInstance.CreateInstance(); }
        }

        static public SoundEffectInstance PlayerAttack
        {
            get { return pShootInstance.CreateInstance(); }
        }

        static public SoundEffectInstance PlayerHurt
        {
            get { return pHurtInstance.CreateInstance(); }
        }
        #endregion
        static public SoundEffectInstance PickUpItem
        {
            get { return pickUpItem.CreateInstance(); }
        }
        #region ENEMY 1
        static public SoundEffectInstance Enemy1Attack
        {
            get { return e1AttackInstance.CreateInstance(); }
        }

        #endregion

        #region ENEMY 2
        static public SoundEffectInstance Enemy2Melee
        {
            get { return e2MeleeInstance.CreateInstance(); }
        }

        static public SoundEffectInstance Enemy2Attack
        {
            get { return e2AttackInstance.CreateInstance(); }
        }

        //Shared for both Enemy 1 and 2
        static public SoundEffectInstance EnemyHurt
        {
            get { return enemyHurtInstance.CreateInstance(); }
        }
        #endregion

        #region MIDBOSS
        static public SoundEffectInstance MidbossMove
        {
            get { return midMoveInstance.CreateInstance(); }
        }

        static public SoundEffectInstance MidbossAttack
        {
            get { return midAttackInstance.CreateInstance(); }
        }

        static public SoundEffectInstance MidbossHurt
        {
            get { return midHurtInstance.CreateInstance(); }
        }
        #endregion

        #region FINAL BOSS
        static public SoundEffectInstance FinalBossAttack
        {
            get { return finAttackInstance.CreateInstance(); }
        }

        static public SoundEffectInstance FinalBossHurt
        {
            get { return finHurtInstance.CreateInstance(); }
        }

        static public SoundEffectInstance FinalBossAwaken
        {
            get { return finAwakenInstance.CreateInstance(); }
        }

        static public SoundEffectInstance FinalBossRecover
        {
            get { return finRecoverInstance.CreateInstance(); }
        }
        static public Song FinaBossSoundTrack
        {
            get { return finalBossMusic; }
        }
        #endregion
        #endregion
    }
}