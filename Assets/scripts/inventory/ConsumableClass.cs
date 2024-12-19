using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{

    [Header("consumable")]
    //data specific to consumable class
    public float healthAdded;
    public override ConsumableClass GetConsumable() { return this; }
}
