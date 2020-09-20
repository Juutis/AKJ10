using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButton : HoverListener
{
    public Ship ship;

    bool hover;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hover && Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnClick()
    {
        ship.Launch();
        MouseManager.INSTANCE.PlayClick();
    }

    public override void OnHover()
    {
        hover = true;
    }

    public override void OnUnHover()
    {
        hover = false;
    }
}
