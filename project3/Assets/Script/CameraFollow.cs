using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the player's Transform
    public float smoothSpeed = 0.125f;  // Smoothing factor for camera movement
    public Vector3 offset = new Vector3(3, 1, -10);  // Offset from the player
    Vector3 currentVelocity;


    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target.position + offset,
                ref currentVelocity,
                smoothSpeed
                );
            /*
            // Calculate the desired camera position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate between the current camera position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;

            // Debug log for position
            //Debug.Log("Desired Position: " + desiredPosition);
            //Debug.Log("Camera Position: " + transform.position);
            */
        }
    }
}