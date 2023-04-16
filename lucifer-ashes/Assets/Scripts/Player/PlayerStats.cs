using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Rigidbody rb;
    public HealthBar healthBar;
    AnimatorHandler animatorHandler;
    StaminaBar staminaBar;

    private void Awake()
    {
        staminaBar = FindObjectOfType<StaminaBar>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation("GetHit_01", true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;

            animatorHandler.PlayTargetAnimation("Dead_01", true);
            rb.velocity = Vector3.zero;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}
