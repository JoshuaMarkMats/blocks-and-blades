using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LearningModule : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject interactIcon;

    public void OnInteract()
    {
        CodeBlockManager.Instance.blockUnlocked = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(false);
    }
}
