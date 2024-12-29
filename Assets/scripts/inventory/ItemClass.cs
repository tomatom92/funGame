using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : ScriptableObject
{
    //data shared across every item
    [Header("Item")]
    public string itemName;
    public Sprite icon;
    public bool isStackable = true;
    public virtual bool IsOrb()
    {
        return false;
    }


    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
    public virtual QuestClass GetQuest() { return null; }

}
