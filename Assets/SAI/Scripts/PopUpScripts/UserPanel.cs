using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UserPanel : MonoBehaviour
{
    [System.Serializable]
    public class Transaction
    {
        public string Description;
        public string Date;
        public float Amount;
        public string Name;
    }

    public GameObject transactionPanel;
    public Transform parentTransform;

    public GameObject walletPanel;
    public GameObject messagesPanel;
    public GameObject nodeControlPanel;
    public GameObject addNodePanel;
    public GameObject iaSetup;
    public GameObject iaConfiguration;
    public GameObject memory;

    public void OpenWalletPanel()
    {
        if (!walletPanel.activeInHierarchy)
        {
            walletPanel.SetActive(true);
            messagesPanel.SetActive(false);
            nodeControlPanel.SetActive(false);
            addNodePanel.SetActive(false);
            iaSetup.SetActive(false);
            iaConfiguration.SetActive(false);
        }
    }

    public void OpenMessagesPanel()
    {
        if (!messagesPanel.activeInHierarchy)
        {
            walletPanel.SetActive(false);
            messagesPanel.SetActive(true);
            nodeControlPanel.SetActive(false);
            addNodePanel.SetActive(false);
            iaSetup.SetActive(false);
            iaConfiguration.SetActive(false);
        }
    }

    public void OpenNodeControlPanel()
    {
        if (!nodeControlPanel.activeInHierarchy)
        {
            walletPanel.SetActive(false);
            messagesPanel.SetActive(false);
            nodeControlPanel.SetActive(true);
            addNodePanel.SetActive(false);
            iaSetup.SetActive(false);
            iaConfiguration.SetActive(false);
        }
    }

    public void OpenAddNodePanel()
    {
        if (!addNodePanel.activeInHierarchy)
        {
            walletPanel.SetActive(false);
            messagesPanel.SetActive(false);
            nodeControlPanel.SetActive(false);
            addNodePanel.SetActive(true);
            iaSetup.SetActive(false);
            iaConfiguration.SetActive(false);
        }
    }

    public void OpenIASetup()
    {
        if (!iaSetup.activeInHierarchy)
        {
            walletPanel.SetActive(false);
            messagesPanel.SetActive(false);
            nodeControlPanel.SetActive(false);
            addNodePanel.SetActive(false);
            iaSetup.SetActive(true);
            iaConfiguration.SetActive(false);
        }
    }

    public void IAConfiguration()
    {
        if(!iaConfiguration.activeInHierarchy)
        {
            walletPanel.SetActive(false);
            messagesPanel.SetActive(false);
            nodeControlPanel.SetActive(false);
            addNodePanel.SetActive(false);
            iaSetup.SetActive(false);
            iaConfiguration.SetActive(true);
        }
    }
    public void InstantiateUI(Transaction data)
    {
        GameObject uiInstance = Instantiate(transactionPanel, parentTransform);
        TMP_Text name;
        TMP_Text description;
        TMP_Text date;
        TMP_Text amount;

        description = uiInstance.transform.GetChild(0).GetComponent<TMP_Text>();
        name = uiInstance.transform.GetChild(2).GetComponent<TMP_Text>();
        date = uiInstance.transform.GetChild(3).GetComponent<TMP_Text>();
        amount = uiInstance.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();

        description.text = data.Description;
        name.text = data.Name;
        date.text = data.Date;
        amount.text = data.Amount.ToString();
    }

    private void Start()
    {
        /*
        Transaction tra = new Transaction();
        tra.Amount = 2.5f;
        tra.Date = "DATE";
        tra.Description = "Some Description";
        tra.Name = "Martin";
        for (int i = 0; i < 3; i++)
        {
            InstantiateUI(tra);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
