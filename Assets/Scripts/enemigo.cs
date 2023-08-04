using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigo : MonoBehaviour
{

    public int rutina;
    public float cronometro;
    public Animator anim;   
    public Quaternion angulo;
    public GameObject target;
    public bool atacando;
    public float velocidad = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
         anim = GetComponent<Animator>();
         target = GameObject.Find("personajeAnimaciones@Running (1)");

    }

    // Update is called once per frame
    void Update()
    {
       Comportamiento_Enemigo();
      
    }
    public void Comportamiento_Enemigo()
    {
        anim.SetBool("run",false);
        if (Vector3.Distance(transform.position, target.transform.position)>20 )
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
            Final_ani();
            
            break;
            case 1:
           int grado = Random.Range(0,160);
            angulo= Quaternion.Euler(0, grado ,0);
            rutina++;

            break;
            case 2:     
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            anim.SetBool("walk",true);
            velocidad = 1.0f;
            break;
        }
    }
    else{
        if(Vector3.Distance(transform.position, target.transform.position)>1 && !atacando ){
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
        anim.SetBool("walk", false);
        anim.SetBool("run",true );
        velocidad = 3.0f;
     transform.Translate(Vector3.forward * velocidad * 2 * Time.deltaTime);


      
    }
    else{
  anim.SetBool("walk", false);
        anim.SetBool("run",false );
    anim.SetBool("attack",true );
    atacando = true;
   
    }
   Final_ani();
    }

}
public void Final_ani()
{
    anim.SetBool("attack",false);
    atacando= false;
}
}
