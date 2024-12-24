using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class BossUI : MonoBehaviour
{
    public Image[] healthSegments;  // Array of UI.Image for each health segment (each representing a part of the health bar)
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private GameObject bossUIHolder;
    public bool playerInRange;

    void Start()
    {
        //bossHealth = GetComponent<BossHealth>();  // get the BossHealth component
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthBar;  // Subscribe to health change event
            bossHealth.OnDeath += HideHealthBar;  // Subscribe to death event to hide the health bar
        }

        UpdateHealthBar(bossHealth.currentHealth);  // Initialize the health bar with current health
    }

    void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= UpdateHealthBar;  // Unsubscribe from event
            bossHealth.OnDeath -= HideHealthBar;  // Unsubscribe from event
        }
    }

    // Updates each health segment based on current health
    public void UpdateHealthBar(float currentHealth)
    {
        // Calculate how many health segments to show based on current health
        int segmentsToShow = Mathf.CeilToInt((currentHealth / bossHealth.maxHealth) * healthSegments.Length);

        for (int i = 0; i < healthSegments.Length; i++)
        {
            healthSegments[i].enabled = i < segmentsToShow;  // Show or hide each health segment
        }
    }

    // Called when the boss dies to hide the health bar
    public void HideHealthBar()
    {
        bossUIHolder.SetActive(false);
    }
    public void ShowHealthBar()
    {
        foreach (var segment in healthSegments)
        {
            segment.enabled = true;  // Hide all health segments
        }
        bossUIHolder.SetActive(true);

    }
}
