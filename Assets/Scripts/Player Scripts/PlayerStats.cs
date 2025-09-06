using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Player Stats")]
    public bool isAlive;        //isCharacterAlive
    public float health;        //currHealth
    public int totalAmmo;       //currTotalAmmo
    public int magAmmo;         //currAmmoInMag

    public Quest quest;
    public AudioSource playerAudio;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private int maxAmmo = 30;   //maxTotalAmmo
    private int maxMagAmmo = 6; //maxAmmoInMag
    private int maxHealth = 100;//maxHealth

    [Header("Keys")]
    public List<string> keys;

    [Header("References")]
    [SerializeField] private PlayerAnimationController pac;
    [SerializeField] private PlayerHUDController playerHUD;

    void Start()
    {
        isAlive = true;
        health = maxHealth;
        totalAmmo = maxAmmo;
        magAmmo = maxMagAmmo;

        keys = new List<string>();

        pac = GetComponent<PlayerAnimationController>();
        playerHUD = FindAnyObjectByType<PlayerHUDController>();
    }

    public void addKey(string keyName)
    {
        if(keyName != null)
        {
            keys.Add(keyName);
        }
    }

    public void addAmmo()
    {
        totalAmmo += 12;

        if(totalAmmo > maxAmmo) 
        {
            totalAmmo = maxAmmo;
        }

        ammoChanged();
    }

    public void addhealth()
    {
        health += 20f;

        if(health > maxHealth)
        {
            health = maxHealth;
        }

        //update hud health
        playerHUD.editSlider();
    }

    public void takeDamage(float dmg)
    {
        pac.takeDamage();
        health -= dmg;
        if(health <= 0)
        {
            isAlive = false;
            playerAudio.PlayOneShot(deathSound);
        }
        else
        {
            playerAudio.PlayOneShot(damageSound);
        }

        //update hud health
        playerHUD.editSlider();
    }

    public void ammoChanged()
    {
        playerHUD.ChangeText(playerHUD.ammoTexts, magAmmo + " / " + totalAmmo);
    }

    public bool HasKey(string keyName)
    {
        return keys.Contains(keyName);
    }
}
