using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    protected bool z_interacted = false;
    [SerializeField] protected bool z_disappear;
    protected AudioSource audioSource;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Start()
    {
        base.Start();
    }

    public SpriteRenderer GetSpriteRenderer() {  return spriteRenderer; }
    public Collider2D GetCollider() {  return z_collider; }
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

        Debug.Log($"collected {item.itemName}");
        InventoryController.instance.PickupAnimation(item);
        audioSource.Play();

        if (animator != null)
            animator.enabled = false;

        spriteRenderer.enabled = false;
        z_collider.enabled = false;
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
        //Debug.Log($"entered trigger {collision.gameObject.name}");

    }
    
    
}
