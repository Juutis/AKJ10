using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passenger : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer rend;

    [SerializeField]
    Hoverable hoverable;

    [SerializeField]
    GameObject InHurryIcon;

    [SerializeField]
    SpriteRenderer iconRend;

    [SerializeField]
    GameObject container;

    [SerializeField]
    Image[] tooltipImages;

    [SerializeField]
    public AudioClip BoardSound;

    Lobby lobby;

    State state = State.ENTERING;

    float idleTimer = 0.0f;
    float gateCheckTimer = 0.0f;

    Vector2 targetPos;

    float LOBBY_RADIUS = 1.8f;
    float IDLE_MIN = 2.0f;
    float IDLE_MAX = 5.0f;
    float CHECK_MIN = 0.2f;
    float CHECK_MAX = 0.5f;
    float MOVE_SPEED = 2.0f;

    int targetPlanet = 0;

    public bool InHurry = false;

    bool addedToLobby = false;

    Ship targetShip = null;

    PassengerHoverListener hoverListener;

    // Start is called before the first frame update
    void Start()
    {
        lobby = Lobby.INSTANCE;
        randomizeTargetPosition();
        hoverable.UpdateOriginalColors();
        hoverListener = GetComponentInChildren<PassengerHoverListener>();
    }

    public void SetTargetPlanet(int planetIndex)
    {
        targetPlanet = planetIndex;
        rend.color = PlanetManager.INSTANCE.planetColors[planetIndex];
        iconRend.color = PlanetManager.INSTANCE.planetColors[planetIndex];
        foreach (var tooltipImage in tooltipImages)
        {
            tooltipImage.color = PlanetManager.INSTANCE.planetColors[planetIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        InHurryIcon.SetActive(InHurry);
        hoverListener.Hasted = InHurry;
        switch (state)
        {
            case State.ENTERING:
                handleEntering();
                break;
            case State.WAITING:
                handleWaiting();
                break;
            case State.BOARDING:
                handleBoarding();
                break;
            case State.RETURNING:
                handleReturning();
                break;
        }

        if (!addedToLobby && Vector2.Distance(transform.position, lobby.transform.position) <= LOBBY_RADIUS)
        {
            lobby.Passengers++;
            addedToLobby = true;
        }

        var diff = (Vector2)transform.position - targetPos;
        if (diff.magnitude > 0.1f)
        {
            if (diff.x > 0)
            {
                container.transform.localScale = new Vector3(-1.0f, container.transform.localScale.y, container.transform.localScale.z);
            }
            else
            {
                container.transform.localScale = new Vector3(1.0f, container.transform.localScale.y, container.transform.localScale.z);
            }
        }

        rend.sortingOrder = -(int)(transform.position.y * 1000);
    }

    void handleEntering()
    {
        if (Lobby.INSTANCE.DEAD)
        {
            Destroy(gameObject);
        }

        moveTowardTarget();
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            idleTimer = Time.time + Random.Range(IDLE_MIN, IDLE_MAX);
            gateCheckTimer = Time.time + Random.Range(CHECK_MIN, CHECK_MAX);
            state = State.WAITING;
        }

    }

    void handleWaiting()
    {
        if (Lobby.INSTANCE.DEAD)
        {
            Destroy(gameObject);
        }

        if (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            moveTowardTarget();
            idleTimer = Time.time + Random.Range(IDLE_MIN, IDLE_MAX);
        }

        if (idleTimer < Time.time)
        {
            randomizeTargetPosition();
        }

        if (gateCheckTimer < Time.time)
        {
            checkGates();
            gateCheckTimer = Time.time + Random.Range(CHECK_MIN, CHECK_MAX);
        }
    }

    void handleBoarding()
    {
        if (!targetShip.IsBoarding())
        {
            state = State.RETURNING;
            targetPos = targetShip.GateWaypoint.position;
            return;
        }
        moveTowardTarget();

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = targetShip.BoardingPoint.position;
        }

        if (Vector2.Distance(transform.position, targetShip.BoardingPoint.position) < 0.1f)
        {
            boardShip();
        }
    }

    void handleReturning()
    {
        if (Lobby.INSTANCE.DEAD)
        {
            Destroy(gameObject);
        }

        moveTowardTarget();
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            randomizeTargetPosition();
            idleTimer = Time.time + Random.Range(IDLE_MIN, IDLE_MAX);
            gateCheckTimer = Time.time + Random.Range(CHECK_MIN, CHECK_MAX);
            state = State.WAITING;
        }
    }

    void checkGates()
    {
        foreach (var ship in PlanetManager.INSTANCE.Ships)
        {
            if (ship.RoomAvailable())
            {
                if (targetPlanet == ship.GetPrimaryPlanet())
                {
                    selectShip(ship);
                    return;
                }
                else if (!InHurry && ship.RouteContains(targetPlanet))
                {
                    selectShip(ship);
                    return;
                }
            }
        }
    }

    void selectShip(Ship ship)
    {
        ship.ReservedSeats++;
        state = State.BOARDING;
        targetShip = ship;
        targetPos = ship.GateWaypoint.position;
    }

    void boardShip()
    {
        targetShip.Passengers++;
        targetShip.ReservedSeats--;
        targetShip.Score += getScoreValue();
        lobby.Passengers--;
        targetShip.audioSource.PlayOneShot(BoardSound);
        Destroy(gameObject);
    }

    int getScoreValue()
    {
        return InHurry ? 3 : targetPlanet == targetShip.GetPrimaryPlanet() ? 2 : 1;
    }

    void randomizeTargetPosition()
    {
        targetPos = (Vector2)lobby.transform.position + Random.insideUnitCircle * new Vector2(LOBBY_RADIUS, LOBBY_RADIUS/2.0f);
    }

    void moveTowardTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, MOVE_SPEED * Time.deltaTime);
    }

    public enum State
    {
        ENTERING,
        WAITING,
        BOARDING,
        RETURNING
    }
}
