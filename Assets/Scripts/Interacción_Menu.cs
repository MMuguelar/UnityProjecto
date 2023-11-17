using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interacción_Menu : MonoBehaviour
{
    public int numeroEscena;

    public void iniciar() {
        SceneManager.LoadScene(numeroEscena);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}