using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector pb;
    bool triggered;
    void Awake()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (triggered) return;
            triggered = true;
            Debug.Log("entered zone");
            pb.Play();
        }

    }
}
