using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string LoginSceneName = "Login Scene";
    public void ChangeToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void ChangeToScene(string sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("App Closed");
    }

    public void LogoutUser()
    {
        print("1");
        SAI.SDK.Login.SignOut();

        print("2"); 
        PlayerPrefs.DeleteAll();

        print("3"); 
        SceneManager.LoadScene(LoginSceneName);


    }
}
