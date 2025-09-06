using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static bool isGameOver = false;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (playerStats.health <= 0)
        {
            gameOver();
        }

        /*
        if (saved daughter)
        {
            win();
        }
        */
    }

    public void gameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void win()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
