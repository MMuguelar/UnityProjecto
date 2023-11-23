using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_image : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float speed=1;

    public void Anim()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().speed = speed;
        GetComponent<Animator>().Play("image_play");
    }

    public void Anim_OUT()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().speed = speed;
        GetComponent<Animator>().Play("image_fadeout");
    }



    public void Anim_image_out(float delay, float speedn = 1 - 0f)
    {
        speed = speedn;
        CancelInvoke("Anim_OUT");
        Invoke("Anim_OUT", delay);
    }




    public void Anim_image(float delay, float speedn = 1-0f)
    {
        speed = speedn;
        CancelInvoke("Anim");
        Invoke("Anim",delay);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
