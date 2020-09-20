using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    public static MusicPlayer INSTANCE;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            INSTANCE = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.pitch = 1.0f + Lobby.INSTANCE.GetCapacityPercentage() * 8.0f;
        }
    }

    bool paused = false;

    public void ToggleMusic()
    {
        if (paused)
        {
            audioSource.UnPause();
            paused = false;
        }
        else
        {
            audioSource.Pause();
            paused = true;
        }
    }
}
