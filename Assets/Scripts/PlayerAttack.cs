using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    /* Attack Stats */
    [SerializeField]
    private float attackCooldown = 0.5f;
    private float attackTimer = 0;
    [SerializeField]
    private int damage = 3;
    [SerializeField]
    private float attackAreaRange = 1.5f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 2f; //radius of attack area
    private Collider2D[] targets;
    private Vector2 attackAreaOffset;
    [SerializeField]
    private LayerMask attackAreaMask;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (attackTimer < attackCooldown)
            attackTimer += Time.deltaTime;

        if (playerController.IsAlive)
        {
            if (Input.GetButtonDown("Fire1") && attackTimer >= attackCooldown)
            {
                Attack();
                animator.SetTrigger("attack");
                attackTimer = 0;
            }
        }
        
    }

    private void Attack()
    {
        attackAreaOffset = (playerController.LookDirection < 0) ? Vector2.left : Vector2.right;

        targets = Physics2D.OverlapCircleAll((Vector2)transform.position + 0.5f * Vector2.up + attackAreaOffset * attackAreaRange, attackAreaRadius, attackAreaMask);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ChangeHealth(-damage);
            }
        }
    }
}
