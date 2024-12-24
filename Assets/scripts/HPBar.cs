using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [Space]
    [Header("Stats")]
    public int hpRemaining;
    public int maxHP = 3;
    [Space]
    [Header("UI")]
    public GameObject deathGUI;
    public GameObject healthHolder;
    private GameObject[] heartArray;

    private SpriteRenderer spriteRenderer;
    private CharacterBehaviour character;
    private Animator animator;
    private Collider2D charCollider;
    private float hurtTimer;

    [Space]
    [Header("sfx")]
    //sfx
    public AudioClip deathSound;
    private AudioSource audioSource;

    [Space]
    [Header("Invincibility variables")]
    // Invincibility variables
    private bool isInvincible = false;  // Flag to track if the player is invincible
    private float invincibilityTime = 1f;  // Time the player is invincible after being hit (in seconds)
    private float invincibilityTimer = 0f; // Timer to track invincibility

    // Flash parameters
    [Space]
    [Header("Flash parameters")]
    [SerializeField] private Color invincibleColor = Color.red; // Color during invincibility
    [SerializeField] private float flashDuration = 0.1f; // Duration of each flash
    [SerializeField] private int flashCount = 3; // How many times to flash


    private void Awake()
    {
        charCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterBehaviour>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        hpRemaining = maxHP;
        
        healthHolder.SetActive(true);

        //filling heart array and setting health based on amount of child heart objects
        heartArray = new GameObject[healthHolder.transform.childCount];
        for (int i = 0; i < healthHolder.transform.childCount; i++)
        {
            heartArray[i] = healthHolder.transform.GetChild(i).gameObject;

        }
        RefreshHP();
    }
    
    public void RemoveHeart(int heartAmount)
    {
        if (isInvincible) return;

        hpRemaining -= heartAmount;
        RefreshHP();

        //hit noise;
        audioSource.Play();
        //death check
        if (hpRemaining <= 0)
        {
            Die();
            
        }
        else
        {
            ActivateInvincibility();
        }
        //Debug.Log($"{gameObject.name}, removed {heartAmount} hearts");

    }
    private void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityTime;

        // Start the invincibility flash effect
        StartCoroutine(FlashInvincibility());
    }

    private IEnumerator FlashInvincibility()
    {
        int flashes = 0;
        while (flashes < flashCount)
        {
            // Flash the color
            spriteRenderer.color = invincibleColor;  // Change to invincible color
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;  // Reset to normal color
            yield return new WaitForSeconds(flashDuration);
            flashes++;
        }
    }
    public void HurtAnimation(float timer)
    {
        //Debug.Log((int)(100 * timer / 16));
        if ((int)(100 * timer / 16) % 2 == 0)
        {
            //Debug.Log($"im red");
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
    // Update is called once per frame
    public void RefreshHP()
    {
        for (int i = 0; i < healthHolder.transform.childCount; i++) { heartArray[i].SetActive(false); }
        for (int i = 0; i < hpRemaining; i++) { heartArray[i].SetActive(true); }
    }
    public void Die()
    {
        // Play death sound
        audioSource.clip = deathSound;
        audioSource.volume = 0.15f;
        audioSource.Play();

        //disable sprite/animator
        spriteRenderer.enabled = false;
        animator.enabled = false;

        // Disable the collider
        charCollider.enabled = false;

        // Handle death-specific logic
        character.isDead = true;
        if (gameObject.CompareTag("Player"))
        {
            deathGUI.SetActive(true);
        }
        if (gameObject.CompareTag("Enemy"))
            Destroy(transform.parent.gameObject, 0.5f);
    }
    private void Update()
    {
        // Update invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;  // End invincibility when timer reaches zero
            }
        }
    }
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
