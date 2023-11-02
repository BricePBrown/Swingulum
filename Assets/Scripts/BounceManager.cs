using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceManager : MonoBehaviour
{
    public float bounceForce = 50f; // The force applied upon collision

    public AudioSource bounceSFX;
    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody component of the colliding object
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calculate the direction of the bounce
            Vector3 bounceDirection = collision.contacts[0].normal;

            // Apply the bounce force to the Rigidbody
            rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);

            bounceSFX.Play();
        }
    }
}
