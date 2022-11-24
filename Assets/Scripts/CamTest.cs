using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CamTest : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] private Transform Sonic;
    [SerializeField] private Transform Tails;
    private Transform playerFollow;

    float xPosSonic;
    float yPosSonic; 
    float xPosTails;
    float yPosTails;

    void Start()
    {
        playerController = Sonic.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!playerController.isSonic)
        {
            playerFollow = Tails.transform;
        }
        else if (playerController.isSonic)
        {
            playerFollow = Sonic.transform;
        }
        transform.position = new Vector3(playerFollow.position.x, playerFollow.position.y, transform.position.z);
    }
}
