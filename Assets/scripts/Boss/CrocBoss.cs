using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocBoss : BossBase
{
    public enum BossState { Idle, GroundAttack, SpawnEnemy, RageMode, Chase, Underwater }
    [Header("state")]
    private BossState currentState;
    private BossState previousState;
    private bool hasSpawnedEnemies = false; // Ensure enemies spawn only once
    private bool hasSpawnedEnemiesInRage = false; // Flag to track enemy spawning in Rage Mode

    [HideInInspector] public List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    [Space]
    [Header("Boss Stats")]

    public float phase2HealthThreshold = 60f;
    public float rageHealthThreshold = 20f;
    public float moveSpeed = 2f;
    public float rageSpeed = 3f;
    public float biteDelay = 2f;
    public float attackRange = 1.5f;
    public float hitboxOffsetDistance = 0.5f; // Adjust for visual effect

    private bool isAttacking = false;
    private float attackTimer = 0f;
    [Space]
    [Header("components")]
    public Transform player;
    public Collider2D waterCollider;
    public Animator biteAnimator;  // Animator for the bite attack
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D hurtBox;

    private BossEffects bossEffects;
    private BossFightManager fightManager;

    [Space]
    [Header("Boss minions")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesToSpawn = 2;
    public int spawnDelay;

    [HideInInspector] public bool isCutsceneActive = false; // Flag to disable boss behavior
    private float rageBite;


    protected override void Start()
    {
        base.Start();
        currentState = BossState.Idle;
        previousState = currentState;
        // Get references to the health and UI scripts
        bossHealth = GetComponent<BossHealth>();
        bossEffects = GetComponent<BossEffects>();

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
        Physics2D.IgnoreCollision(hurtBox, waterCollider);
    }

    protected void Update()
    {
        if (fightManager.isFightActive == false) return;
        if (isCutsceneActive) return;
        if (bossHealth.isDead) return;

        //check if spawned enemies are dead
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        StartCoroutine(PrintState());
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

    // BOSS STATE MACHINE
    private void HandleStates()
    {
        if (bossHealth.currentHealth <= rageHealthThreshold)
        {
            currentState = BossState.RageMode;
        }
        else if (bossHealth.currentHealth <= phase2HealthThreshold && !hasSpawnedEnemies)
        {
            currentState = BossState.SpawnEnemy;
        }
        else if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            currentState = BossState.GroundAttack;
        }
        else
        {
            currentState = BossState.Chase;
        }

        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.GroundAttack:
                PerformGroundAttack(biteDelay);
                break;
            case BossState.SpawnEnemy:
                PerformSpawnEnemy();
                break;
            case BossState.RageMode:
                PerformRageMode();
                break;
            case BossState.Chase:
                PerformChase(); // Call the chase logic
                spriteRenderer.enabled = true;
                hurtBox.enabled = true;
                break;
        }

        previousState = currentState;
    }

    private void PerformChase()
    {
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the boss should stop chasing (e.g., if another state is active)
        if (currentState != BossState.Chase) return;

        // Move towards the player if out of attack range
        if (distanceToPlayer > attackRange)
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

    // ATTACKS
    private void PerformGroundAttack(float delay)
    {
        if (isAttacking) return; // If so, don't start a new one.

        StartCoroutine(Bite(delay));
    }

    private IEnumerator Bite(float delay)
    {
        isAttacking = true;

        // Get the direction parameter from the animator
        float directionValueX = animator.GetFloat("moveX");
        float directionValueY = animator.GetFloat("moveY");

        // Calculate offset based on direction parameter
        Vector3 offset = new Vector3(directionValueX, directionValueY, 0);

        // Calculate hitbox position
        Vector3 attackPosition = transform.position + (offset * hitboxOffsetDistance);

        // Set hitbox position
        biteAnimator.transform.position = attackPosition;
        biteAnimator.SetTrigger("bite");

        yield return new WaitForSeconds(delay);

        isAttacking = false;
    }

    private void PerformSpawnEnemy()
    {
        if (previousState != BossState.SpawnEnemy)
        {
            bossEffects.PlayRoar();
            HideBoss(); // Make the boss dive underwater
        }

        if (!hasSpawnedEnemies)
        {
            // Spawn enemies only once
            SpawnEnemies();
            hasSpawnedEnemies = true;
        }

 
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null) return; // Ensure the prefab is assigned

        spawnedEnemies.Clear(); // Reset the list of spawned enemies

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // Spawn enemies at predefined spawn points
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                EnemyController enemyController = enemy.transform.GetChild(0).GetComponent<EnemyController>();
                Physics2D.IgnoreCollision(enemy.transform.GetChild(0).GetComponent<Collider2D>(), waterCollider);
                enemyController.target = player;
            }
        }
    }

    private void HideBoss()
    {
        spriteRenderer.enabled = false; // Hide the boss sprite
        hurtBox.enabled = false; // Disable the collider
        bossEffects.PlaySplashEffect(); // Play a visual effect for diving
    }

    private void ReappearBoss()
    {
        spriteRenderer.enabled = true; // Show the boss sprite
        hurtBox.enabled = true; // Re-enable the collider
        hasSpawnedEnemies = false; // Reset the spawn flag for the next phase
    }

    private void PerformRageMode()
    {
        if (previousState != BossState.RageMode)
        {
            bossEffects.PlayRoar();
        }
        if (!hasSpawnedEnemiesInRage)
        {
            // Spawn enemies only once in Rage Mode
            SpawnEnemies();
            hasSpawnedEnemiesInRage = true;
        }

        // Fast, aggressive attacks
        spriteRenderer.color = Color.red;
        rageBite = biteDelay / 1.5f; // Faster attacks
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Move towards the player if out of attack range
        if (distanceToPlayer > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            
            // Update the boss's position towards the player
            transform.position += (Vector3)(rageSpeed * Time.deltaTime * direction);
        }
        else
        {
            PerformGroundAttack(rageBite);
        }
    }

    protected void Die()
    {
        bossHealth.isDead = true;
        animator.SetTrigger("Die");

        Debug.Log($"{name} died");
        spriteRenderer.enabled = false;
        hurtBox.enabled = false;
        animator.enabled = false;
    }
    private IEnumerator PrintState()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log($"state is: {currentState}");
    }
}
