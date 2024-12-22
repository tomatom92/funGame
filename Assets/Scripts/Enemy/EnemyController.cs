using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using static CrocBoss;


public class EnemyController : CharacterBehaviour
{

    [Space]
    [Header("Character stats")]
    [SerializeField] float range;
    [SerializeField] private int damage = 1;
    Animator animator;
    AIPath aStar;
    AIDestinationSetter destinationSetter;
    public bool isTouching = false;
    private HPBar playerHpbar;
    private Transform player;
    //pathfinding
    [Space]
    [Header("Pathfinding")]
    public Vector2 enemyDir;
    public Transform homePoint;
    [SerializeField] public Transform target;
    [HideInInspector] public NavMeshAgent agent;
    

    protected override void Start()
    {
        playerHpbar = PlayerController.instance.playerHPBar;
        player = PlayerController.instance.transform;
        animator = GetComponent<Animator>();
        aStar = transform.GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    void FixedUpdate()
    {
        //checking if moving
        animator.SetBool("IsMoving", true);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        // Move towards the player if in detection range
        if (target != null)
        {
            if (distanceToPlayer < range)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                animator.SetFloat("moveX", direction.x);
                animator.SetFloat("moveY", direction.y);
                enemyDir = direction;

                //when in detection range go after target
                agent.SetDestination(target.position);

            }
            else if(homePoint != null)
            {
                 
                //if you dont have a home point, just always gun for player.
                agent.SetDestination(homePoint.position);
            }
        }

        
    }
    public void onDeath()
    {

    }
    private float waitToHurt = 2f;
    
    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
