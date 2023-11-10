using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Character
{
    NavMeshAgent agente;
    private float rangoDeAlerta = 10f;
    public LayerMask capaDelJugador;
    private Jugador player;

    protected override void Awake()
    {
        maxLife = 15f;
        healthSlider.value = life;
        contactDamage = 2f;
        base.Awake();
        
        agente = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        
        //healthSlider.value = life;
        //bool estarAlerta = Physics.CheckSphere(transform.position, rangoDeAlerta, capaDelJugador);
        /*if (estarAlerta) 
        {
            //agente.SetDestination(player.transform.position);
        }*/
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Jugador player = collision.gameObject.GetComponent<Jugador>();
            if (player != null)
            {
               // TakeDamage(player.contactDamage);
               // player.TakeDamage(contactDamage);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente Enemy en el objeto de colisión.");
            }
        }
    }
}