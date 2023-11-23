using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterLogIn : MonoBehaviour
{
    public TMP_Text user;
    public TMP_Text password;
    public Button goButton;

    void Update()
    {
        if (user.text != null && password.text != null && Input.GetKeyDown(KeyCode.Return))
        {
            print("Enter Key Pressed!");
            goButton.onClick.Invoke();
        }
    }
}
