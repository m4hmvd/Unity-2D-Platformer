using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class CollectHeart : MonoBehaviour
{
    public AudioClip heartSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponentInParent<PlayerStats>();

            if (player == null) return;

            player.lives++;
            PlayerStats.hasHeart = true;

            AudioManager.Instance.PlayMusicSFX(heartSound);

            Debug.Log("Lives: " + player.lives);

            Destroy(gameObject);
        }
    }
}
