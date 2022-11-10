using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float jumpForce = 20;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //I don't know why the spring works despite not including the new "Player 2" tag but I'll keep it as it is for the time being.
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        }
    }

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
            
    //    }
    //}

}
