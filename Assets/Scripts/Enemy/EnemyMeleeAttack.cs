using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    /* Attack */
    [SerializeField]
    private int lightAttackDamage = 5;
    [SerializeField]
    private int heavyAttackDamage = 10;
    [SerializeField]
    private int parryDamage = 3;
    [SerializeField]
    private float detectionRange = 2f;
    [SerializeField]
    private float attackAreaRange = 2f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 3f; //radius of attack area
    [SerializeField]
    private Vector2 attackCenterOffset = Vector2.up; //offset for center of attack stuff
    [SerializeField]
    private LayerMask attackAreaMask; //what can be attacked

    public GameManager.AttackType currentAttackType = GameManager.AttackType.NONE;
    private Vector2 attackDirection;
    private int damage = 10;
    private bool isAttacking = false;
    private Collider2D[] targets;

    private EnemyBase enemyController;
    private Animator animator;    

    private const string LIGHT_ATTACK_TRIGGER = "lightAttack";
    private const string HEAVY_ATTACK_TRIGGER = "heavyAttack";
    private const string PARRY_TRIGGER = "parry";

    void Start()
    {
        enemyController= GetComponent<ChasingEnemy>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //if player in range and not currently attacking, randomly choose an attack
        if (!isAttacking && Physics2D.OverlapCircle((Vector2)transform.position + attackCenterOffset, detectionRange, attackAreaMask) != null)
            AttackCheck((GameManager.AttackType)Random.Range(0, 3));
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

    private void AttackCheck(GameManager.AttackType attackType)
    {
        if (!enemyController.IsAlive)
            return;

        isAttacking = true;
        enemyController.MovementPaused = true;
        currentAttackType = attackType;
        switch (attackType)
        {
            case GameManager.AttackType.LIGHT_ATTACK:
                animator.SetTrigger(LIGHT_ATTACK_TRIGGER);
                damage = lightAttackDamage;
                break;
            case GameManager.AttackType.HEAVY_ATTACK:
                animator.SetTrigger(HEAVY_ATTACK_TRIGGER);
                damage = heavyAttackDamage;
                break;
            case GameManager.AttackType.PARRY:
                animator.SetTrigger(PARRY_TRIGGER);
                damage = parryDamage;
                break;
        }
        //attackTimer = 0;
    }

    private void Attack()
    {
        attackDirection = (enemyController.LookDirection < 0) ? Vector2.left : Vector2.right;

        targets = Physics2D.OverlapCircleAll((Vector2)transform.position + attackCenterOffset + attackDirection * attackAreaRange, attackAreaRadius, attackAreaMask);

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
        Debug.Log("finishattack called");
        isAttacking = false;
        currentAttackType = GameManager.AttackType.NONE;
        enemyController.MovementPaused = false;
    }
}
