using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : InteractableObject
{
    [SerializeField] private QuestClass orbData;
    private int orbID;


    protected override void Start()
    {
        base.Start();
        orbID = orbData.QuestItemID;
        orbData.itemName = name;
        orbData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        Collect(orbData, 1);
    }
}
