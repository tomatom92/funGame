using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : InteractableObject
{
    [SerializeField] private QuestClass orbData;
    [SerializeField] private int orbID;


    protected override void Awake()
    {
        base.Awake();
        orbData.QuestItemID = orbID;
        orbData.itemName = name;
        orbData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        Collect(orbData, 1);
    }
}
