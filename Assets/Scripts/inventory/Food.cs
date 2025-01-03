using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : InteractableObject
{

    [SerializeField] private ConsumableClass foodData;



    protected override void Start()
    {
        base.Start();
        foodData.itemName = name;
        foodData.icon = GetComponent<SpriteRenderer>().sprite;
    }
    protected override void OnInteract()
    {
        //play eat noise and heal
        PlayerController.instance.playerHPBar.AddHeart(foodData.healthAdded);

    }


}
