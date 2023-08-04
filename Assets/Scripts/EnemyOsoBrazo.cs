using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyOsoBrazo : Character
{
    NavMeshAgent agente;
    private float rangoDeAlerta = 10f;
    public LayerMask capaDelJugador;
    private Jugador player;

    protected override void Awake()
    {
      
        contactDamage = 2f;
        base.Awake();
     
    }

    protected override void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador player = collision.gameObject.GetComponent<Jugador>();
            if (player != null)
            {
                TakeDamage(player.contactDamage);
                player.TakeDamage(contactDamage);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente Enemy en el objeto de colisión.");
            }
        }
    }
}