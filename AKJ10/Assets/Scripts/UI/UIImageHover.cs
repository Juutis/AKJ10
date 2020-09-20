using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image text;

    [SerializeField]
    Color hoverColor;

    Color originalColor;
    bool originalColorSet = false;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = text.color;
        originalColorSet = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        if (originalColorSet)
        {
            text.color = originalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
    }
}
