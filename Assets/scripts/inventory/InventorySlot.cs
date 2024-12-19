using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel.Design;
using Unity.VisualScripting;

[System.Serializable]
public class InventorySlot
{

    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }
    public InventorySlot(ItemClass _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }
    
    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }

    public void AddQuantity(int _quantity) { quantity += _quantity; }
    public void SubQuantity(int _quantity) { quantity -= _quantity; }
    


}
   
