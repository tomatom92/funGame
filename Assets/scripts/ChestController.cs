using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : InteractableObject
{
    [SerializeField] private int chestID;
    [SerializeField] private InteractableObject item;
    [SerializeField] private ItemClass chestItemClass;
    public bool isChestOpen = false;


    protected override void Awake()
    {
        base.Awake();
        item.gameObject.SetActive(false);
        item.transform.position = transform.position;
    }
    
    protected override void OnInteract()
    {
        
        if (!isChestOpen)
        {
            item.gameObject.SetActive(true);
            animator.SetBool("IsOpen", true);
            GetComponent<BoxCollider2D>().enabled = false;
            audioSource.Play();
        }
    }
    IEnumerator WaitSeconds(int seconds)
    {

        //yield on a new YieldInstruction that waits for input # seconds.
        yield return new WaitForSeconds(seconds);

    }
}
