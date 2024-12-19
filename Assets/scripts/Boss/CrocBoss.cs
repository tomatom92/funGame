using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrocBoss : BossBase
{
    public enum BossState { Idle, GroundAttack, WaterMove, RageMode, Chase}
    private BossState currentState;

    public float phase2HealthThreshold = 50f;
    public float rageHealthThreshold = 20f;

    public float moveSpeed = 2f;
    public float attackDelay = 2f;
    private float attackTimer = 0f;
    public float attackRange = 1.5f;
    private bool isAttacking = false;

    public Transform player;
    public LayerMask attackLayer;

    private BossUI bossUI;
    private Animator animator;
    public Animator biteAnimator;  // Animator for the bite attack
    private SpriteRenderer spriteRenderer;
    private Collider2D hurtBox;
    public Collider2D tilemapCollider;

    private BossFightManager fightManager;
    public bool isCutsceneActive = false; // Flag to disable boss behavior

    protected override void Start()
    {
        base.Start();
        currentState = BossState.Idle;

        // Get references to the health and UI scripts
        bossHealth = GetComponent<BossHealth>();
        bossUI = FindObjectOfType<BossUI>();  // Find the BossHealthBar object in the scene

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        hurtBox = GetComponent<Collider2D>();
        fightManager = GetComponent<BossFightManager>();
        // Subscribe to health change and death events
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthUI;
            bossHealth.OnDeath += Die;
        }
        //ignore water collisions
        
    }

    protected void Update()
    {
        if (fightManager.isFightActive == false) return;
        if (isCutsceneActive) return;
        if (bossHealth.isDead) return;

        HandleStates();
        attackTimer -= Time.deltaTime;
    }
    public void SetState(BossState state)
    {
        currentState = state;
    }
    public BossState GetState()
    {
        return currentState;
    }

    //BOSS STATE MACHINE
    private void HandleStates()
    {
        if (bossHealth.currentHealth <= rageHealthThreshold)
        {
            currentState = BossState.RageMode;
        }
        else if (bossHealth.currentHealth <= phase2HealthThreshold)
        {
            currentState = BossState.WaterMove;
        }
        else if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            currentState = BossState.GroundAttack;
        }
        else
        {
            currentState = BossState.Chase;
        }

        //Debug.Log($"Current State: {currentState}"); // Log the current state
        switch (currentState)
        {
            case BossState.Idle:
                // Maybe idle animations or effects.
                break;
            case BossState.GroundAttack:
                PerformGroundAttack();
                break;
            case BossState.WaterMove:
                PerformWaterMove();
                break;
            case BossState.RageMode:
                PerformRageMode();
                break;
            case BossState.Chase:
                PerformChase(); // Call the chase logic
                break;
        }
    }
    private void PerformChase()
    {
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Check if the boss should stop chasing (e.g., if another state is active)
        if (currentState != BossState.Chase) return;

        // Move towards the player if out of attack range
        if (distanceToPlayer > attackRange) // Example stopping distance for an attack
        {
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            

            // Update the boss's position towards the player
            transform.position += (Vector3)(moveSpeed * Time.deltaTime * direction);

        }
        else
        {
            // Transition to an attack state when in range
            currentState = BossState.GroundAttack;
            
            
        }
    }


    //ATTACKS
    private void PerformGroundAttack()
    {
        if (isAttacking) return; // If so, don't start a new one.

        StartCoroutine(BiteAttackRoutine());
    }
    private IEnumerator BiteAttackRoutine()
   {
        isAttacking = true;
       yield return new WaitForSeconds(attackDelay);
       Debug.Log("BITE");
       biteAnimator.SetTrigger("bite");
       biteAnimator.transform.position = player.position;
       isAttacking = false;
       //Vector3 offset = _spriteRenderer.flipX ? Vector3.left : Vector3.right;
       //Vector3 attackPosition = transform.position + (offset * hitboxOffsetDistance);


    }

    private void PerformWaterMove()
    {
        // Crocodile dives underwater and creates water hazards
        if (attackTimer <= 0f)
        {
            //add separate cooldowns for each ability
            //bossUI.HideHealthBar();
            animator.SetTrigger("DiveAttack");
            attackTimer = attackDelay;
            //
        }
    }

    private void PerformRageMode()
    {
        // Fast, aggressive attacks or a special mechanic (e.g., roars)
        if (attackTimer <= 0f)
        {
            animator.SetTrigger("RageAttack");
            attackTimer = attackDelay / 2f; // Faster attacks
        }
    }

    protected void Die()
    {
        bossHealth.isDead = true;
        animator.SetTrigger("Die");
        // Handle death (e.g., drop loot, change environment)

        Debug.Log($"{name} died");
        spriteRenderer.enabled = false;
        hurtBox.enabled = false;

        if (bossUI != null)
        {
            bossUI.HideHealthBar();  // Hide the health bar on death
        }
    }
    public void ActivateSpecialAbility()
    {
        // Example of a special move like a roar or a charge
    }

}
