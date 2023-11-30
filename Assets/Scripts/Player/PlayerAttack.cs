using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour, IRPSAttacker
{
    /* Attack Stats */
    [SerializeField]
    private float attackCooldown = 0.5f;
    private float attackTimer = 0;
    [SerializeField]
    private int lightAttackDamage = 5;
    [SerializeField]
    private int heavyAttackDamage = 10;
    [SerializeField]
    private int parryDamage = 3;
    [SerializeField]
    private float rpsWinDamageModifier = 2f;

    [SerializeField]
    private Vector2 attackCenterOffset; //center to base attacks on
    [SerializeField]
    private float attackAreaRange = 1.5f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 2f; //radius of attack area
    [SerializeField]
    private LayerMask attackAreaMask;
    [SerializeField]
    private LayerMask lineOfSightMask;

    private PlayerController playerController;
    private Animator animator;
    private Vector2 attackDirection; //whether to swing left or right
    private int damage = 0; //damage to be dealt, modified by attack type
    private bool isAttacking = false;

    private Collider2D[] targets;   
    private GameManager.AttackType currentAttackType = GameManager.AttackType.NONE;

    public GameManager.AttackType CurrentAttackType { get { return currentAttackType; } }

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
    }

    private void OnLightAttack()
    {
        AttackCheck(GameManager.AttackType.LIGHT_ATTACK);
    }

    private void OnHeavyAttack()
    {
        AttackCheck(GameManager.AttackType.HEAVY_ATTACK);
    }

    private void OnParry()
    {
        AttackCheck(GameManager.AttackType.PARRY);
    }

    private void AttackCheck(GameManager.AttackType attackType)
    {
        if (!playerController.IsAlive || playerController.IsStaggered)
            return;
        if (attackTimer < attackCooldown || isAttacking)
            return;

        isAttacking = true;
        playerController.IsMovementPaused = true;
        currentAttackType = attackType;
        switch (attackType)
        {
            case GameManager.AttackType.LIGHT_ATTACK:
                animator.SetTrigger("lightAttack");
                damage = lightAttackDamage;
                break;
            case GameManager.AttackType.HEAVY_ATTACK:
                animator.SetTrigger("heavyAttack");
                damage = heavyAttackDamage;
                break;
            case GameManager.AttackType.PARRY:
                animator.SetTrigger("parry");
                damage = parryDamage;
                break;
        }       
        attackTimer = 0;
    }

    private void Attack()
    {
        attackDirection = (playerController.LookDirection < 0) ? Vector2.left : Vector2.right;
        Vector2 attackCenter = (Vector2)transform.position + attackCenterOffset;

        targets = Physics2D.OverlapCircleAll(attackCenter + attackDirection * attackAreaRange, attackAreaRadius, attackAreaMask);

        foreach (Collider2D target in targets)
        {
            //if not damageable, skip
            if (!target.TryGetComponent<IDamageable>(out var damageable))
                continue;

            //if not in line of sight, skip
            Vector2 vectorToTarget = (Vector2)target.transform.position - (Vector2)transform.position;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, vectorToTarget, vectorToTarget.magnitude, lineOfSightMask);
            if (raycastHit.transform != target.transform)
                continue;

            //if not RPS attacker, only deal damage
            if (!target.TryGetComponent<IRPSAttacker>(out var rpsAttacker))
            {
                damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier))); //add multiplier if enemy is staggered
                continue;
            }

            if (currentAttackType == GameManager.AttackType.LIGHT_ATTACK)
            {
                if (rpsAttacker.CurrentAttackType == GameManager.AttackType.HEAVY_ATTACK)
                {
                    damageable.Stagger();
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == GameManager.AttackType.PARRY)
                    playerController.Stagger();
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier))); 
            }

            if (currentAttackType == GameManager.AttackType.HEAVY_ATTACK)
            {
                if (rpsAttacker.CurrentAttackType == GameManager.AttackType.PARRY)
                {
                    damageable.Stagger();
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == GameManager.AttackType.LIGHT_ATTACK)
                    continue;
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier)));
            }

            if (currentAttackType == GameManager.AttackType.PARRY)
            {
                if (rpsAttacker.CurrentAttackType == GameManager.AttackType.LIGHT_ATTACK)
                {
                    damageable.ChangeHealth((int)(-damage * rpsWinDamageModifier));
                }
                else if (rpsAttacker.CurrentAttackType == GameManager.AttackType.HEAVY_ATTACK)
                    continue;
                else
                    damageable.ChangeHealth((int)(-damage * (damageable.IsStaggered ? 1 : rpsWinDamageModifier)));
            }

        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        currentAttackType = GameManager.AttackType.NONE;
        playerController.IsMovementPaused = false;
    }
}
