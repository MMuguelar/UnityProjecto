using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textolook : MonoBehaviour
{
    public Transform target;
    //public float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
      target = GameObject.Find("Personaje principal").transform;
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 targetDirection = target.position - transform.position;
      //transform.rotation = Quaternion.LookRotation(targetDirection);
      // = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
