using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestToggleButton : MonoBehaviour
{
    public TMP_Text Toggle1;
    public TMP_Text Toggle2;
    public TMP_Text Toggle3;
    public TMP_Text Toggle4;
    public Toggle toggleIsOn1;
    public Toggle toggleIsOn2;
    public Toggle toggleIsOn3;
    public Toggle toggleIsOn4;
    public Button testButton;

    void Update()
    {
        int intValue1 = int.Parse(Toggle1.text);
        int intValue2 = int.Parse(Toggle2.text);
        int intValue3 = int.Parse(Toggle3.text);
        int intValue4 = int.Parse(Toggle4.text);

        

        // We check if at least one of the values ​​is different from zero
        if ((toggleIsOn1.isOn && intValue1 != 0 )|| (toggleIsOn2.isOn && intValue2 != 0 )|| (toggleIsOn3.isOn && intValue3 != 0) || (toggleIsOn4.isOn && intValue4 != 0))
        {
            testButton.interactable = true; // Make the button interactive
        }
        else
        {
            testButton.interactable = false; // Make the button not interactive
        }
    }
}
