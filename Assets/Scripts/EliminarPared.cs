using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eliminar : MonoBehaviour
{
    private Vector3 initialPos = new Vector3(51.5499992f,0.730000019f,9.23999977f);
    private Vector3 newPos = new Vector3(69.2900009f,0.730000019f,10.9799995f);
    public GameObject area, techo;

    private void Awake() {

        initialPos= transform.position;
    }
    void Update()
    {
        if (ControladorCompletado.Instance.condicional == true)
        {
            transform.position = newPos;
            Destroy(gameObject);
            area.SetActive(false);
            techo.SetActive(false);
        }else{
            transform.position= initialPos;
            area.SetActive(true);
            techo.SetActive(true);
        }
    }
}
