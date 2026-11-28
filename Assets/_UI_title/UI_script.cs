using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_script : MonoBehaviour
{
    [Header("SampleScene")]
    public string Level1 = "Level1";

    [Header("Scene_title")]
    public GameObject tutorialPanel;
    public GameObject creditsPanel;

    public void OnClickStart()
    {
        SceneManager.LoadScene(Level1);
    }

    public void OnClickTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void OnClickCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void OnClickClosePanels()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
}