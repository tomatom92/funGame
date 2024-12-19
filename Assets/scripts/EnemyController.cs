using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class EnemyController : CharacterBehaviour
{

    [SerializeField] public ContactFilter2D contactFilter;
    [SerializeField] float range;
    [SerializeField] private int damage = 1;
    HPBar enemyHpbar;
    Animator animator;

    public bool isTouching = false;

    private HPBar playerHpbar;
    private GameObject Player;
    public Vector2 enemyDir;

    void Start()
    {
        isStunned = false;
        playerHpbar = PlayerController.instance.playerHPBar;
        Player = PlayerController.instance.gameObject;
        animator = GetComponent<Animator>();
        enemyHpbar = transform.GetComponent<HPBar>();
    }


    void FixedUpdate()
    {
        //checking if moving
        animator.SetBool("IsMoving", true);

        float xDirection = (Player.transform.position.x - transform.position.x);
        animator.SetFloat("moveX", xDirection);

        float yDirection = (Player.transform.position.y - transform.position.y);
        animator.SetFloat("moveY", yDirection);

        enemyDir = new Vector2(xDirection, yDirection);

        
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
}
