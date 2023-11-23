
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GPSLocation : MonoBehaviour
{
    // Variables para almacenar la latitud y longitud
    public float latitude;
    public float longitude;

    // Inicia la corrutina al inicio
    void Start()
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        return;
#elif UNITY_ANDROID
    if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }    
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
    return;
#else
    return;
#endif



    }
    
    public void GetGPSLocation()
    {
        StartCoroutine(GetLocation());
    }

    // Corrutina para obtener la ubicación
    IEnumerator GetLocation()
    {
        // Comprueba si el servicio de ubicación del usuario no está habilitado
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Servicio de ubicación no habilitado por el usuario");
            SAI.SDK.errorHandler.ShowPopup("Location Service is Disabled","Error");
            yield break;
        }

        // Inicia el servicio de ubicación
        Input.location.Start();

        // Espera hasta que el servicio se inicialice
        int maxWait = 20; // tiempo máximo de espera en segundos
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Si el servicio no se inicia en 20 segundos, sale
        if (maxWait < 1)
        {
            Debug.Log("Tiempo de espera agotado");
            SAI.SDK.errorHandler.ShowPopup("Location Service timeout", "Error");
            yield break;
        }

        // Si la conexión falló, muestra el mensaje de error
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("No se pudo determinar la ubicación del dispositivo");
            SAI.SDK.errorHandler.ShowPopup("Location Service Failure", "Error");
            yield break;
        }
        else
        {
            // Accede a los datos de ubicación
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            Debug.Log("Ubicación: " + latitude + ", " + longitude);
            //SAI.SDK.errorHandler.ShowPopup("Ubicación: " + latitude + ", " + longitude, "GPS");            
            SAI.SDK.locationManager.latitude = latitude.ToString();
            SAI.SDK.locationManager.longitude = longitude.ToString();
        }

        // Detiene el servicio de ubicación si ya no se necesita
        Input.location.Stop();
    }
}
