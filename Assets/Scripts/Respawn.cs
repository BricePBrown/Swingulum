using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
  
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private GameObject currentPlayer;
    bool playerSpawned = false;
    bool isAlive;

    
    // Special Effects
    [SerializeField] ParticleSystem checkpointParticles;
    [SerializeField] AudioSource checkpointSFX;

    private void Start()
    {
        //  respawnPoint = new Vector3(-21.7f, -0.53f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Debug.Log("spawning character");
            SpawnCharacter();
        }
    }

    public void SpawnCharacter()
    {
        // Get all TrailRenderers in the children of currentPlayer
        TrailRenderer[] trails = currentPlayer.GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trail in trails)
        {
            StartCoroutine(LerpTrailTimeToZero(trail, 0.25f));  // 0.5 seconds duration
        }

        // Assuming currentPlayer is a GameObject with a Rigidbody component
        Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset velocity and angular velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        currentPlayer.GetComponent<MeshRenderer>().enabled = false;

        // Enable the TrailRenderer after a short delay
        foreach (TrailRenderer trail in trails)
        {
            StartCoroutine(EnableTrailAfterDelay(trail, 0.75f, 2));  // 0.5 seconds delay
        }
    }

    IEnumerator LerpTrailTimeToZero(TrailRenderer trail, float duration)
    {
        float originalTime = trail.time;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trail.time = Mathf.Lerp(originalTime, 0, t);
            yield return null;
        }

        trail.time = 0;
        trail.enabled = false;
    }

    IEnumerator EnableTrailAfterDelay(TrailRenderer trail, float delay, float originalTime)
    {
        yield return new WaitForSeconds(delay);

        // Reset the character's position and any other necessary attributes
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        // Reactivate any components or scripts that were disabled during death
        currentPlayer.SetActive(true);
        currentPlayer.GetComponent<MeshRenderer>().enabled = true;

        currentPlayer.GetComponent<PlayerMovement>().boostRemaining = currentPlayer.GetComponent<PlayerMovement>().maxBoost;

        // Set character back to alive
        isAlive = true;

        // Enable the trail but keep the time at 0
        trail.enabled = true;
        trail.time = 0;

        // Gradually increase the time to its original value
        float elapsedTime = 0;
        float duration = 0.5f;  // Duration to lerp the time value
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trail.time = Mathf.Lerp(0, originalTime, t);
            yield return null;
        }

        // Set the time to its original value
        trail.time = originalTime;
    }

    public void SetRespawnPoint(Transform input)
    {
        if(input != respawnPoint)
        {
            respawnPoint = input;
            PlayCheckpointEffects();
        }
    }

    private void PlayCheckpointEffects()
    {
        checkpointParticles.Play();
        checkpointSFX.Play();
    }
}
