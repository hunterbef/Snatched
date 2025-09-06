using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameSceneChanger : MonoBehaviour
{
    [Header("Behavior")]
    public string targetSceneName;

    public UnityEvent sceneChanged;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checking if object entering is player
        if (collider.CompareTag("Player"))
        {
            sceneChanged.Invoke();
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
