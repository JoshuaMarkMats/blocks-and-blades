using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleScript : MonoBehaviour
{
    public GameObject GameObject;
    public GameObject GameObject2;
    bool toggle = false;
    static bool isPaused = false;

    void Update()
    {
        var a = Input.GetKeyDown(KeyCode.Tab);
        var b = Input.GetKeyDown(KeyCode.Escape);

        if (a)
        {
            toggle = !toggle;
            GameObject.SetActive(toggle);
        }

        if (b)
        {
            isPaused = !isPaused;
            GameObject2.SetActive(isPaused);
            Pause();
        }
    }

    void Pause()
    {
        if (isPaused == true)
        {
            Time.timeScale = 0.0f;
        }

        else
        {
            Time.timeScale = 1.0f;
        }

    }

    public void CloseBox()
    {
        if (GameObject2 == true)
        {
            isPaused = false;
            Pause();
            GameObject2.SetActive(false);
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
