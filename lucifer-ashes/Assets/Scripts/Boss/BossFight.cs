using System.Collections;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    public GameObject knifePrefab;
    public GameObject explosionPrefab;
    public float spawnRadius = 10f;
    public float fallSpeed = 10f;
    public GameObject Boss;
    private BossStats BossStats;
    public LayerMask groundLayer;
    public bool newPhase = false;
    

    private bool isBossFightActive = false;

    private void StartBossFight()
    {
        isBossFightActive = true;
        StartCoroutine(SpawnKnivesWithCooldown());
        
    }

    private void EndBossFight()
    {
        isBossFightActive = false;
    }

    private IEnumerator SpawnKnivesWithCooldown()
    {
        float timer = 0f; 
        while (isBossFightActive && timer < 10f) 
        {
            int numKnives = 30;

            for (int i = 0; i < numKnives; i++)
            {
                
                float offsetX = Random.Range(-spawnRadius, spawnRadius);
                float offsetZ = Random.Range(-spawnRadius, spawnRadius);

                
                Vector3 spawnPos = transform.position + new Vector3(offsetX, 0f, offsetZ);

                
                GameObject knife = Instantiate(knifePrefab, spawnPos, Quaternion.identity);

                knife.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

                Vector3 playerDirection = transform.position - knife.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(playerDirection, Vector3.up);

                knife.transform.rotation = Quaternion.RotateTowards(knife.transform.rotation, targetRotation, 30f);

                RaycastHit hit;
                if (Physics.Raycast(knife.transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
                {
                    Vector3 floorPos = hit.point;

                    
                    float fallDistance = knife.transform.position.y - floorPos.y;
                    float fallTime = fallDistance / fallSpeed;

                    
                    Rigidbody knifeRb = knife.GetComponent<Rigidbody>();
                    if (knifeRb != null)
                    {
                        knifeRb.useGravity = true;
                        knifeRb.velocity = Vector3.zero;
                        knifeRb.AddForce(Vector3.down * fallSpeed, ForceMode.VelocityChange);
                    }

                    
                    if (explosionPrefab != null)
                    {
                        SpawnExplosion(floorPos);

                        
                        DestroyKnifeAndExplosion(knife, fallTime);
                    }
                }

                
                yield return new WaitForSeconds(1f);
            }

            timer += 1f; 
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }

    private void DestroyKnifeAndExplosion(GameObject knife, float delay)
    {
        StartCoroutine(DestroyKnifeAndExplosionDelayed(knife, delay));
    }

    private IEnumerator DestroyKnifeAndExplosionDelayed(GameObject knife, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(knife);
        DestroyExplosion();
    }

    private void DestroyExplosion()
    {
        GameObject explosion = GameObject.FindWithTag("Explosion");
        if (explosion != null)
        {
            Destroy(explosion);
        }
    }

    // Example method to start the boss fight
    private void Start()
    {
        BossStats = GetComponent<BossStats>();
        StartBossFight();
    }
}