using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLabirinth : MonoBehaviour
{
     private CharacterController characterController;
    //private Animator anim;
    public float speed = 4f;
    public float rotationSpeed = 500f;
    public float gravity = -9.8f;
    private Vector3 movement;
    private Rigidbody rb;    private bool isMovingToMouse = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Mover horizontal");
        float ver = Input.GetAxis("Mover vertical"); 

        // Movimiento del personaje con las teclas de flecha
        movement = new Vector3(hor, 0f, ver);
        movement.Normalize();

        Vector3 movimiento = new Vector3(hor * speed, 0.0f, ver * speed);
        /*if(Input.GetMouseButtonDown(1))
        {
            anim.SetBool("IsRunW",true);
        }
         if(Input.GetMouseButtonUp(1))
        {
            anim.SetBool("IsRunW",false);
        }*/
        rb.AddForce(movimiento);
    }
}
