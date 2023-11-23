using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Receiver_Old : MonoBehaviour
{
    // Define el puerto que deseas escuchar
    public int puerto = 1234;

    // Define el socket UDP
    UdpClient receptor;

    void Start()
    {
        // Crea un nuevo socket UDP
        receptor = new UdpClient(puerto);

        // Crea un hilo para escuchar continuamente
        // Nota: Esto es un ejemplo b�sico y puede no ser la mejor pr�ctica en un proyecto real
        // Deber�as considerar usar hilos o t�cnicas de manejo de hilos m�s avanzadas para un proyecto m�s complejo
        // Consulta la documentaci�n de Unity sobre el manejo de hilos para obtener m�s informaci�n
        ThreadStart ts = new ThreadStart(Escuchar);
        System.Threading.Thread thread = new System.Threading.Thread(ts);
        thread.Start();
    }

    // M�todo para escuchar los datos
    private void Escuchar()
    {
        print($"UDP Server is Listening on port {puerto}");
        while (true)
        {
            IPEndPoint cliente = new IPEndPoint(IPAddress.Any, 0);
            byte[] datos = receptor.Receive(ref cliente);
            string texto = Encoding.ASCII.GetString(datos);

            // Haz lo que necesites con los datos recibidos
            // Por ejemplo, puedes imprimirlos en la consola
            Debug.Log("Datos recibidos: " + texto);
        }
    }

    private void OnDisable()
    {
        receptor.Close();
    }

    // Aseg�rate de cerrar el socket cuando el objeto se destruye
    void OnApplicationQuit()
    {
        receptor.Close();
    }
}