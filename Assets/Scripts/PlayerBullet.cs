using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public ABShooting player;
    public int damage;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float timer = 0;
    private float lifeTime = 2;

    void Start()
    {
        player = FindObjectOfType<ABShooting>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr.flipX = player.sr.flipX;
    }

    void Update()
    {
        if (sr.flipX)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if ((timer += Time.deltaTime) >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().EnemyTakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
