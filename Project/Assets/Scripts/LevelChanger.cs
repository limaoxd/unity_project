using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            FadeToLevel(1);
    }

    // Update is called once per frame
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    { 
        SceneManager.LoadScene(levelToLoad);
    }
}
