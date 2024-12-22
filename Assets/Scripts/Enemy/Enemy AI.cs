using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 10f;

    public float nextWaypointDistance = 3;

    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;
    private float range;
    EnemyController enemyController;

    private void Update()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //seeker takes a start pos, a target pos, and then a method to call when its finished

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            path = null;
        }
    }
    //if path didnt error, set current path to newly generated path p, and set current waypoint to 0 to start at the beginning of new path
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float playerDist = Vector2.Distance(rb.position, target.position);
        //if player is outside of range
        if (playerDist > range) return;

        if (path == null) return;

        //if current waypoint is greater than the total amount of waypoints along the path, then weve reached the end of the path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        }
        else
        {
            //reachedEndOfPath = false;
        }

        //direction to next waypoint
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //setting how fast to move enemy
        Vector2 force = direction * speed * Time.deltaTime;
        //add force to enemy
        rb.AddForce(force);
        //distance to next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        //if that distance is less than our nextWaypoint distance than we've reached the waypoint
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }
}
