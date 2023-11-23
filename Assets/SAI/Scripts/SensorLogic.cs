using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.IO;
using Newtonsoft.Json;

public class SensorLogic : MonoBehaviour
{
    public static Dictionary<string, (string, double, double, Func<double, double>, string, Func<double, Color>?)> _sensorValues = new Dictionary<string, (string, double, double, Func<double, double>, string, Func<double, Color>?)>();

    public TMP_InputField sensorNameInputField;
    public TMP_InputField sensorDescriptionInputField;
    public TMP_InputField sensorMaxValueInputField;
    public TMP_InputField sensorMinValueInputField;
    public TMP_InputField sensorLambdaExpressionInputField;
    public TMP_InputField sensorUnitsInputField;

    public TextAsset SensorsJSON;





    public static void InitializeSensorValues()
    {
        // Here's an example of how you could initialize the dictionary with some sample sensor values:
        if (_sensorValues.Count == 0)
        {
            _sensorValues.Add("currentSensor", ("Voltaje Resistencia Shunt", 0, 5000, x => (x / 1000) / 4.5, "mV", null));
            _sensorValues.Add("voltageSensor100", ("Voltaje de Rectificador", 0, 5000, x => ((x / 1000) * 20) + 2.1, "V", null));
            _sensorValues.Add("voltageSensor3", ("Voltaje en el Ducto", 0, 5000, x =>  (((((x / 1000) * 6) / 5) - 3f) + 0.18f), "V", null));
            _sensorValues.Add("voltageVibr", ("Voltaje de Bateria", 0, 5000, x => ((x / 1000) * 20) + 2.3, "V", null));
        }
    }

    //public void CreateNewSensor()
    //{
    //    string conversionFunctionString = "x => x / 2";
    //    Func<double, double> conversionFunction = LambdaParser.Parse(conversionFunctionString);


    //    AddSensorValue(sensorNameInputField.text, sensorDescriptionInputField.text, Convert.ToDouble(sensorMinValueInputField.text), Convert.ToDouble(sensorMaxValueInputField.text), conversionFunction, sensorUnitsInputField.text, null);
    //}

    public static class LambdaParser
    {
        public static Func<double, double> Parse(string lambdaString)
        {
            var parts = lambdaString.Split(new[] { "=>" }, StringSplitOptions.None);
            var parameter = Expression.Parameter(typeof(double), parts[0].Trim());
            var body = DynamicExpressionParser.ParseLambda(new[] { parameter }, null, parts[1].Trim());
            return (Func<double, double>)body.Compile();
        }
    }

    //public void AddSensorValue(string sensorName, string sensorDescription, double minValue, double maxValue, Func<double, double> conversionFunction, string units, Func<double, Color>? valueColorFunction)
    //{
    //    if (!_sensorValues.ContainsKey(sensorName))
    //    {
    //        //_sensorValues.Add(sensorName, (sensorDescription, minValue, maxValue, conversionFunction, units, valueColorFunction));
    //        _sensorValues.Add(sensorName, (sensorDescription, minValue, maxValue, conversionFunction, units, null));
    //        SaveSensorValuesToJson();
    //    }
    //    else
    //    {
    //        // Handle the case where a sensor value with the same name already exists
    //        Debug.LogError($"Sensor value with name {sensorName} already exists.");
    //    }
    //    Debug.Log(_sensorValues.Count);
    //}

    //private void SaveSensorValuesToJson()
    //{
    //    string json = JsonConvert.SerializeObject(_sensorValues, Formatting.Indented);
    //    string path = AssetDatabase.GetAssetPath(SensorsJSON);
    //    File.WriteAllText(path, json);
    //}

    public static bool TryCalculateSensorValue(string code, double value, out double result)
    {
        if (_sensorValues.TryGetValue(code, out var sensorValue))
        {
            Debug.Log("TRY GET VALUE: " + sensorValue);
            double min = sensorValue.Item2;
            double max = sensorValue.Item3;

            Debug.Log("MIN: " + sensorValue.Item2 + "MAX: " + sensorValue.Item3);

            Func<double, double> calculation = sensorValue.Item4;
            Debug.Log("CALCULATION: " + calculation(value));

            if (value < min || value > max)
            {
                result = default;
                return false;
            }

            result = calculation(value);
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryGetSensorNameAndUnitOfMeasurement(string code, out string name, out string unitOfMeasurement)
    {
        if (_sensorValues.TryGetValue(code, out var sensorValue))
        {
            name = sensorValue.Item1;
            Debug.Log("SENSOR NAME: " + name);
            unitOfMeasurement = sensorValue.Item5;
            return true;
        }

        name = null;
        unitOfMeasurement = null;
        return false;
    }

    public static Color GetValueColor(double value, double min, double max)
    {
        if (_sensorValues.TryGetValue("myKey", out var myValue) && myValue.Item6 != null && value >= min && value <= max)
        {
            if (value <= 1000)
            {
                return Color.white;
            }
            else if (value <= 2500)
            {
                return Color.yellow;
            }
            else
            {
                return Color.red;
            }
        }
        else
        {
            return Color.white;
        }
    }
}
