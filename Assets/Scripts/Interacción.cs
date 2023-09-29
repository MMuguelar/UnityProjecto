using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaccion : MonoBehaviour
{
    public int numeroEscena;
    public GameObject Texto;
    private bool zona;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.E) && zona == true)
        {
            SceneManager.LoadScene(numeroEscena);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("entre");
            Texto.SetActive(true);
            zona = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Texto.SetActive(false);
            zona = false;
        }
    }
}
