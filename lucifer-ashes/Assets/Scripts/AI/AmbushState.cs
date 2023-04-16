﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace SL
{
	public class AmbushState : State
	{
		public bool isSleeping;
		public float detectionRadius = 2;
		public string sleepAnimation;
		public string wakeAnimation;
		public LayerMask detectionLayer;

		public PursueTargetState pursueTargetState;
	    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
	    {
		    if (isSleeping && enemyManager.isInteracting == false)
		    {
			    enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
		    }
		    #region Handle Target Detection

		    Collider[] colliders =
			    Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
		    for (int i = 0; i < colliders.Length; i++)
		    {
			    CharacterStats characterStats= colliders[i].transform.GetComponent<CharacterStats>();
			    if (characterStats != null)
			    {
				    Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
				    enemyManager.viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

				    if (enemyManager.viewableAngle > enemyManager.minimumDetectionAngle && enemyManager.viewableAngle < enemyManager.maximumDetectionAngle)
				    {
					    enemyManager.currentTarget = characterStats;
					    isSleeping = false;
					    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
				    }
			    }
		    }
		    #endregion
		    
		    #region Handle State Change

		    if (enemyManager.currentTarget != null)
		    {
			    return pursueTargetState;
		    }
		    else
		    {
			    return this;
		    }
		    #endregion

	    }
    }

	//juste pour pouvoir implémenter mes fonctions	
	public class PursueTargetState : State
	{
		public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
		{
			throw new System.NotImplementedException();
		}
	}

	public partial class PlayerManager : MonoBehaviour
	{
		

	}
}

