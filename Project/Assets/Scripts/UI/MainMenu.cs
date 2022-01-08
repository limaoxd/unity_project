using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string part0 = "part_0";
    public GameObject MainPanel;
    public GameObject LoadGamePanel;
    public GameObject SettingsPanel;
    public Sound_emitter MenuSound;
    // Start is called before the first frame update
    void Start()
    {
        MenuSound = GetComponent<Sound_emitter>();
    }

    // Update is called once per frame
    public void NewGameClick()
    {
        SceneManager.LoadScene(part0);
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void BackClick()
    {
        MenuSound.run = true;
        MainPanel.SetActive(true);
        LoadGamePanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }
}