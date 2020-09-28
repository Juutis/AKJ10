using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : ClickListener
{
    public RouteDesigner Designer;
    public Hoverable hoverable;

    // Start is called before the first frame update
    void Start()
    {
        hoverable = GetComponent<Hoverable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClick()
    {
        Designer.ButtonCallBack(this);
        MouseManager.INSTANCE.PlayClick();
    }

    public void Disable()
    {
        if (hoverable != null)
        {
            hoverable.DisableHovering();
        }
    }

    public void Enable()
    {
        if (hoverable != null)
        {
            hoverable.EnableHovering();
        }
    }
}
