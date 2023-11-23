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
        // Nota: Esto es un ejemplo básico y puede no ser la mejor práctica en un proyecto real
        // Deberías considerar usar hilos o técnicas de manejo de hilos más avanzadas para un proyecto más complejo
        // Consulta la documentación de Unity sobre el manejo de hilos para obtener más información
        ThreadStart ts = new ThreadStart(Escuchar);
        System.Threading.Thread thread = new System.Threading.Thread(ts);
        thread.Start();
    }

    // Método para escuchar los datos
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

    // Asegúrate de cerrar el socket cuando el objeto se destruye
    void OnApplicationQuit()
    {
        receptor.Close();
    }
}