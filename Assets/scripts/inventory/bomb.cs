using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Bomb : InteractableObject
{

    
    [SerializeField]
    public MiscClass bombData;
    

    public void Collect()
    {
        Destroy(gameObject);
        Debug.Log($"entered trigger {GetComponent<Collider>().gameObject.name}");
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        InventoryController.instance.Add(bombData);
        Debug.Log("collected");
        gameObject.SetActive(false);


    }
}
