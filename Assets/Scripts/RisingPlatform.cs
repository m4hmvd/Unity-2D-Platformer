
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;

    public float speed;
    private bool AtoB;

    private Vector3 targetPosition;

    void Start()
    {
    }

    void Update()
    {
        if (AtoB == true)
        {
            targetPosition = PointA.position;
        }
        else
        {
            targetPosition = PointB.position;
        }

        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.MoveTowards(transform.position.y, targetPosition.y, speed * Time.deltaTime);

        transform.position = newPosition;

        if (Mathf.Abs(transform.position.y - targetPosition.y) < 0.1f)
        {
            AtoB = !AtoB;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
