using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject objectivePanel;

    void Start()
    {
        // UI panels initally not active at start of game level
        isPaused = false;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        objectivePanel.SetActive(true); // Objective panel is active when game is not paused
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.activeSelf)
            {
                closeSettings();
            }

            else if (isPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    // Closes the pause menu and resumes game functions
    public void Resume()
    {
        pauseMenu.SetActive(false);
        objectivePanel.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        // Makes sure player can't fire until buttons are released
        resetPlayerInput();
    }

    // Pause game functions and opens pause menu
    public void Pause()
    {
        objectivePanel.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Close settings menu and go back to pause menu if ESC key is clicked
    public void closeSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    // Go to main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Close game
    public void Quit()
    {
        Application.Quit();
    }

    private void resetPlayerInput()
    {
        Input.ResetInputAxes(); // Clears input states for one frame
    }
}
