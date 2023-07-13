using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using UnityEngine.UI;

public class Jugador : MonoBehaviour//Character
{
    private float maxLife = 20f;
    public float life { get; private set; }
    public float contactDamage = 3.5f;
    private float damageCooldown = 0.5f;
    private float damageTimer = 0.0f;
    public Slider barraVida;
    private Enemy enemy;
    
    void Awake()
    {
        life = maxLife;
    }

    void Update()
    {
        Debug.Log("vida jugador:" + barraVida.value);
        //Debug.Log("Cooldown: " + damageTimer);
        //Debug.Log("El enemigo tiene: " + enemy.life + " de vida");
        //Debug.Log("El jugador tiene: " + life + " de vida");
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("gola");
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                /*player*/TakeDamage(enemy.contactDamage);
                /*enemy*/enemy.TakeDamage(contactDamage);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente Enemy en el objeto de colisión.");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (damageTimer <= 0)
        {
            life -= damage;
            Debug.Log(life);
            barraVida.value = life; 

            if (life <= 0)
            {
                // Eliminar el enemigo si la vida llega a 0
                Destroy(gameObject);
            }

            damageTimer = damageCooldown;
        }
        
    }
}
