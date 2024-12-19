using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : InteractableObject
{
    public bool isBroken = false;
    public ToolClass pickaxe;
    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKeyDown(KeyCode.E) && collidedObject.CompareTag("Player"))
        {
            BreakRock();
        }
    }
    
    public void BreakRock()
    {
        if (isBroken) return;

        if (InventoryController.instance.Contains(pickaxe) != null)
        {
            //Debug.Log($"rock broken");
            isBroken = true;
            audioSource.Play();
            spriteRenderer.enabled = false;
            itemCollider.enabled = false;

        }
        else
            Debug.Log("dont have pickaxe :(");

    }

}
