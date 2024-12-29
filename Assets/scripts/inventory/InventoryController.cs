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
    [SerializeField] private GameObject orbSlotHolder;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private ItemClass equippedItem;
    
    public static InventoryController instance;
    public bool opened =false;

    public List<InventorySlot> items = new();
    public List<InventorySlot> orbs = new();
    public OrbUi orbUi;

    private GameObject player;
    private GameObject[] slots;
    private void Start()
    {

        slots = new GameObject[slotHolder.transform.childCount];

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();



        //Add(itemToAdd);
        //Remove(itemToRemove);
        
        //EquipItem(equippedItem);

    }
    private void Awake()
    {
        instance = this;
        orbUi = OrbUi.instance;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && opened == false)
        {
            slotHolder.SetActive(true);
            inventoryObject.SetActive(true);
            opened = true;
            RefreshUI();
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            slotHolder.SetActive(false);
            inventoryObject.SetActive(false);
            opened = false;
        }

        
    }
    
    public void EquipItem(ItemClass itemToEquip)
    {
        equippedItem = itemToEquip;
    }
    public void RefreshUI()
    {
        //inventory slots
        for (int i = 0;i < slots.Length;i++)
        {
            try
            {
                //child 0 is slot image
                //child 1 is the icon
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = items[i].GetItem().icon;
                //child 2 is the stackSizeText
                //setting stack size
                if(items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                else
                    slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                //child 3 is the labelText
                //setting labelText
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = items[i].GetItem().itemName;
            }
            catch
            {
                //icon is 1
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled=false;
                //stack size is 2
                slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                //labelText is 3
                slots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";

            }
        }
        
    }
    public bool Add(ItemClass item)
    {
        if (item.IsOrb() && item != null)
        {
            // Handle orbs
            InventorySlot orbSlot = Contains(item);
            if (orbSlot != null)
            {
                orbSlot.AddQuantity(1); // Increase quantity if stackable
            }
            else
            {
                if (orbs.Count < slots.Length)
                    orbs.Add(new InventorySlot(item, 1)); // Add new orb slot
                else
                    return false; // Inventory full
            }
        }
        else if (item is QuestClass)
        {
            // Handle non-orb quest items
            InventorySlot questSlot = Contains(item);
            if (questSlot != null && item.isStackable)
            {
                questSlot.AddQuantity(1); // Increase quantity if stackable
            }
            else
            {
                if (items.Count < slots.Length)
                    items.Add(new InventorySlot(item, 1)); // Add new slot for quest item
                else
                    return false; // Inventory full
            }
        }
        else
        {
            // Handle regular items
            InventorySlot slot = Contains(item);
            if (slot != null && slot.GetItem().isStackable)
            {
                slot.AddQuantity(1); // Increase quantity if stackable
            }
            else
            {
                if (items.Count < slots.Length)
                    items.Add(new InventorySlot(item, 1)); // Add new slot for regular item
                else
                    return false; // Inventory full
            }
        }

        RefreshUI();
        return true;
    }


    public bool Remove(ItemClass item)
    {
        if (item.IsOrb() && item != null) // If item is an orb
        {
            InventorySlot orbSlot = ContainsOrb(item);
            if (orbSlot != null)
            {
                if (orbSlot.GetQuantity() > 1)
                {
                    orbSlot.SubQuantity(1);
                }
                else
                {
                    orbs.Remove(orbSlot); // Remove orb entirely
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Handle normal item logic
            InventorySlot temp = Contains(item);
            if (temp != null)
            {
                if (temp.GetQuantity() > 1)
                {
                    temp.SubQuantity(1);
                }
                else
                {
                    items.Remove(temp); // Remove item
                }
            }
            else
            {
                return false;
            }
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
    public InventorySlot ContainsOrb(ItemClass item)
    {
        foreach (InventorySlot slot in orbs)
        {
            if (slot.GetItem() == item)
                return slot;
        }
        return null;
    }
}
