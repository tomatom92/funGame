using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wand : InteractableObject
{
    // Start is called before the first frame update
    [SerializeField] private ToolClass wandData;

    [SerializeField] private int wandNum;
    

    protected override void Start()
    {
        base.Start();
        wandData.toolID = wandNum;
        wandData.itemName = name;
        wandData.icon = GetComponent<SpriteRenderer>().sprite;
    }

    protected override void OnInteract()
    {

        Collect(wandData, 1);

    }

}
