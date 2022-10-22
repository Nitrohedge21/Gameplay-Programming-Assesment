using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private CapsuleCollider2D capsuleCollider2d;
    float directionX;
    private SpriteRenderer sprite;
    RaycastHit2D raycastHit;
    private float minSpeed = 10f;
    bool facingRight = true;

    [Header("Required Stuff")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private LayerMask jumpableGround;
    int CloserGround;
    int FurtherGround;
    int jumpForce = 10;



    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        //IgnoreLayerCollision uses the index numbers on the project.
        Physics2D.IgnoreLayerCollision(0, 7, true);


    }

    void Update()
    {
        //Changed GetAxisRaws into GetAxis to get a smoother movement and slowdown.
        directionX = Input.GetAxis("Horizontal");

        rigidbody2d.velocity = new Vector2(directionX * moveSpeed, rigidbody2d.velocity.y);
        //The player gains speed over time instead of input, gonna try to figure out how to fix this.
        if (Input.GetAxis("Horizontal") != 0)
        {
            //Added != 0 here so that i could use bool in an if statement. Thanks to logicandchaos from Unity answers.
            moveSpeed += 0.001f;
        }
        else
        {
            moveSpeed -= 0.01f;
        }

        if (moveSpeed < minSpeed)
        {
            moveSpeed = minSpeed;
        }
        if (moveSpeed > maxSpeed)
        {
            moveSpeed = maxSpeed;
        }


        // Putting isGrounded after the input makes the ray not show up for some reason.
        if (/*isGrounded() && */Input.GetButtonDown("Jump"))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
        }
    }

    public bool isGrounded()
    {
        // Draws a line that stays green while it's grounded and turns red when it's not.
        // Got this part from coding monkey

        float extraHeightText = .5f;
        raycastHit = Physics2D.Raycast(capsuleCollider2d.bounds.center, Vector2.down, capsuleCollider2d.bounds.extents.y + extraHeightText, jumpableGround);

        if (directionX < 0 && facingRight)
        {
            flip();
        }
        else if (directionX > 0 && !facingRight)
        {
            flip();
        }

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(capsuleCollider2d.bounds.center, Vector2.down * (capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;

    }


    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       //After the player collides with the launching spring, make it change the layermask(jumpableGround).


       if(other.gameObject.tag == "Spring")
        {
            
            Physics2D.IgnoreLayerCollision(0, 6, true);
            Physics2D.IgnoreLayerCollision(0, 7, false);
            rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rigidbody2d.transform.position = new Vector3(transform.position.x, transform.position.y, 25);
            
        }
    }

}
