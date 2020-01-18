using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPathfinder : MonoBehaviour
{
    public float speed;
    public float nextWaypointDist = 2f;
    public float endReachedDist = 0.1f;
    public Vector3 target;
    public Vector2 direction;

    private int currentWaypoint = 0;
    private bool reachEndOfPath = false;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;

    public void UpdateTarget(Vector3 target)
    {
        this.target = target;
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target, OnPathComplete);
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = transform.position;
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;
        if (Vector3.Distance(path.vectorPath[path.vectorPath.Count - 1], transform.position) < endReachedDist)
        {
            reachEndOfPath = true;
            direction = Vector2.zero;
            return;
        }
        reachEndOfPath = false;

        direction = ((Vector2)(path.vectorPath[currentWaypoint] - transform.position)).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        rb.AddForce(force);

        float dist = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDist && currentWaypoint < path.vectorPath.Count - 1)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
            return;
        path = p;
        currentWaypoint = 0;
    }
}
