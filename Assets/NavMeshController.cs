using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public GameObject navMesh;

    private void Awake()
    {
        navMesh.SetActive(true);
    }


}

