using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCompletado : MonoBehaviour
{
    public static ControladorCompletado Instance;
    public bool condicional;

    private void Awake() {
        condicional = false;
        if(ControladorCompletado.Instance == null){
            ControladorCompletado.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void CheckBool(bool newBool)
    {
         condicional = newBool;
        /*if (newBool == true)
        {
            condicional = true;
        }else{
            condicional = false;
        }*/
    }
}
