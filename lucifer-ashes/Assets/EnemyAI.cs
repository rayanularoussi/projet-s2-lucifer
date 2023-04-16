using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private Transform player;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;

    [Header("Stats")]

    [SerializeField]
    private float detectionRadius;

    private bool hasDestination;

    [Header("Wandering parameters")]

    [SerializeField]
    private float wanderingWaitTimeMin;

    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField]
    private float wanderingDistanceMin;

    [SerializeField]
    private float wanderingDistanceMax;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if(agent.remainingDistance < 0.75f && !hasDestination)
            {
                StartCoroutine(GetNewDestination());
            }
        }
    }

    IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) * new Vector3(Random.Range(-1f, 1), 0f, Random.Range(-1f, 1f)).normalized;

        UnityEngine.AI.NavMeshHit hit;
        if(UnityEngine.AI.NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, UnityEngine.AI.NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        hasDestination = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
