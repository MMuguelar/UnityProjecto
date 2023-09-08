using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : Character
{
    private Enemy enemy;

    protected override void Awake()
    {
        maxLife = 20f;
        healthSlider.value = life;
        contactDamage = 3.5f;
        damageCooldown = 1.0f;
        base.Awake();
    }

    protected override void Update()
    {
        healthSlider.value = life;
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.contactDamage);
                enemy.TakeDamage(contactDamage);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente Enemy en el objeto de colisión.");
            }
        }
    }
}

