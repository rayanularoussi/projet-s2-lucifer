using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SL
{
	public class PursueState : State
	{
		public CombatState combatState;

		public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats,
			EnemyAnimatorManager enemyAnimatorManager)
		{
			if (enemyManager.isPerformingAction)
				return this;
			
			Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
			enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
			
			if (enemyManager.distanceFromTarget > enemyManager.maximumAttackingRange)
			{
				enemyAnimatorManager.animator.SetFloat("Vetical", 1, 0.1f, Time.deltaTime);
			}
			
			HandleRotateTowardsTarget(enemyManager);
			enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
			enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

			if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackingRange)
			{
				return combatState;
			}
			return this;
		}
		
		private void HandleRotateTowardsTarget(EnemyManager enemyManager)
		{
			//rotate manually
			if (enemyManager.isPerformingAction)
			{
				Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
				direction.y = 0;
				direction.Normalize();
				if (direction == Vector3.zero)
				{
					direction = transform.forward;
				}

				Quaternion targetRotation = Quaternion.LookRotation(direction);
				enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
			}
			//rotate with pathfinding (navmesh)
			else
			{
				Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
				Vector3 targetVelocity =enemyManager.enemyRigidbody.velocity;

				enemyManager.navMeshAgent.enabled = true;
				enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
				enemyManager.enemyRigidbody.velocity = targetVelocity;
				enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation,
					enemyManager.rotationSpeed / Time.deltaTime);
			}
         
			HandleRotateTowardsTarget(enemyManager);
			enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
			enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
		}
		
	}
}


