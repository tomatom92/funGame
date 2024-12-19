using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public int projectileDamage = 1;
    private bool isPlayerProj;
    private bool isEnemyProj;
    [SerializeField] private float knockBackForce = 10f;
    Animator animator;
    public GameObject bulletOwner;
    public float bulletDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlayerProj(bool input)
    {
        isPlayerProj = input;
    }
    public void SetEnemyProj(bool input)
    {
        isEnemyProj = input;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            HPBar hpBar = collision.GetComponent<HPBar>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            //bullet position
            Vector2 bulletPos = gameObject.transform.position;

            //colliding with enemies
            if (collision.CompareTag("Enemy") && !isEnemyProj)
            {
                //saving the game object of the collision
                GameObject enemy = collision.gameObject;
                Vector2 enemyPos = rb.transform.position;
                hpBar.RemoveHeart(projectileDamage);
                Debug.Log("ENEMY HIT WITH PROJECTILE!");

                //enemy Knockback
                Vector2 direction = (enemyPos - bulletPos).normalized;
                rb.AddForce(direction * knockBackForce);

                //stun
                enemy.GetComponent<EnemyController>().isStunned = true;

                //animation
                animator.SetTrigger("hit");

                float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
                Destroy(gameObject, animTime + 0.5f);

            }//colliding with players
            else if (collision.CompareTag("Player") && !isPlayerProj)
            {
                hpBar.RemoveHeart(projectileDamage);
                animator.SetTrigger("hit");
                float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
                Destroy(gameObject, animTime + 0.5f);
                Debug.Log("Player HIT WITH PROJECTILE!");
            }
            //colliding with walls
            if (collision.CompareTag("EnemyCantWalkHere"))
            {
                animator.SetTrigger("hit");
                float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
                Destroy(gameObject, animTime + 0.5f);
            }
            //colliding with boss
            if (collision.CompareTag("Boss"))
            {
                Debug.Log("hit boss");
                //collision.GetComponent<BossHealth>().TakeDamage(bulletDamage);
                collision.GetComponent<BossHealth>().TakeDamage(20);
                //animations
                animator.SetTrigger("hit");
                float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
                //destroy
                Destroy(gameObject, animTime + 0.5f);
            }
            
        }
    }
}
