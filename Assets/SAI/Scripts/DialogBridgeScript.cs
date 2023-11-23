using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBridgeScript : MonoBehaviour
{
    public DialogScript dialogScript;


    public void OpenDialog()
    {
        GameObject dialogBridgeObject = GameObject.Find("DialogBridgeGameObject");
        dialogScript = dialogBridgeObject.GetComponent<DialogScript>();

        Transform desiredChild = transform.GetChild(2); // Get the desired child
        string childName = desiredChild.name;

        dialogScript.OpenDialog(childName);
    }

    public void CloseDialog()
    {
        GameObject dialogBridgeObject = GameObject.Find("DialogBridgeGameObject");
        dialogScript = dialogBridgeObject.GetComponent<DialogScript>();
        dialogScript.CloseDialog();
    }

}
