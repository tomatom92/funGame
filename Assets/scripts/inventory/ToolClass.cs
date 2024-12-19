using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{

    [Header("Tool")]
    //data specific to Tool class
    

    public ToolType toolType;
    public int toolID;
    
    public enum ToolType
    {
        weapon,
        pickaxe,
        axe,
        key,

    }
    public override ToolClass GetTool() {return this;}


     
}
