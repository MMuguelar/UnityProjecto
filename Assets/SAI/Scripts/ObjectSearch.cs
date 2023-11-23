using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectSearch : MonoBehaviour
{
    public TMP_InputField searchInputField;
    public string tagToInclude;
    List<TMP_Text> tmpTextList = new List<TMP_Text>();

    // Find all game objects with the specified tag
    GameObject[] taggedObjects;

    public void Start()
    {
        RefreshTaggedObjects();
    }

    public void RefreshTaggedObjects()
    {
        taggedObjects = GameObject.FindGameObjectsWithTag(tagToInclude);
        Debug.Log(taggedObjects.Length);
    }

    public void Search()
    {
        // Get the search query from the input field
        string searchQuery = searchInputField.text.ToLower();

        // Loop through the game objects with the specified tag
        foreach (GameObject go in taggedObjects)
        {
            // Get the TMP_Text component from the game object
            TMP_Text tmpText = go.GetComponentInChildren<TMP_Text>();

            // Check if the TMP_Text component exists and add it to the list
            if (tmpText != null)
            {
                Debug.Log("ENTERED IF!");
                tmpTextList.Add(tmpText);
            }
        }

        Debug.Log("TAGGEDOBJECTS AMOUNT: " + taggedObjects.Length);
        Debug.Log("TMPTEXT AMOUNT: " + tmpTextList.Count);

        // Loop through the list of TMP_Text components
        foreach (TMP_Text tmpText in tmpTextList)
        {
            // Check if the text contains the search query
            if (tmpText.text.ToLower().Contains(searchQuery))
            {
                // Show the game object
                tmpText.gameObject.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                // Hide the game object
                tmpText.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}