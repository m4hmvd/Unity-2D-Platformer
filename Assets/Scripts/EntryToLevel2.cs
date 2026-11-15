using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lab 7, Exercise 20: the player can only go to Level 2 after finding the heart.
public class EntryToLevel2 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerStats.hasHeart == true)
            {
                SceneManager.LoadScene(2);   // Level2 (Build Settings number)
            }
            else
            {
                Debug.Log("You need the heart to enter Level 2!");
                FindObjectOfType<LevelManager>().RespawnPlayer();
            }
        }
    }
}
