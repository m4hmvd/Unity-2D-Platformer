using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject CurrentCheckpoint;

    private Vector3 startPosition;   // where the player starts the level

    void Start()
    {
        CurrentCheckpoint = null;
        startPosition = FindObjectOfType<PlayerController>().transform.position;
    }

    public void RespawnPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (CurrentCheckpoint != null)
        {
            player.transform.position = CurrentCheckpoint.transform.position;
        }
        else
        {
            player.transform.position = startPosition;
        }

        // stop the player's movement so they don't keep falling after respawning
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
