using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteDesigner : MonoBehaviour
{
    List<Planet> route = new List<Planet>();

    Planet[] planets;

    [SerializeField]
    LineRenderer[] routeRenderers;

    [SerializeField]
    LineRenderer homeRenderer;

    [SerializeField]
    LineRenderer mouseRenderer;

    [SerializeField]
    Transform home;

    [SerializeField]
    GameObject container;

    [SerializeField]
    Button applyButton;

    [SerializeField]
    Button cancelButton;

    [SerializeField]
    Text jumpsRemainingText;

    [SerializeField]
    Text totalDistanceText;

    [SerializeField]
    Text timeTakenText;

    Ship selectedShip;

    int MAX_ROUTE_LENGTH = 4;

    public bool CanSelectMore()
    {
        return getJumpsRemaining() > 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        planets = GetComponentsInChildren<Planet>(true);
        foreach (var planet in planets)
        {
            planet.RegisterRouteDesigner(this);
        }
        container.SetActive(false);
        applyButton.Designer = this;
        cancelButton.Designer = this;
    }

    // Update is called once per frame
    void Update()
    {
        updateLine();

        if (Input.GetMouseButtonDown(1))
        {
            if (route.Count > 0)
            {
                route.RemoveAt(route.Count - 1);
                foreach (var planet in planets)
                {
                    if (!route.Contains(planet))
                    {
                        planet.Reset();
                    }
                }
                MouseManager.INSTANCE.PlayClick();
                updateUI();
            }
        }
    }

    public void Select(Planet planet)
    {
        route.Add(planet);
        updateUI();
    }

    public void Open(Ship ship)
    {
        reset();
        MouseManager.INSTANCE.OpenModal();
        container.SetActive(true);
        selectedShip = ship;
    }

    public void Close()
    {
        reset();
        MouseManager.INSTANCE.CloseModal();
        container.SetActive(false);
    }

    public void Apply()
    {
        selectedShip.ApplyRoute(route, getTimeTaken());
        Close();
        selectedShip = null;
    }

    public void Cancel()
    {
        Close();
        selectedShip = null;
    }

    public void ButtonCallBack(Button button)
    {
        if (button == applyButton)
        {
            Apply();
        }
        else if (button == cancelButton)
        {
            Cancel();
        }
    }

    void reset()
    {
        route = new List<Planet>();
        foreach (var planet in planets)
        {
            planet.Reset();
        }
        updateUI();
    }

    void updateLine()
    {
        var mousePositions = new List<Vector3>();
        var homePositions = new List<Vector3>();

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 1.0f;

        if (route.Count == 0)
        {
            mousePositions.Add(home.position);
            mousePositions.Add(mousePos);
        }
        else if (route.Count < MAX_ROUTE_LENGTH)
        {
            mousePositions.Add(route[route.Count - 1].transform.position);
            mousePositions.Add(mousePos);
        }

        if (route.Count > 0)
        {
            homePositions.Add(route[route.Count - 1].transform.position);
            homePositions.Add(home.position);
        }

        var prevPosition = home.position;

        for (var i = 0; i < MAX_ROUTE_LENGTH; i++)
        {
            var positions = new List<Vector3>();
            if (i < route.Count)
            {
                positions.Add(prevPosition);
                positions.Add(route[i].transform.position);
                prevPosition = route[i].transform.position;
            }
            routeRenderers[i].SetPositions(positions.ToArray());
            routeRenderers[i].positionCount = positions.Count;
        }
    
        mouseRenderer.SetPositions(mousePositions.ToArray());
        mouseRenderer.positionCount = mousePositions.Count;
        homeRenderer.SetPositions(homePositions.ToArray());
        homeRenderer.positionCount = homePositions.Count;
    }

    void updateUI()
    {
        jumpsRemainingText.text = getJumpsRemaining().ToString();
        totalDistanceText.text = ((int)getTotalDistance() * 10).ToString();
        timeTakenText.text = getTimeTaken().ToString("F1");
    }

    int getJumpsRemaining()
    {
        return MAX_ROUTE_LENGTH - route.Count;
    }

    float getTotalDistance()
    {
        float dist = 0.0f;
        var prevPos = home.position;
        foreach (var planet in route)
        {
            dist += Vector2.Distance(prevPos, planet.transform.position);
            prevPos = planet.transform.position;
        }
        dist += Vector2.Distance(prevPos, home.position);
        return dist;
    }

    float getTimeTaken()
    {
        return getTotalDistance() / 1.5f + route.Count * 2.5f;
    }
}
