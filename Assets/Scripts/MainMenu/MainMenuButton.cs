using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioSource audioSource;

    public GameObject setting;
    bool isOn = false;

    private void Awake()
    {
        audioSource.Play();
    }

    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        isOn = !isOn;
        setting.SetActive(isOn);
    }

    public void exit()
    {
        Application.Quit();
    }





}
