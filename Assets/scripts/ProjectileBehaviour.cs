using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject fireEffectPrefab; // Assign your fire prefab in the Inspector
    private FireEffect fireEffect;

    [Header("bullet stats")]
    [SerializeField] private float knockBackForce = 10f;
    [SerializeField] private int projectileDamage = 1;
    public float projectileSpeed = 1;
    private bool isPlayerProj;
    private bool isEnemyProj;
    [Space]
    [Header("specific projectile stats")]
    [SerializeField] private float redFireInterval = 0.2f; // Interval between each fire effect spawn
    [SerializeField] private float fireDuration = 3.0f; // Duration the fire exists


    Animator animator;
    Collider2D bulletCollider;




    private void OnDestroy()
    {
        CancelInvoke();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bulletCollider = GetComponent<Collider2D>();
    }

    public void SetPlayerProj(bool input)
    {
        isPlayerProj = input;
    }
    public void SetEnemyProj(bool input)
    {
        isEnemyProj = input;
    }
    private IEnumerator HitboxExtender()
    {
        yield return new WaitForSeconds(0.2f);
        bulletCollider.enabled = false;
    }
    public void DestroyProjectile()
    {
        animator.SetTrigger("hit");
        HitboxExtender();
        float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animTime + 0.5f);
    }

    public void ApplyElementalEffect(GameObject projectile, OrbUi.Orb activeOrbType)
    {
        switch (activeOrbType)
        {
            case OrbUi.Orb.Red:
                
                InvokeRepeating(nameof(SpawnFire), 0f, redFireInterval);
                break;

            case OrbUi.Orb.Yellow:

                break;

            case OrbUi.Orb.Green:

                break;

            case OrbUi.Orb.Blue:

                break;

            case OrbUi.Orb.none:
                // Default behavior
                break;
        }
    }

    public void SpawnFire()
    {
        // Position the fire effect behind the projectile
        Vector3 spawnPosition = transform.position;

        Debug.Log("SpawnFireBehindProjectile");

        // Instantiate the fire effect
        GameObject fireEffect = Instantiate(fireEffectPrefab, spawnPosition, Quaternion.identity);

        // Destroy the fire effect after the specified duration
        Destroy(fireEffect, fireDuration);

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
                Debug.Log($"{collision.transform.GetChild(0).name} ENEMY HIT WITH PROJECTILE!");

                //enemy Knockback
                Vector2 direction = (enemyPos - bulletPos).normalized;
                rb.AddForce(direction * knockBackForce);

                //stun

                //animation
                DestroyProjectile();

            }//colliding with players
            else if (collision.CompareTag("Player") && !isPlayerProj)
            {
                hpBar.RemoveHeart(projectileDamage);
                DestroyProjectile();

                Debug.Log("Player HIT WITH PROJECTILE!");
            }
            //colliding with walls
            if (collision.CompareTag("EnemyCantWalkHere"))
            {
                DestroyProjectile();

            }
            //colliding with boss
            if (collision.CompareTag("Boss"))
            {
                Debug.Log($"hit {collision.name}");
                //collision.GetComponent<BossHealth>().TakeDamage(bulletDamage);
                collision.GetComponent<BossHealth>().TakeDamage(20);
                //animations                
                DestroyProjectile();

            }

        }
    }
}
