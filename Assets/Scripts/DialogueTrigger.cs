using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Pickup behavior")]
    [SerializeField] private bool playerNear;
    public string requiredKey; // Key required to show different dialogue (optional)

    [Header("References")]
    [SerializeField] private Canvas NPCCanvas;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private ParticleSystem particleEffect;

    public Dialogue dialogueWithoutKey;
    public Dialogue dialogueWithKey;

    // Boolean to check if currently in dialogue
    public bool inDialogue;
    private bool particlePlayed = false;

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
            //get player input, add key to playerstats when E is pressed
            if(Input.GetKeyDown(KeyCode.E))
            {
                inDialogue = true; // Mark dialogue as active
                TriggerDialogue();
            }
        }

        // Check if dialogue is finished
        if (inDialogue)
        {
            DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();

            // Track the current sentence
            int currentSentence = dialogueSystem.GetCurrentSentenceIndex();

            // Trigger particle effect on the fourth sentence
            if (currentSentence == 3 && !particlePlayed)
            {
                PlayParticleEffect();
            }


            if (dialogueSystem.IsDialogueFinished())
            {
                EndDialogue();
            }
        }
    }

    public void TriggerDialogue ()
    {
        // Determine which dialogue to play based on whether the player has the required key
        if (!string.IsNullOrEmpty(requiredKey) && playerStats.HasKey(requiredKey))
        {
            FindObjectOfType<DialogueSystem>().StartDialogue(dialogueWithKey);
        }
        else
        {
            FindObjectOfType<DialogueSystem>().StartDialogue(dialogueWithoutKey);
        }
    }

    public void EndDialogue()
    {
        inDialogue = false; // Reset the dialogue state when finished
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

    private void PlayParticleEffect()
    {
        if (particleEffect != null)
        {
            particleEffect.Play();
            particlePlayed = true; // Ensure the effect only plays once
        }
    }
}
