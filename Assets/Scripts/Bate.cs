using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bate : MonoBehaviour
{
    private Movement mov; // Declarar una variable para mantener una referencia al script Movement

    void Start()
    {
        mov = GameObject.Find("Personaje principal").GetComponent<Movement>(); // Mover esta línea a Start
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||  collision.gameObject.CompareTag("Jefe"))
        {
            enemigo enemy = collision.gameObject.GetComponent<enemigo>();

            // Disminuir la salud del enemigo usando el método ApplyDamage de Movement
            if (mov != null) // Asegúrate de que mov no sea nulo
            {
                mov.ApplyDamage(enemy);
            }
            Debug.Log("El enemigo tiene: " + enemy.life + " de vida");
        }
    }
}




  