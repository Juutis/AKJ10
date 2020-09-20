using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverable : MonoBehaviour
{
    SpriteRenderer[] renderers;
    Color[] originalColors;

    ClickListener[] clickListeners;
    HoverListener[] hoverListeners;

    bool hover = false;
    bool wasHovered = false;

    public bool CanHover { get; private set; } = true;

    private static readonly float HOVER_MULTIPLIER = 1.25f;
    private static readonly float HOVER_ADD = 0.2f;

    [SerializeField]
    public int RenderLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateOriginalColors();
        clickListeners = GetComponents<ClickListener>();
        hoverListeners = GetComponents<HoverListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hover && !CanHover)
        {
            UnHover();
        }

        if (Input.GetMouseButtonDown(0) && hover)
        {
            foreach(var listener in clickListeners)
            {
                listener.OnClick();
            }
        }
    }

    public void EnableHovering()
    {
        CanHover = true;
    }

    public void DisableHovering()
    {
        CanHover = false;
    }

    public void Hover()
    {
        if (!hover && CanHover)
        {
            applyHoverColors();

            foreach (var listener in hoverListeners)
            {
                listener.OnHover();
            }
        }
        hover = true;
    }

    public void UnHover()
    {
        if (hover)
        {
            applyOriginalColors();

            foreach (var listener in hoverListeners)
            {
                listener.OnUnHover();
            }
        }
        hover = false;
    }

    private void applyHoverColors()
    {
        for (var i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = getHoverColor(originalColors[i]);
        }
    }

    private void applyOriginalColors()
    {
        for (var i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = originalColors[i];
        }
    }
    
    private Color getHoverColor(Color color)
    {
        return new Color(color.r * HOVER_MULTIPLIER + HOVER_ADD, color.g * HOVER_MULTIPLIER + HOVER_ADD, color.b * HOVER_MULTIPLIER + HOVER_ADD, color.a);
    }

    public void UpdateOriginalColors()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[renderers.Length];
        for (var i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }
    }
}
