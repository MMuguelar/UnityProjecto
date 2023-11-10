using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Muerte : MonoBehaviour
{
    public int numeroEscena;
    private void Update() {

        if(ControladorMuerte.Instance.condicional == true)
        {
            Debug.Log("hola, estoy muerto");
            SceneManager.LoadScene(numeroEscena);
        } 
    }
}
