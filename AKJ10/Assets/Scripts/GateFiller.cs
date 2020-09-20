using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateFiller : MonoBehaviour
{
    [SerializeField]
    Image Bar;

    [SerializeField]
    GameObject FullText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFillAmount(float amount, bool showText)
    {
        amount = Mathf.Clamp(amount, 0.0f, 1.0f);
        Bar.fillAmount = amount;
        if (showText && amount > 0.999f)
        {
            FullText.SetActive(true);
        }
        else
        {
            FullText.SetActive(false);
        }
    }
}
