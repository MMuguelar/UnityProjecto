using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClosePopupButton : MonoBehaviour
{
    public void ClosePopup()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
