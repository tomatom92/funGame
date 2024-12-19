using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public Vector3Int TilePosition;
    public bool playerAbove;

    private void Awake()
    {
        
    }

    //public Vector3Int GetTilePosition(TileBase tile)
    //{
    //    //Vector2 tilePos = player.transform.position;
    //    //Vector3Int playerCellPos = map.WorldToCell(playerPos);


    //    //return tilePosition[tile];
    //}


    
}
