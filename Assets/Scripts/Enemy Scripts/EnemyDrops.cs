using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private bool itemHasDropped;
    [Header("References")]
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private GameObject itemDrop;
    [SerializeField] private Transform itemDropPos;

    // Start is called before the first frame update
    void Start()
    {
        itemHasDropped = false;
        enemyStats = GetComponent<EnemyStats>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemyStats.isAlive && !itemHasDropped)
        {
            Instantiate(itemDrop, itemDropPos.position, transform.rotation);
            itemHasDropped = true;
        }
    }
}
