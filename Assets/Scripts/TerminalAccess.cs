using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class TerminalAccess : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject blockPuzzle;
    [SerializeField]
    private GameObject interactIcon;

    public UnityEvent puzzleSolved;
    private GameObject currentPuzzle;
    private bool solved = false;
    [SerializeField]
    private bool isActive = false;

    private void Start()
    {
        //puzzleSolved
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

    private void Update()
    {
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
