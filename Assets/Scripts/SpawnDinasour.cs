using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class SpawnDinasour : MonoBehaviour
{
    public Transform enemy;
    public Transform spawnPoint;

    private bool hasSpawned = false;

    void RespawnEnemy()
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hasSpawned)
        {
            RespawnEnemy();
            hasSpawned = true;
        }
    }
}
