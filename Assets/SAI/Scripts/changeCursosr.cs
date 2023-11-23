using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeCursosr : MonoBehaviour
{
    public Texture2D newCursor;

    private void Start()
    {
        // Establece el cursor predeterminado al inicio.
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }
}
