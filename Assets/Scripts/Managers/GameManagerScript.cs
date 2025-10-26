using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    public RewardUI rewardUI;
    public List<CursedPower> allPowers = new List<CursedPower>();
    

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
        Cursor.visible = true;
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
    public void LoadGameScene()
    {
        // AudioManager.instance.InitializeMusic(FMODEvents.instance.musicLevelOne);
        SceneManager.LoadScene("GameScene");
        Cursor.visible = false;
    }

    public void LoadFAQScene()
    {
        SceneManager.LoadScene("FAQScene");
        Cursor.visible = true;
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game has exited.");
    }

    public void ShowRewardOptions(string dollName)
    {
        var options = allPowers
            .Where(p => p.associatedDollName == dollName)
            .OrderBy(x => Random.value)
            .Take(3)
            .ToList();

        rewardUI.Open(options);
    }

    public void OnPowerChosen(CursedPower chosen)
    {
        // Use the singleton instead of FindObjectOfType
        PlayerPowerManager.Instance.AddPower(chosen);

        // Close the reward UI
        rewardUI.Close();
    }
}