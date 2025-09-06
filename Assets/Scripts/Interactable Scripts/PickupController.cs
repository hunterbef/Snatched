using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupController : MonoBehaviour
{
    [Header("Pickup behavior")]
    [SerializeField] private bool playerNear;
    public string keyName;
    public bool isPickedUp;
    public AudioSource playerAudio;
    public AudioClip keysPickedUp;

    [Header("References")]
    [SerializeField] private PlayerHUDController playerHUD; //player's hud, changes textboxes
    [SerializeField] private Transform player;              //the player's position
    [SerializeField] private PlayerStats playerStats;       //player health/ammo/stats
    [SerializeField] private SpriteRenderer spriteRenderer;

    public UnityEvent itemPickup;

    void Start()
    {
        playerNear = false;
        isPickedUp = false;
        //auto fill references
        playerHUD = GameObject.FindObjectOfType<PlayerHUDController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerNear)
        {
            //get player input, add key to playerstats when E is pressed
            if(Input.GetKeyDown(KeyCode.E) && !isPickedUp)
            {
                isPickedUp = true;
                StartCoroutine(AddKey());
                
            }
        }
    }

    private IEnumerator AddKey()
    {
        itemPickup.Invoke();
        playerAudio.PlayOneShot(keysPickedUp);
        playerStats.addKey(keyName);
        playerHUD.ChangeText(playerHUD.interactables, "");
        //hide object before destroying
        Color srColor = spriteRenderer.color;
        srColor.a = 0f;
        spriteRenderer.color = srColor;
 
        playerHUD.ChangeText(playerHUD.bottomTexts, keyName.ToUpper() + " ADDED");
        yield return new WaitForSeconds(1f);
        
        playerHUD.ChangeText(playerHUD.bottomTexts, "");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checking if object entering is player
        if (collider.CompareTag("Player"))
        {
            playerHUD.ChangeText(playerHUD.interactables, "[E] PICK UP");
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            //empty text box
            playerHUD.ChangeText(playerHUD.interactables, "");
            playerNear = false;
        }
    }
}
