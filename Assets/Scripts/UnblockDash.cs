using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnblockDash : MonoBehaviour
{
    public bool DashActivo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DashActivo = true;
            gameObject.SetActive(false);
        }
    }
}
