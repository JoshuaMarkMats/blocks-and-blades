using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField]
    private int amount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            controller.ChangeHealth(amount);
            gameObject.SetActive(false);
        }
    }
}
