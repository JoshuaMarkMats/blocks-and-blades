using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Vector2 attackCenterOffset; //center to base attacks on
    [SerializeField]
    private float attackAreaRange = 1.5f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 2f; //radius of attack area
    private Collider2D[] targets;
    private Vector2 attackAreaOffset; //whether to swing left or right
    [SerializeField]
    private LayerMask attackAreaMask;

    private bool isAttacking = false;

    private void OnDrawGizmosSelected()
    {
        //attack areas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.left * attackAreaRange, attackAreaRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.right * attackAreaRange, attackAreaRadius);
    }

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
            if (Input.GetButtonDown("Fire1") && attackTimer >= attackCooldown && !isAttacking)
            {
                isAttacking = true;
                playerController.MovementPaused = true;
                //Attack();
                animator.SetTrigger("lightAttack");
                attackTimer = 0;
            }
        }
        
    }

    private void Attack()
    {
        attackAreaOffset = (playerController.LookDirection < 0) ? Vector2.left : Vector2.right;

        targets = Physics2D.OverlapCircleAll((Vector2)transform.position + attackCenterOffset + attackAreaOffset * attackAreaRange, attackAreaRadius, attackAreaMask);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ChangeHealth(-damage);
            }
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
        playerController.MovementPaused = false;
    }
}
