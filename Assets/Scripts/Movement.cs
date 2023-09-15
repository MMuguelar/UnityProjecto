using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anim;
    public float speed = 4f;
    public float rotationSpeed = 1000f;
    public float gravity = -9.8f;
    private Vector3 movement;
    private bool isMovingToMouse = false;
    public Jugador jug ;
    

    void Start()
    {
        //jug = GameObject.Find("Personaje principal");
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        jug = GameObject.Find("Personaje principal").GetComponent<Jugador>();
    }

    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        // Movimiento del personaje con las teclas de flecha
        movement.Normalize();

        if (Input.GetMouseButtonDown(1))
        {
            isMovingToMouse = true;
        
        }

        if (Input.GetMouseButtonUp(1))
        {
            isMovingToMouse = false;
        }

        if (isMovingToMouse)
        {
            // Rotación del personaje hacia la dirección del mouse
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDistance;

            if (groundPlane.Raycast(mouseRay, out rayDistance))
            {
                Vector3 point = mouseRay.GetPoint(rayDistance);
                Vector3 lookDirection = point - transform.position;

                if (lookDirection != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }

            // Mover al personaje hacia adelante
            movement = transform.forward;
           
        }

        // Gravedad
        movement.y += gravity * Time.deltaTime;

        // Mover al personaje
        characterController.Move(movement * speed * Time.deltaTime);

        // Animaciones
        float movementSpeed = movement.magnitude;
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetBool("IsRunW",true);
        }
         if(Input.GetMouseButtonUp(1))
        {
            anim.SetBool("IsRunW",false);
        }
        if(jug.life == 0){
            anim.SetBool("IsRunW",false);
            anim.SetBool("die",true);

        }
    }
}