using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Script_menu : MonoBehaviour
{
    public void EmpezarNivel(string NombreNivel){
        SceneManager.LoadScene(NombreNivel);
        
    }
    // Start is called before the first frame update
    public void Salir(){
        Application.Quit();
        Debug.Log("Aquí se cierra el juego");
    }
}
