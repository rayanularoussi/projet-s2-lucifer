using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public GameObject supress;

    Rigidbody rb;

    public HealthBar healthBar;

    Animator animator;

    public GameObject bossPack;
    public bool bossOrNot = false;

    public GameObject fogWallObject;
    public AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        if(healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        
        
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if(healthBar != null)
        {
            healthBar.SetCurrentHealth(currentHealth);
        }
        if(!bossOrNot)
        {
            animator.Play("GetHit_01");
        }
        

        if (currentHealth <= 0)
        {   
            currentHealth = 0;
            if(bossOrNot)
            {
                Destroy(supress);
                bossOrNot = false;
            }
            else
            {
                animator.Play("Death Monster");
                if(audioSource != null)
                {
                    audioSource.Stop();
                }
            
                if(bossPack != null)
                {
                    bossPack.SetActive(false);
                }

                if(fogWallObject != null)
                {
                    fogWallObject.SetActive(false);
                }
                rb.velocity = Vector3.zero;
            }
            
        }
    }
}