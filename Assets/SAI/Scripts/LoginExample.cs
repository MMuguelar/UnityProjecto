using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoginExample : MonoBehaviour
{
    
    void Start()
    {
        SAI.SDK.Login.SignIn("spaceai@spaceai.com", "spaceai");
    }

    private void OnEnable()
    {
        LoginSystem.LoginSuccessTrigger += LoginOk;
        LoginSystem.LoginFailureTrigger += LoginBad;
    }

    private void OnDisable()
    {
        LoginSystem.LoginSuccessTrigger -= LoginOk;
        LoginSystem.LoginFailureTrigger -= LoginBad;
    }

    private void LoginBad()
    {
        print("Fallo al logear");
    }

    private void LoginOk()
    {
        print("Exito al logear, su token es : " + SAI.SDK.Login.SessionKey);
    }

    


}
