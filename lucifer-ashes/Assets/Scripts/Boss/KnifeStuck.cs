using UnityEngine;

public class KnifeStuck : MonoBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask groundLayer;
    public LayerMask characterLayer;
    public int damageAmount = 10;

    private bool isStuck = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsCollidingWithGround(collision) && !isStuck)
        {
            Rigidbody knifeRigidbody = GetComponent<Rigidbody>();
            if (knifeRigidbody != null)
            {
                knifeRigidbody.isKinematic = true;
                knifeRigidbody.useGravity = false;
            }

            transform.SetParent(collision.transform);

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, 5f);

            isStuck = true;
        }
        if (IsCollidingWithCharacter(collision))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }

            isStuck = true;
        }
    }

    private bool IsCollidingWithGround(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.layer == groundLayer)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCollidingWithCharacter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.layer == characterLayer)
            {
                return true;
            }
        }
        return false;
    }
}