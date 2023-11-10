using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDash : MonoBehaviour
{
    public static ControladorDash Instance;
    public bool condicional;

    private void Awake() {
        condicional = false;
        if(ControladorDash.Instance == null){
            ControladorDash.Instance = this;
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