using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuScript : MonoBehaviour
{
    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(1);
    }

}
