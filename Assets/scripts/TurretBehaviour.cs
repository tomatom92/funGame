using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretBehaviour : MonoBehaviour
{


    public GameObject bullet;
    public Direction direction;
    public float shootInterval = 0.5f;
    public float detectionRange = 2f;
    public float bulletSpeed;
    Vector2 shootingDir;

    private Transform player;
    private bool inRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Locate player by tag
    }
    private void Update()
    {
        if (player == null) return;

        // Check distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        inRange = distanceToPlayer <= detectionRange;        
        if (inRange && !IsInvoking(nameof(Shoot))) // Start shooting if the player is near
        {
            InvokeRepeating(nameof(Shoot), 0f, shootInterval);
        }
        else if (!inRange) // Stop shooting if the player moves out of range
        {
            CancelInvoke(nameof(Shoot));
        }

    }
    private void Shoot()
    {
        //direction and normalizing it
        if (direction == Direction.Towards)
        {
            shootingDir = (player.position - transform.position).normalized;
        }
        else
        {
            shootingDir = direction.ToVector2();
            shootingDir.Normalize();
        }

        
        //create projectile
        GameObject projectile = Instantiate(bullet, transform.position, Quaternion.identity);
        ProjectileBehaviour projectileScript = projectile.GetComponent<ProjectileBehaviour>();
        projectileScript.SetEnemyProj(true);

        //shooting projectile in shooting direction
        projectile.GetComponent<Rigidbody2D>().velocity = shootingDir * bulletSpeed;

        //rotation optional
        //projectile.transform.Rotate(0, 0, Mathf.Atan2(shootingDir.y, shootingDir.x) * Mathf.Rad2Deg);
    }
    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Towards
}
public static class DirectionExtensions
{
    public static Vector2 ToVector2(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2.up; // (0, 1)
            case Direction.Down:
                return Vector2.down; // (0, -1)
            case Direction.Left:
                return Vector2.left; // (-1, 0)
            case Direction.Right:
                return Vector2.right; // (1, 0)
            default:
                return Vector2.zero; // Fallback
        }
    }
}
