using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public static class NewSharedData
    {
        public static string fromDropDown { get; set; }
        public static string toDropDown { get; set; }
    }
    class DatePickerDropDownTextMeshPro : DatePickerDropDownBase
    {
        public TMPro.TextMeshProUGUI Label= null;

        protected override void SetText(string text)
        {
            if (Label != null)
                Label.text = text;
        }

        public void from()
        {
            var dropdown = GetComponent<DatePickerDropDownBase>();
            DateTime? selectedDate = dropdown.GetSelectedDate();
            NewSharedData.fromDropDown = selectedDate.ToString();
            Debug.Log("Test date" + selectedDate);
            
        }
        
        public void to()
        {
            var dropdown = GetComponent<DatePickerDropDownBase>();
            DateTime? selectedDate = dropdown.GetSelectedDate();
            NewSharedData.toDropDown = selectedDate.ToString();
            Debug.Log("Test date" + selectedDate);
        }
    }
}
