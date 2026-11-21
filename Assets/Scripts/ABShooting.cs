using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABShooting : MonoBehaviour
{
    public KeyCode Return;
    public Transform shootingPoint;
    public GameObject bullet;
    public SpriteRenderer sr;
    public AudioClip shootSound;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public void Update()
    {
        if (Input.GetKeyDown(Return))
        {
            Shooting();
        }
    }
    public void Shooting()
    {
        Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);

        // PlayOneShot lets quick shots overlap instead of cutting each other off
        if (AudioManager.Instance != null && shootSound != null)
        {
            AudioManager.Instance.sfxSource.PlayOneShot(shootSound);
        }
    }
}
