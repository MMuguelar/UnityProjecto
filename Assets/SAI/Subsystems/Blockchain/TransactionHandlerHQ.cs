using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Last Update : 10/08/23
/// USAGE :
///  Calling Method "AddRandomTransactionForTesting" will create a new Transaction object on the scene and  fill this object with random data or
///  use "AddFixedTransaction" Method if you wanna use fixed data. You can also set specific data using params//  
/// 
/// </summary>


public class TransactionHandlerHQ : MonoBehaviour
{
    #region Required Vars

    [Header("Required")]
    [Tooltip("This should be on the Prefabs/SceneName/ Folder")]
    public GameObject TransactionPrefab;   
    [Tooltip("The root object where transactions will take place")]
    public GameObject TransactionsRoot;
    [Tooltip("If check will auto full with 50 entries automatically")]
    public bool autoFill = false;

    #endregion

    #region Random Data Vars
    [Header("Random Local Data")]
    [Tooltip("Fill this with Temporal Data then replace with data from API")]
    public List<string> Names = new List<string>();
    public List<string> Owner = new List<string>();
    public List<string> Date = new List<string>();
    public float minValue = 0f;
    public float maxValue = 100f;
    #endregion

    #region Fixed Data Vars
    [Header("Fixed Data")]
    [Tooltip("Use this if you want to add fixed data.")]
    public string fixedTittle;
    public float fixedAmount;
    public string fixedOwner;
    public string fixedDate;
    #endregion

   


    #region Public Methods
    public void GetBalance(string wallet) {  }
    public void PerformTransaction(string sender, string reciver, double amount) { }
    public void GetTransactionHistory(string username) {  }
    #endregion

    #region Private Methods
    IEnumerator co_GetBalance(string username)
    {
        yield return null;
    }
    IEnumerator co_PerformTransaction(string sender, string reciver, double amount)
    {
        yield return null;
    }
    IEnumerator co_GetTransactionHistory(string username)
    {
        yield return null;
    }
    #endregion 

    private void Start()
    {
        if(autoFill) for(int i = 0;i<50;i++) { AddRandomTransactionForTesting(); } // Loads 50 Random entries 
    }
    /// <summary>
    /// Adds a Transaction with Random Data
    /// </summary>
    public void AddRandomTransactionForTesting()
    {
        Debug.Log("AddRandomTransactionForTesting just got called");
        GameObject transaction = Instantiate(TransactionPrefab);
        transaction.gameObject.transform.SetParent(TransactionsRoot.transform);
        TransactionsDisplayHQ transactionData = transaction.GetComponent<TransactionsDisplayHQ>();
        
        transactionData.NewTransaction(Names[Random.Range(0, Names.Count)], Random.Range(minValue, maxValue), Owner[Random.Range(0, Owner.Count)], Date[Random.Range(0, Date.Count)]);
    }
    /// <summary>
    /// Adds a Transaction with Fixed Data
    /// </summary>
    public void AddFixedTransaction()
    {
        Debug.Log("AddFixedTransaction just got called");
        GameObject transaction = Instantiate(TransactionPrefab);
        transaction.gameObject.transform.SetParent(TransactionsRoot.transform);
        TransactionsDisplayHQ transactionData = transaction.GetComponent<TransactionsDisplayHQ>();

        transactionData.NewTransaction(fixedTittle, fixedAmount, fixedOwner, fixedDate);
    }

    /// <summary>   
    /// Same as above but with params 
    /// </summary>
    /// <param name="tittle"> Transaction Tittle </param>
    /// <param name="amount"> Transaction Amount </param>
    /// <param name="owner"> Owner </param>
    /// <param name="date"> Date </param>
    public void AddFixedTransaction(string tittle,float amount,string owner,string date)
    {
        Debug.Log("AddFixedTransaction just got called");
        GameObject transaction = Instantiate(TransactionPrefab);
        transaction.gameObject.transform.SetParent(TransactionsRoot.transform);
        TransactionsDisplayHQ transactionData = transaction.GetComponent<TransactionsDisplayHQ>();

        transactionData.NewTransaction(tittle, amount, owner, date);
    }
}
