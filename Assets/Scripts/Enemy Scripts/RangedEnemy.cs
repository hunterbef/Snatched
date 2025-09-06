using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("EnemyBehavior")]
    public bool isActive;
    [SerializeField] private Vector2 directionToPlayer;
    [SerializeField] private Quaternion rotationToPlayer;
    [SerializeField] private bool canAttack;
    [SerializeField] private float yOffset;
    [SerializeField] private float xOffset;
    [SerializeField] private Vector2 firingPos;
    public AudioSource enemySounds;
    public AudioClip nearPlayer;


    [Header("References")]
    [SerializeField] private Transform player;                  //player's transform/position
    [SerializeField] private bool playerNear;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private PlayerStats playerStats;           //player stats script: player damage
    [SerializeField] private EnemyStats enemyStats;             //this enemy's stats, check if alive
    [SerializeField] private Animator enemyAnimator;            //handles enemy animations
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimator = GetComponent<Animator>();

        playerNear = false;

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStats.isAlive)
        {
            FindPlayer();
            if (isActive)
            {
                AttackPlayer();
            }         
        }
    }

    private void FindPlayer()
    {


        //direction
        directionToPlayer = player.position - transform.position;

        //turn left if player is left, vice versa
        if (directionToPlayer.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            firingPos = (Vector2)transform.position + new Vector2(0f - xOffset, yOffset);
        }
        else
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            firingPos = (Vector2)transform.position + new Vector2(xOffset, yOffset);
        }
        directionToPlayer = (Vector2)player.position - firingPos;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        rotationToPlayer = Quaternion.Euler(0, 0, angleToPlayer);
    }

    private void AttackPlayer()
    {
        if (canAttack && playerNear)
        {
            StartCoroutine(WaitBetweenAttacks());
            StartCoroutine(StartAttack());
        }
    }

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

    private IEnumerator WaitBetweenAttacks()
    {
        canAttack = false;

        yield return new WaitForSeconds(enemyStats.attackSpeed);

        canAttack = true;
    }

    private IEnumerator StartAttack()
    {
        enemyAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.25f);
        GameObject throwable = Instantiate(projectilePrefab, firingPos, rotationToPlayer);
    }
}
