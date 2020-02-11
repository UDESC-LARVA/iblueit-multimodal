using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class FillYearsDropdown : MonoBehaviour
    {
        private void OnEnable()
        {
            FillWithYears();
        }

        private void FillWithYears()
        {
            var yearList = new List<string>();

            for (var i = DateTime.Today.Year; i >= 1900; i--)
                yearList.Add(i.ToString());

            GetComponent<Dropdown>().AddOptions(yearList);
        }
    }
}