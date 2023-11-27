using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{   
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    private Coroutine invincibleBlinkCoroutine;
    private SpriteRenderer playerRenderer;
    private Animator animator;
    private float lookDirection = 1;

    /* Base Stats */
    [SerializeField]
    private int currentHealth;
    public int maxHealth = 100;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private float baseSpeed = 0.1f;
    [SerializeField]
    private float invincibleBlinkInterval = 0.1f;
    private bool isAlive = true;
    private bool movementPaused = false;

    /* Invincibility */
    [Space(0)]
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public int Health { get { return currentHealth; } }
    public bool IsAlive { get { return isAlive; } }
    public float LookDirection { get { return lookDirection; }  }

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

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

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
        if (!isAlive || movementPaused)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //sprite direction
        if (!Mathf.Approximately(horizontal, 0.0f))
            lookDirection = horizontal;
        animator.SetFloat("lookX", lookDirection);

        //sprite idle or moving
        if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        //don't move if dead or paused
        if (!isAlive || movementPaused)
            return;

        Vector2 position = transform.position;

        position.y += baseSpeed * vertical;
        position.x += baseSpeed * horizontal;

        rigidbody2d.MovePosition(position);

    }

    public void MakeInvincible(float duration)
    {
        if (isAlive)
        {
            isInvincible = true;
            invincibleTimer = duration;
            if (invincibleBlinkCoroutine != null)
            {
                StopCoroutine(invincibleBlinkCoroutine);
                playerRenderer.enabled = true;
            }
        }
       
    }

    public void ChangeHealth(int value)
    {
        if (isAlive)
        {
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
        animator.SetTrigger("death");
        //stop blinking to make sure player is visible
        StopCoroutine(invincibleBlinkCoroutine);
        playerRenderer.enabled = true;
    }

}
