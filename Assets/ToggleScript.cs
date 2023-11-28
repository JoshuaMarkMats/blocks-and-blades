using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    public GameObject GameObject;
    bool toggle = false;

    void Update()
    {
        var a = Input.GetKeyDown(KeyCode.Tab);

        if (a)
        {
            toggle = !toggle;
            GameObject.SetActive(toggle);
        }
    }
}
