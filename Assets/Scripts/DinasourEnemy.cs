using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DinasourEnemy : EnemyController
{
    void FixedUpdate()
    {
        if (sr.flipX)
        {
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(-maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
