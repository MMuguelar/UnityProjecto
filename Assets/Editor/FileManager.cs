using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    public void OpenFileBrowser()
    {
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Select a file", "", "");

        if (filePath.Length != 0)
        {
            // Replace the following line with your existing code that receives the file.
            Debug.Log("Selected file: " + filePath);
        }
    }
}
