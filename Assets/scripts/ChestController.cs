using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : InteractableObject
{
    [SerializeField] private int chestID;
    [SerializeField] private InteractableObject item;
    [SerializeField] private ItemClass chestItemClass;
    public bool isChestOpen = false;
    [HideInInspector] public Collider2D chestCollider;



    protected override void Update()
    {
        base.Update();
        chestCollider = z_collider;
        
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        if (item != null)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
            item.GetComponent<Collider2D>().enabled = false;
            item.transform.position = transform.position;
        }

    }


    protected override void OnInteract()
    {
        
        if (!isChestOpen)
        {
            item.GetSpriteRenderer().enabled = true;
            item.GetCollider().enabled = true;

            animator.SetBool("IsOpen", true);
            chestCollider.enabled = false;
            audioSource.Play();
        }
    }
    IEnumerator WaitSeconds(int seconds)
    {

        //yield on a new YieldInstruction that waits for input # seconds.
        yield return new WaitForSeconds(seconds);

    }
}
