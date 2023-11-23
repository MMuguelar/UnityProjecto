using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    public Image on;
    public Image off;

    public void On()
    {
        on.gameObject.SetActive(false);
        off.gameObject.SetActive(true);
    }
    public void Off()
    {
        on.gameObject.SetActive(true);
        off.gameObject.SetActive(false);
    }
}
