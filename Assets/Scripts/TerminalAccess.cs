using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerminalAccess : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPuzzle;

    public UnityEvent puzzleSolved;
    
    private bool solved = false;


}
