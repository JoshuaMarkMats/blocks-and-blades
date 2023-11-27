using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ContactAttack : MonoBehaviour
{ 
    [SerializeField]
    private int damage = 10;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ChangeHealth(-damage);
        }
    }
}
