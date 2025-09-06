using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GhostEnemyHealth : MonoBehaviour
{
    [Header("Ghost Parts")]
    public List<string> parts;      //list of ghost parts
    public List<string> partsFound; //list of ghost parts already deducted from hp
    [Header("References")]
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private PlayerStats playerStats;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        playerStats = FindObjectOfType<PlayerStats>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckParts();
    }

    private void CheckParts()
    {
        //check part list, if any in playerstats matches parts, take 1 damage
        //ghost will take 4 damage with 4 parts
        foreach (string part in parts)
        {
            if(playerStats.keys.Contains(part) && !partsFound.Contains(part))
            {
                partsFound.Add(part);
                enemyStats.TakeDamage(1f);
            }
        }
    }
}
