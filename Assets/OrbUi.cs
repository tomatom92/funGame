using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static Cinemachine.CinemachineFreeLook;
using System;

public class OrbUi : MonoBehaviour
{
    public GameObject[] orbSlotButtons;
    public GameObject activeOrbSlot;
    public QuestClass[] orbs;
    public static OrbUi instance;
    private Orb orbEnum;

    InventoryController inventory;
    QuestClass activeOrb;

    public enum Orb
    {
        Red,
        Yellow,
        Green,
        Blue,
        none
    }

    void Start()
    {
        instance = this;
        inventory = InventoryController.instance;
        orbEnum = Orb.none;
        // Add click listeners to each button
        for (int i = 0; i < orbSlotButtons.Length; i++)
        {
            int index = i; // Capture the index for the button
            if (orbSlotButtons[i].TryGetComponent<Button>(out var button))
            {
                button.onClick.AddListener(() => OnOrbSlotClicked(index));
            }
            else
            {
                Debug.LogError($"Orb slot {i} is missing a Button component!");
            }
        }
    }
    private Dictionary<int, QuestClass> slotOrbMap = new Dictionary<int, QuestClass>();

    private void Update()
    {
        // Keep track of which slots are used
        slotOrbMap.Clear(); // Reset mapping for this frame

        // Assign orbs to slots dynamically
        int slotIndex = 0;
        foreach (QuestClass orb in orbs)
        {
            if (inventory.ContainsOrb(orb) != null && slotIndex < orbSlotButtons.Length)
            {
                GameObject slot = orbSlotButtons[slotIndex];
                slot.transform.GetChild(1).gameObject.SetActive(true);
                slot.transform.GetChild(3).gameObject.SetActive(true);
                slot.transform.GetChild(1).GetComponent<Image>().sprite = orb.icon;
                slot.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = orb.itemName;

                // Store the orb in the slot map
                slotOrbMap[slotIndex] = orb;

                slotIndex++;
            }
        }

        // Clear remaining slots
        for (int i = slotIndex; i < orbSlotButtons.Length; i++)
        {
            GameObject slot = orbSlotButtons[i];
            slot.transform.GetChild(1).gameObject.SetActive(false);
            slot.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public Orb GetActiveOrbType()
    {
        return orbEnum; // Returns the currently selected orb type
    }


    void OnOrbSlotClicked(int slotIndex)
    {
        if (slotOrbMap.ContainsKey(slotIndex))
        {
            QuestClass selectedOrb = slotOrbMap[slotIndex];
            Debug.Log($"Selected Orb: {selectedOrb.name}");

            // Set this orb as the active orb
            activeOrb = selectedOrb;

            // Update the orbEnum based on the selected orb
            orbEnum = GetOrbEnumFromQuestClass(selectedOrb);

            // Update visuals and weapon type
            SetWeaponType();
        }
        else
        {
            Debug.Log($"Slot {slotIndex} is empty or no orb assigned.");
        }
    }

    void SelectOrb(int slotIndex)
    {
        // Ensure we're selecting the correct orb
        activeOrb = orbs[slotIndex]; // Set the active orb
        Debug.Log($"Selected Orb (Index {slotIndex}): {activeOrb.name}");

        // Update visuals for the active orb slot
        SetWeaponType();
    }


    public void SetWeaponType()
    {
        if (activeOrb != null)
        {
            Debug.Log($"Active weapon set to: {activeOrb.name}");

            // Update active orb slot visuals
            activeOrbSlot.transform.GetChild(1).GetComponent<Image>().sprite = activeOrb.icon;
            activeOrbSlot.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = activeOrb.itemName;

            switch (orbEnum)
            {
                case Orb.Blue:
                    PlayerController.instance.spriteRenderer.color = new Color(70f, 70f, 200f);
                    break;
                case Orb.Red:
                    PlayerController.instance.spriteRenderer.color = Color.red;
                    break;
                case Orb.Green:
                    PlayerController.instance.spriteRenderer.color = Color.green;
                    break;
                case Orb.Yellow:
                    PlayerController.instance.spriteRenderer.color = Color.yellow;
                    break;
                default:
                    PlayerController.instance.spriteRenderer.color = Color.white;
                    break;

            }
        }
        else
        {
            Debug.Log("No active orb to set.");
        }
    }
    private Orb GetOrbEnumFromQuestClass(QuestClass questOrb)
    {
        if (questOrb == null) return Orb.none;

        switch (questOrb.QuestItemID)
        {
            case 0: return Orb.Red;
            case 1: return Orb.Yellow;
            case 2: return Orb.Green;
            case 3: return Orb.Blue;
            default: return Orb.none;
        }
    }


}
