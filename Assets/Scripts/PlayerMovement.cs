using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    // Physics components
    private Rigidbody rb;

    // Boost mechanics
    [HideInInspector] public int maxBoost = 1;
    [HideInInspector] public int boostRemaining;
    private float impulseStrength = 9.0f;
    public MMFeedbacks ballBoostFeedbacks;
    [HideInInspector] public bool canBoost = false;

    // Camera movement
    public float lerpSpeed = 1.0f;
    private bool shouldMoveCamera = false;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;
    private float lerpTime = 0;

    // Color change on boost
    Renderer rend;
    public float colorChangeSpeed = 1f;
    Color targetColor = Color.white;


    private void Start()
    {
        InitializeComponents();
        boostRemaining = maxBoost;
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        CheckForBoostInput();
        HandleCameraMovement();
    }

    // Checks for boost input and applies if conditions are met
    private void CheckForBoostInput()
    {
        if (canBoost && Input.GetKeyDown(KeyCode.Space) && boostRemaining > 0)
        {
            ApplyImpulse();
        }
    }

    // Moves the camera smoothly to the target position
    private void HandleCameraMovement()
    {
        if (!shouldMoveCamera) return;

        lerpTime += Time.deltaTime * lerpSpeed;
        float smoothStep = Mathf.SmoothStep(0, 1, lerpTime);

        Camera.main.transform.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, smoothStep);

        if (lerpTime >= 1)
        {
            shouldMoveCamera = false;
            lerpTime = 0;
        }
    }

    // Applies a boost in the direction the player is currently moving
    private void ApplyImpulse()
    {
        Vector3 currentDirection = rb.velocity.normalized;
        rb.AddForce(currentDirection * impulseStrength, ForceMode.Impulse);
        boostRemaining--;

        // Visual and auditory feedback for the boost
        ballBoostFeedbacks.PlayFeedbacks();
        rend.material.color = Color.Lerp(rend.material.color, targetColor, Time.deltaTime * colorChangeSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTriggerCollisions(other, rb.velocity.x);
    }

    private void OnCollisionEnter(Collision other)
    {
        HandlePhysicsCollisions(other, rb.velocity.x);
    }

    // Handles interactions with triggers (e.g. Vines, Ground, Pendulum)
    private void HandleTriggerCollisions(Collider other, float velocity)
    {
        UpdateCameraTargetPosition(other, velocity);
    }

    // Handles interactions with physics objects (e.g. Spikes, Ground)
    private void HandlePhysicsCollisions(Collision other, float velocity)
    {
        UpdateCameraTargetPosition(other.collider, velocity);
    }

    // Updates the camera's target position based on the object the player collided with
    private void UpdateCameraTargetPosition(Collider other, float velocity)
    {
        if ((other.transform.parent == null || !other.transform.parent.name.Contains("Pendulum")) &&
    !other.name.ToLower().Contains("bounce"))
        {
            // Exit the method if the conditions are not met
            return;
        }

        shouldMoveCamera = true;
        initialCameraPosition = Camera.main.transform.position;

        // Calculate potential new positions
        float offset = (velocity >= 0) ? 25 : -25;
        float newX = initialCameraPosition.x;
        //float newY = initialCameraPosition.y;

        if (other.transform.parent != null && other.transform.parent.name.Contains("Pendulum"))
        {
            newX = other.transform.parent.position.x + offset;
            //newY = other.transform.parent.position.y - 11;
        }
        else if (other.name.ToLower().Contains("bounce"))
        {
            newX = other.transform.position.x + velocity;
            //newY = other.transform.position.y - 11;
        }

        // Apply the threshold
        targetCameraPosition = new Vector3(
            Mathf.Abs(newX - initialCameraPosition.x) > 15 ? newX : initialCameraPosition.x,
            initialCameraPosition.y,
            Camera.main.transform.position.z
        );

        boostRemaining = maxBoost;
        lerpTime = 0;
    }

}
