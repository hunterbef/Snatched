using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Behavior")]
    public float fireRate;
    public float reloadDuration;
    public bool canFire;
    public bool isReloading;
    public float verticalOffset;
    public float horizontalOffset;


    [Header("HUD behavior")]
    public bool ammoHUDIsToggled;
    public AudioSource weaponSource;
    public AudioClip gunShot;
    public AudioClip reload;
    public AudioClip emptyWeapon;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerAnimationController pac;
    [SerializeField] private PlayerStats ps;
    [SerializeField] private PlayerHUDController hud;
    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        isReloading = false;
        ammoHUDIsToggled = false;
        pac = GameObject.FindObjectOfType<PlayerAnimationController>();
        ps = GameObject.FindObjectOfType<PlayerStats>();
        hud = FindObjectOfType<PlayerHUDController>();
        hud.ToggleTextVisibility(hud.ammoTexts, false);
    }

    // Update is called once per frame
    void Update()
    {
        //prevent weapon from firing when paused
        if(PauseMenu.isPaused)
        {
            return;
        }

        //block fire input until all fire keys are released after resuming
        if (!canFire && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
        {
            return;
        }

        //probably add check to see if the game is paused
        if (pac.gunIsEquipped)
        {
            if(ammoHUDIsToggled != true)
            {
                ps.ammoChanged();
                hud.ToggleTextVisibility(hud.ammoTexts, true);
                ammoHUDIsToggled = true;
            }
            
            //firing input
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                if (canFire && !isReloading)
                {
                    StartCoroutine(FireWeapon());
                }
            }

            //reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (ps.magAmmo != 6 && isReloading == false)
                {
                    StartCoroutine(ReloadWeapon());
                }
            }
        }
        else
        {
            if (ammoHUDIsToggled != false)
            {
                hud.ToggleTextVisibility(hud.ammoTexts, false);
                ammoHUDIsToggled = false;
            }
        }
    }

    private IEnumerator FireWeapon()
    {
        canFire = false;

        if (ps.magAmmo > 0)
        {
            weaponSource.PlayOneShot(gunShot);
            Vector2 firingPos;
            GameObject bullet;
            switch (pac.currDirection)
            {
                case PlayerAnimationController.Direction.Left:
                    //instanstiate projectile left
                    firingPos = (Vector2)transform.position + new Vector2(-0.90f, .48f);
                    bullet = Instantiate(bulletPrefab, firingPos, Quaternion.Euler(0, 0, 90));
                    break;
                case PlayerAnimationController.Direction.Right:
                    //instanstiate projectile right
                    firingPos = (Vector2)transform.position + new Vector2(0.90f, .48f);
                    bullet = Instantiate(bulletPrefab, firingPos, Quaternion.Euler(0, 0, 270));
                    break;
                case PlayerAnimationController.Direction.Up:
                    //instanstiate projectile up
                    firingPos = (Vector2)transform.position;
                    bullet = Instantiate(bulletPrefab, firingPos, Quaternion.identity);
                    break;
                case PlayerAnimationController.Direction.Down:
                    //instanstiate projectile down
                    firingPos = (Vector2)transform.position;
                    bullet = Instantiate(bulletPrefab, firingPos, Quaternion.Euler(0, 0, 180));
                    break;
            }
            pac.FireGun();
            ps.magAmmo -= 1;
            ps.ammoChanged();
        }
        else
        {
            if(ps.totalAmmo != 0)
            {
                weaponSource.PlayOneShot(emptyWeapon);
                hud.ChangeText(hud.bottomTexts, "[R] RELOAD");
            }
        }

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    private IEnumerator ReloadWeapon()
    {
        isReloading = true;
        weaponSource.PlayOneShot(reload);
        
        if (ps.totalAmmo > 0)
        {
            hud.ChangeText(hud.bottomTexts, "RELOADING...");

            yield return new WaitForSeconds(reloadDuration);

            hud.ChangeText(hud.bottomTexts, "");
            int ammoToReload = 6 - ps.magAmmo;

            //enough ammo to full reload
            if (ps.totalAmmo > ammoToReload)
            {
                ps.magAmmo += ammoToReload;
                ps.totalAmmo -= ammoToReload;  
            }
            //not enough ammo to full reload
            else
            {
                ps.magAmmo += ps.totalAmmo;
                ps.totalAmmo = 0;
            }

            
            ps.ammoChanged();
            
            //play reload animation
        }

        isReloading = false;
    }
}
