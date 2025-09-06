using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI[] interactables;
    public TextMeshProUGUI[] bottomTexts;
    public TextMeshProUGUI[] ammoTexts;
    public Slider healthSlider;
    public PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        ChangeText(interactables, "");
        ChangeText(bottomTexts, "");
        healthSlider.value = 100;
    }

    public void ChangeText(TextMeshProUGUI[] targets, string text)
    {
        foreach (var target in targets)
        {
            target.text = text;
        }
    }

    public void ToggleTextVisibility(TextMeshProUGUI[] targets, bool isVisible)
    {
        foreach (var target in targets)
        {
            target.gameObject.SetActive(isVisible);
        }
    }

    public void editSlider()
    {
        healthSlider.value = playerStats.health;
    }
}
