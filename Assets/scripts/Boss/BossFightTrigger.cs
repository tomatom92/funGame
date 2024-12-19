using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    public BossFightManager bossFightManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the player
        if (collision.CompareTag("Player"))
        {
            bossFightManager.TriggerCutsceneAndStartFight();
            GetComponent<Collider2D>().enabled = false;  // Disable the trigger after activation
        }
    }
}

