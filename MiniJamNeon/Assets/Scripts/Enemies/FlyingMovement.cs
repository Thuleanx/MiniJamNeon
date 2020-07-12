using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class FlyingMovement : MonoBehaviour
{
    private Transform target;

    public float updateRate = 2f;
    

    private Seeker seeker;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3;

    // number tiles away that ai can see you from
    public float vision = 15f;
  

    
    public LayerMask whatToHit;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (target == null)
        {
            Debug.LogError("No player found? Panic");
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        InvokeRepeating("UpdatePath", 0, 1f / updateRate);

        
        
    }

    void UpdatePath()
    {
        if (target == null)
        {
            //TODO: Insert a player search here
            Debug.LogError("why this here");
            return;
        }

        if (Vector2.Distance(target.transform.position, transform.position) > vision)
        {
            return;
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position, range, whatToHit);
        //Debug.DrawLine(transform.position, target.position);
        //// if raycast hits player
        //if (hit.collider != null)
        //{
        //    Debug.Log("I see you");
        //    return;
        //}

        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("We got a path. Did it have an error " + p.error);
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
        
        if (target.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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
