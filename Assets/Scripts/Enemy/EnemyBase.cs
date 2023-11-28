using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    private const string IS_MOVING_BOOL = "isMoving";
    private const string LOOKX_VALUE = "lookX";
    private const string FORCE_IDLE_TRIGGER = "forceIdle";
    private const string DEATH_TRIGGER = "death";


    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    protected int currentHealth;
    [SerializeField]
    private HealthBar healthBar;

    /* Movement */
    [SerializeField]
    protected float baseSpeed = 0.1f;
    [SerializeField]
    private bool isStationary = false; //enemy incapable of movement   

    /* Enemy Hit */
    [Space()]
    private Material material;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private float flashDuration = 0.1f;

    [SerializeField]
    private float staggerDuration = 1.5f;
    [SerializeField]
    private GameObject staggerEffect;

    private bool isAlive = true;
    private bool isMovementPaused = false; //enemy movement paused (saves lookX)
    private bool isStaggered = false;
    protected Vector2 moveDirection = Vector2.zero; //direction of movement, also used for determining sprite direction for overrides

    private EnemyMeleeAttack enemyMeleeAttack;
    private Coroutine flashCoroutine;
    private Coroutine staggerCoroutine;
    private Animator animator;
    private float lookDirection = 1;
    protected Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    public bool IsAlive { get { return isAlive; } }
    public float LookDirection { get { return lookDirection; } }
    public bool IsStationary { get { return isStationary; } set { isStationary = value; } }
    public bool IsMovementPaused
    {
        get { return isMovementPaused; }
        set
        {
            if (value == false && isStaggered)
                return; //don't remove movement pause if staggered

            isMovementPaused = value;
            if (value == true) //safely set isMoving
                animator.SetBool(IS_MOVING_BOOL, false);
        }
    }

    public bool IsStaggered { get { return isStaggered; } }

    protected virtual void Start()
    {
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        if (healthBar != null )
            healthBar.SetMaxHealth(maxHealth);
    }

    protected virtual void Update()
    {
        //if not stationary, do sprite movement change
        if (!isStationary && isAlive)
        {
            if (!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                animator.SetBool(IS_MOVING_BOOL, true);
            else
                animator.SetBool(IS_MOVING_BOOL, false);
        }

    }

    private void LateUpdate()
    {
        //if paused or dead, don't do direction changes
        if (isMovementPaused || !isAlive)
            return;

        //sprite direction
        if (!Mathf.Approximately(moveDirection.x, 0.0f))
            lookDirection = moveDirection.x;
        animator.SetFloat(LOOKX_VALUE, lookDirection);
    }

    private void FixedUpdate()
    {
        if (!isStationary && !isMovementPaused && isAlive)
            Move();
    }

    protected virtual void Move()
    {
        Vector2 position = (Vector2)transform.position + (baseSpeed * moveDirection);
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int value)
    {
        if (!isAlive)
            return;

        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
            Death();
    }

    public virtual void Stagger()
    {
        if (enemyMeleeAttack != null)
            enemyMeleeAttack.EndAttack();

        if (staggerCoroutine != null)
            StopCoroutine(staggerCoroutine);
        staggerCoroutine = StartCoroutine(Staggered(staggerDuration));
    }

    IEnumerator Staggered(float duration)
    {
        staggerEffect.SetActive(true);
        isStaggered = true;
        isMovementPaused = true;
        animator.SetTrigger(FORCE_IDLE_TRIGGER);
        yield return new WaitForSeconds(duration);
        staggerEffect.SetActive(false);      
        isMovementPaused = false;
        isStaggered = false;
    }

    IEnumerator FlashEffect()
    {
        WaitForSeconds flashDuration = new(this.flashDuration);

        spriteRenderer.material = flashMaterial;

        yield return flashDuration;

        spriteRenderer.material = material;
    }

    private void Death()
    {
        isAlive = false;
        if (staggerCoroutine != null)
            StopCoroutine(staggerCoroutine);

        //reset stagger effect
        staggerEffect.SetActive(false);
        animator.SetTrigger(DEATH_TRIGGER);

        //safely pause movement
        IsMovementPaused = true;
    }
}
