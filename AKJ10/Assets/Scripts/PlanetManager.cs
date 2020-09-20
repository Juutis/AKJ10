using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    [SerializeField]
    public Color[] planetColors;

    [SerializeField]
    Text ScoreText;

    public static PlanetManager INSTANCE;

    public List<Ship> Ships = new List<Ship>();

    public int Score;

    void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
    }
}
