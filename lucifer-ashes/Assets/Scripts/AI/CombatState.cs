using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SL
{
   public class CombatState : State
   {
      public AttackState attackState;
      public PursueState pursueState;
      public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
      {

         enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);
         //potentially circle player or walk around them
         //check for attack range

         if (enemyManager.currentRecoveryTime <= 0 &&
             enemyManager.distanceFromTarget <= enemyManager.maximumAttackingRange)
         {
            return attackState;
         }
         if (enemyManager.distanceFromTarget > enemyManager.maximumAttackingRange)
         {
            return pursueState;
         }
         else
         {
            return this;
         }
      }
   } 
}

