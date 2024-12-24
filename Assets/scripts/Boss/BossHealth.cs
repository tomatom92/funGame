using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    private BossFightManager fightManager;
    private void Start()
    {
        currentHealth = maxHealth;
        fightManager = GetComponent<BossFightManager>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);

        Debug.Log($"{gameObject.name} took {damage} damage");    

        if (currentHealth <= 0)
        {
            isDead  = true;
            OnDeath?.Invoke();
            fightManager.EndBossFight();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth);  // Trigger health change event
    }
}

