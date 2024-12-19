using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = aiPath.steeringTarget;



        
        //todo

        //animator.SetFloat("Horizontal", x);
        //animator.SetFloat("Vertical", y);
        animator.SetFloat("Speed", aiPath.maxSpeed);

        

    }
}
