using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Teleporter : MonoBehaviour
{
    [SerializeField] GameObject endPointObject;
    public bool isLocked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isLocked) return;
        
        PlayerController.instance.transform.position = endPointObject.transform.position;
        gameObject.GetComponent<AudioSource>().Play();

    }
}
