using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject ContinueButton;
    public GameObject LoadGamePanel;
    public GameObject SettingsPanel;
    public LoadGame Loader;
    public Sound_emitter MenuSound;
    public LevelChanger levelChanger;
    // Start is called before the first frame update
    void Start()
    {
        MenuSound = GetComponent<Sound_emitter>();
        try
        {
            File.ReadAllText(Application.dataPath + @"/StreamingAssets/archive.json");
        }
        catch
        {
            ContinueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    public void NewGameClick()
    {
        MenuSound.run = true;
        levelChanger.FadeToLevel(1);
    }

    public void ContinueClick()
    {
        MenuSound.run = true;
        levelChanger.FadeToLevel(Loader.CheckScene());
    }

    public void SettingsClick()
    {
        MenuSound.run = true;
        MainPanel.SetActive(false);
        LoadGamePanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void QuitGameClick()
    {
        MenuSound.run = true;
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void BackClick()
    {
        MainPanel.SetActive(true);
        LoadGamePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MenuSound.run = true;
    }

    public void ButtonClickUp()
    {
        MenuSound.run = false;
    }
}