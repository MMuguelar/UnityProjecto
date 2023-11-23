using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonInteraction : MonoBehaviour
{

    public GameObject aura;
    public GameObject sphere;
    public Material newMaterial;

    private Material originalMaterial;
    private Renderer objectRenderer;

    private void Start()
    {
        aura.SetActive(false);
        if (sphere != null)
        {
            objectRenderer = sphere.GetComponent<Renderer>();
            originalMaterial = objectRenderer.material;
        }
    }

    public void OnButtonEnter()
    {
        // Activate the GameObject
        aura.SetActive(true);

        // Change the GameObject's material
        if (objectRenderer != null && newMaterial != null)
        {
            objectRenderer.material = newMaterial;
        }
    }

    public void OnButtonExit()
    {
        // Deactivate the GameObject
        aura.SetActive(false);

        // Reset the GameObject's material to the original material
        if (objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
}
