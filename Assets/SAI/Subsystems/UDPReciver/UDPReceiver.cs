using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.IO;
using System;

public class UDPReceiver : MonoBehaviour
{
    // Define el puerto que deseas escuchar
    public int puerto = 1234;

    public string latestData;

    // Define el socket UDP
    public UdpClient receptor;

    public bool autoStart = false;
    public bool saveLogs = true;

    void Start()
    {

        if (!autoStart) return;
        Lister();
       
    }


    
    public void Lister()
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

            latestData = texto;
            // Store Data
            if(saveLogs) GuardarEnArchivo(latestData, "UDPLog");



        }
    }


    public void GuardarEnArchivo(string texto, string nombreArchivo)
    {
        // Obtener la hora actual
        DateTime currentTime = DateTime.Now;

        // Formatear la hora actual como una cadena con el formato deseado
        string timestamp = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

        // Puedes ajustar el formato seg�n tus preferencias, aqu� se usa "a�o-mes-d�a hora:minuto:segundo"

        // Mostrar la cadena con la marca de tiempo en la consola
        

        // Obtener la ruta completa del archivo
        string rutaArchivo = Path.Combine(Application.dataPath, nombreArchivo + ".txt");

        // Verificar si el archivo ya existe
        if (File.Exists(rutaArchivo))
        {
            // Abrir el archivo en modo append (agregar al final)
            using (StreamWriter escritor = File.AppendText(rutaArchivo))
            {
                // Escribir el texto en una nueva l�nea
                escritor.WriteLine($"[{timestamp}] : {texto}");
            }
        }
        else
        {
            // Si el archivo no existe, crearlo y escribir el texto
            using (StreamWriter escritor = File.CreateText(rutaArchivo))
            {
                escritor.WriteLine($"[{timestamp}] : {texto}");
            }
        }

        Debug.Log("Texto guardado en " + nombreArchivo + ".txt");
    }

    private void OnDisable()
    {
        if(receptor != null)
        {
            receptor.Close();
        }
        
    }
}