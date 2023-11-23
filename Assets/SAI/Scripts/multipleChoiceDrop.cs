using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class multipleChoiceDrop : MonoBehaviour
{
    public TMP_Text label;
    public TMP_Dropdown memoryType;
    public List<string> selectedOptions = new List<string>();
    private void Start()
    {
        memoryType.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        string option = memoryType.options[index].text;

        if (!selectedOptions.Contains(option))
        {
            selectedOptions.Add(option);
        }
        else
        {
            selectedOptions.Remove(option);
        }
        label.text = string.Join(", ", selectedOptions.ToArray());
    }
}

