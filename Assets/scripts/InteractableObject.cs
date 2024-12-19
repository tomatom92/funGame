using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    protected bool z_interacted = false;
    [SerializeField] protected bool z_disappear;
    protected AudioSource audioSource;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D itemCollider;

    protected virtual void Awake()
    {
        itemCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }
    /*
     * Collect function
     * input a scriptable object item and amount
     * adds item to inventory
     * plays noise
     * disables graphics
     */
    public void Collect(ItemClass item, int amount)
    {
        if (item == null) return;

        for (int i = 0; i < amount; i++)
        {
            InventoryController.instance.Add(item);
        }

        Debug.Log($"collected {gameObject.name}");
        audioSource.Play();

        if (animator != null)
            animator.enabled = false;

        spriteRenderer.enabled = false;
        itemCollider.enabled = false;
    }
    protected virtual void OnInteract()
    {
        Debug.Log("interacted with " + name);
        if (!z_interacted)
        {
            z_interacted = true;
            //Debug.Log("z_interacted = true");
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) return;
        Debug.Log($"entered trigger {collision.gameObject.name}");

    }
}
