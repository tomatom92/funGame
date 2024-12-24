using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;
using UnityEditor;


public class DoorManager : MonoBehaviour
{
    //[SerializeField] GUIStyle gui;
    [SerializeField] private TileBase doorOpenedTile;
    [SerializeField] private Tilemap map;
    [SerializeField] private Teleporter teleporter;

    private Vector3Int doorCellPosition;


    [SerializeField]
    private ToolClass keyToDoor;
    public bool doorOpened;

    private void Awake()
    {
        doorOpened = false;
        Vector2 doorPos = transform.position;
        doorCellPosition = map.WorldToCell(doorPos);
    }

    public void OpenDoorWithKey()
    {
        //Debug.Log($"reached");
        if (doorOpened) return;

        if (InventoryController.instance.Remove(keyToDoor))
        {
            Debug.Log($"door is opened");
            doorOpened = true;
            MapManager.Instance.map.SetTile(doorCellPosition, doorOpenedTile);
            teleporter.isLocked = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
            Debug.Log("dont have key :(");


    }
    public void OpenDoor()
    {
        if (doorOpened) return;

        Debug.Log($"door is opened");
        doorOpened = true;
        MapManager.Instance.map.SetTile(doorCellPosition, doorOpenedTile);
        teleporter.isLocked = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
    private void OnDrawGizmos()
    {
        //drawing cell position above door
        //Handles.color = Color.red;
        //Handles.Label(gameObject.transform.position + Vector3.up/3 , $"Door cell pos: {doorCellPosition} ", gui);

    }
}
