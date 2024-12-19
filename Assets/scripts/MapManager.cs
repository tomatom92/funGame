using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    DoorManager doorManager;

    public static MapManager Instance;
    public Tilemap map;
    [HideInInspector]
    public Vector2 playerPos;
    [HideInInspector]
    public Vector3Int playerCellPos;

    private GameObject player;

    //todo fix tile data code
    [SerializeField]
    private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;


    private void Start()
    {
        player = PlayerController.instance.gameObject;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
            Instance = this;


        /*
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }*/

    }
    void Update()
    {
        playerPos = player.transform.position;
        playerCellPos = map.WorldToCell(playerPos); 
        TileBase currentTile = map.GetTile(playerCellPos);
    }

    //public TileData GetTileData(Vector3Int tilePosition)
    //{
    //    TileBase tile = map.GetTile(tilePosition);
    //    if (tile == null)
    //        return null;
    //    else
    //        return dataFromTiles[tile];
    //}
    
    
}
