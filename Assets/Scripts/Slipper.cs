using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slipper : AutoDestroyPoolableObject
{
    [SerializeField]
    public int damage = 10;

    private Rigidbody2D rb;
    public Vector2 direction = Vector2.zero;
    [SerializeField]
    public float thrownVelocity = 0.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + direction * thrownVelocity);
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            controller.ChangeHealth(damage);
            gameObject.SetActive(false);
        }
    }
}
