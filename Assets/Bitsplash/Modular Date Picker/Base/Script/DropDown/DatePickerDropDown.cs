using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public static class NewSharedData2
    {
        public static string fromDropDown { get; set; }
        public static string toDropDown { get; set; }
    }
    public class DatePickerDropDown : DatePickerDropDownBase
    {
        public Text Label;

        protected override void SetText(string text)
        {
            if (Label != null)
                Label.text = text;
        }

        public void from()
        {
            var dropdown = GetComponent<DatePickerDropDownBase>();
            DateTime? selectedDate = dropdown.GetSelectedDate();
            NewSharedData2.fromDropDown = selectedDate.ToString();
            Debug.Log("Test date" + selectedDate);

        }

        public void to()
        {
            var dropdown = GetComponent<DatePickerDropDownBase>();
            DateTime? selectedDate = dropdown.GetSelectedDate();
            NewSharedData2.toDropDown = selectedDate.ToString();
            Debug.Log("Test date" + selectedDate);
        }
    }


}
