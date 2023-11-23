
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessagePanelPopUpInfo : MonoBehaviour
{
    public TMP_Text tittle_text;
    public TMP_Text message_text;

    private bool isExpanded = false;
    private RectTransform panelRectTransform;
    private Vector2 originalSizeDelta;


    // drag to destroy
    private Vector3 startPosition;
    private Transform canvasTransform;
    private Transform messageTransform;
    private GameObject mensajePopup;

    public void OnDrag(PointerEventData eventData)
    {
        // Mueve el mensaje junto con el puntero del mouse/finger.
        messageTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Si se suelta el mensaje fuera del Canvas, destruye el mensaje.
        Vector3 dropPosition = eventData.pointerCurrentRaycast.worldPosition;

        if (!RectTransformUtility.RectangleContainsScreenPoint(canvasTransform as RectTransform, dropPosition))
        {
            EliminarMensaje();
        }
    }

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
        originalSizeDelta = panelRectTransform.sizeDelta;
        mensajePopup = this.gameObject;
        canvasTransform = transform.parent.parent; // Asume que el padre del MensajePopup es el Canvas.
        messageTransform = transform.parent;
        
    }

    
    public void MostrarMensaje(string msgTittle, string msgContent)
    {
        tittle_text.text = msgTittle;
        message_text.text = msgContent;
        isExpanded = false;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void ToggleExpansion()
    {
        print("toggling expansion");
        isExpanded = !isExpanded;
        if (isExpanded)
        {
            // Expandir el panel
            panelRectTransform.sizeDelta = new Vector2(originalSizeDelta.x, originalSizeDelta.y * 2f); // Ajusta el factor de expansión según sea necesario
        }
        else
        {
            // Colapsar el panel
            panelRectTransform.sizeDelta = originalSizeDelta;
        }
    }

    public void EliminarMensaje()
    {
        Destroy(gameObject);
    }
}
