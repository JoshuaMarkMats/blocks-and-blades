using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : EnemyBase
{
    [SerializeField]
    private Vector2 wanderLimit1;
    [SerializeField]
    private Vector2 wanderLimit2;

    private Vector3[] limitPreviewBox;

    private Vector2 waypoint;

    protected override void Start()
    {
        base.Start();
        SetNewWaypoint();   
    }

    protected override void Update()
    {
        base.Update();
        
    }

    void OnDrawGizmosSelected()
    {
        limitPreviewBox = new Vector3[4]
        {
            new Vector3(wanderLimit1.x, wanderLimit1.y, 0),
            new Vector3(wanderLimit2.x, wanderLimit1.y, 0),
            new Vector3(wanderLimit2.x, wanderLimit2.y, 0),
            new Vector3(wanderLimit1.x, wanderLimit2.y, 0)
        };

        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(limitPreviewBox, true);
    }

    protected override void Move()
    {
        Vector2 vectorToTarget = waypoint - (Vector2)transform.position;
        moveDirection = vectorToTarget.normalized;
        Vector2 movementToTarget = baseSpeed * moveDirection;

        //if distance to waypoint less than movement distance this tick, just tp to waypoint and set new one
        if (vectorToTarget.magnitude < movementToTarget.magnitude)
        {
            rigidbody2d.MovePosition((Vector2)transform.position + vectorToTarget);
            SetNewWaypoint();
            return;
        }
            
        rigidbody2d.MovePosition((Vector2)transform.position + movementToTarget);
    }

    //create new waypoint within limits
    private void SetNewWaypoint()
    {
        waypoint = new Vector2(Random.Range(wanderLimit1.x, wanderLimit2.x), Random.Range(wanderLimit1.y, wanderLimit2.y));
    }

    

}
