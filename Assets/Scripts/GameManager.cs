using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the game manager
    public static GameManager Instance { get; private set; }

    public UnityEvent game_overEvent = new();
    public UnityEvent game_winEvent = new();

    public enum AttackType
    {
        LIGHT_ATTACK,
        HEAVY_ATTACK,
        PARRY,
        NONE
    }

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        game_overEvent.AddListener(CodeBlockManager.Instance.EraseData);
        game_winEvent.AddListener(CodeBlockManager.Instance.EraseData);
    }

   
}
