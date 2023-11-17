using UnityEngine;
using UnityEngine.SceneManagement;

public class Ganar : MonoBehaviour
{
    public int numeroEscena;
    private void Update() {

        if(ControladorJefe.Instance.condicional == true)
        {
            Debug.Log("hola, estoy muerto");
            SceneManager.LoadScene(numeroEscena);
        } 
    }
}