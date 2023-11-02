using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    [SerializeField]
    private float forceAmount = 5f; // Amount of force

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply the force
            rb.AddForce(transform.up * forceAmount, ForceMode.Impulse);
        }
    } 
}
