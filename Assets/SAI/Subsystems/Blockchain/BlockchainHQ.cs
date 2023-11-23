using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class BlockchainHQ : MonoBehaviour
{
    /// <summary>
    /// Checks the user Balance, result will be shown in Event :
    /// </summary>
    /// <param name="user">wallet ID</param>
   public void GetBalance(string user)
    {
        TransactionHandlerHQ transaction = new TransactionHandlerHQ();
        transaction.GetBalance(user);        

    }

    /// <summary>
    /// Gets the user History trough event
    /// </summary>
    /// <param name="user"></param>
    public void GetHistory(string user)
    {
        TransactionHandlerHQ transaction = new TransactionHandlerHQ();
        transaction.GetTransactionHistory(user);
    }

    public void PerformTransaction(string sender,string reciver,double amount)
    {
        TransactionHandlerHQ transaction = new TransactionHandlerHQ();
        transaction.PerformTransaction(sender, reciver, amount);
    }
}
