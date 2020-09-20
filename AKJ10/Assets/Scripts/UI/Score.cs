using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    Text[] texts;

    [SerializeField]
    Text ScoreText;

    [SerializeField]
    Text MultiplierText;

    [SerializeField]
    Color MinColor;

    [SerializeField]
    Color MaxColor;

    [SerializeField]
    Animator anim;

    float started;
    Color startColor, endColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - started < 1.0f)
        {
            updateColor();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Play(float amount, int score, int multiplier)
    {
        amount = Mathf.Clamp(amount, 0.0f, 1.0f);
        startColor = Color.Lerp(MinColor, MaxColor, amount);
        endColor = startColor;
        endColor.a = 0.0f;
        anim.Play("ScorePop");
        started = Time.time;
        ScoreText.text = score.ToString();
        MultiplierText.text = multiplier.ToString();
    }

    void updateColor()
    {
        var color = Color.Lerp(startColor, endColor, Time.time - started);
        foreach (var text in texts)
        {
            text.color = color;
        }
    }
}
