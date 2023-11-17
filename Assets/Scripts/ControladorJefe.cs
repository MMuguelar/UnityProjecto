using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJefe : MonoBehaviour
{
    public static ControladorJefe Instance;
    public bool condicional;

    private void Awake() {
        condicional = false;
        if(ControladorJefe.Instance == null){
            ControladorJefe.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void CheckBool(bool newBool)
    {
        if (newBool == true)
        {
            condicional = true;
        }else{
            condicional = false;
        }
    }
}