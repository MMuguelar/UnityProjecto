
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteraccionLaberinto : MonoBehaviour
{
    public int numeroEscena;
    //public GameObject Texto;
    private bool zona = false;
    private bool player1 = false;   
    private bool player2 = false;
    
    private void Update() {

        if(player1 == true & player2 == true)
        {
            zona = true;
        }
        if (zona == true)
        {
            SceneManager.LoadScene(numeroEscena);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                player1 = true;
                Debug.Log("Entro Player");
            break;
            case "Enemy":
                player2 = true;
                Debug.Log("Entro Enemy");
            break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                player1 = false;
                Debug.Log("Salió Player");
            break;
            case "Enemy":
                player2 = false;
                Debug.Log("Salió Enemy");
            break;
        }
    }
}
