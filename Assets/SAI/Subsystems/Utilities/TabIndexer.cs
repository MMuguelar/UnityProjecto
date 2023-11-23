using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class TabIndexer : MonoBehaviour
{
    EventSystem system;
    public Selectable Inputs;
    

    // Start is called before the first frame update
    void Start()
    {
        initi();

    }

    public void initi()
    {
        system = EventSystem.current;
        Inputs.Select();
    }

    // Update is called once per frame
    void Update()
    {
        changeOfParameters();
    }

    void changeOfParameters()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }
}


