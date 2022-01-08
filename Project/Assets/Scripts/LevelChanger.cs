using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
 
    // Update is called once per frame
    public void FadeToLevel(int levelIndex)
    {
        Time.timeScale = 1;
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoad);
    }
}
