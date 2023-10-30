using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    //For determining how fast the caera should catch up with the player

    public Transform target;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z; 
        //to put the z-position of player always in the z-position of the camera 
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
    }
}
