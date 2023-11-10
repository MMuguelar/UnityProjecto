using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMuerte : MonoBehaviour
{
    public static ControladorMuerte Instance;
    public bool condicional;

    private void Awake() {
        condicional = false;
        if(ControladorMuerte.Instance == null){
            ControladorMuerte.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the boolean to false whenever a new scene is loaded
        condicional = false;
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