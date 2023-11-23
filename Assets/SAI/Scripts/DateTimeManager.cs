using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DateTimeManager : MonoBehaviour
{

    public TMP_Text dateText;
    private void Start()
    {
        // Update the date and time initially
        UpdateDateTime();

        // Call the UpdateDateTime function every second to keep it up-to-date
        InvokeRepeating("UpdateDateTime", 0f, 1f);
    }

    private void UpdateDateTime()
    {
        // Get the current date and time
        DateTime currentDateTime = DateTime.Now;

        // Extract the day, month, hours, and minutes
        int day = currentDateTime.Day;
        int month = currentDateTime.Month;
        int hours = currentDateTime.Hour;
        int minutes = currentDateTime.Minute;

        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        string dayOfWeek = currentDateTime.ToString("ddd", cultureInfo);

        // Display the date, day of the week, and time in the Unity console (you can modify this to update UI elements)
        dateText.text = dayOfWeek + " " + day + "/" + month + " " + hours.ToString("00") + ":" + minutes.ToString("00");
    }

}
