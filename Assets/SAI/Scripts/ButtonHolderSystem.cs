using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHolderSystem : MonoBehaviour
{
    [Header("Button Holder")]
    public GameObject wallet;
    public GameObject messages;
    public GameObject nodeControl;
    public GameObject addNode;
    public GameObject iaSetup;
    
    [Header("Navigation's Buttons")]
    public GameObject nextButton;
    public GameObject prevButton;

    public void previous()
    {
        if(iaSetup.activeInHierarchy)
        {
            nextButton.SetActive(true);
            wallet.SetActive(true);
            messages.SetActive(true);
            nodeControl.SetActive(true);
            addNode.SetActive(true);
            iaSetup.SetActive(false);
            prevButton.SetActive(false);
        }
    }

    public void next()
    {
        if (!iaSetup.activeInHierarchy)
        {
            nextButton.SetActive(false);
            wallet.SetActive(false);
            messages.SetActive(false);
            nodeControl.SetActive(false);
            addNode.SetActive(false);
            iaSetup.SetActive(true);
            prevButton.SetActive(true);
        }
       
    }
}
