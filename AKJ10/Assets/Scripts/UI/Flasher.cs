using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flasher : MonoBehaviour
{
    [SerializeField]
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var color = text.color;
        color.a = 0.5f + 0.5f * Mathf.Sin(Time.time * 10);
        text.color = color;
    }
}
