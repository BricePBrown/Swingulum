using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    private Transform currentSwinging;
    private ConstantForce myConstantForce;
    private Rigidbody rb;
    private Vector3 currentSwingingVelocity;

    private bool swinging = false;
    private bool isGrounded;

    [HideInInspector] public int maxBoost = 1;
    [HideInInspector]
    public int boostRemaining;

    public Vector3 vineVelocityWhenGrabbed;

    private float impulseStrength = 9.0f; // The strength of the impulse

    public float lerpSpeed = 1.0f;  // Speed of the camera movement
    private bool shouldMoveCamera = false;  // Flag to control camera movement
    private Vector3 initialCameraPosition;  // Initial position of the camera
    private Vector3 targetCameraPosition;  // Target position for the camera
    private float lerpTime = 0;  // Time for Lerp function
    [HideInInspector]
    public bool canBoost = false;

    //change color when boosting
    Renderer rend;
    public float colorChangeSpeed = 1f;
    Color targetColor = Color.white;
    
    public MMFeedbacks ballBoostFeedbacks;


    void Start()
    {
        myConstantForce = GetComponent<ConstantForce>();
        rb = GetComponent<Rigidbody>();
        currentSwingingVelocity = GetComponent<Rigidbody>().velocity;
        boostRemaining = maxBoost;

        rend = GetComponent<Renderer>();
        

    }

    void Update()
    {
        //isGrounded = rb.velocity.y <= 0.5f;

        //if (isGrounded)
        //{
        //    boostRemaining = maxBoost;
        //}

        //if (swinging)
        //{
        //    myConstantForce.enabled = false;
        //    transform.position = currentSwinging.position;
        //    currentSwingingVelocity = currentSwinging.GetComponent<Rigidbody>().velocity; // Store the vine's velocity

        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        Debug.Log("Leaving Vine");
        //        swinging = false;
        //        rb.velocity = currentSwingingVelocity; // Set the player's velocity to the vine's velocity
        //        rb.useGravity = true;
        //    }
        //}
        if (canBoost && Input.GetKeyDown(KeyCode.Space) && boostRemaining > 0)
        {
            Debug.Log("Boosting");
            ApplyImpulse();

            
        }

        
        
        // If shouldMoveCamera is true, move the camera
        if (shouldMoveCamera)
        {
            lerpTime += Time.deltaTime * lerpSpeed;
            float smoothStep = Mathf.SmoothStep(0, 1, lerpTime);

            Camera.main.transform.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, smoothStep);

            // Stop moving the camera when it reaches the target position
            if (lerpTime >= 1)
            {
                shouldMoveCamera = false;
                lerpTime = 0;
            }
        }
    }

    void ApplyImpulse()
    {
        Debug.Log("Boosting");
        Vector3 currentDirection = rb.velocity.normalized;
        rb.AddForce(currentDirection * impulseStrength, ForceMode.Impulse);
        boostRemaining -= 1;

        ballBoostFeedbacks.PlayFeedbacks();

   
        rend.material.color = Color.Lerp(rend.material.color, targetColor, Time.deltaTime * colorChangeSpeed); // gradually change color to white when boosting
        Debug.Log(targetColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vine")
        {
            other.GetComponent<Rigidbody>().velocity = vineVelocityWhenGrabbed;
            swinging = true;
            
            currentSwinging = other.transform;
            boostRemaining = maxBoost;
        }

        if (other.gameObject.tag == "ground")
        {
            this.GetComponent<Respawn>().SpawnCharacter();
        }
        else
        {
            Debug.Log("collision" + other.name);
            shouldMoveCamera = true;
            initialCameraPosition = Camera.main.transform.position;
            Transform parentTransform = null;

            if (other.transform.parent != null)
            {
                parentTransform = other.transform.parent;
            }
            if (parentTransform != null && parentTransform.name.Contains("Pendulum"))
            {
                Debug.Log("Moving Camera to New Pendulum");
                targetCameraPosition = new Vector3(parentTransform.position.x + 25, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
            else if (!other.name.Equals("Goal"))
            {
                // Base it off the player's X position
                Debug.Log("Moving Camera to Bounce Object");
                targetCameraPosition = new Vector3(other.transform.position.x + 25, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
            boostRemaining = maxBoost;
            
            lerpTime = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ground")
        {
            this.GetComponent<Respawn>().SpawnCharacter();
        }
        else
        {
            Debug.Log("collision" + other.gameObject.name);
            shouldMoveCamera = true;
            initialCameraPosition = Camera.main.transform.position;
            if (!other.gameObject.name.Contains("Spike"))
            {
                // Base it off the player's X position
                Debug.Log("Moving Camera to Bounce Object");
                targetCameraPosition = new Vector3(other.transform.position.x + 25, Camera.main.transform.position.y, Camera.main.transform.position.z);
                boostRemaining = maxBoost;
            }

            lerpTime = 0;
        }
    }
}
