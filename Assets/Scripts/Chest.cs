using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{

    private const string OPEN_TRIGGER = "open";
    private const int OPENED_LAYER = 0;

    [SerializeField]
    private GameObject loot;
    [SerializeField]
    private GameObject interactIcon;

    public UnityEvent puzzleSolved;
    private Animator animator;
    [SerializeField]
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnInteract()
    {
        if (isOpen)
            return;
        animator.SetTrigger(OPEN_TRIGGER);
        isOpen = true;
        interactIcon.SetActive(false);
    }

    private void SpawnLoot()
    {
        loot.SetActive(true);
        gameObject.layer = OPENED_LAYER;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
            return;
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOpen)
            return;
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(false);
    }
}
