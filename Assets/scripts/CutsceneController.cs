using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector pb;
    bool triggered;
    public CinemachineVirtualCamera cutsceneCamera;
    public CinemachineVirtualCamera followCamera;

    public void StartCutscene()
    {
        //disable the follow camera and activate the cutscene camera
        followCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
    }
    public void EndCutscene()
    {
        //disable the cutscene camera and enable the follow camera
        cutsceneCamera.gameObject.SetActive(false);
        followCamera.gameObject.SetActive(true);
    }
    void Awake()
    {
        triggered = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (triggered) return;
    //        triggered = true;
    //        Debug.Log("entered zone");
    //        StartCutscene();
    //        pb.Play();
            
    //    }

    //}
}
