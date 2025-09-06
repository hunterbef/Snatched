using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("AnimationStats")]
    public bool isWalking;              //is the player walking
    public bool gunIsEquipped;          //is the gun equipped
    public Direction currDirection;     //enum, what direction player is facing
    

    [Header("References")]
    [SerializeField] private Animator bodyAnimator;     //controls the upper body animations
    [SerializeField] private Animator legsAnimator;     //controls the lower body animations
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    // Start is called before the first frame update
    void Start()
    {
        bodyAnimator = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Animator>();
        legsAnimator = GameObject.FindGameObjectWithTag("PlayerLegs").GetComponent<Animator>();
        currDirection = Direction.Right;
        gunIsEquipped = false;
        isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks to see if the player is in a dialogue
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.isTalking())
        {
            isWalking = false;
            stopMoveAnimation();
            return;
        }
        //equip weapon
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipGun();
        }

        //single button movement
        HandleMovement(KeyCode.W, Direction.Up);        // W or Up Arrow
        HandleMovement(KeyCode.A, Direction.Left);      // A or Left Arrow
        HandleMovement(KeyCode.S, Direction.Down);      // S or Down Arrow
        HandleMovement(KeyCode.D, Direction.Right);     // D or Right Arrow

        HandleMovement(KeyCode.UpArrow, Direction.Up);  // Arrow Up
        HandleMovement(KeyCode.LeftArrow, Direction.Left); // Arrow Left
        HandleMovement(KeyCode.DownArrow, Direction.Down); // Arrow Down
        HandleMovement(KeyCode.RightArrow, Direction.Right); // Arrow Right

        // Handle chorded (combined) movement, prioritizing horizontal keys
        HandleMovement(KeyCode.W, KeyCode.A, Direction.Left, Direction.Up);  // W + A
        HandleMovement(KeyCode.S, KeyCode.A, Direction.Left, Direction.Down); // S + A

        HandleMovement(KeyCode.W, KeyCode.D, Direction.Right, Direction.Up); // W + D
        HandleMovement(KeyCode.S, KeyCode.D, Direction.Right, Direction.Down); // S + D

        HandleMovement(KeyCode.UpArrow, KeyCode.LeftArrow, Direction.Left, Direction.Up); // Arrow Up + Left Arrow
        HandleMovement(KeyCode.DownArrow, KeyCode.LeftArrow, Direction.Left, Direction.Down); // Arrow Down + Left Arrow

        HandleMovement(KeyCode.UpArrow, KeyCode.RightArrow, Direction.Right, Direction.Up); // Arrow Up + Right Arrow
        HandleMovement(KeyCode.DownArrow, KeyCode.RightArrow, Direction.Right, Direction.Down); // Arrow Down + Right Arrow
        WalkingAnimation();
    }

    void HandleMovement(KeyCode key, Direction direction)
    {
        if (Input.GetKey(key))
        {
            isWalking = true;
            currDirection = direction;
        }
        if (Input.GetKeyUp(key))
        {
            isWalking = false;
            currDirection = direction;
        }
    }

    void HandleMovement(KeyCode key, KeyCode key2, Direction direction, Direction direction2)
    {
        if (Input.GetKey(key) && Input.GetKey(key2))
        {
            isWalking = true;
            currDirection = direction;
        }
        if (Input.GetKeyUp(key))
        {
            currDirection = direction2;
        }
        else if (Input.GetKeyUp(key2))
        {
            currDirection = direction;
        }

        if (Input.GetKeyUp(key) && Input.GetKeyUp(key2))
        {
            isWalking = false;
        }
    }
    private void EquipGun()
    {
        
        if (!gunIsEquipped)
        {
           
            gunIsEquipped = true;
            bodyAnimator.SetBool("IsGun", true);
        }
        else
        {
            gunIsEquipped = false;
            bodyAnimator.SetBool("IsGun", false);
        }


        if (gunIsEquipped)
        {
            switch (currDirection)
            {
                case Direction.Left:
                case Direction.Right:
                    bodyAnimator.SetTrigger("HorizontalEquip");
                    break;
                case Direction.Up:
                    bodyAnimator.SetTrigger("BackEquip");
                    break;
                case Direction.Down:
                    bodyAnimator.SetTrigger("FrontEquip");
                    break;
            }
        }
        else
        {
            switch (currDirection)
            {
                case Direction.Left:
                case Direction.Right:
                    bodyAnimator.SetTrigger("HorizontalUnequip");
                    break;
                case Direction.Up:
                    bodyAnimator.SetTrigger("BackUnequip");
                    break;
                case Direction.Down:
                    bodyAnimator.SetTrigger("FrontUnequip");
                    break;
            }
        }
    }

    public void FireGun()
    {
        switch (currDirection)
        {
            case Direction.Left:
            case Direction.Right:
                bodyAnimator.SetTrigger("HorizontalFire");
                break;
            case Direction.Up:
                bodyAnimator.SetTrigger("BackFire");
                break;
            case Direction.Down:
                bodyAnimator.SetTrigger("FrontFire");
                break;
        }
    }

    /*This code works using a blend tree instead of the regular animation state/transition stuff.
     * (1,0),(-1,0) is horizontal animations
     * (0,1),(0,-1) is front animations
     * (1,1),(-1,-1) is back animations
     */
    private void WalkingAnimation()
    {
        //arbitrarily placed coordinates in blend tree, sets animation state based on coordinates
        if(isWalking)
        {
            switch (currDirection)
            {
                case Direction.Left:
                    //mirror player
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", 1);
                    bodyAnimator.SetFloat("Vertical", 0);

                    legsAnimator.SetFloat("Horizontal", 1);
                    legsAnimator.SetFloat("Vertical", 0);
                    break;
                case Direction.Right:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", 1);
                    bodyAnimator.SetFloat("Vertical", 0);

                    legsAnimator.SetFloat("Horizontal", 1);
                    legsAnimator.SetFloat("Vertical", 0);

                    break;
                case Direction.Up:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", 1);
                    bodyAnimator.SetFloat("Vertical", 1);

                    legsAnimator.SetFloat("Horizontal", 1);
                    legsAnimator.SetFloat("Vertical", 1);

                    break;
                case Direction.Down:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", 0);
                    bodyAnimator.SetFloat("Vertical", 1);

                    legsAnimator.SetFloat("Horizontal", 0);
                    legsAnimator.SetFloat("Vertical", 1);
                    break;
            }
        }
        else
        {
            switch (currDirection)
            {
                case Direction.Left:
                    //mirror player
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", -1);
                    bodyAnimator.SetFloat("Vertical", 0);

                    legsAnimator.SetFloat("Horizontal", -1);
                    legsAnimator.SetFloat("Vertical", 0);
                    break;
                case Direction.Right:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", -1);
                    bodyAnimator.SetFloat("Vertical", 0);

                    legsAnimator.SetFloat("Horizontal", -1);
                    legsAnimator.SetFloat("Vertical", 0);

                    break;
                case Direction.Up:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", -1);
                    bodyAnimator.SetFloat("Vertical", -1);

                    legsAnimator.SetFloat("Horizontal", -1);
                    legsAnimator.SetFloat("Vertical", -1);

                    break;
                case Direction.Down:
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    bodyAnimator.SetFloat("Horizontal", 0);
                    bodyAnimator.SetFloat("Vertical", -1);

                    legsAnimator.SetFloat("Horizontal", 0);
                    legsAnimator.SetFloat("Vertical", -1);
                    break;
            }
        }
    }

    public void takeDamage()
    {
        bodyAnimator.SetTrigger("Hit");
        legsAnimator.SetTrigger("Hit");
    }

    //used to stop movement animations and perform idle animation whenever engaging in dialogue
    private void stopMoveAnimation()
    {
        switch (currDirection)
        {
            case Direction.Left:
                //mirror player
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                bodyAnimator.SetFloat("Horizontal", -1);
                bodyAnimator.SetFloat("Vertical", 0);

                legsAnimator.SetFloat("Horizontal", -1);
                legsAnimator.SetFloat("Vertical", 0);
                break;
            case Direction.Right:
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                bodyAnimator.SetFloat("Horizontal", -1);
                bodyAnimator.SetFloat("Vertical", 0);

                legsAnimator.SetFloat("Horizontal", -1);
                legsAnimator.SetFloat("Vertical", 0);

                break;
            case Direction.Up:
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                bodyAnimator.SetFloat("Horizontal", -1);
                bodyAnimator.SetFloat("Vertical", -1);

                legsAnimator.SetFloat("Horizontal", -1);
                legsAnimator.SetFloat("Vertical", -1);

                break;
            case Direction.Down:
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                bodyAnimator.SetFloat("Horizontal", 0);
                bodyAnimator.SetFloat("Vertical", -1);

                legsAnimator.SetFloat("Horizontal", 0);
                legsAnimator.SetFloat("Vertical", -1);
                break;
        }

        bodyAnimator.SetBool("IsGun", gunIsEquipped);
    }
}
