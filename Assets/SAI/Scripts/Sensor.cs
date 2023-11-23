using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sensor
{

    public string date{ get; set; }
    public double latitud{ get; set; }
    public double longitud { get; set; }
    public float currentSensor{ get; set; }
    public float voltageSensor100 { get; set; }
    public float voltageSensor3 { get; set; }
    public float voltageVibr{ get; set; }
    public string dateStamp{ get; set; }
    public string timeStamp{ get; set; }


    //consructor
    public Sensor(string date, double latitud, double longitud, float currentSensor, float voltageSensor100, float voltageSensor3, float voltageVibr, string dateStamp, string timeStamp)
    {
        this.date = date;
        this.latitud = latitud;
        this.longitud = longitud;
        this.currentSensor = currentSensor;
        this.voltageSensor100 = voltageSensor100;
        this.voltageSensor3 = voltageSensor3;
        this.voltageVibr = voltageVibr;
        this.dateStamp = dateStamp;
        this.timeStamp = timeStamp;
    }
}
