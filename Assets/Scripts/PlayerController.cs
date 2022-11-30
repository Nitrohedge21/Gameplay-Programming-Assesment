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
    //Changed isTails into isSonic because of the mess I had in the code but it might be changed later on.
    bool facingRight = true;
    public bool isSonic = true;     //Had to make this public in order to access it on camera's code but I might make a get/set to do that and set this bool back to private.
    private Transform followSonic;
    private Transform followTails;
    private float stoppingDistance;
    [Header("Required Stuff")]
    [SerializeField] private float sonicSpeed = 10f;
    [SerializeField] private float tailsSpeed = 7f;
    [SerializeField] private float jumpHeight = 10f;
    private float maxSpeedSonic = 15f;
    private float maxSpeedTails = 12f;
    private float minSpeedSonic = 10f;
    private float minSpeedTails = 7f;
    [SerializeField] public LayerMask jumpableGround;
    GameObject player1;
    GameObject player2;
    private float delayTime;


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
        stoppingDistance = 1.75f;
        player1.GetComponent<PlayerController>().jumpableGround = 1 << 6;
        player2.GetComponent<PlayerController>().jumpableGround = 1 << 6;
        //This shit uses a really weird system for the int values, check drive for the cheat sheet.
    }

    private void FixedUpdate()
    {
        AIFollow();

        //if (player1.GetComponent<PlayerController>().isGrounded() || player2.GetComponent<PlayerController>().isGrounded())
        //{
        //    DelayedFollow();
        //}

    }
    void Update()
    {
        CharacterSwitch();
        //Changed GetAxisRaws into GetAxis to get a smoother movement and slowdown.
        directionX = Input.GetAxis("Horizontal");

        if (this.tag == "Player" ? isSonic : !isSonic)
        {
            rigidbody2d.velocity = new Vector2(directionX * sonicSpeed, rigidbody2d.velocity.y);

            //The player gains speed over time instead of input, gonna try to figure out how to fix this.
            //Honestly don't remember what I was going for when I made the comment above but I assume this is a leftover from my sonic project.

            if (Input.GetAxis("Horizontal") != 0)
            {
                //Added != 0 here so that i could use bool in an if statement. Thanks to logicandchaos from Unity answers.
                if (this.tag == "Player")
                {
                    sonicSpeed += 0.001f;
                }
                else if (this.tag == "Player 2")
                {
                    tailsSpeed += 0.001f;
                }

            }
            else
            {
                if (this.tag == "Player")
                {
                    sonicSpeed -= 0.01f;
                }
                else if (this.tag == "Player 2")
                {
                    tailsSpeed -= 0.01f;
                }
            }

            if (sonicSpeed < minSpeedSonic)
            {
                sonicSpeed = minSpeedSonic;
            }
            if (sonicSpeed > maxSpeedSonic)
            {
                sonicSpeed = maxSpeedSonic;
            }

            if (tailsSpeed < minSpeedTails)
            {
                tailsSpeed = minSpeedTails;
            }
            if (tailsSpeed > maxSpeedTails)
            {
                tailsSpeed = maxSpeedTails;
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
            isSonic = !isSonic;
            //Fixed the sprite order issue by changing the if statements' parantheses.
            if (isSonic)
            {

                player1.GetComponent<SpriteRenderer>().sortingOrder = 2;
                player2.GetComponent<SpriteRenderer>().sortingOrder = 1;

                //While this does change the sorting order, it does not revert them back yet because I haven't been able to figure it out.
            }
            else if (!isSonic)
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

        if (isSonic)
        {
            //While sonic is being controlled

            if (Vector2.Distance(player2.transform.position, followSonic.position) > stoppingDistance /*&& player1.GetComponent<PlayerController>().isGrounded()*/)
            {
                player2.transform.position = Vector2.MoveTowards(player2.transform.position, followSonic.position, tailsSpeed / 1.5f * Time.deltaTime);
            }
        }

        if (!isSonic)
        {
            //While Tails is being controlled

            if (Vector2.Distance(player1.transform.position, followTails.position) > stoppingDistance /*&& player1.GetComponent<PlayerController>().isGrounded()*/)
            {
                player1.transform.position = Vector2.MoveTowards(player1.transform.position, followTails.position, sonicSpeed / 1.5f * Time.deltaTime);
            }


            //While one of them is being controlled, the other one follows the other one on all axes.
            //Make it so that it does not instantly teleport into the z axis when collided with a spring. (Check Sonic Mania to see how they did it)
            //Make it so that the following one "respawns" (could probably just teleport it to the sky and change the z axis to give it the respawn feeling??) when it gets too far away from the other.
        }

    }

    void DelayedFollow()
    {
        Invoke(nameof(AIFollow), 0.5f);
    }
}


//Changed this line with IgnoreCollision on start after using seperate layers for both player game objects.
//if(this.gameObject == player1 && other.gameObject  == player2)
//{
//    Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
//}

// Unused following ai from morebblakeyyy on yt

/*float distance = Vector2.Distance(player2.transform.position, followSonic.position);
Vector2 direction = followSonic.position - player2.transform.position;
direction.Normalize();

if (distance < 3)
{
    player2.transform.position = Vector2.MoveTowards(player2.transform.position, followSonic.position, moveSpeed / 2 * Time.deltaTime);

}*/

//Tested following method
//this just teleported them rather than actually making them follow smoothly unlike the vector2 method
/*
 player1.transform.position = new Vector3(followTails.position.x, followTails.position.y, player1.transform.position.z);
 player2.transform.position = new Vector3(followSonic.position.x, followSonic.position.y, player2.transform.position.z);
*/

//another tested following method
//This pretty much did the same thing as the one above
/*
  player2.transform.position = Vector3.Lerp(player2.transform.position, followSonic.position, Time.time);
  player1.transform.position = Vector3.Lerp(player1.transform.position, followTails.position, Time.time);
 */