using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSettings : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    public bool isPaused;

    private void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        PauseGame();
    }

    public void CloseSettings()
    {
        ResumeGame();
        settingsPanel.SetActive(false);
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
