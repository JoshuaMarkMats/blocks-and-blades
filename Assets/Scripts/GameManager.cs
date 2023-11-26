using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the game manager
    public static GameManager Instance { get; private set; }

    public UnityEvent fpsThirty;
    public UnityEvent fpsFortyFive;
    public UnityEvent fpsSixty;
    public UnityEvent fpsOneHundredTwenty;

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        fpsThirty.AddListener(setFpsThirty);
        fpsFortyFive.AddListener(setFpsFortyFive);
        fpsSixty.AddListener(setFpsSixty);
        fpsOneHundredTwenty.AddListener(setFpsOneHundredTwenty);
    }

    private void Update()
    {
        //Debug.Log(gameState);
    }

    public void InvokeDeath()
    {
        //death.Invoke();
    }

    public void setFpsThirty()
    { 
        Application.targetFrameRate = 30;
    }

    public void setFpsFortyFive()
    {
        Application.targetFrameRate = 45;
    }

    public void setFpsSixty()
    {
        Application.targetFrameRate = 60;
    }

    public void setFpsOneHundredTwenty()
    {
        Application.targetFrameRate = 120;
    }
}
