using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaDash : MonoBehaviour
{
    public Vector3 moveDirection;
    public const float maxDashTime = 0.1f;
    public float dashDistance = 0.1f; // Ajusta este valor para reducir la distancia del dash
    public float dashStoppingSpeed = 0.1f;
    float currentDashTime = maxDashTime;
    float dashSpeed = 10f;
    float cooldown = 3f;

    private void FixedUpdate() {

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(0, cooldown); // Asegurarse de que el cooldown no sea negativo
            Debug.Log("Cooldown: " + cooldown);
        }
    }
    private void Update()
    {
        /*if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(0, cooldown); // Asegurarse de que el cooldown no sea negativo
            Debug.Log("Cooldown: " + cooldown);
        }*/
    }

    // Update is called once per frame
    public void UseDash(CharacterController controller, Quaternion playerRotation)
    {
        if (Input.GetButtonDown("Dash") && cooldown <= 0){
        Debug.Log("Hola");
        }else{
            //Debug.Log("Coolgown: "+cooldown);
        }
        if (Input.GetButtonDown("Dash") && cooldown <= 0)
        {
            Debug.Log("Dash");
            cooldown = 3f;
            currentDashTime = 0;

            // Calcular la dirección del dash en función de la rotación del personaje
            Vector3 lookDirection = playerRotation * Vector3.forward;
            moveDirection = lookDirection * dashDistance;
        }
        else if (currentDashTime < maxDashTime)
        {
            // Continuar el dash actual
            currentDashTime += Time.fixedDeltaTime;

            // Puedes seguir aplicando la dirección y velocidad actual aquí si lo deseas
        }
        else
        {
            // Detener el dash si se ha alcanzado el tiempo máximo
            moveDirection = Vector3.zero;
        } 
        if (cooldown > 0)  // Add this condition to decrement cooldown when not dashing
        {
            cooldown -= Time.fixedDeltaTime;
            cooldown = Mathf.Max(0, cooldown);
            Debug.Log("Cooldown: " + cooldown);
        }

        // Mover al personaje
        controller.Move(moveDirection * Time.deltaTime * dashSpeed);
    }
}
