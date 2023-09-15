using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigo : Character
{
    public float distanciaDePersecucion = 20.0f;
    public float distanciaDeAtaque = 8.0f;
    public float tiempoEntreAtaques = 2.0f;
    public float velocidad = 1.0f;
    private Jugador player;

    private Animator anim;
    public Transform target;
    private float cronometroAtaque;
    private bool atacando;

    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.Find("Personaje principal").transform;
    }

    void Update()
    {
        ComportamientoEnemigo();
        base.Awake();
    }

    void ComportamientoEnemigo()
    {
        anim.SetBool("run", false);
        float distanciaAlObjetivo = Vector3.Distance(transform.position, target.position);

        if (distanciaAlObjetivo <= distanciaDePersecucion)
        {
            if (distanciaAlObjetivo <= distanciaDeAtaque)
            {
                RotarHaciaObjetivo();
                anim.SetBool("walk", false);
                anim.SetBool("run", false);
                anim.SetBool("attack", true);
                atacando = true;

                if (cronometroAtaque >= tiempoEntreAtaques)
                {
                    cronometroAtaque = 0;
                }
                else
                {
                    cronometroAtaque += Time.deltaTime;
                }
            }
            else
            {
                RotarHaciaObjetivo();
                anim.SetBool("walk", false);
                anim.SetBool("run", true);
                velocidad = 6.0f;
                transform.Translate(Vector3.forward * velocidad * 2 * Time.deltaTime);
                FinalizarAnimacionAtaque();
            }
        }
        else
        {
            FinalizarAnimacionAtaque();
        }
    }

    void RotarHaciaObjetivo()
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
    }

    void FinalizarAnimacionAtaque()
    {
        anim.SetBool("attack", false);
        atacando = false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hola");
        }
    }
}