using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnblockDash : MonoBehaviour
{
    public bool DashActivo;
    // Start is called before the first frame update
    private void Awake() {
        DashActivo = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag =="Player")
        {
            DashActivo = true;
            ControladorDash.Instance.CheckBool(DashActivo);
            gameObject.SetActive(false);
        }
    }
}
