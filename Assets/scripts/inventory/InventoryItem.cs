using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryItem : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int stackSize;
    public ItemData itemData;

    

    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddToStack();

    }

    


    public void AddToStack()
    {
        stackSize++;

    }
    public void RemoveFromStack()
    {
        stackSize--;

    }
    
}
