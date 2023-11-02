using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public float upwardSpeed = 2.0f;
    private float timeToHold = 1f; // Time to hold the player in seconds

    private GameObject player; // To store the player GameObject
    private Rigidbody playerRb; // To store the player's Rigidbody

    private Rigidbody bubbleRb;

    private Vector3 combinedVelocity; // To store the combined velocity

    public AudioSource bubbleSFX;

    private void Start()
    {
        bubbleRb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // Move the bubble upward
        bubbleRb.MovePosition(bubbleRb.position + Vector3.up * upwardSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            player = other.gameObject;
            playerRb = player.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Combine the velocities of the player and the bubble
                combinedVelocity = bubbleRb.velocity + playerRb.velocity;

                // Set the new velocity for the bubble
                bubbleRb.velocity = combinedVelocity / 1.3f;

                // Move the player to the center of the bubble
                player.transform.position = transform.position;

                // Make the player a child of the bubble
                player.transform.SetParent(transform);

                // Disable the player's Rigidbody to ensure it follows the bubble
                playerRb.isKinematic = true;

                // Start the coroutine to release the player after a delay
                StartCoroutine(ReleasePlayerAfterDelay());

                bubbleSFX.Play();
            }
        }
    }

    private IEnumerator ReleasePlayerAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(timeToHold);

        // Detach the player from this GameObject
        player.transform.SetParent(null);

        playerRb.isKinematic = false;

        // Apply the stored combined velocity to the player
        playerRb.velocity = combinedVelocity;

        // Destroy this GameObject
        Destroy(gameObject);
    }
}
