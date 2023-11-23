using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.Net;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject video;
    public string urlVideo = "Rtsp://192.168.2.181";
    private string username = "admin";
    private string password = "tlon2023@";


    void Start()
    {
        videoPlayer.url = urlVideo;
        StartCoroutine(CheckAccess());
    }

    IEnumerator CheckAccess()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlVideo);

        
        string base64Encoded = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password));
        request.SetRequestHeader("Authorization", "Basic " + base64Encoded);

      
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            video.SetActive(true);
            string responseText = request.downloadHandler.text;
            Debug.Log("Successful access. Response: " + responseText);
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Error accessing the page: " + request.error);
        }
    }
}
