using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorPopUp : MonoBehaviour
{
    public TMP_Text errorText;
    public TMP_Text tittleText;

    public void initialize(string text,string tittle)
    {
        errorText.text = text;
        tittleText.text = tittle;
    }

   
}
