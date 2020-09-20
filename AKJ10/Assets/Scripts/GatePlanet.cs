using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePlanet : MonoBehaviour
{
    Color origColor;

    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        origColor = rend.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlanetIndex(int index)
    {
        if (index == -1)
        {
            rend.color = origColor;
        }
        else
        {
            rend.color = PlanetManager.INSTANCE.planetColors[index];
        }
    }
}
