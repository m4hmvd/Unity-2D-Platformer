using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    // private bool isFacingRight;
    public KeyCode Spacebar;
    public KeyCode L;
    public KeyCode R;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(Spacebar) && grounded)
        {
            Jump();
        }

        float moveInput = 0f;

        if (Input.GetKey(L))
        {
            moveInput = -1f;

            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else if (Input.GetKey(R))
        {
            moveInput = 1f;

            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(
            moveInput * moveSpeed,
            rb.velocity.y
        );
        Debug.Log($"Input: {moveInput}, X Velocity: {rb.velocity.x}");
        Debug.Log($"Position: {rb.position}");
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("Height", rb.velocity.y);
            anim.SetBool("Grounded", grounded);
        }
    }




    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
    }
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    void LateUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Debug.Log(
            $"Position: {rb.position} | Velocity: {rb.velocity}"
        );
    }
}
