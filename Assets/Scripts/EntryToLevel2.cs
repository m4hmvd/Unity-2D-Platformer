using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryToLevel2 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerStats.hasHeart == true)
            {
                // keep lives and health so Level 2 continues from here
                PlayerStats stats = other.GetComponentInParent<PlayerStats>();
                if (stats != null) stats.SaveForNextLevel();
                SceneManager.LoadScene(2);
            }
            else
            {
                Debug.Log("You need the heart to enter Level 2!");
                FindObjectOfType<LevelManager>().RespawnPlayer();
            }
        }
    }
}
