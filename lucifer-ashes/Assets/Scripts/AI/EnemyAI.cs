using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private Transform player;

    [SerializeField]
    public PlayerStats playerStats;
    private InputManager inputManager;
    public EnemyStats enemyStats;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [Header("Stats")]

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private float chaseSpeed;

    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    public int damageDelt = 25;

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

    [SerializeField]
    private float attackRadius;

    private bool isAttacking;

    void Start()
    {
        
    }

    void Update()
    {
        if(enemyStats.currentHealth <= 0)
        {
            return;
        }
        else
        {
            if(Vector3.Distance(player.position, transform.position) < detectionRadius)
            {
                agent.speed = chaseSpeed;

                if(!isAttacking)
                {
                    if(Vector3.Distance(player.position, transform.position) < attackRadius)
                    {
                        StartCoroutine(attackPlayer());
                    }
                    else
                    {
                        agent.SetDestination(player.position);
                    }
                }
            }
            else
            {
                agent.speed = walkSpeed;

                if(agent.remainingDistance < 0.75f && !hasDestination)
                {
                    StartCoroutine(GetNewDestination());
                }
            }

            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    IEnumerator attackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        animator.SetTrigger("Attack");

        playerStats.TakeDamage(damageDelt);
        
        yield return new WaitForSeconds(attackDelay);

        agent.isStopped = false;
        isAttacking = false;
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
