using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private HealthBar healthBar;

    /* Movement */
    [SerializeField]
    protected float baseSpeed = 0.1f;
    [SerializeField]
    private bool isStationary = false; //enemy incapable of movement
    private bool movementPaused = false; //enemy movement paused (saves lookX)
    protected Vector2 moveDirection = Vector2.zero; //direction of movement, also used for determining sprite direction for overrides

    /* Enemy Hit */
    [Space()]
    private Material material;
    [SerializeField]
    private Material flashMaterial;
    [SerializeField]
    private float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

    private bool isAlive = true;

    private Animator animator;
    private float lookDirection = 1;
    protected Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    public bool IsAlive { get { return isAlive; } }
    public float LookDirection { get { return lookDirection; } }
    public bool IsStationary { get { return isStationary; } set { isStationary = value; } }
    public bool MovementPaused
    {
        get { return movementPaused; }
        set
        {
            movementPaused = value;
            //safely set isMoving
            if (value == true)
                animator.SetBool("isMoving", false);
        }
    }

    protected virtual void Start()
    {
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
        if (!isStationary)
        {
            if (!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);
        }

        

    }

    private void LateUpdate()
    {
        //if paused, don't do direction changes
        if (movementPaused)
            return;

        //sprite direction
        if (!Mathf.Approximately(moveDirection.x, 0.0f))
            lookDirection = moveDirection.x;
        animator.SetFloat("lookX", lookDirection);
    }

    private void FixedUpdate()
    {
        if (!isStationary && !movementPaused)
            Move();


    }

    protected virtual void Move()
    {
        Vector2 position = (Vector2)transform.position + (baseSpeed * moveDirection);
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
    {
        WaitForSeconds flashDuration = new(this.flashDuration);

        spriteRenderer.material = flashMaterial;

        yield return flashDuration;

        spriteRenderer.material = material;
    }
}
