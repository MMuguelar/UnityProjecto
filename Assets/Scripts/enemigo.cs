using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigo : MonoBehaviour
{

    public int rutina;
    public float cronometro;
    public Animator anim;   
    public Quaternion angulo;
    // Start is called before the first frame update
    void Start()
    {
         anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       Comportamiento_Enemigo();
    }
    public void Comportamiento_Enemigo()
    {
        cronometro += 1 * Time.deltaTime;
        if (cronometro >=4)
        {
            rutina = Random.Range(0,2);
            cronometro = 0;
        }
        switch(rutina){
            case 0:
            anim.SetBool("walk",false);
           
            break;
            case 1:
           int grado = Random.Range(0,160);
            angulo= Quaternion.Euler(0, grado ,0);
            rutina++;

            break;
            case 2:     
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
            transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            anim.SetBool("walk",true);
            break;
        }
    }

}
