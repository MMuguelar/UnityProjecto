using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPopupButton : MonoBehaviour
{
    public string popupSceneName; // The name of the popup scene you created

    public void OpenPopup()
    {
        SceneManager.LoadScene(popupSceneName, LoadSceneMode.Additive);
    }
}

