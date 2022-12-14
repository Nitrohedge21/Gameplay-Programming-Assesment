using UnityEngine;

public class Spring : MonoBehaviour
{
    public float jumpForce = 15;
    GameObject player1;
    GameObject player2;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player 2");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Moved the main layer switch + collision ignore code into here and edited it to work inside the springs rather than player objects.
        //I don't know why the spring works despite not including the new "Player 2" tag but I'll keep it as it is for the time being.
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //The part below is for the closer ground/foreground platforms
        if (this.gameObject.layer == 6 && ((other.gameObject.tag == "Player" && other.gameObject.layer == 8) || (other.gameObject.tag == "Player 2" && other.gameObject.layer == 9)))
        {
            if (other.gameObject == player1)
            {
                //Launch the player and then do the thing.
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x, transform.position.y, 25);
                Physics2D.IgnoreLayerCollision(8, 6, true);
                Physics2D.IgnoreLayerCollision(8, 7, false);
                player1.GetComponent<PlayerController>().jumpableGround = 1 << 7;
            }
            //If the player hits the spring on the foreground(closerground in the editor), the collisionable layer swaps to background(furtherground in the editor)
            else if (other.gameObject == player2)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x, transform.position.y, 25);
                Physics2D.IgnoreLayerCollision(9, 6, true);
                Physics2D.IgnoreLayerCollision(9, 7, false);
                player2.GetComponent<PlayerController>().jumpableGround = 1 << 7;
            }

        }
        //This part is for futher ground/background platforms
        else if (this.gameObject.layer == 7 && ((other.gameObject.tag == "Player" && other.gameObject.layer == 8) || (other.gameObject.tag == "Player 2" && other.gameObject.layer == 9)))
        {
            if (other.gameObject == player1)
            {
                Physics2D.IgnoreLayerCollision(8, 7, true);
                Physics2D.IgnoreLayerCollision(8, 6, false);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                player1.GetComponent<PlayerController>().jumpableGround = 1 << 6;
            }
            else if (other.gameObject == player2)
            {
                Debug.Log("This part is not working");
                Physics2D.IgnoreLayerCollision(9, 7, true);
                Physics2D.IgnoreLayerCollision(9, 6, false);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                other.gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                player2.GetComponent<PlayerController>().jumpableGround = 1 << 6;
            }

        }


    }

}