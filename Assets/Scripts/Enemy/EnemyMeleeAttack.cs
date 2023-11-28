using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using static GameManager;

public class EnemyMeleeAttack : MonoBehaviour, IRPSAttacker
{
    /* Attack */
    [SerializeField]
    private int lightAttackDamage = 5;
    [SerializeField]
    private int heavyAttackDamage = 10;
    [SerializeField]
    private int parryDamage = 3;
    [SerializeField]
    private float rpsWinDamageModifier = 1.5f;
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

    public AttackType currentAttackType = AttackType.NONE;
    private Vector2 attackDirection;
    private int damage = 10;
    private bool isAttacking = false;
    private Collider2D[] targets;

    private EnemyBase enemyController;
    private Animator animator;    

    private const string LIGHT_ATTACK_TRIGGER = "lightAttack";
    private const string HEAVY_ATTACK_TRIGGER = "heavyAttack";
    private const string PARRY_TRIGGER = "parry";

    public AttackType CurrentAttackType { get { return currentAttackType; } }

    void Start()
    {
        enemyController= GetComponent<EnemyBase>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {

        //Debug.Log($"enemy checking attack, attacking: {isAttacking}, attack type: {currentAttackType}");
        //if player in range and not currently attacking and not staggered AND alive, randomly choose an attack
        if (!isAttacking && !enemyController.IsStaggered && enemyController.IsAlive && Physics2D.OverlapCircle((Vector2)transform.position + attackCenterOffset, detectionRange, attackAreaMask) != null)
            AttackCheck((AttackType)Random.Range(0, 3));
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

    private void AttackCheck(AttackType attackType)
    {
        if (!enemyController.IsAlive)
            return;

        isAttacking = true;
        enemyController.IsMovementPaused = true;
        currentAttackType = attackType;
        switch (attackType)
        {
            case AttackType.LIGHT_ATTACK:
                animator.SetTrigger(LIGHT_ATTACK_TRIGGER);
                damage = lightAttackDamage;
                break;
            case AttackType.HEAVY_ATTACK:
                animator.SetTrigger(HEAVY_ATTACK_TRIGGER);
                damage = heavyAttackDamage;
                break;
            case AttackType.PARRY:
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
            //if not damageable, skip
            if (!target.TryGetComponent<IDamageable>(out var damageable))
                continue;

            //if not RPS attacker, only deal damage
            if (!target.TryGetComponent<IRPSAttacker>(out var rpsAttacker))
            {
                damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier))); //add multiplier if enemy is staggered
                continue;
            }


            if (currentAttackType == AttackType.LIGHT_ATTACK)
            {
                if (rpsAttacker.CurrentAttackType == AttackType.HEAVY_ATTACK)
                {
                    damageable.Stagger();
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == AttackType.PARRY)
                    enemyController.Stagger();
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier)));
            }

            if (currentAttackType == AttackType.HEAVY_ATTACK)
            {
                if (rpsAttacker.CurrentAttackType == AttackType.PARRY)
                {
                    damageable.Stagger();
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == AttackType.LIGHT_ATTACK)
                    continue;
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier)));
            }

            if (currentAttackType == AttackType.PARRY)
            {
                if (rpsAttacker.CurrentAttackType == AttackType.LIGHT_ATTACK)
                {
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == AttackType.HEAVY_ATTACK)
                    continue;
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier)));
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        currentAttackType = AttackType.NONE;
        enemyController.IsMovementPaused = false;
    }
}
