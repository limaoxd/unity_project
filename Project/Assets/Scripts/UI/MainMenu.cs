using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject LoadGamePanel;
    public GameObject SettingsPanel;
    public Sound_emitter MenuSound;
    public LevelChanger levelChanger;
    // Start is called before the first frame update
    void Start()
    {
        MenuSound = GetComponent<Sound_emitter>();
    }

    // Update is called once per frame
    public void NewGameClick()
    {
        MenuSound.run = true;
        levelChanger.FadeToLevel(1);
    }

    public void LoadGameClick()
    {
        MenuSound.run = true;
        MainPanel.SetActive(false);
        LoadGamePanel.SetActive(true);
        SettingsPanel.SetActive(false);
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
        Debug.Log("AAA");
        MenuSound.run = false;
    }
}