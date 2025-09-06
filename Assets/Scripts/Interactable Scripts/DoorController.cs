using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [Header("Door Behavior")]
    [SerializeField] private bool playerNear;
    public bool isLocked;
    public string[] keyNames;
    public bool isEntering;
    public AudioSource doorSounds;
    public AudioClip lockedDoor;
    public AudioClip openDoor;

    [Header("References")]
    public Transform teleTarget;    //YOU NEED TO ATTACH A TELETARGET OF A DOOR TO THIS IN THE INSPECTOR TO TELEPORT
    [SerializeField] private PlayerHUDController playerHUD;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Animator HUDAnim;

    public UnityEvent doorOpened;

    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;
        isEntering = false;
        playerHUD = GameObject.FindObjectOfType<PlayerHUDController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        doorAnim = GetComponent<Animator>();
        HUDAnim = playerHUD.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerNear)
        {
            //get player input
            if(Input.GetKeyDown(KeyCode.E))
            {
                //if door is locked, check playerstats for keyname in keys
                if(isLocked)
                {
                    //array of keys for multilocked doors
                    bool hasKeys = true;
                    if(keyNames.Length > 0)
                    {
                        //enter for loop, flag is false if any of keys are missing
                        foreach (string keyName in keyNames)
                        {
                            if (!playerStats.keys.Contains(keyName))
                            {
                                hasKeys = false;
                            }
                        }
                    }

                    if(hasKeys)
                    {
                        if(!isEntering)
                        {
                            StartCoroutine(EnterDoor());
                        }
                    }
                    else
                    {
                        StartCoroutine(LockedDoor());
                    }
                }
                else
                {
                    if (!isEntering)
                    {
                        StartCoroutine(EnterDoor());
                    }
                }
            }
        }
    }

    public void Enter()
    {
        StartCoroutine(EnterDoor());
    }

    private IEnumerator EnterDoor()
    {
        doorSounds.PlayOneShot(openDoor);
        isEntering = true;  //used to prevent animation spamming
        doorAnim.SetTrigger("Open");                //play door open anim
        HUDAnim.SetTrigger("FadeIn");               //play fade animation
        yield return new WaitForSeconds(0.5f);      //wait 0.5seconds
        HUDAnim.SetTrigger("FadeOut");              //play fade out animation
        player.position = teleTarget.position;      //teleport player
        doorOpened.Invoke();

        yield return new WaitForSeconds(1f);      //wait for fade in/fade out to complete, then can enter again
        isEntering = false; 

    }

    private IEnumerator LockedDoor()
    {
        doorSounds.PlayOneShot(lockedDoor);
        doorAnim.SetTrigger("Locked");                              //play door locked animation
        playerHUD.ChangeText(playerHUD.bottomTexts, "LOCKED...");   //set HUD text to locked
        yield return new WaitForSeconds(1f);                        //wait 1 sec
        playerHUD.ChangeText(playerHUD.bottomTexts, "");            //empty HUD text
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checking if object entering is player
        if (collider.CompareTag("Player"))
        {
            playerHUD.ChangeText(playerHUD.interactables, "[E] ENTER");
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerHUD.ChangeText(playerHUD.interactables, "");
            playerNear = false;
        }
    }
}
