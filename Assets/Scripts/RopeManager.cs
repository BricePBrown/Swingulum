 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class RopeManager : MonoBehaviour
{
    private bool ballAttached = false;  // Flag to check if the ball is attached
    GameObject player;
    Collider playerCollider;
    Collider thisCollider;

    public AudioSource attachedSFX;

    
    public MMFeedbacks ballShootFeedback;
    void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check if the colliding object is the ball and if the ball is not already attached
        if (collision.gameObject.name.Equals("Player") && !ballAttached)
        {
            player = collision.gameObject;
            playerCollider = player.GetComponent<Collider>();
            player.GetComponent<PlayerMovement>().canBoost = false;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
            AttachPlayerToPendulum();
            attachedSFX.Play();
        }
    }

    private void AttachPlayerToPendulum()
    {
        // Attach the ball to the pendulum
        player.transform.SetParent(transform);

        Vector3 parenScale = player.transform.parent.transform.localScale;
        player.transform.localScale = new Vector3(2 / parenScale.x, 2 / parenScale.y, 2 / parenScale.z);
        //player.transform.localPosition = new Vector3(0, -1, 0);  // Adjust the position based on your specific setup
        player.transform.localRotation = Quaternion.identity;

        // Set x and z positions instantly
        Vector3 newPos = player.transform.localPosition;
        newPos.x = 0;
        newPos.z = 0;
        player.transform.localPosition = newPos;

        // Set the flag to indicate the ball is attached
        ballAttached = true;

        // Lerp the y position
        StartCoroutine(LerpToTargetY(player.transform.localPosition.y, -1, 0.25f));
    }

    private IEnumerator LerpToTargetY(float startY, float endY, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float newY = Mathf.Lerp(startY, endY, timeElapsed / duration);
            Vector3 playerPos = player.transform.localPosition;
            playerPos.y = newY;
            player.transform.localPosition = playerPos;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure the final position is exactly what you want
        Vector3 finalPos = player.transform.localPosition;
        finalPos.y = endY;
        player.transform.localPosition = finalPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ballAttached)
            {// Temporarily ignore collision between the player and the pendulum
                Physics.IgnoreCollision(playerCollider, thisCollider, true);

                // Get the angular velocity of the pendulum
                Rigidbody pendulumRb = transform.parent.GetComponent<Rigidbody>();
                Vector3 angularVelocity = pendulumRb.angularVelocity;

                // Determine the direction of rotation based on the angular velocity
                float rotationDirection = Mathf.Sign(angularVelocity.z);  // Assuming rotation around the Z-axis

                // Get the torque applied to the pendulum
                float torque = transform.parent.GetComponent<PendulumManager>().maxTorque;

                // Calculate the radius (distance from the pivot to the ball)
                Vector3 radiusVector = player.transform.position - transform.position;
                float radius = radiusVector.magnitude;

                Rigidbody playerRb = player.GetComponent<Rigidbody>();
                float mass = pendulumRb.mass + playerRb.mass;

                //Debug.Log(torque + " " + radius + " " + mass + " ");
                // Calculate the approximate tangential velocity based on the torque
                float tangentialSpeed = Mathf.Sqrt(2 * torque * radius / mass);

                // Calculate the tangential direction
                Vector3 axisOfRotation = new Vector3(0, 0, 1);  // Z-axis for rotation in the YX plane
                Vector3 tangentialDirection = Vector3.Cross(axisOfRotation, radiusVector).normalized;

                // Apply the direction of rotation
                tangentialDirection *= rotationDirection;

                // Calculate the tangential velocity
                Vector3 tangentialVelocity = tangentialSpeed * tangentialDirection;

                // Detach the player from the pendulum
                player.transform.SetParent(null);

                // Restore the player's original scale
                //player.transform.localScale = new Vector3(2, 2, 2);

                // Restore the player's physics
                playerRb.isKinematic = false;

                // Apply the calculated tangential velocity to the player
                playerRb.velocity = tangentialVelocity * 15;

                // Reset the flag to indicate the ball is no longer attached
                ballAttached = false;

                // Re-enable the collision after a short delay
                StartCoroutine(ReEnableCollision());

                ballShootFeedback.PlayFeedbacks();
                Debug.Log("playing sfx");

            }
            else if (player != null && player.GetComponent<PlayerMovement>() != null)
            {
          
                 StartCoroutine(EnableBoostAfterDelay(0.1f));
   
            }
        }
    }



    private IEnumerator EnableBoostAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Enable boosting
        player.GetComponent<PlayerMovement>().canBoost = true;
    }


    IEnumerator ReEnableCollision()
    {
        yield return new WaitForSeconds(0.4f);  // Wait for 1 second
        Physics.IgnoreCollision(playerCollider, thisCollider, false);
        player.GetComponent<PlayerMovement>().canBoost = true;
    }
}
