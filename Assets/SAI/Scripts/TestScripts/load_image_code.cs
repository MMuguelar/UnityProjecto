using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class load_image_code : MonoBehaviour
{

    public Image imagen;
    public GameObject loader;

    void Start()
    {
        
    }

    public void set_texture(byte[] imaged)
    {

        Texture2D myTexture = new Texture2D(2, 2);
        myTexture.LoadImage(imaged);


        if (myTexture != null)
        {
            imagen.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            imagen.preserveAspect = true;
        }


    }

    public void load_texture(string url)
    {
        imagen.sprite = null;
        imagen.enabled = false;
        loader.SetActive(true);
        StartCoroutine(GetTexture(url));
    }

    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture2D myTexture = DownloadHandlerTexture.GetContent(www);

        if (myTexture != null)
        {
            imagen.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            imagen.preserveAspect = true;
        }

        imagen.enabled = true;
        loader.SetActive(false);
    }

   
}