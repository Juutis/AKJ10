using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public static Lobby INSTANCE;

    public static int CAPACITY = 100;
    public static int WARN = 50;
    public static int CRITICAL = 90;
    public int Passengers = 0;


    public static int DAMAGE1 = 50;
    public static int DAMAGE2 = 60;
    public static int DAMAGE3 = 70;
    public static int DAMAGE4 = 80;
    public static int DAMAGE5 = 90;
    int maxPassengers = 0;

    [SerializeField]
    Text UsedCapacity;

    [SerializeField]
    Text MaxCapacity;

    [SerializeField]
    Color Normal;

    [SerializeField]
    Color Warn;

    [SerializeField]
    Color Critical;

    [SerializeField]
    GameObject PlatformCapacityUi;

    Text[] uiTexts;

    [SerializeField]
    GameObject[] Damage1;

    [SerializeField]
    GameObject[] Damage2;

    [SerializeField]
    GameObject[] Damage3;

    [SerializeField]
    GameObject[] Damage4;

    [SerializeField]
    GameObject[] Damage5;

    [SerializeField]
    AudioClip ShortAlarm;

    [SerializeField]
    AudioClip LoopAlarm;

    [SerializeField]
    AudioClip EndAlarm;

    [SerializeField]
    AudioClip Explosion;

    [SerializeField]
    GameObject IntactLobby;

    [SerializeField]
    GameObject DestroyedLobby;

    [SerializeField]
    GameObject WarningLabel;

    [SerializeField]
    GameObject CriticalLabel;

    AudioSource audioSource;

    bool alarmLooping = false;

    bool played1, played2, played3, played4, played5, playedFinal;
    public bool dying = false;

    public bool DEAD = false;


    void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxCapacity.text = CAPACITY.ToString();
        uiTexts = PlatformCapacityUi.GetComponentsInChildren<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UsedCapacity.text = Passengers.ToString();
        if (Passengers < WARN)
        {
            setUiTextColor(Normal);
            WarningLabel.SetActive(false);
            CriticalLabel.SetActive(false);
        }
        if (Passengers >= WARN)
        {
            setUiTextColor(Warn);
            WarningLabel.SetActive(true);
            CriticalLabel.SetActive(false);
        }
        if (Passengers >= CRITICAL)
        {
            setUiTextColor(Critical);
            if (!audioSource.isPlaying && !playedFinal && !dying)
            {
                audioSource.clip = LoopAlarm;
                audioSource.loop = true;
                audioSource.Play();
                alarmLooping = true;
            }
            WarningLabel.SetActive(false);
            CriticalLabel.SetActive(true);
        }
        else
        {
            if (alarmLooping)
            {
                audioSource.Stop();
            }
        }
        if (Passengers > CAPACITY)
        {
            if (!playedFinal)
            {
                audioSource.loop = false;
            }
            Die();
        }

        if (dying && !playedFinal && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(EndAlarm);
            playedFinal = true;
            IntactLobby.SetActive(false);
            DestroyedLobby.SetActive(true);
            Invoke("PlayExplosion", 0.05f);
            Invoke("PlayExplosion", 0.25f);
            Invoke("PlayExplosion", 0.4f);
            DEAD = true;
        }

        if (Passengers > maxPassengers)
        {
            if (Passengers >= DAMAGE1)
            {
                if (!played1)
                {
                    audioSource.PlayOneShot(ShortAlarm);
                    audioSource.PlayOneShot(Explosion);
                    played1 = true;
                }
                foreach (var go in Damage1)
                {
                    go.SetActive(true);
                }
            }
            if (Passengers >= DAMAGE2)
            {
                if (!played2)
                {
                    audioSource.PlayOneShot(ShortAlarm);
                    audioSource.PlayOneShot(Explosion);
                    played2 = true;
                }
                foreach (var go in Damage2)
                {
                    go.SetActive(true);
                }
            }
            if (Passengers >= DAMAGE3)
            {
                if (!played3)
                {
                    audioSource.PlayOneShot(ShortAlarm);
                    audioSource.PlayOneShot(Explosion);
                    played3 = true;
                }
                foreach (var go in Damage3)
                {
                    go.SetActive(true);
                }
            }
            if (Passengers >= DAMAGE4)
            {
                if (!played4)
                {
                    audioSource.PlayOneShot(ShortAlarm);
                    audioSource.PlayOneShot(Explosion);
                    played4 = true;
                }
                foreach (var go in Damage4)
                {
                    go.SetActive(true);
                }
            }
            if (Passengers >= DAMAGE5)
            {
                if (!played5)
                {
                    audioSource.PlayOneShot(Explosion);
                    played5 = true;
                }
                foreach (var go in Damage5)
                {
                    go.SetActive(true);
                }
            }
        }

        if (Passengers > maxPassengers)
        {
            maxPassengers = Passengers;
        }
    }

    void setUiTextColor(Color color)
    {
        foreach(var text in uiTexts)
        {
            text.color = color;
        }
    }

    void Die()
    {
        Invoke("GameOver", 2.0f);
        dying = true;
    }

    public void PlayExplosion()
    {
        audioSource.PlayOneShot(Explosion);
    }

    void GameOver()
    {
        UI.INSTANCE.GameOver();
    }

    public float GetCapacityPercentage()
    {
        return playedFinal ? 0.0f : Mathf.Clamp((float)Passengers / CAPACITY, 0.0f, 1.0f);
    }
}
