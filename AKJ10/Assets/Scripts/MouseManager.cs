using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    Hoverable previousHoverable;

    public static MouseManager INSTANCE;

    public int ActiveLayer = 0;

    public int layerBeforeMenu;

    [SerializeField]
    AudioClip ClickSound;

    AudioSource audioSource;

    void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Select();
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(ClickSound);
    }

    GameObject Select()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            var hoverable = hit.transform.GetComponent<Hoverable>();
            if (hoverable.RenderLayer == ActiveLayer && hoverable.CanHover)
            {
                UpdateHover(hoverable);
                return hit.transform.gameObject;
            }
        }
        UpdateHover(null);
        return null;
    }

    void UpdateHover(Hoverable hoverable)
    {
        if (hoverable != previousHoverable)
        {
            if (hoverable != null)
            {
                hoverable.Hover();
            }
            if (previousHoverable != null)
            {
                previousHoverable.UnHover();
            }
        }
        previousHoverable = hoverable;
    }

    public void OpenModal()
    {
        ActiveLayer = 1;
    }
    
    public void CloseModal()
    {
        ActiveLayer = 0;
    }

    public void OpenMenu()
    {
        if (ActiveLayer < 2)
        {
            layerBeforeMenu = ActiveLayer;
        }
        ActiveLayer = 2;
    }

    public void CloseMenu()
    {
        ActiveLayer = layerBeforeMenu;
    }
}
