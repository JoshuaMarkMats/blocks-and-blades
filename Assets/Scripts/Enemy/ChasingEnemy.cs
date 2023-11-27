using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyBase
{
    [SerializeField]
    private Transform target;

    protected override void Update()
    {
        base.Update();

        //update pathfinding
        moveDirection = target.position - transform.position;
        moveDirection.Normalize();

    }
}
