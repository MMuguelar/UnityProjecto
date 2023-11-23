using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhoneNumbers;
using System.Collections.Immutable;

/// <summary>
/// By Ares.
/// CognitionHQ.
/// </summary>


public class PhoneNumberFormatterHQ : MonoBehaviour
{

 
    /// <summary>
    /// Transform given number into International Phone Number if able
    /// </summary>
    /// <param name="number">Number to convert</param>
    /// <returns>formated version as string</returns>
    
    public string InternationalFormatter(string number)
    {       

        if(number.Length < 6) return number;



        //print("Intentando parsear esto : " + number);

        //print("creando instancia pu");
        PhoneNumberUtil pu = PhoneNumberUtil.GetInstance();
        //print("Instancia creada");

        //print("Voy a usar el pu para parsear el numero");
        PhoneNumber phoneNumber = pu.Parse(number, null);

        //print("voy a preguntar si el numero es valido");
        if (pu.IsValidNumber(phoneNumber)) 
        {
            string result = pu.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
            return result;
        }
        else
        {
            return number;
        }
    }
    




}
