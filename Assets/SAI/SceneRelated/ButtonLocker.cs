using Org.BouncyCastle.Asn1.BC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonLocker : MonoBehaviour
{
    public Button btn;
    public UnityEvent onDestroyGO;
    

    public void LockBtn()
    {
        btn.interactable = false;
        btn.gameObject.GetComponent<Image>().color = Color.gray;
    }

    public void UnlockBtn()
    {
        btn.interactable = true;
        btn.gameObject.GetComponent<Image>().color = Color.blue;
    }


    public void DestroyBtn()
    {
        onDestroyGO.Invoke();
        
    }

    public void objToHide()
    {

    }


}
