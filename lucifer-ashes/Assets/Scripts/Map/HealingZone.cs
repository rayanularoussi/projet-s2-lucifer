using UnityEngine;
using UnityEngine.UI;

public class HealingZone : MonoBehaviour
{
    public PlayerStats playerStats; 

    public AudioClip musicClip;

    private AudioSource audioSource;

    public HealthBar healthBar;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            audioSource.Play();
            ActivateTargetObject();
            playerStats.currentHealth = playerStats.maxHealth;
            healthBar.SetCurrentHealth(playerStats.currentHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            audioSource.Stop();
        }
    }

    private void ActivateTargetObject()
    {
        
    }
}