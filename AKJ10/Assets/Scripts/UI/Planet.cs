using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : ClickListener
{
    [SerializeField]
    public int PlanetIndex;

    [SerializeField]
    SpriteRenderer rend;

    Hoverable hoverable;

    RouteDesigner routeDesigner;

    // Start is called before the first frame update
    void Start()
    {
        var color = PlanetManager.INSTANCE.planetColors[PlanetIndex];
        rend.color = color;

        hoverable = GetComponentInChildren<Hoverable>();
        hoverable.UpdateOriginalColors();
    }

    // Update is called once per frame
    void Update()
    {
        if (!routeDesigner.CanSelectMore())
        {
            hoverable.DisableHovering();
        }
    }

    public void RegisterRouteDesigner(RouteDesigner routeDesigner)
    {
        this.routeDesigner = routeDesigner;
    }

    override public void OnClick()
    {
        routeDesigner.Select(this);
        hoverable.DisableHovering();
        MouseManager.INSTANCE.PlayClick();
    }

    public void Reset()
    {
        if (hoverable != null)
        {
            hoverable.EnableHovering();
        }
    }
}
