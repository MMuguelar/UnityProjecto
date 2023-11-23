using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
public class DateInputField : MonoBehaviour
{
    public TMPro.TMP_InputField from;

    private void Start()
    {
        from.onValueChanged.AddListener(ValidateInput);
    }

    private void LateUpdate()
    {
        from.MoveToEndOfLine(false, true);
    }

    private void ValidateInput(string text)
    {
        if (text.Length >= 3)
        {
            if (text.Substring(2, 1) != "/")
            {
                text = text.Substring(0, 2) + "/"  + text.Substring(3);
            }
        }

        if (text.Length >= 6)
        {
            if (text.Substring(5, 1) != "/")
            {
                text = text.Substring(0, 5) + "/" + text.Substring(6);
            }
        }
        from.text = text;
        from.caretPosition++;
    }
}
