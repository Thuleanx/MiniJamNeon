﻿using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyFlyingMelee : MonoBehaviour
{
    public Transform target;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;
    private SpriteRenderer renderer;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        if (target == null)
        {
            Debug.LogError("No player found? Panic");
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
        
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: Insert a player search here
            Debug.LogError("why this here");
            yield break;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        //TODO: Always look at player
        
        if (target.transform.position.x < transform.position.x)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

}
