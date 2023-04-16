using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;



	public class EnemyManager : CharacterManager
	{
		EnemyLocomotionManager enemyLocomotionManager;
		EnemyAnimatorManager enemyAnimatonManager;
		EnemyStats enemyStats;
		public NavMeshAgent navMeshAgent;

		public State currentStats;
		public CharacterStats currentTarget;
		public bool isPerformingAction;
		public bool isInteracting;
		public Rigidbody enemyRigidbody;

		public float distanceFromTarget;
		public float rotationSpeed = 15;
		public float maximumAttackingRange = 1.5f;

		/*public EnemyAttackAction[] enemyAttacks;
		public EnemyAttackAction currentAttack;*/

		[Header("A.I Settings")] public float detectionRadius = 20;

		//the higher, and lower, respectively these angles are, the greater detection FILED OF VIEW
		public float maximumDetectionAngle = 50;
		public float minimumDetectionAngle = -50;

		public float viewableAngle;

		public float currentRecoveryTime = 0;

		private void Awake()
		{
			enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
			enemyAnimatonManager = GetComponentInChildren<EnemyAnimatorManager>();
			navMeshAgent = GetComponentInChildren<NavMeshAgent>();
			enemyStats = GetComponent<EnemyStats>();
			navMeshAgent.enabled = false;
			enemyRigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			enemyRigidbody.isKinematic = false;
		}

		private void Update()
		{
			HandleRecoveryTimer();
			isInteracting = enemyAnimatonManager.animator.GetBool("isInteracting");
		}

		private void FixedUpdate()
		{
			HandleStateMachine();
		}

		private void HandleStateMachine()
		{
			if (currentStats != null)
			{
				State nextState = currentStats.Tick(this, enemyStats, enemyAnimatonManager);
				if (nextState != null)
				{
					SwitchToNextState(nextState);
				}
			}


			/*if (enemyLocomotionManager.currentTarget != null)
			{
				enemyLocomotionManager.distanceFromTarget =
					Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

			}

			if (enemyLocomotionManager.currentTarget == null)
			{
				enemyLocomotionManager.HandleDetection();
			}
			else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
			{
				enemyLocomotionManager.HandleMoveToTarget();
			}
			else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
			{
				AttackTarget();
			}*/
		}

		private void SwitchToNextState(State state)
		{
			currentStats = state;
		}

		private void HandleRecoveryTimer()
		{
			if (currentRecoveryTime > 0)
			{
				currentRecoveryTime -= Time.deltaTime;
			}

			if (isPerformingAction)
			{
				if (currentRecoveryTime <= 0)
				{
					isPerformingAction = false;
				}
			}
		}
	}

