using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public PlayerStats player;

    public TMP_Text questTitle;
    public TMP_Text questDescription;

    public bool isFirstTimeOnly;
    private bool alreadyCompleted = false;

    public UnityEvent questGiven;

    public void AcceptQuest()
    {
        if (isFirstTimeOnly && alreadyCompleted == false)
        {
            questGiven.Invoke();
            player.quest = quest;
            questTitle.text = quest.title + ":";
            questDescription.text = quest.description;
            quest.isActive = true;
        }
    }

    public void QuestComplete()
    {                
        alreadyCompleted = true;
        //questTitle.text = "";
        questDescription.text = "Completed";
        player.quest.Complete();
        //player.quest = null;
        if (isFirstTimeOnly)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AcceptQuest();
        }
    }
}
