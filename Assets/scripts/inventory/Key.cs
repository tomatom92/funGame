using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Key : InteractableObject
{
    //[SerializeField] public GameObject[] keys;
    [SerializeField] private ToolClass keyData;
    
    [SerializeField] private int keyNum;
    

    protected override void Awake()
    {
        base.Awake();
        keyData.toolID = keyNum;
        keyData.itemName = name;
        keyData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        Collect(keyData, 1);

    }
}
