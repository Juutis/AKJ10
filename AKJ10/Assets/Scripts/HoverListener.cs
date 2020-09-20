using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoverListener : ClickListener
{
    public abstract void OnHover();
    public abstract void OnUnHover();
}
