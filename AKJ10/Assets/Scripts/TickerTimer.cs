using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickerTimer : MonoBehaviour
{
    Text text;
    
    Color origColor;
    Color transparent = new Color(0, 0, 0, 0);

    int prevSeconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        origColor = text.color;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReEnable()
    {
        text.color = origColor;
    }

    public void SetTime(int seconds)
    {
        if (text != null && seconds != prevSeconds)
        {
            prevSeconds = seconds;
            var minutes = seconds / 60;
            seconds = seconds - minutes * 60;
            text.text = minutes.ToString() + ":" + seconds.ToString("D2");
            if (seconds < 10)
            {
                text.color = transparent;
                Invoke("ReEnable", 0.3f);
            }
        }
    }
}
