using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject setting;
    bool isOn = false;

    public AudioSource AudioSource;

    public Slider Slider;

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


    private void Update()
    {
        StaticVolumeValue.volume = Slider.value;

        AudioSource.volume = StaticVolumeValue.volume;
        Debug.Log(AudioSource.volume);
       
    }



}
