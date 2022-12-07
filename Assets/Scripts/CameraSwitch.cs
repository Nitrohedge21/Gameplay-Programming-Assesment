using UnityEngine;
using Cinemachine;
public class CameraSwitch : MonoBehaviour
{
    //Note to myself; The issue with the camera tracking the z axis can be resolved with setting the tracked object offset to 0 in cinemachine's body.
    //Also setting the X to zero resembles the sonic camera so I might keep it that way.
    //Set the Screen Y on cinemachine's body to change the position as it could not be done through the object itself.
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

    
    void Update()
    {
        // I'm probably doing this part wrong but the following target should switch depending on which one is being controlled.
        //Object reference not set to an instance of an object -> I'm not referencing it right. Try to figure out how to do it properly.
        if(!playerController.isSonic)
        {
            vcam.Follow = player2.transform;
        }
        else if (playerController.isSonic)
        {
            vcam.Follow = player1.transform;
        }

        // All the other previous codes were fucking working fine, my dumb ass had added the script to another object in the scene as well so it was giving errors for that one.

    }
}
