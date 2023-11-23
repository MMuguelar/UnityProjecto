using UnityEngine;

public class Scale3DWithUI : MonoBehaviour
{
    // Reference to the 3D object you want to scale
    public Transform objectToScale;

    private Vector3 initialScale;
    private Vector2 initialSizeDelta;

    private void Start()
    {
        // Store the initial scale of the 3D object
        initialScale = objectToScale.localScale;

        // Store the initial sizeDelta of the UI object
        initialSizeDelta = GetComponent<RectTransform>().sizeDelta;
    }

    private void Update()
    {
        // Calculate the scale factor for all three axes based on the UI object's current scale
        float scaleFactor = GetComponent<RectTransform>().sizeDelta.x / initialSizeDelta.x;

        // Update the 3D object's scale based on the UI object's current scale
        objectToScale.localScale = initialScale * scaleFactor;
    }
}
