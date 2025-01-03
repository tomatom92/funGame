using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Cinemachine;

public class PlayerController : CharacterBehaviour
{
    [SerializeField] PlayerInputActions playerControls;
    //[SerializeField] CinemachineVirtualCamera playerCam;
    public static PlayerController instance;
    private Animator animator;


    [Space]
    [Header("Character stats")]
    [SerializeField] private float meleeSpeed;
    [SerializeField] private float bombDelay = 1f;
    [SerializeField] private float damage;
    [SerializeField] private float PROJECTILE_BASE_SPEED = 1.0f;
    [SerializeField] private float interactDist;
    public float moveSpeed = 5f;
    public float sprintSpeed;
    [HideInInspector] public bool isSprinting;

    private float attackCooldown = 0; //tracks attack cooldown
    private float currentOrbCooldown = 0f; // Tracks the cooldown of the currently active orb
    private float bombCooldown = 0f; // Tracks the cooldown of bomb

    [Header("bulletCooldowns")]
    [SerializeField, Range(0, 1)] float fireDelay = 1f; // Cooldown duration (time between attacks)

    public float defaultCooldown = 1f;
    public float redCooldown = 0.6f;
    public float yellowCooldown = 1.2f;
    public float greenCooldown = 1.2f;
    public float blueCooldown = 1f;

    private Dictionary<OrbUi.Orb, float> bulletCooldowns;

    [Space]
    [Header("info")]
    public Vector2 playerDir;


    [HideInInspector]
    public HPBar playerHPBar;

    private InputAction move;
    private InputAction fire;
    private InputAction interact;
    private InputAction sprint;
    private InputAction sprintRelease;
    private InputAction bomb;

    [Space]
    [Header("Prefabs")]
    public GameObject normalBullet;
    public GameObject redBullet;
    public GameObject yellowBullet;
    public GameObject greenBullet;
    public GameObject blueBullet;
    public GameObject bombPrefab;

    //movement inputs go here
    private Vector2 movement;

    protected override void Awake()
    {
        instance = this;
        float sprintSpeed = moveSpeed * 1.5f;
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
        playerHPBar = GetComponent<HPBar>();
        animator = GetComponent<Animator>();

        bulletCooldowns = new Dictionary<OrbUi.Orb, float>
        {
            { OrbUi.Orb.none, defaultCooldown },    // Default projectile
            { OrbUi.Orb.Red, redCooldown },     // Red orb fires faster
            { OrbUi.Orb.Yellow, yellowCooldown },  // Yellow orb fires the fastest
            { OrbUi.Orb.Green, greenCooldown },   // Green orb fires slower
            { OrbUi.Orb.Blue, blueCooldown }     // Blue orb normal speed
        };
    }

    private void Update()
    {
        
        // getting moveement inputs and sending to animator

        movement = move.ReadValue<Vector2>();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        float x = animator.GetFloat("Horizontal");
        float y = animator.GetFloat("Vertical");

        playerDir = new(x, y);

        //flipping sprite
        if (animator.GetFloat("Horizontal") > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        //cooldown timer
        // Decrease the cooldown timer if it's above 0
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        //bomb timer
        if (bombCooldown > 0)
        {
            bombCooldown -= Time.deltaTime;
        }


    }
    private Rigidbody2D rb;
    void FixedUpdate()
    {
        //if dead then skip logic
        if (isDead) return;
        //actual movement here
        //sprinting
        if (isSprinting) { rb.MovePosition(rb.position + movement.normalized * sprintSpeed * Time.fixedDeltaTime); }
        //walking
        else { rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime); }



    }



    private void OnEnable()
    {

        move = playerControls.Player.Move;
        move.Enable();
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
        sprint = playerControls.Player.Sprint;
        sprint.Enable();
        sprint.performed += Sprint;
        sprintRelease = playerControls.Player.SprintRelease;
        sprintRelease.Enable();
        sprintRelease.performed += SprintRelease;
        bomb = playerControls.Player.Bomb;
        bomb.Enable();
        bomb.performed += Bomb;



    }
    private void OnDisable()
    {
        sprint.Disable();
        move.Disable();
        fire.Disable();
        interact.Disable();
        bomb.Disable();
    }


    private void Move(InputAction.CallbackContext context)
    {

    }

