using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
///  *** Temp script ***
///  Description : Fills the Transaction´s Data with external info.///  
///  Last Update : 10/8/2023
///  USAGE : after creating a transaction object call method "NewTransaction" to configure the display data.
/// </summary>

public class TransactionsDisplayHQ : MonoBehaviour
{ 
    /// <summary>
    /// Sets a new Transaction with the following params
    /// </summary>
    /// <param name="TittleText"> Tittle of the Transaction </param>
    /// <param name="AmountText"> Transaction Currency Value </param>
    /// <param name="OwnerNameText"> Name of the Transaction´s Owner</param>
    /// <param name="DateText"> Date of the transaction </param>
   public void NewTransaction(string TittleText,float AmountText,string OwnerNameText,string DateText)
    {
        m_TittleText.text = TittleText;
        m_AmountText.text = AmountText.ToString("F2");
        m_OwnerNameText.text = OwnerNameText;
        m_DateText.text = DateText;

    }

    #region Private Vars
    public TMP_Text m_TittleText;
    public TMP_Text m_AmountText;
    public TMP_Text m_OwnerNameText;
    public TMP_Text m_DateText;
    #endregion

}
