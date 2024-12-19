using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToolClass;

[CreateAssetMenu(fileName = "new QuestItem Class", menuName = "Item/Quest")]
public class QuestClass : ItemClass
{
    //data specific to Quest Item class

    [Header("QuestItem")]
    //data specific to Tool class
    public int QuestItemID;


    public override QuestClass GetQuest() { return this; }

}
