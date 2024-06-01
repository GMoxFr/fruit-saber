using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour
{
    public Transform vrControllerTransform; // Assign the VR controller transform in the inspector

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the saber.");
        }
    }

    void FixedUpdate()
    {
        if (vrControllerTransform != null)
        {
            // Update the Rigidbody's position and rotation to match the VR controller
            rb.MovePosition(vrControllerTransform.position);
            rb.MoveRotation(vrControllerTransform.rotation);
        }
    }
}