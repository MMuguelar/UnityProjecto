using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SAISDK;

public class Interacción_Menu : MonoBehaviour
{
    public int numeroEscena;

    public void iniciar() {
        SceneManager.LoadScene(numeroEscena);
    }
    public void ExampleLogOut(){
        StartCoroutine(LoginSys.LogOut(LogoutCallBack));
    }
    private void LogoutCallBack(bool response){

        print("Logout Callback response : " + response);

        if(response)
        {
            print("Logout OK");
            SceneManager.LoadScene(0);
        }
        else
        {
            print("Logout Bad");
        }
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