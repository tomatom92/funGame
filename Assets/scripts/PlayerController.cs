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
    [SerializeField] private float damage;
    [SerializeField] private float PROJECTILE_BASE_SPEED = 1.0f;
    [SerializeField] private float interactDist;
    private float attackCooldown = 0; //tracks cooldown
    [SerializeField, Range(0, 1)] float fireDelay = 1f; // Cooldown duration (time between attacks)

    public float moveSpeed = 5f;
    public float sprintSpeed;
    [HideInInspector] public bool isSprinting;

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

    [Space]
    [Header("Prefabs")]
    public GameObject bullet;

    //movement inputs go here
    private Vector2 movement;

    private void Awake()
    {
        instance = this;
        float sprintSpeed = moveSpeed * 1.5f;
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
        playerHPBar = GetComponent<HPBar>();
        animator = GetComponent<Animator>();
        //weaponGraphics = transform.GetChild(0).GetComponent<WeaponGraphics>();

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


    }
    private void OnDisable()
    {
        sprint.Disable();
        move.Disable();
        fire.Disable();
        interact.Disable();
    }


    private void Move(InputAction.CallbackContext context)
    {

    }


    private void Fire(InputAction.CallbackContext context)
    {
        // Check if the cooldown has finished
        if (attackCooldown <= 0)
        {
            // Reset the cooldown timer
            attackCooldown = fireDelay;

            // Direction and normalizing it
            Vector2 shootingDir = playerDir;
            shootingDir.Normalize();

            // Create projectile
            GameObject projectile = Instantiate(bullet, transform.position, Quaternion.identity);
            ProjectileBehaviour projectileScript = projectile.GetComponent<ProjectileBehaviour>();
            projectileScript.SetPlayerProj(true);

            // Shooting projectile in shooting direction
            projectile.GetComponent<Rigidbody2D>().velocity = shootingDir * PROJECTILE_BASE_SPEED;
            projectile.transform.Rotate(0, 0, Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg);

            // Destroy the projectile after 2 seconds
            Destroy(projectile, 2f);
        }
        else
        {
            Debug.Log("Attack on cooldown. Time remaining: " + attackCooldown);
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
}