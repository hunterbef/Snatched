using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxController : MonoBehaviour
{
    [Header("Pickup behavior")]
    [SerializeField] private bool playerNear;
    [SerializeField] private bool isOpened;
    public AudioSource boxSounds;
    public AudioClip openBox;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerHUDController playerHUD;
    [SerializeField] private Animator lootAnimator;
    void Start()
    {
        playerNear = false;
        isOpened = false;

        //auto fill references
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        playerHUD = GameObject.FindObjectOfType<PlayerHUDController>();
        lootAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            //get player input, incr health and ammo
            if (Input.GetKeyDown(KeyCode.E) && !isOpened)
            {
                isOpened = true;
                StartCoroutine(OpenLoot());
            }
        }
    }

    private IEnumerator OpenLoot()
    {
        boxSounds.PlayOneShot(openBox);
        lootAnimator.SetTrigger("Open");
        playerHUD.ChangeText(playerHUD.interactables, "");
        playerHUD.ChangeText(playerHUD.bottomTexts, "+12 Ammo +20 HP");
        playerStats.addAmmo();
        playerStats.addhealth();
        
        yield return new WaitForSeconds(2f);
        playerHUD.ChangeText(playerHUD.bottomTexts, "");

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checking if object entering is player
        if (collider.CompareTag("Player") && !isOpened)
        {
            playerHUD.ChangeText(playerHUD.interactables, "[E] OPEN");
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNear = false;
            playerHUD.ChangeText(playerHUD.interactables, "");
        }
    }
}
