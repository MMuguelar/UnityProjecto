using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLogout : MonoBehaviour
{
    private void OnApplicationQuit()
    {
       //if (SAI.subsystems.api.sessionKey != string.Empty) SAI.subsystems.api.Logout();
    }
}
