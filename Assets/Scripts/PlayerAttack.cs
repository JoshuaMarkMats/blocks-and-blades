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
    private int lightAttackDamage = 5;
    [SerializeField]
    private int heavyAttackDamage = 10;
    [SerializeField]
    private int parryDamage = 3;

    [SerializeField]
    private Vector2 attackCenterOffset; //center to base attacks on
    [SerializeField]
    private float attackAreaRange = 1.5f; //how far out to create the attack area
    [SerializeField]
    private float attackAreaRadius = 2f; //radius of attack area
    [SerializeField]
    private LayerMask attackAreaMask;

    private Collider2D[] targets;
    private Vector2 attackDirection; //whether to swing left or right
    private GameManager.AttackType currentAttackType = GameManager.AttackType.NONE;
    private int damage = 0; //damage to be dealt, modified by attack type
    
    private bool isAttacking = false;

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
        if (!playerController.IsAlive)
            return;
        if (attackTimer < attackCooldown || isAttacking)
            return;

        isAttacking = true;
        playerController.MovementPaused = true;
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
        playerController.MovementPaused = false;
    }
}
