using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TermsAndCondLogic : MonoBehaviour
{
    public Toggle toggle;
    public Button button;

    private void Start()
    {
        toggle.onValueChanged.AddListener(ToggleValueChanged);
        ToggleValueChanged(toggle.isOn);
    }

    public void OpenURL()
    {
        Application.OpenURL("https://spaceai.com/terms-and-conditions");
    }


    public void OpenURL(string url) { SAI.SDK.Util.OpenURL(url); }

    private void ToggleValueChanged(bool newValue)
    {
        // Check the new value of the toggle and activate/deactivate the button accordingly
        if (newValue)
        {
            button.interactable = true; // Activate the button
           // Debug.Log("Button Activated");
        }
        else
        {
            button.interactable = false; // Deactivate the button
           // Debug.Log("Button Deactivated");
        }
    }
}
