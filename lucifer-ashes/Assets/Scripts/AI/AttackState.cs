using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SL
{
    public class AttackState : State
    {
	    public CombatState combatState;

	    public EnemyAttackAction[] enemyAttacks;
	    public EnemyAttackAction currentAttack;

	    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,
		    EnemyAnimatorManager enemyAnimatorManager)
	    {
		    Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
		    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
		    if (enemyManager.isPerformingAction)
			    return combatState;
		    if (currentAttack != null)
		    {
			    //if we are too close to the enemy, get a new attack
			    if (enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
			    {
				    return this;
			    }
			    else if (enemyManager.distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
			    {
				    //if our enemy is within our attacks viewable angle, we attack
				    if (enemyManager.viewableAngle <= currentAttack.maximumAttackAngle &&
				        enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
				    {
					    if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
					    {
						    enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
						    enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
						    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
						    enemyManager.isPerformingAction = true;
						    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
						    currentAttack = null;
						    return combatState;
					    }
				    }
			    }
		    }
		    else
		    {
			    GetNewAttack(enemyManager);
		    }

		    return combatState;
	    }

	    private void GetNewAttack(EnemyManager enemyManager)
	        {
				Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
				float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
				enemyManager.distanceFromTarget =
					Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
	
				int maxScore = 0;
	
				for (int i = 0; i < enemyAttacks.Length; i++)
				{
					EnemyAttackAction enemyAttackAction = enemyAttacks[i];
					if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
					    enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
					{
						if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
						    viewableAngle >= enemyAttackAction.minimumAttackAngle)
						{
							maxScore += enemyAttackAction.attackScore;
						}
					}
				}
	
				int randomValue = Random.Range(0, maxScore);
				int temporaryScore = 0;
	
				for (int i = 0; i < enemyAttacks.Length; i++)
				{
					EnemyAttackAction enemyAttackAction = enemyAttacks[i];
					if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
					    enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
					{
						if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
						    viewableAngle >= enemyAttackAction.minimumAttackAngle)
						{
							if (currentAttack != null)
								return;
							temporaryScore += enemyAttackAction.attackScore;
	
							if (temporaryScore > randomValue)
							{
								currentAttack = enemyAttackAction;
							}
						}
					}
				}
			}
	        
        }
    }

