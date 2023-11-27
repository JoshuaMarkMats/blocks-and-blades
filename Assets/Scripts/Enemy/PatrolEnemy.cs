using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatrolEnemy : EnemyBase
{
    [SerializeField]
    private Vector3[] waypoints;

    private Vector2 currentWaypoint;
    [SerializeField]
    private int currentWaypointIndex = 0;

    protected override void Start()
    {
        base.Start();
        SetWaypoint(currentWaypointIndex);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(waypoints, true);
    }

    protected override void Move()
    {
        Vector2 vectorToTarget = currentWaypoint - (Vector2)transform.position;
        moveDirection = vectorToTarget.normalized;
        Vector2 movementToTarget = baseSpeed * moveDirection;

        //if distance to waypoint less than movement distance this tick, just tp to waypoint and set new one
        if (vectorToTarget.magnitude < movementToTarget.magnitude)
        {
            rigidbody2d.MovePosition((Vector2)transform.position + vectorToTarget);
            //check if next index is valid, otherwise start from 0
            if (currentWaypointIndex + 1 < waypoints.Length)
                currentWaypointIndex++;
            else
                currentWaypointIndex = 0;
            SetWaypoint(currentWaypointIndex);
            return;
        }

        rigidbody2d.MovePosition((Vector2)transform.position + movementToTarget);
    }

    //create new waypoint within limits
    private void SetWaypoint(int index)
    {
        currentWaypoint = new Vector2(waypoints[index].x, waypoints[index].y);
    }
}
