using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 2;
    public int damage = 1;
    public SpriteRenderer sr;
    public int health = 10;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

    }
    public void EnemyTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Enemy Finished");
            Destroy(gameObject);
        }
    }
    public void Flip()
    {
        sr.flipX = !sr.flipX;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerStats>().TakeDamage(damage);
            Flip();
        }
        else if (other.tag == "Wall")
        {
            Flip();
        }


    }


}
