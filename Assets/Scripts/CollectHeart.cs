using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHeart : MonoBehaviour
{
    public AudioClip heartSound;
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponentInParent<PlayerStats>();
            if (player == null) return;

            // touching the heart always counts as finding it (needed for Level 2)
            PlayerStats.hasHeart = true;

            // full health, so leave the heart where it is
            if (player.Heal(healAmount) == false) return;

            AudioManager.Instance.PlayMusicSFX(heartSound);
            Debug.Log("Health: " + player.health);

            Destroy(gameObject);
        }
    }
}
