using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    public bool isAlive;
    public bool isDying;
    public float maxHealth;
    public float currHealth;
    public float moveSpeed;
    public float damage;
    public float attackSpeed;
    public AudioSource enemySounds;
    public AudioClip takeDamage;
    public AudioClip dyingSound;
   
    [Header("EnemyRefs")]
    [SerializeField] private Animator animator;

    public ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        isAlive = true;
        isDying = false;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float dmg)
    {
        if(isAlive)
        {
            currHealth -= dmg;
            if(currHealth > 0)
            {
                enemySounds.PlayOneShot(takeDamage);
            }
            animator.SetTrigger("Hit");
        }
        if (currHealth <= 0 && !isDying)
        {
            isAlive = false;
            StartCoroutine(Die());
            Instantiate(deathParticles, transform.position, transform.rotation);
        }
    }

    private IEnumerator Die()
    {
        enemySounds.PlayOneShot(dyingSound);
        isDying = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
