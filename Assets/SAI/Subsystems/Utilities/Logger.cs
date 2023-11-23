using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void LogToFile(string log,string nombreArchivo,bool timestamp = true,bool append =true)
    {
       
        // Obtener la ruta del directorio de persistencia de datos de Unity
        string rutaCompleta = Path.Combine(Application.persistentDataPath, nombreArchivo);

        if (append)
        {
            try
            {
                // Agregar el texto al archivo (sin borrar el contenido existente)

                if (timestamp)
                {
                    File.AppendAllText(rutaCompleta, $"[{DateTime.Now.ToString()}] {log}" + "\n");
                } 
                else
                {
                    File.AppendAllText(rutaCompleta, log + "\n");
                }

                   
            }
            catch (Exception e)
            {
                print(e);
            }
        }
        else
        {
            try
            {
                if (timestamp) { File.WriteAllText(rutaCompleta, $"[{DateTime.Now.ToString()}] {log}" + "\n"); }
                else
                {
                    File.WriteAllText(rutaCompleta, log + "\n");
                }

                File.WriteAllText(rutaCompleta, log);
            }
            catch (Exception e)
            {
                
                
            }
        }

       
    }
}
