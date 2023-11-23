using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loader_code : MonoBehaviour
{

    public float play_time= 4;

    public float audio_time= 3;
    public GameObject fx;

    public string next_scene = "main";
    void Start()
    {
        fx.SetActive(false);
        Invoke("go_next", play_time);
        Invoke("play_audio", audio_time);
       
    }
    public void play_audio()
    {
        fx.SetActive(true);
        GetComponent<AudioSource>().Play();
    }
    public void go_next()
    {
        
        SceneManager.LoadScene(next_scene);
    }

   
}
