using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("items")]
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private ItemClass equippedItem;

    [Space]
    [Header("ui")]
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject orbSlotHolder;
    [SerializeField] private GameObject pauseMenu;
    [Space]
    [Header("pickup effect")]
    [SerializeField] private GameObject pickupAnimationPrefab;

    
    public static InventoryController instance;
    public bool isPaused =false;

    public List<InventorySlot> items = new();
    public List<InventorySlot> orbs = new();
    private GameObject player;
    private GameObject[] slots;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = PlayerController.instance.gameObject;

        slots = new GameObject[slotHolder.transform.childCount];

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
        pauseMenu.SetActive(false);

        //Add(itemToAdd);
        //Remove(itemToRemove);
        //EquipItem(equippedItem);

    }
    private void TogglePauseMenu()
    {
        // Toggle pause state
        isPaused = !isPaused;

        // Show or hide the pause menu
        pauseMenu.SetActive(isPaused);

        // Freeze or unfreeze the game
        Time.timeScale = isPaused ? 0 : 1;
    }
    public void ResumeGame()
    {
        // Resume the game and hide the pause menu
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePauseMenu();
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

    public void PickupAnimation(ItemClass item)
    {

        if (item != null)
        {
            Debug.Log("item not null");
            GameObject pickup = Instantiate(pickupAnimationPrefab, player.transform.position, Quaternion.identity);
            ItemAnim itemAnim = pickup.transform.GetChild(0).GetComponent<ItemAnim>();
            pickup.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.icon;

            itemAnim.name = item.itemName;
            //Destroy(pickup, 3f);
        }
        else
        {
            Debug.Log("item null");
        }
    }
}
