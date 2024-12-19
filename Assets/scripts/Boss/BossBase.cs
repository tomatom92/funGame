using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    protected BossHealth bossHealth;  // Reference to the BossHealth script
    

    protected virtual void Start()
    {
        bossHealth = GetComponent<BossHealth>();  // Get the BossHealth component attached to the same GameObject


        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthUI;  // Subscribe to health change event
            bossHealth.OnDeath += HandleDeath;  // Subscribe to the OnDeath event to handle the boss's death 
        }
    }

    void OnDestroy()
    {

        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= UpdateHealthUI;  // Unsubscribe from event
            bossHealth.OnDeath -= HandleDeath;  // Unsubscribe from the OnDeath event to prevent memory leaks
        }
    }

    // Called when the boss dies
    protected virtual void HandleDeath()
    {
        // Implement what happens when the boss dies, e.g., play death animation, disable the boss, etc.
        //Debug.Log("The boss has died!");

        // Example death logic: destroy the boss GameObject
        //Destroy(gameObject);
    }
    protected virtual void UpdateHealthUI(float currentHealth)
    {
        // This function can be left empty if the actual UI updating is handled in another class (BossHealthBar)
    }
}
