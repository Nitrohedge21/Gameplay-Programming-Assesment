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
    public bool playerSwitch = true;
    public bool isTails = false;
    public Transform followSonic;
    public Transform followTails;

    [Header("Required Stuff")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private LayerMask jumpableGround;
    GameObject player1;
    GameObject player2;


    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        //IgnoreLayerCollision uses the index numbers on the project.
        
        
        Physics2D.IgnoreLayerCollision(8, 9, true);
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player 2");
        followSonic = player1.transform;
        followTails = player2.transform;
        
    }

    void Update()
    {
        CharacterSwitch();
        AIFollow();
        //Changed GetAxisRaws into GetAxis to get a smoother movement and slowdown.
        directionX = Input.GetAxis("Horizontal");

        if (this.tag == "Player" ? playerSwitch : !playerSwitch)
        {
            rigidbody2d.velocity = new Vector2(directionX * moveSpeed, rigidbody2d.velocity.y);

            //The player gains speed over time instead of input, gonna try to figure out how to fix this.
            //Honestly don't remember what I was going for when I made the comment above but I assume this is a leftover from my sonic project.

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
            // I don't know what I meant by this because I can see it fine - 29.10.2022
            if (isGrounded() && Input.GetButtonDown("Jump"))
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
            }
        }
        
    }

    void CharacterSwitch()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerSwitch = !playerSwitch;
            //Fixed the sprite order issue by changing the if statements' parantheses.
            if(playerSwitch)
            {

                player1.GetComponent<SpriteRenderer>().sortingOrder = 2;
                player2.GetComponent<SpriteRenderer>().sortingOrder = 1;
                
                //While this does change the sorting order, it does not revert them back yet because I haven't been able to figure it out.
            }
            else if (!playerSwitch)
            {
                //Reverts the thing above
                player1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                player2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
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

    void AIFollow()
    {
        //Fixed the issue with crashing but the following one is literally teleporting every frame.

        if (playerSwitch)
        {
            //While sonic is being controlled

            player2.transform.position = Vector2.MoveTowards(player2.transform.position, followSonic.position, moveSpeed * Time.deltaTime);

        }
        
        if (!playerSwitch)
        {
            //While Tails is being controlled

            player1.transform.position = Vector2.MoveTowards(player1.transform.position, followTails.position, moveSpeed * Time.deltaTime);
        }


        //While one of them is being controlled, the other one follows the other one on all axes.
        //Make it so that it does not instantly teleport into the z axis when collided with a spring. (Check Sonic Mania to see how they did it)
        //Make it so that the following one "respawns" (could probably just teleport it to the sky and change the z axis to give it the respawn feeling??) when it gets too far away from the other.
    }

}


//Changed this line with IgnoreCollision on start after using seperate layers for both player game objects.
//if(this.gameObject == player1 && other.gameObject  == player2)
//{
//    Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
//}