using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class Emitter_Old : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // Cambia esto a la IP del receptor UDP
    public int serverPort = 12345; // Cambia esto al puerto del receptor UDP

    private UdpClient udpClient;
    private IPEndPoint endPoint;

    private void Start()
    {
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
    }


    
    public void EnviarMsgUDP(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, endPoint);
        Debug.Log("Mensaje enviado: " + message);
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string message = "Hola desde el emisor UDP";
            byte[] data = Encoding.ASCII.GetBytes(message);
            udpClient.Send(data, data.Length, endPoint);
            Debug.Log("Mensaje enviado: " + message);
        }
        */
    }

    private void OnApplicationQuit()
    {
        udpClient.Close();
    }
}