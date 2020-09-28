using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject passengerPrefab;

    [SerializeField]
    float InitialDelay = 1.0f;

    float spawnTimer;

    float started;

    int groupSize = 0;
    int targetPlanet = 0;
    bool inHurry;

    int MAX_GROUP_SIZE = 15;
    int MIN_GROUP_SIZE = 1;

    float MIN_GROUPS_PER_SECOND = 0.075f;
    float MAX_GROUPS_PER_SECOND = 1.0f;

    float MIN_PER_SECOND = 0.15f;
    float MAX_PER_SECOND = 2.5f;

    int groupsGenerated = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Time.time + InitialDelay;
        started = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer < Time.time)
        {
            var delay = spawnPassenger();
            spawnTimer = Time.time + delay;
        }
    }

    float spawnPassenger()
    {
        if (groupSize <= 0)
        {
            targetPlanet = getTargetPlanet();
            inHurry = isInHurry();
            groupSize = getGroupSize();
            groupsGenerated++;
        }
        GameObject passenger = Instantiate(passengerPrefab);
        passenger.transform.position = transform.position;
        passenger.GetComponent<Passenger>().SetTargetPlanet(targetPlanet);
        passenger.GetComponent<Passenger>().InHurry = inHurry;
        groupSize--;
        return groupSize <= 0 ? getGroupInterval() : getInterval();
    }

    float getInterval()
    {
        var elapsedTime = getElapsedTime();
        var t = Mathf.Sqrt(elapsedTime) / 25f + Random.Range(-0.1f, 0.1f);
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var groupsPerSecond = Mathf.Lerp(MIN_PER_SECOND, MAX_PER_SECOND, t);

        var value = 1.0f / groupsPerSecond;
        return value;
    }

    float getGroupInterval()
    {
        var elapsedTime = getElapsedTime();
        var t = Mathf.Sqrt(elapsedTime) / 40f + Random.Range(-0.1f, 0.1f);
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var groupsPerSecond = Mathf.Lerp(MIN_GROUPS_PER_SECOND, MAX_GROUPS_PER_SECOND, t);

        var value = 1.0f / groupsPerSecond;
        return value;
    }

    int getTargetPlanet()
    {
        return Random.Range(0, getPossiblePlanets());
    }

    bool isInHurry()
    {
        var elapsedTime = getElapsedTime();
        var hurryChance = 0.1f;

        if (elapsedTime < 30)
        {
            hurryChance = 0.0f;
        }
        else if (elapsedTime < 60)
        {
            hurryChance = 0.1f;
        }
        else if (elapsedTime < 90)
        {
            hurryChance = 0.15f;
        }
        else if (elapsedTime < 120)
        {
            hurryChance = 0.2f;
        }
        else
        {
            hurryChance = 0.25f;
        }

        if (groupsGenerated == 3)
        {
            hurryChance = 1.0f;
        }

        return Random.Range(0.0f, 1.0f) < hurryChance;
    }

    int getGroupSize()
    {
        var elapsedTime = getElapsedTime();
        int value = 1;

        if (elapsedTime < 30)
        {
            value = Random.Range(1, 3);
        }
        else if (elapsedTime < 60)
        {
            value = Random.Range(3, 8);
        }
        else if (elapsedTime < 90)
        {
            value = Random.Range(3, 10);
        }
        else if (elapsedTime < 120)
        {
            value = Random.Range(1, 3);
        }
        else
        {
            value = Random.Range(1, 6);
        }

        return Mathf.Min(MAX_GROUP_SIZE, Mathf.Max(MIN_GROUP_SIZE, value));
    }

    int getPossiblePlanets()
    {
        int value = 2 + (int)(getElapsedTime()/20);
        return Mathf.Min(PlanetManager.INSTANCE.planetColors.Length, Mathf.Max(0, value));
    }

    float getElapsedTime()
    {
        return Time.time - started;
    }
}
