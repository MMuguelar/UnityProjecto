using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;


public class LambdaExpressionTests : MonoBehaviour
{
    // Define a delegate type for the lambda expression
    delegate object LambdaDelegate(object x);

    // Define a dictionary to hold the lambda expressions
    Dictionary<string, LambdaDelegate> lambdaExpressions = new Dictionary<string, LambdaDelegate>();

    // Define the path to the JSON file
    string jsonFilePath;

    void Start()
    {

        var json = LambdaSerializer.Serialize(() => "Hello, world!");
        Debug.Log(json);
        //jsonFilePath = Application.dataPath + "/LambdaTestsJSON.json";

        //// Load the lambda expressions from the JSON file
        //LoadLambdaExpressions();

        //// Call a lambda expression by name
        //int result = (int)lambdaExpressions["AddOne"](5);
        //Debug.Log("Result: " + result);

        // Save a new lambda expression to the JSON file
        //SaveLambdaExpression("AddTwo", (object x) => (int)x + 2);
    }

    void LoadLambdaExpressions()
    {
        // Check if the JSON file exists
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON file as a string
            string json = File.ReadAllText(jsonFilePath);

            // Parse the JSON string into a dictionary of lambda expressions
            JObject jsonObject = JObject.Parse(json);
            foreach (var property in jsonObject.Properties())
            {
                string key = property.Name;
                string expressionString = property.Value.ToString();

                // Compile the lambda expression from the string
                LambdaDelegate expression = CompileLambdaExpression(expressionString);

                // Add the lambda expression to the dictionary
                lambdaExpressions[key] = expression;
                Debug.Log("Loaded lambda expression: " + key);
            }
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonFilePath);
        }
    }

    void SaveLambdaExpression(string name, LambdaDelegate expression)
    {
        // Add the new lambda expression to the dictionary
        lambdaExpressions[name] = expression;

        // Serialize the dictionary as a JSON string
        JObject jsonObject = new JObject();
        foreach (var entry in lambdaExpressions)
        {
            jsonObject[entry.Key] = JToken.FromObject(entry.Value.ToString());
        }
        string json = jsonObject.ToString();

        // Write the JSON string to the file
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("Saved lambda expression: " + name);
    }

    LambdaDelegate CompileLambdaExpression(string expressionString)
    {
        // Parse the expression string into a lambda expression
        LambdaExpression lambdaExpression = DynamicExpressionParser.ParseLambda(typeof(object), typeof(object), expressionString);

        // Compile the lambda expression and return it as a LambdaDelegate
        return (LambdaDelegate)lambdaExpression.Compile();
    }
}