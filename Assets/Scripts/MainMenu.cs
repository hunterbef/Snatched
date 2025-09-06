using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Called when Play Button in main menu is clicked
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Called when quit Button in main menu is clicked
    public void QuitGame()
    {
        Application.Quit();
    }

}
