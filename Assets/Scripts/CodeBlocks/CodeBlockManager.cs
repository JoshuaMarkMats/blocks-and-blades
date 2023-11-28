using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class CodeBlockManager : MonoBehaviour
{
    //custom event to get error type as parameter
    [Serializable]
    public class CodeBlockErrorEvent : UnityEvent<string> { };

    public static CodeBlockManager Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public CodeBlockErrorEvent codeBlockErrorEvent;
    public UnityEvent solvedEvent;

    public Transform lastSelected;

    public const string HEAD_BLOCK = "HeadBlock";
}
