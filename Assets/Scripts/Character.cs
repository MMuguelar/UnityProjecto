using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    protected float maxLife ;
    public float life ;
    public float contactDamage { get; set;}
    protected float damageCooldown;
    protected float damageTimer;
    public Slider healthSlider;

 
    protected virtual void Awake()
    { 
        life = maxLife;
    }
    protected virtual void Update()
    {
        healthSlider.maxValue = maxLife;
        healthSlider.value = life;
        //Debug.Log("Vida:" + healthSlider.value);
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            damageTimer = Math.Max(0, damageTimer);
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
                
            }

            damageTimer = damageCooldown;
        }
    }
}