using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HoverListener
{
    [SerializeField]
    private float startDelay = 0.0f;

    [SerializeField]
    RouteDesigner routeDesigner;

    [SerializeField]
    LaunchButton launchButton;

    [SerializeField]
    GatePlanet[] gatePlanets;

    [SerializeField]
    public Transform GateWaypoint;

    [SerializeField]
    public Transform BoardingPoint;

    [SerializeField]
    public GateFiller GateFiller;

    [SerializeField]
    Score ScorePop;

    [SerializeField]
    AudioClip LandSound;

    [SerializeField]
    AudioClip LaunchSound;

    [SerializeField]
    AudioClip ClickSound;

    public AudioSource audioSource;

    Animator animator;
    Hoverable hoverable;

    private State state = State.AWAY;

    bool clicked = false;
    bool hover = false;

    private List<Planet> route;

    private float cycleDelay = 3.0f;

    private int MAX_PASSENGERS = 10;
    public int Passengers = 0;
    public int ReservedSeats = 0;

    public int Score;
    
    public bool RoomAvailable()
    {
        return Passengers + ReservedSeats < MAX_PASSENGERS;
    }

    public int GetPrimaryPlanet()
    {
        return state == State.WAITING_FOR_PASSENGERS && route.Count > 0 ? route[0].PlanetIndex : -1;
    }

    public bool RouteContains(int id)
    {
        return state == State.WAITING_FOR_PASSENGERS && routeContains(id);
    }

    bool routeContains(int id)
    {
        if (route.Count == 0)
        {
            return false;
        }
        foreach (var planet in route)
        {
            if (planet.PlanetIndex == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsBoarding()
    {
        return state == State.WAITING_FOR_PASSENGERS;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        hoverable = GetComponentInChildren<Hoverable>();
        hoverable.DisableHovering();
        Invoke("StartLanding", startDelay);
        launchButton.ship = this;
        launchButton.gameObject.SetActive(false);

        PlanetManager.INSTANCE.Ships.Add(this);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.WAITING_FOR_PASSENGERS)
        {
            GateFiller.SetFillAmount((float)Passengers / MAX_PASSENGERS, true);
        }
        else
        {
            GateFiller.SetFillAmount(1.0f, false);
        }

        switch (state)
        {
            case State.LANDING:
                handleLanding();
                break;
            case State.WAITING_FOR_ROUTE:
                handleWaitingRoute();
                break;
            case State.WAITING_FOR_PASSENGERS:
                handleWaitingForPassengers();
                break;
            case State.LAUNCHING:
                handleLaunching();
                break;
            case State.AWAY:
                handleAway();
                break;
        }
        clicked = false;
    }

    private void handleLanding()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Land")
        {
            land();
        }
    }

    private void handleWaitingRoute()
    {
        if (clicked)
        {
            routeDesigner.Open(this);
        }
    }

    private void handleWaitingForPassengers()
    {
        if (clicked)
        {
            launchButton.gameObject.SetActive(true);
        }
    }

    private void handleLaunching()
    {

    }

    private void handleAway()
    {

    }

    private void land()
    {
        state = State.WAITING_FOR_ROUTE;
        hoverable.EnableHovering();
    }

    void StartLanding()
    {
        animator.Play("Land");
        state = State.LANDING;
        route = new List<Planet>();
        Invoke("PlayLandSound", 0.2f);
    }
    
    void PlayLandSound()
    {
        audioSource.PlayOneShot(LandSound);
    }

    public void ApplyRoute(List<Planet> route, float travelTime)
    {
        this.route = route;
        state = State.WAITING_FOR_PASSENGERS;

        for (int i = 0; i < gatePlanets.Length; i++)
        {
            if (i < route.Count)
            {
                gatePlanets[i].SetPlanetIndex(route[i].PlanetIndex);
            }
            else
            {
                gatePlanets[i].SetPlanetIndex(-1);
            }
        }
        cycleDelay = travelTime;
    }

    public void Launch()
    {
        animator.Play("Launch");
        state = State.LAUNCHING;
        hoverable.DisableHovering();
        launchButton.gameObject.SetActive(false);
        addScore();
        Invoke("StartLanding", cycleDelay);

        foreach (var gatePlanet in gatePlanets)
        {
            gatePlanet.SetPlanetIndex(-1);
        }

        Passengers = 0;
        ReservedSeats = 0;
        audioSource.PlayOneShot(LaunchSound);
    }

    private void addScore()
    {
        PlanetManager.INSTANCE.Score += Score * Passengers;
        if (Score * Passengers > 0)
        {
            ScorePop.gameObject.SetActive(true);
            ScorePop.Play((float)Passengers / MAX_PASSENGERS, Score, Passengers);
        }
    }

    public enum State
    {
        AWAY,
        LANDING,
        WAITING_FOR_ROUTE,
        WAITING_FOR_PASSENGERS,
        LAUNCHING
    }

    override public void OnClick()
    {
        clicked = true;
        audioSource.PlayOneShot(ClickSound);
    }

    public override void OnHover()
    {
        hover = true;
    }

    public override void OnUnHover()
    {
        hover = false;
    }
}
