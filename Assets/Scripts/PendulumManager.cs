using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumManager : MonoBehaviour
{
    [HideInInspector]
    public float maxTorque = 1f;  // Maximum torque to apply for rotation
    private float maxAngle = 40;  // Maximum rotation angle
    private Rigidbody rb;  // Rigidbody component
    public float currentTorque;  // Current torque direction

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Initialize current torque to max torque
        currentTorque = maxTorque;
        // Initialize shouldFlipTorque to false
    }

    void FixedUpdate()
    {
        // Get the current Z rotation
        float zRotation = transform.eulerAngles.z;

        // Convert to -180 to 180 degrees
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        Vector3 angularVelocity = rb.angularVelocity;
        // Check if the pendulum is within 10 degrees of the max angle
        if (zRotation > maxAngle - 20 && angularVelocity.z > 0)
        {
            //Debug.Log(this.gameObject.name + " " + zRotation + ": Go CW : " + angularVelocity.z);
            // Apply a stronger counter-torque
            currentTorque = -2 * maxTorque;
        }
        else if (zRotation < (-maxAngle) + 20 && angularVelocity.z < 0)
        {
            //Debug.Log(this.gameObject.name + " " + zRotation + ": Go CCW : " + angularVelocity.z);
            // Apply a stronger counter-torque
            currentTorque = 2 * maxTorque;
        }
        else
        {
            //Debug.Log(this.gameObject.name + " " + zRotation + ": Slow down : " + angularVelocity.z);
            // Reset to the maximum torque
            currentTorque = Mathf.Sign(currentTorque) * maxTorque;
        }

        // Apply the torque around the Z-axis
        rb.AddTorque(0, 0, currentTorque);
    }
}
