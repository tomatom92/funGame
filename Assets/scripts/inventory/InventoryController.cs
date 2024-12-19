using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private GameObject slotHolder;
    private GameObject player;
    [SerializeField] private ItemClass equippedItem;
    
    public static InventoryController instance;
    public bool opened =false;
    //[SerializeField] private GameObject inventoryPanel;


    public List<InventorySlot> items = new();

    private GameObject[] slots;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && opened == false)
        {
            slotHolder.SetActive(true);
            opened = true;
            RefreshUI();
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            slotHolder.SetActive(false);
            opened = false;
        }

        
    }
    private void Awake()
    {
        
        instance = this;
    }
    private void Start()
    {

        slots = new GameObject[slotHolder.transform.childCount];

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        

        Add(itemToAdd);
        Remove(itemToRemove);
        
        EquipItem(equippedItem);

        
    }
    public void EquipItem(ItemClass itemToEquip)
    {
        equippedItem = itemToEquip;
    }
    public void RefreshUI()
    {
        for (int i = 0;i < slots.Length;i++)
        {
            try
            {
                //child 0 is the icon
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().icon;
                //child 1 is the stackSizeText
                //setting stack size
                if(items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                else
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                //child 3 is the labelText
                //setting labelText
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = items[i].GetItem().itemName;
            }
            catch
            {
                //icon
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled=false;
                //stack size
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                //labelText
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";

            }
        }
    }
    public bool Add(ItemClass item)
    {

        //is the item in the inventory? if yes then add to stack

        InventorySlot slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
        { 
            slot.AddQuantity(1);
        }
        else
        {
            if(items.Count < slots.Length)
                items.Add(new InventorySlot(item, 1));
            else
                return false;
        }
        //items.Add(item);
        RefreshUI();
        return true;


        // the item is not in the dictionary? create a new inventory item and then store it in list and dict so next time we can just increase stack size
        
    }
    public bool Remove(ItemClass item)
    {
        //items.Remove(item);

        InventorySlot temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(1);
            }
            else
            {
                InventorySlot slotToRemove = new InventorySlot();
                foreach (InventorySlot slot in items)
                {
                    if (slot.GetItem() == item)
                    {
                        slotToRemove = slot;
                        break;
                    }
                }
                items.Remove(slotToRemove);

            }
        }
        else
        {
            return false;
        }
            
        RefreshUI();
        return true;
    }
    public InventorySlot Contains(ItemClass item)
    {
        foreach (InventorySlot slot in items)
        {
            if(slot.GetItem() == item)
                return slot;
        }
        return null;
    }
}
