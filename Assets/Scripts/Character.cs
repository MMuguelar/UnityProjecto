using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    protected float maxLife;
    public float life { get; protected set; }
    public float contactDamage;
    protected float damageCooldown;
    protected float damageTimer;
    public Slider healthSlider;

    protected virtual void Awake()
    {
        life = maxLife;
    }

    protected virtual void Update()
    {
       healthSlider.value = life;

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Implement collision logic in derived classes (Jugador, Enemy)
    }

    public void TakeDamage(float damage)
    {
        if (damageTimer <= 0)
        {
            life -= damage;
            Debug.Log("Vida restante: " + life);

            if (life <= 0)
            {
                Destroy(gameObject);
            }

            damageTimer = damageCooldown;
        }
    }
}