using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerminalAccess : MonoBehaviour, IInteractable
{
    private const string SOLVED_TRIGGER = "solved";

    [SerializeField]
    private GameObject blockPuzzle;
    [SerializeField]
    private GameObject interactIcon;

    public UnityEvent puzzleSolved;
    private GameObject currentPuzzle;
    private Animator animator;
    [SerializeField]
    private bool isActive = false;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        puzzleSolved.AddListener(OnSolved);
    }

    public void OnInteract()
    {
        if (isActive)
            return;

        isActive = true;
        currentPuzzle = Instantiate(blockPuzzle);
        currentPuzzle.GetComponentInChildren<Controller>().terminalAccess = this;
    }

    public void CloseTerminal()
    {
        isActive = false;
        Destroy(currentPuzzle);
    }

    public void OnSolved()
    {
        animator.SetTrigger(SOLVED_TRIGGER);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
            return;
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
            interactIcon.SetActive(false);
    }
}
