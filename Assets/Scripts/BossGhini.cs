using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGhini : MonoBehaviour
{

    GameObject[] enemies = new GameObject[20];

    public void Die()
    {
        FindObjectOfType<EnemySpawnManager>().enemies.CopyTo(enemies);
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealthManager>().invulnerable = false;
                enemy.GetComponent<EnemyHealthManager>().HitEnemy(99, 0);
            }
        }
    }
}