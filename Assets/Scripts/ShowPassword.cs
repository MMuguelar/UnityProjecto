using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class ShowPassword : MonoBehaviour
{
    [SerializeField] private TMP_InputField userPassword;
 
    public void ShowUserPassword()
    {
        if (userPassword.contentType == TMP_InputField.ContentType.Password)
        {
            userPassword.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            userPassword.contentType = TMP_InputField.ContentType.Password;
        }
        userPassword.ForceLabelUpdate();
    }
}