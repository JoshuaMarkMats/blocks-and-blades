using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationBox : MonoBehaviour 
{
    [SerializeField]
    private TextMeshProUGUI textaaaa;

    public string Text { get { return textaaaa.text; } set { textaaaa.text = value; } }

    public void CloseBox()
    {
        gameObject.SetActive(false);
    }
}
