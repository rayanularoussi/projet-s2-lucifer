using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsWoHB : CharacterStats
{
    Rigidbody rb;

    Animator animator;

    public GameObject bossPack;

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
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        animator.Play("GetHit_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;

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