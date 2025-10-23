using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager found. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager Awake called");
    }

    public void MainMenuScene()
    {
        // if (AudioManager.instance != null)
        // {
        //     AudioManager.instance.SetAmbienceParameter("Ambience Intensity",0.0f);
        //     AudioManager.instance.StopMusic();
        // }
        SceneManager.LoadScene("MainMenuScene");
        Cursor.visible = true;
    }

    public void LoadDeathScene()
    {
        // AudioManager.instance.SetAmbienceParameter("Ambience Intensity", 0.0f);
        // AudioManager.instance.StopMusic();
        // ScoreManager.instance.Reset();
        SceneManager.LoadScene("DeathScene");
        Cursor.visible = true;
    }

    public void LoadCreditsScene()
    {
        // AudioManager.instance.SetAmbienceParameter("Ambience Intensity",0.0f);
        // AudioManager.instance.StopMusic();
        SceneManager.LoadScene("CreditsScene");
        Cursor.visible = true;
    }

    public void LoadStartScene()
    {
        // AudioManager.instance.SetAmbienceParameter("Ambience Intensity",0.0f);
        // AudioManager.instance.StopMusic();
        // if (ScoreManager.instance != null) ScoreManager.instance.Reset();
        SceneManager.LoadScene("StartScene");
        Cursor.visible = true;
    }

    public void LoadGameScene()
    {
        // AudioManager.instance.InitializeMusic(FMODEvents.instance.musicLevelOne);
        Cursor.visible = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}