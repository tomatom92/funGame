using System.Collections;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private float fireDuration = 3.0f; // Duration the fire exists
    [SerializeField] private float damagePerSecond = 5.0f; // Damage dealt per second
    [SerializeField] private GameObject fireEffectPrefab;


    public void SpawnFireBehindProjectile()
    {
        // Position the fire effect behind the projectile
        Vector3 spawnPosition = transform.position - transform.forward * 0.5f; // Adjust the distance as needed
        Quaternion spawnRotation = transform.rotation;

        Debug.Log("SpawnFireBehindProjectile");

        // Instantiate the fire effect
        GameObject fireEffect = Instantiate(fireEffectPrefab, spawnPosition, spawnRotation);

        // Destroy the fire effect after the specified duration
        Destroy(fireEffect, fireDuration);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage
            HPBar enemy = collision.GetComponent<HPBar>();
            if (enemy != null)
            {
                enemy.RemoveHeart(1);
            }
        }
    }
}
