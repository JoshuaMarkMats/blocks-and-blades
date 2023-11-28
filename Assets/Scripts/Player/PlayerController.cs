using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private const string IS_MOVING_BOOL = "isMoving";
    private const string LOOKX_VALUE = "lookX";
    private const string FORCE_IDLE_TRIGGER = "forceIdle";
    private const string DEATH_TRIGGER = "death";

    /* Base Stats */
    [SerializeField]
    private int currentHealth;
    public int maxHealth = 100;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float baseSpeed = 0.1f;
    [Space(0)]

    /* Invincibility */
    [SerializeField]
    private float invincibleBlinkInterval = 0.1f;     
    public float timeInvincible = 0.3f;
    [Space(0)]

    [SerializeField]
    private float staggerDuration = 1f;
    [SerializeField]
    private GameObject staggerEffect;

    private bool isAlive = true;
    private bool isMovementPaused = false;
    private bool isStaggered = false;

    private PlayerAttack playerAttack;
    private Rigidbody2D rigidbody2d;
    private Coroutine invincibleBlinkCoroutine;
    private Coroutine staggerCoroutine;
    private SpriteRenderer playerRenderer;
    private Animator animator;
    private float lookDirection = 1;
    private Vector2 moveDirection = Vector2.zero;

    bool isInvincible;
    float invincibleTimer;

    public bool IsAlive { get { return isAlive; } }
    public float LookDirection { get { return lookDirection; }  }
    public bool IsStaggered { get { return isStaggered; } }

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

    public GameManager.AttackType AttackType
    {
        get { return playerAttack.CurrentAttackType; }
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        playerAttack = GetComponent<PlayerAttack>();
        playerRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {            
        //invincibility effect
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
                if (invincibleBlinkCoroutine != null)
                {
                    StopCoroutine(invincibleBlinkCoroutine);
                    playerRenderer.enabled = true;
                }
                    
            }               
        }

        //don't do the sprite stuff if player is dead or paused
        if (!isAlive || isMovementPaused)
            return;

        //sprite direction
        if (!Mathf.Approximately(moveDirection.x, 0.0f))
            lookDirection = moveDirection.x;
        animator.SetFloat(LOOKX_VALUE, lookDirection);

        //sprite idle or moving
        if (!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
            animator.SetBool(IS_MOVING_BOOL, true);
        else
            animator.SetBool(IS_MOVING_BOOL, false);
    }

    void FixedUpdate()
    {
        //don't move if dead or paused
        if (!isAlive || isMovementPaused)
            return;

        Vector2 position = transform.position;

        rigidbody2d.MovePosition(position + baseSpeed * moveDirection);

    }

    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();
    }

    public void MakeInvincible(float duration)
    {
        if (!isAlive)
            return;

        isInvincible = true;
        invincibleTimer = duration;
        if (invincibleBlinkCoroutine != null)
        {
            StopCoroutine(invincibleBlinkCoroutine);
            playerRenderer.enabled = true;
        }

    }

    public void ChangeHealth(int value)
    {
        if (!isAlive)
            return;

        if (value < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            invincibleBlinkCoroutine = StartCoroutine(InvincibleBlink(invincibleBlinkInterval));
        }

        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            PlayerDeath();
    }

    public void Stagger()
    {
        playerAttack.EndAttack();
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

    //coroutine to blink the player
    IEnumerator InvincibleBlink(float interval)
    {
        WaitForSeconds intervalTime = new(interval);

        while (true)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return intervalTime;
        }      
    }

    private void PlayerDeath()
    {
        isAlive = false;

        //stop blinking to make sure player is visible
        if (invincibleBlinkCoroutine != null)
            StopCoroutine(invincibleBlinkCoroutine);
        if (staggerCoroutine != null)
            StopCoroutine(staggerCoroutine);
        //reset some other effects
        staggerEffect.SetActive(false);
        playerRenderer.enabled = true;

        animator.SetTrigger(DEATH_TRIGGER);
        
    }

}
