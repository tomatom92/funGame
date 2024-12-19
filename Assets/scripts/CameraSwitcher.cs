using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public string triggerTag;
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    public CinemachineVirtualCamera primaryCamera;
    [SerializeField] private GameObject darknessOverlay;
    [SerializeField] private SpriteRenderer[] darkMask;


    [ContextMenu("Get all virtual cameras")]
    private void GetAllVirtualCameras()
    {
        cameras = FindObjectsOfType<CinemachineVirtualCamera>();
    }


    private void Start()
    {
        SwitchCamera(primaryCamera);
    }
    public void SwitchCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineVirtualCamera c in cameras)
        {
            c.enabled = c == targetCamera;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DarkZone(collision);
        
        if (collision.CompareTag(triggerTag))
        {
            CinemachineVirtualCamera targetCamera = collision.GetComponentInChildren<CinemachineVirtualCamera>();

            SwitchCamera(targetCamera);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DarkZone(collision);
    }
    public void DarkZone(Collider2D collider)
    {
        if (!collider.CompareTag("DarkZone"))
        {
            darknessOverlay.SetActive(false);
            foreach (SpriteRenderer sr in darkMask)
                sr.enabled = false;
        }
        else
        {
            darknessOverlay.SetActive(true);
            foreach (SpriteRenderer sr in darkMask)
                sr.enabled = true;
            CinemachineVirtualCamera targetCamera = collider.GetComponentInChildren<CinemachineVirtualCamera>();

            SwitchCamera(targetCamera);
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(triggerTag))
    //    {
    //        CinemachineVirtualCamera targetCamera = collision.GetComponentInChildren<CinemachineVirtualCamera>();
    //        SwitchCamera(targetCamera);
    //    }
    //}


}
