using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.AI;
using UnityEngine;


   public class EnemyLocomotionManager : MonoBehaviour
   {
      EnemyManager enemyManager; 
      EnemyAnimatorManager enemyAnimatorManager;

      public Rigidbody enemyRigidbody;

      public CharacterStats currentTarget;
      public LayerMask detectionLayer;

      public float distanceFromTarget;
      public float stoppingDistance = 1f;
      public float rotationSpeed = 15;
      
      private void Awake()
      {
         enemyManager = GetComponent<EnemyManager>();
         enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
      }
      
      public void HandleDetection()
      {
         Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

         for (int i = 0; i < colliders.Length; i++)
         {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
               Vector3 targetDirection = characterStats.transform.position - transform.position;
               float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

               if (viewableAngle > enemyManager.minimumDetectionAngle &&
                   viewableAngle < enemyManager.maximumDetectionAngle)
               {
                  currentTarget = characterStats;
               }
            }
         }
      }
   } 


