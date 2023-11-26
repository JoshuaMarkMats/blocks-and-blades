using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class CodeBlockManager : MonoBehaviour
{
    [Serializable]
    public class CodeBlockErrorEvent : UnityEvent<CodeBlockErrorType> { };

    public enum CodeBlockErrorType
    {
        VARIABLE_NOT_FOUND
    }

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

    public Transform lastSelected;

    public const string HEAD_BLOCK = "HeadBlock";
}
