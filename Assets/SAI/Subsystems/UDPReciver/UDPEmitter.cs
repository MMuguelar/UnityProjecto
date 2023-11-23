using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDPEmitter : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // Cambia esto a la IP del receptor UDP
    public int serverPort = 1234; // Cambia esto al puerto del receptor UDP

    private UdpClient udpClient;
    private IPEndPoint endPoint;


    
    public void EnviarMsgUDP(string message)
    {

        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, endPoint);
        Debug.Log("Mensaje enviado: " + message);
    }


  
}