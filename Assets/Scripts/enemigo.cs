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

    public int damageFrame = 10; // Change this to the frame you want.

    protected override void Awake() {
        maxLife = 15f;
        healthSlider.value = life;
        contactDamage = 2f;
        damageCooldown = 1.0f;
        base.Awake();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Personaje principal").transform;
    }

    protected override void Update()
    {
        ComportamientoEnemigo();
        base.Update();
    }

   void ComportamientoEnemigo()
{
    anim.SetBool("run", false);
    float distanciaAlObjetivo = Vector3.Distance(transform.position, target.position);

    // Check if the player is within the pursuit distance.
    if (distanciaAlObjetivo <= distanciaDePersecucion)
    {
        RotarHaciaObjetivo();
        anim.SetBool("walk", false);
        anim.SetBool("run", false);

        // Check if the player is within the attack range.
        if (distanciaAlObjetivo <= distanciaDeAtaque)
        {
            RotarHaciaObjetivo();
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
            anim.SetBool("attack", true);

            // Check if we are at the specified damage frame.
            if (!atacando)
            {
                DealDamage(); // Call a method to apply damage to the player.
                //Debug.Log("empuje");
            }

            // Check if enough time has passed to allow another attack.
            if (cronometroAtaque >= tiempoEntreAtaques)
            {
                cronometroAtaque = 0;
                atacando = true; // Set atacando to true for continuous attacks.
                  //Debug.Log("real");
            }
            else
            {
                cronometroAtaque += Time.deltaTime;
                atacando = false; // Set atacando to false if the attack cooldown is not complete.
                //Debug.Log("falso");
            }
        }
        else
        {
            anim.SetBool("attack", false);
            atacando = false;

            anim.SetBool("run", true);
            velocidad = 6.0f;
            transform.Translate(Vector3.forward * velocidad * 2 * Time.deltaTime);
        }
        

    }
    else
    {
        FinalizarAnimacionAtaque();
          Debug.Log("falsoPosi");

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

    void DealDamage()
    {
        // Implement your damage logic here.
        // You can access the player reference (this.player) and apply damage to it.
        // Example: this.player.TakeDamage(contactDamage);
        // Apply a force to push the player back slightly.
        if (this.player != null)
        {
            Rigidbody playerRigidbody = this.player.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                Vector3 pushDirection = (this.player.transform.position - transform.position).normalized;
                float pushForce = 10f; // Adjust this value as needed to control the force of the push.
                playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SEÑORAAAAAAAAAAAAAAA hola");
        }
    }
}
