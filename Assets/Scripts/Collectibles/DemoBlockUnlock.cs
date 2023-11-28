using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseCollectible : MonoBehaviour
{
    [SerializeField]
    private float duration = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller)
        {
            controller.MakeInvincible(duration);
            Destroy(gameObject);
        }
    }
}
