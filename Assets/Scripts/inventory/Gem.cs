using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : InteractableObject
{
    [SerializeField] private QuestClass gemData;
    [SerializeField] private int gemID;


    protected override void Start()
    {
        base.Start();
        gemData.QuestItemID = gemID;
        gemData.itemName = name;
        gemData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        Collect(gemData, 1);
    }
}
