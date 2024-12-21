using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static CrocBoss;


public class EnemyController : CharacterBehaviour
{

    [SerializeField] public ContactFilter2D contactFilter;
    [SerializeField] float range;
    [SerializeField] private int damage = 1;
    Animator animator;
    AIPath aStar;
    AIDestinationSetter destinationSetter;

    public bool isTouching = false;

    private HPBar playerHpbar;
    private Transform player;
    public Vector2 enemyDir;
    public Transform homePoint;

    void Start()
    {
        isStunned = false;
        playerHpbar = PlayerController.instance.playerHPBar;
        player = PlayerController.instance.transform;
        animator = GetComponent<Animator>();
        aStar = transform.GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
    }


    void FixedUpdate()
    {
        //checking if moving
        animator.SetBool("IsMoving", true);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        // Move towards the player if in detection range
        if (distanceToPlayer < range) 
        {
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            enemyDir = direction;

            //when in detection range go after plaeyr
            destinationSetter.target = player;

        }
        else
        {
            //when not in detection range go back to home point
            destinationSetter.target = homePoint;


        }

        
    }
    private void Update()
    {
       
        //long touch damage
        if (isTouching)
        {
            waitToHurt -= Time.deltaTime;
            if (waitToHurt <= 0)
            {
                playerHpbar.RemoveHeart(damage);
                waitToHurt = 2f;
            }
        }
    }
    private float waitToHurt = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //player collision
        if (collision.CompareTag("Player") && !PlayerController.instance.isStunned)
        {
            if (collision.CompareTag("Player")) isTouching = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isTouching = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isTouching = false;
    }
    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
