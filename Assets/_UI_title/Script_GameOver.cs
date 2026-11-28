using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_gameover : MonoBehaviour
{
    public string Scene_title = "Title";

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene(GameState.lastScene);
    }

    public void OnClickBackToTitle()
    {
        SceneManager.LoadScene(Scene_title);
    }
}