using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
 
    private Collider2D biteHitbox; // Assign the BiteHitbox in the Inspector
    
    private void Start()
    {
        biteHitbox = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if it's the player
        {
            HPBar hpBar = collision.GetComponent<HPBar>();
            if (hpBar != null)
            {
                hpBar.RemoveHeart(1); // Adjust damage as needed
            }
        }
    }
    // Called by the animation event to activate the hitbox
    public void EnableBiteHitbox()
    {
        biteHitbox.enabled = true;
    }

    // Called by the animation event to deactivate the hitbox
    public void DisableBiteHitbox()
    {
        biteHitbox.enabled = false;
    }

}
