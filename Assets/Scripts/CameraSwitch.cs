using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraSwitch : MonoBehaviour
{
    PlayerController playerController;
    private CinemachineVirtualCamera vcam;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        playerController = player1.GetComponent<PlayerController>();
        vcam.Follow = player1.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // I'm probably doing this part wrong but the following target should switch depending on which one is being controlled.
        //Object reference not set to an instance of an object -> I'm not referencing it right. Try to figure out how to do it properly.
        if(!playerController.isTails)
        {
            vcam.Follow = player2.GetComponent<Rigidbody2D>().transform;
        }
        else if (playerController.isTails)
        {
            vcam.Follow = player1.GetComponent<Rigidbody2D>().transform;
        }

        // All the other previous codes were fucking working fine, my dumb ass had added the script to another object in the scene as well so it was giving errors for that one.

    }
}
