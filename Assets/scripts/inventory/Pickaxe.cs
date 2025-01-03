using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : InteractableObject
{
    [SerializeField] private ToolClass pickData;
    [SerializeField] private int keyNum;


    protected override void Start()
    {
        base.Start();
        pickData.toolID = keyNum;
        pickData.itemName = name;
        pickData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        Collect(pickData,1);
    }
}
