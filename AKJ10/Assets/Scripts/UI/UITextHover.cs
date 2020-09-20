using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Text text;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
    }

    void OnEnable()
    {
        if (originalColorSet)
        {
            text.color = originalColor;
        }
    }
}