    private void Fire(InputAction.CallbackContext context)
    {
        // Determine the projectile type based on the active orb
        GameObject projectilePrefab = GetProjectilePrefab();

        // Get the current active orb type from the OrbUi instance
        OrbUi.Orb activeOrbType = OrbUi.instance.GetActiveOrbType();

        // Determine the cooldown for the active orb
        if (bulletCooldowns.TryGetValue(activeOrbType, out float cooldown))
        {
            currentOrbCooldown = cooldown;
        }
        else
        {
            Debug.LogWarning($"Cooldown not found for orb type: {activeOrbType}. Using default.");
            currentOrbCooldown = 1.0f; // Fallback to default cooldown
        }

        // Check if the cooldown has finished
        if (attackCooldown <= 0)
        {
            // Reset cooldown timer
            attackCooldown = currentOrbCooldown;

            // Direction and normalization
            Vector2 shootingDir = playerDir;
            shootingDir.Normalize();

            // Create the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            //Debug.Log(projectile.name);
            
            ProjectileBehaviour prodBehav = projectile.GetComponent<ProjectileBehaviour>();
            //set projectile variables
            float orbSpeed = prodBehav.projectileSpeed;
            prodBehav.SetPlayerProj(true);
            //elemental effect
            prodBehav.ApplyElementalEffect(projectile, activeOrbType);

            //speed up and rotate towards shooting direction.
            projectile.GetComponent<Rigidbody2D>().velocity = shootingDir * PROJECTILE_BASE_SPEED * orbSpeed;
            projectile.transform.Rotate(0, 0, Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg);

            // Destroy the projectile after 2 seconds
            Destroy(projectile, 2f);
        }
        else
        {
            Debug.Log("Attack on cooldown. Time remaining: " + attackCooldown);
        }
    }

    private GameObject GetProjectilePrefab()
    {
        OrbUi.Orb activeOrb = OrbUi.instance?.GetActiveOrbType() ?? OrbUi.Orb.none;

        switch (activeOrb)
        {
            case OrbUi.Orb.Red:
                return redBullet;
            case OrbUi.Orb.Yellow:
                return yellowBullet;
            case OrbUi.Orb.Green:
                return greenBullet;
            case OrbUi.Orb.Blue:
                return blueBullet;
            default:
                return normalBullet;
        }
    }
    private void Interact(InputAction.CallbackContext context)
    {

        //calling interactableobject method
        //Debug.Log("interact");

        float x = animator.GetFloat("Horizontal");
        float y = animator.GetFloat("Vertical");

        //right
        if (x > 0)
        {


            //raycast
            RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, interactDist, LayerMask.GetMask("Raycast"));
            //if the ray hits something
            if (ray.collider != null)
            {
                //getting the grid position of the raycast
                Vector3Int rayCellPos = MapManager.Instance.map.WorldToCell(ray.collider.transform.position);
                //draw test line
                Debug.Log($"hit {ray.collider.name} at {rayCellPos}");
                Debug.DrawLine(gameObject.transform.position, ray.collider.transform.position, Color.red, 2f);
                // checking for door
                if (ray.collider.CompareTag("Door"))
                {
                    ray.transform.GetComponent<DoorManager>().OpenDoorWithKey();
                }
            }
        }
        //left
        else if (x < 0)
        {
            //raycast
            RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, -gameObject.transform.right, interactDist, LayerMask.GetMask("Raycast"));
            //if the ray hits something

            if (ray)
            {
                Vector3Int rayCellPos = MapManager.Instance.map.WorldToCell(ray.collider.transform.position);

                Debug.Log($"hit {ray.collider.name} at {rayCellPos}");
                Debug.DrawLine(gameObject.transform.position, ray.collider.transform.position, Color.red, 2f);
                // checking for door
                if (ray.collider.CompareTag("Door"))
                {
                    ray.transform.GetComponent<DoorManager>().OpenDoorWithKey();
                }

            }
        }
        //up
        else if (y > 0)
        {
            //raycast
            RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.up, interactDist, LayerMask.GetMask("Raycast"));
            //if the ray hits something

            if (ray)
            {
                Vector3Int rayCellPos = MapManager.Instance.map.WorldToCell(ray.collider.transform.position);
                Debug.Log($"hit {ray.collider.name} at {rayCellPos}");
                Debug.DrawLine(gameObject.transform.position, ray.collider.transform.position, Color.red, 2f);
                // checking for door
                if (ray.collider.CompareTag("Door"))
                {
                    ray.transform.GetComponent<DoorManager>().OpenDoorWithKey();
                }
            }
        }
        //down
        else if (y < 0)
        {
            //raycast
            RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, -gameObject.transform.up, interactDist, LayerMask.GetMask("Raycast"));
            //if the ray hits something

            if (ray)
            {
                Vector3Int rayCellPos = MapManager.Instance.map.WorldToCell(ray.collider.transform.position);
                Debug.Log($"hit {ray.collider.name} at {rayCellPos}");
                Debug.DrawLine(gameObject.transform.position, ray.collider.transform.position, Color.red, 2f);
                // checking for door
                if (ray.collider.CompareTag("Door"))
                {
                    ray.transform.GetComponent<DoorManager>().OpenDoorWithKey();
                }
            }
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        //Debug.Log("sprinting");
        isSprinting = true;
    }
    private void SprintRelease(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }
    private void Bomb(InputAction.CallbackContext context)
    {
        //create bomb at position
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
        if(bombCooldown <= 0)
        {
            bombCooldown = bombDelay;
        }
    }
}