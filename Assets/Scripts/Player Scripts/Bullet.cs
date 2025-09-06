using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Behavior")]
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyStats enemyStats;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if bullet hits an enemy
        if (collision.CompareTag("EnemyHitBox"))
        {
            enemy = collision.transform.parent.gameObject;  //get enemy that collided with bullet
            enemyStats = enemy.GetComponent<EnemyStats>();  //get enemystats in enemy gameobject
            enemyStats.TakeDamage(damage);                  //enemy loses health
            Destroy(this.gameObject);
        }


        //if bullet hits a wall
        if(collision.CompareTag("Walls"))
        {
            Destroy(this.gameObject);
        }
    }
}
