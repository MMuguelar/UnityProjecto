
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDP : MonoBehaviour
{

    public UDPReceiver Receiver;
    public UDPEmitter Emitter;


    
    public void StartReceiver(int port)
    {
        Receiver.puerto = port;
        Receiver.Lister();
      
    }


    
    public void StopReceiver()
    {
        Receiver.receptor.Close();
    }


    
    public void SendUDPMessage(string ip,int port,string message)
    {
        Emitter.serverIP = ip;
        Emitter.serverPort = port;
        Emitter.EnviarMsgUDP(message);
    }

    
    
}
