using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJefe : MonoBehaviour
{
    public static ControladorJefe Instance;
    public bool condicional;

    private void Awake() {
        condicional = false;
        if(ControladorJefe.Instance == null){
            ControladorJefe.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        condicional = false;
    }

    public void CheckBool(bool newBool)
    {
        condicional = newBool;
    }
}