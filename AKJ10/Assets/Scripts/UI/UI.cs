using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameObject QuitMenu;

    [SerializeField]
    GameObject RestartMenu;

    [SerializeField]
    GameObject GameOverMenu;

    [SerializeField]
    Text FinalScore;

    bool gameOver = false;

    public static UI INSTANCE;

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
        
    }

    public void Quit()
    {
        if (!gameOver)
        {
            MouseManager.INSTANCE.PlayClick();
            RestartMenu.SetActive(false);
            QuitMenu.SetActive(true);
            MouseManager.INSTANCE.OpenMenu();
        }
    }

    public void Restart()
    {
        if (!gameOver)
        {
            MouseManager.INSTANCE.PlayClick();
            QuitMenu.SetActive(false);
            RestartMenu.SetActive(true);
            MouseManager.INSTANCE.OpenMenu();
        }
    }

    public void ReallyQuit()
    {
        MouseManager.INSTANCE.PlayClick();
        Invoke("ReallyReallyQuit", 0.1f);
    }

    public void ReallyRestart()
    {
        MouseManager.INSTANCE.PlayClick();
        Invoke("ReallyReallyRestart", 0.1f);
    }

    public void ReallyReallyQuit()
    {
        Application.Quit();
    }

    public void ReallyReallyRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Cancel()
    {
        MouseManager.INSTANCE.PlayClick();
        QuitMenu.SetActive(false);
        RestartMenu.SetActive(false);
        MouseManager.INSTANCE.CloseMenu();
    }

    public void GameOver()
    {
        FinalScore.text = PlanetManager.INSTANCE.Score.ToString();
        QuitMenu.SetActive(false);
        RestartMenu.SetActive(false);
        GameOverMenu.SetActive(true);
        MouseManager.INSTANCE.OpenMenu();
        gameOver = true;
    }

    public void ToggleMusic()
    {
        MusicPlayer.INSTANCE.ToggleMusic();
    }

    [SerializeField]
    AudioListener listener;

    public void MuteAll()
    {
        if (listener.enabled)
        {
            listener.enabled = false;
        }
        else
        {
            listener.enabled = true;
        }
    }
}
