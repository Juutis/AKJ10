using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerHoverListener : HoverListener
{
    [SerializeField]
    GameObject HoverInfo;

    [SerializeField]
    GameObject HoverInfoHaste;

    public bool Hasted = false;

    // Start is called before the first frame update
    void Start()
    {
        HoverInfo.SetActive(false);
        HoverInfoHaste.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnHover()
    {
        if (Hasted)
        {
            HoverInfoHaste.SetActive(true);
        }
        else
        {
            HoverInfo.SetActive(true);
        }
    }

    public override void OnUnHover()
    {
        HoverInfo.SetActive(false);
        HoverInfoHaste.SetActive(false);
    }

    public override void OnClick()
    {
    }
}
