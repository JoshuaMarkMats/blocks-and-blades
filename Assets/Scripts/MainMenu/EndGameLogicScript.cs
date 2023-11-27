using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuScript : MonoBehaviour
{
    AudioSource audioSource;
    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
        audioSource.GetComponent<AudioSource>();
    }

    public void playAgain()
    {
        SceneManager.LoadScene(1);

        audioSource.volume = StaticVolumeValue.volume;
    }

}
