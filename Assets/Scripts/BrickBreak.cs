using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreak : MonoBehaviour
{
    private SpriteRenderer sr;

    public Sprite explodedBlock;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.tag == "Player" && other.GetContact(0).point.y < transform.position.y)
        {


            sr.sprite = explodedBlock;

            Object.Destroy(gameObject, .2f);
        }
    }
}
