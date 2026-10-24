using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : EnemyController
{

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = false;
    }


    void Update()
    {

    }
    void FixedUpdate()
    {
        if (sr.flipX == true)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, this.GetComponent<Rigidbody2D>().velocity.y);

        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, this.GetComponent<Rigidbody2D>().velocity.y);

        }
    }
}
