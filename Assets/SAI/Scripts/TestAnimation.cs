using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestAnimation : MonoBehaviour
{
    public Button bookNio;
    public Button backBookNio;
    public GameObject bookNioUI;

    // Start is called before the first frame update
    void Start()
    {
        bookNio.onClick.AddListener(buttonPressed);
        backBookNio.onClick.AddListener(buttonPressed2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void buttonPressed()
    {
        Debug.Log("IsPressed");
        Vector2 position = bookNioUI.transform.position;
        //Vector2 newPosition = new Vector2(-354.0f, 516.24f);
        Vector2 newPosition = new Vector2(2274.01f, 516.24f);
        bookNioUI.transform.position = newPosition;
    }

    private void buttonPressed2()
    {
        Debug.Log("IsPressed");
        Vector2 position = bookNioUI.transform.position;
        Vector2 newPosition = new Vector2(960.01f, 516.24f);
        bookNioUI.transform.position = newPosition;
    }
}
