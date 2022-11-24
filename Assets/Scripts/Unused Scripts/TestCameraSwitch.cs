using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraSwitch : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private void Update()
    {
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    //Got the code from a tutorial on youtube but it wasn't working well. It was zoomed in the character and the character was looking like it was stuttering.
    // https://www.youtube.com/watch?v=ZBj3LBA2vUY
}

