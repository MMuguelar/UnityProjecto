using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCompletado : MonoBehaviour
{
    public static ControladorCompletado Instance;
    //public InteraccionLaberinto datosLab;

    public bool completo;

    private void Awake() {
        completo = false;
        if(ControladorCompletado.Instance == null){
            ControladorCompletado.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void CheckCompletado(bool completoLab)
    {
        if (completoLab == true)
        {
            completo = true;
        }else{
            completo = false;
        }
    }
}
