using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    /* Attack */
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float windupDuration = 0.4f;
    [SerializeField]
    private float attackDuration = 1f;
    [SerializeField]
    private float detectionRange = 2f;
    [SerializeField]
    private float attackAreaRange = 2f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 3f; //radius of attack area
    [SerializeField]
    private Vector2 attackCenterOffset = Vector2.up; //offset for center of attack stuff
    private Vector2 attackDirection;
    [SerializeField]
    private LayerMask targetMask;
    private bool isAttacking = false;

    private ChasingEnemy enemyController;
    private Animator animator;

    private const string WINDUP_TRIGGER = "windup";
    private const string ATTACK_TRIGGER = "attack";
    private const string FINISH_ATTACK_TRIGGER = "finishAttack";

    void Start()
    {
        enemyController= GetComponent<ChasingEnemy>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (!isAttacking && Physics2D.OverlapCircle((Vector2)transform.position + attackCenterOffset, detectionRange, targetMask) != null)
            StartCoroutine(Attack());
    }

    private void OnDrawGizmosSelected()
    {
        //attack areas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.left * attackAreaRange, attackAreaRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset + Vector2.right * attackAreaRange, attackAreaRadius);
        //detection range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + attackCenterOffset, detectionRange);
    }

    IEnumerator Attack()
    {
        enemyController.IsMovementPaused = true;
        isAttacking = true;
        animator.SetTrigger(WINDUP_TRIGGER);

        yield return new WaitForSeconds(windupDuration);

        animator.SetTrigger(ATTACK_TRIGGER);

        attackDirection = (enemyController.LookDirection < 0) ? Vector2.left : Vector2.right;

        Collider2D[] targets = Physics2D.OverlapCircleAll((Vector2)transform.position + attackCenterOffset + attackDirection * attackAreaRange, attackAreaRadius, targetMask);

        foreach (Collider2D target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ChangeHealth(-damage);
            }
        }

        yield return new WaitForSeconds(attackDuration);
        isAttacking= false;
        enemyController.IsMovementPaused = false;
        animator.SetTrigger(FINISH_ATTACK_TRIGGER);
    }
}
