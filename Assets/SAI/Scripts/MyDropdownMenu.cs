using UnityEngine;
using UnityEngine.UI;

public class MyDropdownMenu : MonoBehaviour
{
    public GameObject dropdownContent;
    public RectTransform dropdownMenuRect;

    private bool isExpanded;
    private RectTransform contentPanelRect;
    private Vector2 originalSize;



    public void ToggleDropdown()
    {
        isExpanded = !isExpanded;
        dropdownContent.SetActive(isExpanded);

        RectTransform rectTransform = GetComponentInChildren<Image>().GetComponent<RectTransform>();
        rectTransform.Rotate(new Vector3(0, 0, 180));
    }
}
