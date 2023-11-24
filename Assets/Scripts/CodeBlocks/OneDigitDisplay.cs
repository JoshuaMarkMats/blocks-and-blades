using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneDigitDisplay : MonoBehaviour
{
    private Image digit;

    void Start()
    {
        digit = GetComponent<Image>();
    }

    public void SetDigit(int digit)
    {
        switch (digit)
        {
            //case 0:

        }
    }
}
