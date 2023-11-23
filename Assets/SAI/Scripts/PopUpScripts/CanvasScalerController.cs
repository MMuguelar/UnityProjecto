using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerController : MonoBehaviour
{
    public RectTransform panelRectTransform;
    public float popupScaleFactor = 0.8f;

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            panelRectTransform.anchorMin = Vector2.zero;
            panelRectTransform.anchorMax = Vector2.one;
            panelRectTransform.offsetMin = Vector2.zero;
            panelRectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            float mainSceneWidth = Screen.width;
            float mainSceneHeight = Screen.height;

            float panelWidth = mainSceneWidth * popupScaleFactor;
            float panelHeight = mainSceneHeight * popupScaleFactor;

            panelRectTransform.sizeDelta = new Vector2(panelWidth, panelHeight);
        }
    }
}
