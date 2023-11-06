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
        HandleTriggerCollisions(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        HandlePhysicsCollisions(other);
    }

    // Handles interactions with triggers (e.g. Vines, Ground, Pendulum)
    private void HandleTriggerCollisions(Collider other)
    {
        UpdateCameraTargetPosition(other);
    }

    // Handles interactions with physics objects (e.g. Spikes, Ground)
    private void HandlePhysicsCollisions(Collision other)
    {
        UpdateCameraTargetPosition(other.collider);
    }

    // Updates the camera's target position based on the object the player collided with
    private void UpdateCameraTargetPosition(Collider other)
    {
        shouldMoveCamera = true;
        initialCameraPosition = Camera.main.transform.position;
        Transform parentTransform = other.transform.parent;

        if (parentTransform != null && parentTransform.name.Contains("Pendulum"))
        {
            targetCameraPosition = new Vector3(parentTransform.position.x + 25, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        else if (!other.name.Equals("Goal") && !other.name.Contains("Spike"))
        {
            targetCameraPosition = new Vector3(other.transform.position.x + 25, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }

        boostRemaining = maxBoost;
        lerpTime = 0;
    }
}
