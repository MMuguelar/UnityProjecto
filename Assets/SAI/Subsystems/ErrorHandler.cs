
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    [Header("Properties")]
    public ErrorPopUp errorPopup;

    
    private void Start()
    {

        if (errorPopup != null) return;
        
        Object[] popups = Resources.FindObjectsOfTypeAll(typeof(ErrorPopUp));
        if(popups.Length > 0)
        {
            errorPopup = Resources.FindObjectsOfTypeAll(typeof(ErrorPopUp))[0].GetComponent<ErrorPopUp>();
        }

        else { Debug.Log("Error Popup Not Present in Scene, wont be able to display err. msg."); }

    }

    
    public void ShowPopup(string mensaje,string titulo)
    {
        if(errorPopup==null) errorPopup = Resources.FindObjectsOfTypeAll(typeof(ErrorPopUp))[0].GetComponent<ErrorPopUp>();
        errorPopup.gameObject.SetActive(true);
        errorPopup.initialize(mensaje, titulo);
        
    }


    public void ShowPopupJson(string JsonError)
    {
        if(errorPopup==null) errorPopup = Resources.FindObjectsOfTypeAll(typeof(ErrorPopUp))[0].GetComponent<ErrorPopUp>();
        ErrorClass err = JsonUtility.FromJson<ErrorClass>(JsonError);
        print($"ERR={err.error},DETAIL={err.detail}");

        errorPopup.gameObject.SetActive(true);
        errorPopup.initialize("Unknown Error","");

        if (err.error == null)
        {
            print("API ERROR: error parameter is missing, using details instead");
            
            if (err.detail == null)
            {
                if(err.message != null)
                {
                    errorPopup.errorText.text = err.message;
                    errorPopup.initialize(err.message, "Error");
                }
                else
                {
                    errorPopup.initialize($"Unknown Error Please Contact Support, Error : {JsonError}", "Error");
                }
            }
                
            else
            {
                print("Se encontro un detalle en el error");
                errorPopup.errorText.text = err.detail;
                errorPopup.initialize(err.detail, "Error");
            }

        }
        else
        {
            print("Se encontro un mensaje de error adjunto");
            errorPopup.initialize(err.error, "Error");
        }
        
       
    }


    [System.Serializable]
    public class ErrorClass
    {
        public string error;
        public string detail;
        public string message;
    }


}
