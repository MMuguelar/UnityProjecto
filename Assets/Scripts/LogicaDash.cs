using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaDash : MonoBehaviour
{
  public Vector3 moveDirection;
    public const float maxDashTime = 0.15f;
    public float dashDistance = 0.1f; // Ajusta este valor para reducir la distancia del dash
    public float dashStoppingSpeed = 0.1f;
    float currentDashTime = maxDashTime;
    float dashSpeed = 10;
    public UnblockDash boolDash;

    private void Update()
    {

    }

    // Update is called once per frame
    public void UseDash(CharacterController controller, Quaternion playerRotation)
    {
        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash");
            currentDashTime = 0;

            // Calcular la dirección del dash en función de la rotación del personaje
            Vector3 lookDirection = playerRotation * Vector3.forward;
            moveDirection = lookDirection * dashDistance;
        }
        else if (currentDashTime < maxDashTime)
        {
            // Continuar el dash actual
            currentDashTime += Time.deltaTime;

            // Puedes seguir aplicando la dirección y velocidad actual aquí si lo deseas
        }
        else
        {
            // Detener el dash si se ha alcanzado el tiempo máximo
            moveDirection = Vector3.zero;
        }

        // Mover al personaje
        controller.Move(moveDirection * Time.deltaTime * dashSpeed);
    }
}
