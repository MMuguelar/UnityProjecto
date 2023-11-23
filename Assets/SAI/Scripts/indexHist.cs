using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class indexHist : MonoBehaviour
{
    public TMP_Text pages;
    private int pagesIndex = 1;

    private void Start()
    {
        pages.text = pagesIndex.ToString();
    }
    public void ResetPagesIndex()
    {
        pagesIndex = 1;
        pages.text = pagesIndex.ToString();
    }
    public void nextButton()
    {
        pagesIndex++;
        pages.text = pagesIndex.ToString();
    }

    public void prevButton()
    {
        pagesIndex--;
        pages.text = pagesIndex.ToString();
    }
}
