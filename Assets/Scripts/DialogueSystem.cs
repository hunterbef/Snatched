using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text nameText; // For displaying the speaker's name
    public TMP_Text dialogueText; // For displaying the dialogue text

    public Animator animator; // Animator for dialogue box transitions (e.g., open/close animations)
    private Queue<string> sentences; // A queue to store sentences in the dialogue

    private bool inDialogue; // Flag to check if dialogue is currently active
    private bool isTyping; // Flag to check if the TypeSentence coroutine is currently running

    private int currentSentenceIndex; // Tracks the current sentence index
    public static DialogueSystem Instance { get; private set; } // So that other scripts can check if dialogue is active

    public delegate void SentenceEvent(int sentenceIndex); // Event for when a sentence is displayed
    public event SentenceEvent OnSentenceDisplayed; // Event handler

    void Awake()
    {
        // Makes sure that only one instance exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        sentences = new Queue<string>(); // Initialize the queue to store dialogue sentences
        isTyping = false; // Initially, no coroutine is running
        currentSentenceIndex = -1; // Start before the first sentence
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent dialogue interaction when paused
        if (PauseMenu.isPaused)
        {
            return;
        }
        // Check if a dialogue is active, the typing coroutine is not running, and the player presses the E key
        if (inDialogue && !isTyping && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence(); // Move to the next sentence in the dialogue
        }
    }

    // Starts the dialogue by initializing the queue and displaying the first sentence
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true); // Play the opening animation for the dialogue box

        nameText.text = dialogue.name; // Set the speaker's name in the UI

        sentences.Clear(); // Clear any previously stored sentences

        // Enqueue all sentences from the provided dialogue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        inDialogue = true; // Mark dialogue as active
        currentSentenceIndex = -1; // Reset sentence index
        DisplayNextSentence(); // Display the first sentence
    }

    // Displays the next sentence in the queue
    public void DisplayNextSentence()
    {
        // If there are no more sentences, end the dialogue
        if (sentences.Count == 0)
        {
            inDialogue = false; // Mark dialogue as inactive
            EndDialogue();
            return;
        }

        // Update sentence index
        currentSentenceIndex++;

        // Get the next sentence from the queue
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // Stop any currently running coroutine (e.g., unfinished typing)
        StartCoroutine(TypeSentence(sentence)); // Start the typing animation for the new sentence

        // Trigger event for current sentence
        OnSentenceDisplayed?.Invoke(currentSentenceIndex);
    }

    // Coroutine to type out the sentence letter by letter
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // Mark that the typing coroutine is running
        dialogueText.text = ""; // Clear the dialogue text in the UI

        // Add each letter in the sentence one by one
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Append the letter to the dialogue text
            yield return null; // Wait until the next frame
        }

        isTyping = false; // Mark that the typing coroutine has finished
    }

    // Ends the dialogue by closing the dialogue box
    void EndDialogue()
    {
        animator.SetBool("isOpen", false); // Play the closing animation for the dialogue box
    }

    // Returns whether the dialogue is finished (used by other scripts if needed)
    public bool IsDialogueFinished()
    {
        return !inDialogue; // Returns true if the dialogue is not active
    }

    // Getter for inDialogue
    public bool isTalking()
    {
        return inDialogue;
    }

    // Getter for the current sentence index
    public int GetCurrentSentenceIndex()
    {
        return currentSentenceIndex;
    }
}
