using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleAndStepperController : MonoBehaviour
{
    public Toggle toggle;
    public GameObject numericStepper;
    public TMP_Text valueText;

    private int currentValue = 0;
    private Color normalColor; // Store the normal color to restore later

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        normalColor = numericStepper.GetComponent<Image>().color; // Store the normal color
       
        toggle.isOn = !toggle.isOn; //
        
        OnToggleValueChanged(toggle.isOn); //
    }

    private void OnToggleValueChanged(bool newValue)
    {
        newValue = !newValue; //
            
        if (!newValue)
        {
            currentValue = 0; // Reset the value when the toggle is turned off
            UpdateValueText();
        }

        UpdateNumericStepperAlpha();
    }

    public void IncrementValue()
    {
        if (toggle.isOn)
        {
            currentValue++;
            UpdateValueText();
        }
    }

    public void DecrementValue()
    {
        if (toggle.isOn && currentValue > 0)
        {
            currentValue--;
            UpdateValueText();
        }
    }

    private void UpdateValueText()
    {
        valueText.text = currentValue.ToString();
    }

    private void UpdateNumericStepperAlpha()
    {
        float alpha = toggle.isOn ? 1.0f : 0.5f; // Adjust alpha based on toggle state

        Image[] images = numericStepper.GetComponentsInChildren<Image>(true); // Get all child images
        foreach (Image image in images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }

        TMP_Text[] tmpTexts = numericStepper.GetComponentsInChildren<TMP_Text>(true); // Get all child TMP_Text components
        foreach (TMP_Text tmpText in tmpTexts)
        {
            Color textColor = tmpText.color;
            tmpText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
        }

        Button[] buttons = numericStepper.GetComponentsInChildren<Button>(true); // Get all child buttons
        foreach (Button button in buttons)
        {
            button.interactable = toggle.isOn;
        }
    }

}
