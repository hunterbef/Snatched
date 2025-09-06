using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("EnemyBehavior")]
    [SerializeField] private bool playerNear;
    [SerializeField] private Vector2 directionToPlayer;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private bool canWalk;
    [SerializeField] private bool canAttack;
    public AudioSource enemySounds;
    public AudioClip nearPlayer;
    public AudioClip attackPlayer;

    [Header("References")]
    [SerializeField] private Transform player;                  //player's transform/position
    [SerializeField] private PlayerStats playerStats;           //player stats script: player damage
    [SerializeField] private EnemyStats enemyStats;             //this enemy's stats, check if alive
    [SerializeField] private Rigidbody2D rigidBody;             //enemy's rigid body, for movement
    [SerializeField] private Animator enemyAnimator;            //handles enemy animations
    [SerializeField] private Collider2D detectionCollider;      //detection collider, triggers enemy movement

    void Start()
    {
        //populating references
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
        rigidBody = GetComponent<Rigidbody2D>();        
        enemyAnimator = GetComponent<Animator>();
        detectionCollider = GetComponent<Collider2D>();

        playerNear = false;
        canWalk = true;
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if (playerNear && enemyStats.isAlive)
        {
            FindPlayer();
            MoveToPlayer();
            AttackPlayer();
        }
    }

    //this toggles on the movement for enemy if the player is close enough
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerNear)
        {
            if (collision.CompareTag("Player"))
            {
                playerNear = true;
                enemySounds.PlayOneShot(nearPlayer);
                Debug.Log("Detected!");
            }
        }
    }

    //get distance and direction to player
    private void FindPlayer()
    {
        //distance
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        //direction
        directionToPlayer = (player.position - transform.position).normalized;

        //turn left if player is left, vice versa
        if (directionToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
    }


    //move towards player
    private void MoveToPlayer()
    {
        if (canWalk)
        {
            enemyAnimator.SetBool("IsWalking", true);
            Vector2 movement = directionToPlayer * enemyStats.moveSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + movement);
        }
        else
        {
            enemyAnimator.SetBool("IsWalking", false);
        }
    }

    //attack player when enemy is close enough
    private void AttackPlayer()
    {
        if (distanceToPlayer <= 1.2)
        {
            if (canAttack)
            {
                StartCoroutine(WaitBetweenAttacks());
                StartCoroutine(StartAttack()); 
            }

        }
    }

    private IEnumerator StartAttack()
    {
        canWalk = false;
        enemyAnimator.SetTrigger("Attack");
        enemySounds.PlayOneShot(attackPlayer);
        yield return new WaitForSeconds(0.5f);
        playerStats.takeDamage(enemyStats.damage);
        canWalk = true;
    }

    private IEnumerator WaitBetweenAttacks()
    {
        canAttack = false;

        yield return new WaitForSeconds(enemyStats.attackSpeed);

        canAttack = true;
    }
}
