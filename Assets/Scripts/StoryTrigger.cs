using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Same as Dialogue Trigger but with few changes

public class StoryTrigger : MonoBehaviour
{
    [Header("Pickup behavior")]
    [SerializeField] private bool playerNear;
    public string requiredKey; // Key required to trigger story dialogue
    public bool keyNeededToPass; // Player cannot progress until he has the key

    [Header("References")]
    [SerializeField] private Canvas NPCCanvas;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats playerStats;

    public Dialogue dialogueWithoutKey;
    public Dialogue dialogueWithKey;

    public UnityEvent triggerWithKey;

    // Boolean to check if currently in dialogue
    public bool inDialogue;

    void Start()
    {
        playerNear = false;
        inDialogue = false;

        //auto fill references
        NPCCanvas = GetComponentInChildren<Canvas>();
        NPCCanvas.gameObject.SetActive(false); //NPCCanvas doesn't show until player is close
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerNear && !inDialogue)
        {          
            // Check for dialogue conditions
            if (keyNeededToPass)
            {
                if (playerStats.HasKey(requiredKey))
                {
                    triggerWithKey.Invoke();
                    inDialogue = true;
                    TriggerDialogue(dialogueWithKey); // Player has the required key
                }
                else
                {
                    inDialogue = true;
                    TriggerDialogue(dialogueWithoutKey); // Player doesn't have the required key
                }
            }
            else
            {
                inDialogue = true;
                TriggerDialogue(dialogueWithKey); // No key is needed to trigger dialogue
            }  
        }

        // Check if dialogue is finished
        if (inDialogue)
        {
            DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
            if (dialogueSystem.IsDialogueFinished())
            {
                EndDialogue();
            }
        }
    }

    public void TriggerDialogue (Dialogue dialogue)
    {
        FindObjectOfType<DialogueSystem>().StartDialogue(dialogue);
    }

    public void EndDialogue()
    {
        //commented this out so that the story trigger can trigger only once in gameplay.
        if (keyNeededToPass && !playerStats.HasKey(requiredKey))
        {
            inDialogue = false; // Reset the dialogue state when finished
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checking if object entering is player
        if (collider.CompareTag("Player"))
        {
            NPCCanvas.gameObject.SetActive(true);
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNear = false;
            NPCCanvas.gameObject.SetActive(false);
        }
    }
}

