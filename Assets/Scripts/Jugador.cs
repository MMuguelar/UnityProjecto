using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : Character
{
    private enemigo enemy;
    public bool muerto;
    public float healRate = 1f; 
    private float healTimer = 0f;

    protected override void Awake()
    {
        maxLife = 20f;
        healthSlider.value = life;
        contactDamage = 3.5f;
        muerto = false;
        base.Awake();
    }

    protected override void Update()
    {
        //healthSlider.value = life;
        base.Update();
        Debug.Log(life);
        healTimer += Time.deltaTime;
        Debug.Log("HealTimer: " + healTimer);
        if(life <= 0){
            muerto = true;
            ControladorMuerte.Instance.CheckBool(muerto);
        }
        if((life < maxLife) && (healTimer >= 5f)) {
            Heal(healRate);
            healTimer = 0f; 
        }
    }
    void Heal(float amount) {
        life += amount;
        life = Mathf.Clamp(life, 0, maxLife);
    }
}

