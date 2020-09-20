using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : ClickListener
{
    public RouteDesigner Designer;

    // Start is called before the first frame update
    void Start()
    {
        
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

}
