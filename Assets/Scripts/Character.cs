using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{   
    public float maxLife;
    public float life { get; private set; }
    public float contactDamage = 3.5f;
    private float damageCooldown = 0.5f;
    private float damageTimer = 0.0f;
    public Enemy enemy;
    public Jugador player;

    void Awake()
    {
        life = maxLife;
    }
    
    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Jugador>();
            /*enemy*/TakeDamage(player.contactDamage);
            /*player*/player.TakeDamage(contactDamage);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
            /*player*/TakeDamage(enemy.contactDamage);
            /*enemy*/enemy.TakeDamage(contactDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (damageTimer <= 0)
        {
            life -= damage;

            if (life <= 0)
            {
                // Eliminar el enemigo si la vida llega a 0
                Destroy(gameObject);
            }

            damageTimer = damageCooldown;
        }
        
    }
}